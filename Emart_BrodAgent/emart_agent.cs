using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using sohoUniLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using WMPLib;

namespace Emart_BrodAgent
{


    
    public partial class emart_agent : Form
    {

        uniUtil util = new uniUtil();

        delegate void fileDownLoadDeleget();

        bool waitFirstCheck = false; //최초 대기 음악 사용

        System.Windows.Forms.Timer sendTimer = new System.Windows.Forms.Timer();
        didInfo didinfo = new didInfo();
        fileDownInfo downInfo = new fileDownInfo();
        uniXml utilXml = new uniXml();


        WMPLib.WindowsMediaPlayer Player = new WMPLib.WindowsMediaPlayer();

        //기본 정보 
        public string program_path = Environment.CurrentDirectory;
        public string basic_mp3_path = System.Environment.CurrentDirectory + "\\basic_mp3\\";
        public string mp3_path = System.Environment.CurrentDirectory + "\\mp3\\";
        public string xml_path = System.Environment.CurrentDirectory + "\\xml\\";
        //신규
        public string send_path = System.Environment.CurrentDirectory + "\\snd\\";
        public string send_pathOld = System.Environment.CurrentDirectory + "\\snd_Old\\";

        public string default_path = System.Environment.CurrentDirectory + "\\basic_music\\";

        public int timeCnt = 1;

        public string pre_mp3PlayTxt = null;


        public emart_agent()
        {
            InitializeComponent();

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            playList01.Visible = false;            
            this.FormBorderStyle = FormBorderStyle.FixedSingle;            
        }
        
        private void emart_agent_Load(object sender, EventArgs e)
        {

            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;

            if (util.GetRegistry("sound_volume").Equals(""))
            {
                trackBar_info.Value = 100; ;
                sohoUniLib.uniUtil.SetRegistry("sound_volume", "100");
            }
            else
                trackBar_info.Value = sohoUniLib.SoundUtil.GetSoundVolume();

            //List < WindowInfo.hddInfo > dirveInfos = WindowInfo.comHddInfo();
            //foreach (WindowInfo.hddInfo info in dirveInfos )
            //{
            //    Console.WriteLine(info.Drive + ":" + info.Volume_label + ":" + info.File_type + ":" + info.TotalFreeSpace + ":" + info.totalSize);
            //}

            if (util.GetRegistry("license_info").Equals(""))
            {
                agent_setting();
            }
            else
            {
                btn_Save.Text = "정보변경";
                didinfo.didId = util.GetRegistry("did_id");
                didinfo.didMac = util.GetRegistry("license_info");
                orderCheck();
            }
            //string brodMessage = "{'command_type':'SP_BASICSCHLST_NEWFILEINFO','FILEINFO':[{'STREFILENM':'FILE_000000000005316.mp3','ORIGNL_FILE_NM':'백아연 이럴거면 그러지말지 (feat. Young K) 20.02.10 20.02.17','FILESTRECOURS':'/202002/'},{'STREFILENM':'FILE_000000000005317.mp3','ORIGNL_FILE_NM':'버스커 버스커 사랑은 타이밍 20.02.10 20.02.17','FILESTRECOURS':'/202002/'},{'STREFILENM':'FILE_000000000005318.mp3','ORIGNL_FILE_NM':'볼빨간사춘기 별 보러 갈래 20.02.10 20.02.17','FILESTRECOURS':'/202002/'},{'STREFILENM':'FILE_000000000005319.mp3','ORIGNL_FILE_NM':'아이유 스물셋 20.02.10 20.02.17','FILESTRECOURS':'/202002/'},{'STREFILENM':'FILE_000000000005320.mp3','ORIGNL_FILE_NM':'에이오에이 짧은 치마 (Miniskirt) 20.02.10 20.02.17','FILESTRECOURS':'/202002/'},{'STREFILENM':'FILE_000000000005323.mp3','ORIGNL_FILE_NM':'TLC_It's Sunny','FILESTRECOURS':'/202002/'}]}";
            //jsonArrayList(brodMessage, "FILEINFO");
            //sendPlayInfo();
        }
        
        private bool sendPlayInfo()
        {
            
            //파일 정보 알아오기
            string[] send_fileLists = util.dirFileLst(send_path, DateTime.Now.ToString("yyyyMMdd")+".xml");
            string sendJosnTxt = "";
            foreach (string send_fileList in send_fileLists)
            {


                if (util.GetRegistry("centerId").ToString() == "")
                {
                    sohoUniLib.uniUtil.SetRegistry("centerId", util.GetRegistry("did_id").Substring(0, (util.GetRegistry("did_id").ToString().Length - 3)));
                }


                //Console.WriteLine(send_fileList);
                if (!string.IsNullOrEmpty(send_fileList))
                {
                    sendJosnTxt += "{\"command_type\":\"SP_BASICPLAYUPDATE\",\"command_data\":[{";
                    sendJosnTxt += "\"DID_ID\":\""+ util.GetRegistry("centerId") +"\",";
                    sendJosnTxt += "\"PLAY_DAY\":\"" + send_fileList.Replace(".xml", "") + "\",";
                    sendJosnTxt += "\"FILEPLAYLIST\":\"Y\",\"playInfo\":[" + utilXml.xmlToJson(send_path + send_fileList, agentConstInfo.xmlPlayXmlInfo) + "]";
                    sendJosnTxt += "}]}";

                    string returnString = sohoUniLib.uniUtil.WebPostDataSend(sendJosnTxt, util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);
                    if (sohoUniLib.ServerComm_json.jsonResult(returnString).Equals("O"))
                    {
                        //파일 이동 
                        if (sohoUniLib.uniUtil.fileMove(send_path + send_fileList, send_pathOld + send_fileList) == true)
                        {
                            util.setLogFile(send_fileList + "이 정상적으로 서버에 업데이트 되었습니다.");
                        }
                    }
                    returnString = "";
                    sendJosnTxt = "";
                }
            }
            return true;
        }

       
        private void SendTimer_Tick(object sender, EventArgs e)
        {
            
            sendTimer.Interval = 1000;
            if (util.programekill("Windows10UpgraderApp") == true)
            {
                util.setLogFile("windows update kill");
            }                         
            agent_state();            
        }
        private void timeCheck ()
        {            
            if ((Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Replace(":", "")) % 100) == 0)
            {
                Console.WriteLine("play 시간" + DateTime.Now.ToString("HH:mm:ss"));
            }
        }

        //사운드 제거 
        private void setRegSound()
        {
            util.program_start(System.Environment.CurrentDirectory, "noSound.bat");
        }
        private void brodAgentStart()
        {

            bool fileCheck = false;            
            
            if (sohoUniLib.uniUtil.dirExite(mp3_path) == false)
            {
                gbSettingInfo.Visible = false;
                gbInfo.Visible = true;
                gbInfo.BringToFront();
                lbl_agentInfo.Text = "에이전트 시작.";

                sendTimer.Interval = 100;
                sendTimer.Tick += SendTimer_Tick;
                sendTimer.Start();
                

                //toTray();
            }
            else
            {
                XDocument doc = XDocument.Load(xml_path + agentConstInfo.xmlFileName);
                var brodFiles = (from brodFile in doc.Descendants("fileList")
                                 orderby brodFile.Element("download_date").Value ascending
                                 select new
                                 {
                                     brodFile.Element("file_name").Value
                                 }).ToList();

                foreach (var brodFile in brodFiles)
                {
                    if (util.fileEx(mp3_path + "\\" + util.fileExit(brodFile.ToString(), "=").Replace(" ", "").Replace("}", "")))
                    {
                        fileCheck = true;
                    }
                    else
                    {
                        fileCheck = false;
                        break;
                    }
                }
                if (fileCheck == true)
                {

                    gbSettingInfo.Visible = false;
                    gbInfo.Visible = true;
                    gbInfo.BringToFront();
                    lbl_agentInfo.Text = "에이전트 시작.";
                    //맞는지 확인
                    playerDispos(Player);
                    waitPlay();

                    sendTimer.Interval = 100;
                    sendTimer.Tick += SendTimer_Tick;
                    sendTimer.Start();
                    //toTray();
                }
                else
                {
                    lbl_agentInfo.Text = "파일 재다운로드.";
                    sendTimer.Interval = 1000;
                    sendTimer.Tick += SendTimer_Tick;
                    sendTimer.Start();
                    brodSchInfo("R");
                }
            }
           
            //여기 부분 확인 하기 

            
        }
        private void orderCheck()
        {

            try
            {
                //명령 전송후 작업 
                if (!util.GetRegistry("MSG_SEQ").Equals("") && !util.GetRegistry("XML_PROCESS_NAME").Equals("") && util.GetRegistry("ORDER_SEND").Equals(""))
                {

                    didinfo.errorMessage = "OK";
                    didinfo.msgSeq = util.GetRegistry("MSG_SEQ");
                    string returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe11), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);

                    if (sohoUniLib.ServerComm_json.jsonResult(returnString).Equals("O"))
                    {
                        sohoUniLib.uniUtil.SetRegistry("ORDER_SEND", DateTime.Now.ToString("HH:mm:ss"));
                    }
                    else
                    {

                    }

                }
                brodAgentStart();
            }
            catch(Exception e)
            {
                lbl_agentInfo.Text = "장애:" + e.ToString();
                network_stateError();
                util.setLogFile("orderCheck:" + e.ToString());
            }
        }
        private void network_stateError()
        {
            if (utilXml.xmlNodeCount(xml_path + agentConstInfo.xmlBrodFileName, "/dataset/brodList") > 0)
            {
                lbl_agentInfo.Text = "서버 접속 애러. \r\n기존 방송으로 재생.";
                lbl_agentInfo.Size = new Size(250, 50);
                pnl_loading.Visible = false;
                brodSoundPlayList();

            }
            else
            {
                sohoUniLib.AutoClosingMessageBox.Show("정상적으로 서버에 접속 하실수 없습니다. 관리자에게 문의 바랍니다.", "alert", 60);
                program_exit();
            }
        }
        //기초 음원 다운로드 
        private void basic_brodDown()
        {
            lbl_agentInfo.Text = "기초음원 다운로드 시작.";
            playerDispos(Player);
            basicSchinfo();
            
        }
        private void basicSchinfo()
        {
            try
            {
                
                sendTimer.Stop();
                string returnString = string.Empty;


                //returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe16), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);


                returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe19), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);

                //basic_mp3삭제
                //추후 수정 
                sohoUniLib.uniUtil.directory_dels(basic_mp3_path);                
                if (returnString.Length > 50)
                {

                    if (playList01.Visible == true)
                    {
                        playList01.Visible = false;
                    }                    
                    //xml생성 
                    sohoUniLib.uniUtil.SetRegistry("download", "B");

                    lbl_agentInfo.Text = "기본 음원 스케줄 확인.";
                    if (backgroundWorker.IsBusy != true)
                    {
                        //toOpen();
                        backgroundWorker.RunWorkerAsync();
                    }
                    else
                    {                     
                        util.setLogFile("backgroundWorker IsBusy");
                    }

                }
                else
                {
                    util.setLogFile("전문 전송 에러 다시 전문 요청");
                    sohoUniLib.uniUtil.Delay(30);
                    basicSchinfo();
                }

            }
            catch (Exception e)
            {

                util.setLogFile("brodSchInfo error:" + e.ToString());
                lbl_agentInfo.Text = "스케줄 에러:" + e.ToString();
            }
        }
        //전문 전송 
        private void server_dateCheck()
        {
            
            try
            {
                if (sohoUniLib.NetWorkClass.UrlConnectionValidation(util.GetRegistry("server_url")) == true)
                {
                    string returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe01), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);
                    
                    string[] didState = sohoUniLib.ServerComm_json.jsonResult(returnString).Split('/');

                    //스케줄 정보 먼저 받아오기
                    if (!didState[3].ToString().Equals("N"))
                    {
                        sohoUniLib.uniUtil.SetRegistry("basicCode", didState[3].ToString());
                        didinfo.basicCode = didState[3].ToString();
                        //기초 음원 다운로드 
                        basic_brodDown();
                    }
                    else if (Convert.ToInt32(didState[0].ToString()) > 0)
                    {
                        util.setLogFile("신규음원이 있으면");
                        //음원 중지                         
                        playerDispos(Player);
                        waitPlay();
                        brodSchInfo("S");
                        lbl_agentInfo.Text = "다운로드 시작.";
                    }
                    else if (Convert.ToInt32(didState[1].ToString()) > 0 && didState[0].ToString().Equals("0"))
                    {
                        brodOrder();
                    }

                    // 일반 음원만 다운 받을떄 
                    //if (Convert.ToInt32(didState[0].ToString()) > 0)
                    //{
                    //    util.setLogFile("신규음원이 있으면");
                    //    //음원 중지                         
                    //    playerDispos(Player);
                    //    waitPlay();
                    //    brodSchInfo("S");
                    //    lbl_agentInfo.Text = "다운로드 시작.";
                    //}
                    //else if (Convert.ToInt32(didState[1].ToString()) > 0 && didState[0].ToString().Equals("0"))
                    //{
                    //    brodOrder();
                    //}

                    //if (Convert.ToInt32(didState[0].ToString()) > 0)
                    //{
                    //    util.setLogFile("신규음원이 있으면");
                    //    //음원 중지                         
                    //    playerDispos(Player);
                    //    waitPlay();
                    //    brodSchInfo("S");
                    //    lbl_agentInfo.Text = "다운로드 시작.";
                    //}
                    //else if (Convert.ToInt32(didState[1].ToString()) > 0 && didState[0].ToString().Equals("0"))
                    //{
                    //    brodOrder();
                    //}

                }
                else
                {

                    util.setLogFile("did.emart.com 으로 접속 안됨");

                    if (utilXml.xmlNodeCount(xml_path + agentConstInfo.xmlBrodFileName, "/dataset/brodList") > 0)
                    {
                        lbl_agentInfo.Text = "서버 접속 애러. \r\n기존 방송으로 재생.";
                        lbl_agentInfo.Size = new Size(250, 50);
                        pnl_loading.Visible = false;
                       //brodSoundPlayList();

                    }
                    else
                    {
                        lbl_agentInfo.Text = "서버 접속 애러. \r\n네트워크를 확인해 주세요.";
                        lbl_agentInfo.Size = new Size(250, 50);
                        pnl_loading.Visible = false;
                    }
                }
            }
            catch (Exception e)
            {
                util.setLogFile("agent_state error:" + e.ToString());

            }
        }

        //에이전트 상태값 알아오기
        private void agent_state()
        {

            try
            {
                
                    if ((Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Replace(":", "")) % 100) == 0)
                    {
                        //util.setLogFile("1분 단위 로그 찍기");
                        lbl_agentInfo.Text = "음원방송 시작.";
                        brodSoundPlayList();
                    }
            }
            catch (Exception e)
            {
                //Console.WriteLine("agent_state error:" + e.ToString());
                util.setLogFile("agent_state error:" + e.ToString());
            }


        }
        private void brodOrder()
        {

            //노드 삭제
            utilXml.delNodeAll(xml_path + agentConstInfo.xmlBrodOrderList, "orderList");      
            
            string returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe07), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);

            DataTable dt = sohoUniLib.ServerComm_json.jsonArrayList(returnString, "CONINFO");
            DataRow[] properIDs = dt.Select("1=1", "");

            for (int i = 0; i < properIDs.Length; i++)
            {
                DataRow temp = properIDs[i];                
                if (utilXml.xmlOrderCreateNode(xml_path + agentConstInfo.xmlBrodOrderList, temp["MSG_SEQ"].ToString(), temp["XML_PROCESS_NAME"].ToString(), "") == true)
                {
                    //xml생성                    
                }
            }
            //returnMessage = "음원 다운로드 완료.";
            orderPlay();

        }
        
        private void orderPlay()
        {
            util.setLogFile("orderPlay");

            XDocument doc = XDocument.Load(xml_path + agentConstInfo.xmlBrodOrderList);
            var orderFiles = (from orderFile in doc.Descendants("orderList")
                             orderby orderFile.Element("MSG_SEQ").Value ascending
                             select new
                             {
                                 MSG_SEQ = orderFile.Element("MSG_SEQ").Value,
                                 XML_PROCESS_NAME = orderFile.Element("XML_PROCESS_NAME").Value,                                 
                             }).First();

            //Console.WriteLine(orderFiles.MSG_SEQ);
            //Console.WriteLine(orderFiles.XML_PROCESS_NAME);

            //레지스트리 정리 
            sohoUniLib.uniUtil.SetRegistry("MSG_SEQ", orderFiles.MSG_SEQ);
            sohoUniLib.uniUtil.SetRegistry("XML_PROCESS_NAME", orderFiles.XML_PROCESS_NAME);
            sohoUniLib.uniUtil.SetRegistry("ORDER_SEND", "");


            
            if (orderFiles.XML_PROCESS_NAME.Equals("SP_DIDRESTART"))
            {
                util.program_start(System.Environment.CurrentDirectory , "program_restart.bat" );
            }
            else if (orderFiles.XML_PROCESS_NAME.Equals("SP_DIDREBOOT"))
            {
                sohoUniLib.uniUtil.PCStateChage("SP_DIDREBOOT");
            }
            else if (orderFiles.XML_PROCESS_NAME.Equals("SP_POWEROFF"))
            {
                sohoUniLib.uniUtil.PCStateChage("SP_POWEROFF");
            }            
            else if (orderFiles.XML_PROCESS_NAME.Equals("SP_REDOWN"))
            {
                brodSchInfo("R");
            }else if (orderFiles.XML_PROCESS_NAME.Equals("SP_DIDENDTIME"))
            {
                //레지스트리 변경 후 
                fn_didEndtime();                    
            }
        }
        private void fn_didEndtime()
        {
            string returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe03), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);

            if (returnString.Length > 50)
            {

                if (playList01.Visible == true)
                {
                    playList01.Visible = false;
                }

                string[] brodInfo = sohoUniLib.ServerComm_json.jsonResult(returnString).Split('|');

                sohoUniLib.uniUtil.SetRegistry("agent_startTime", brodInfo[1].ToString());
                sohoUniLib.uniUtil.SetRegistry("agent_endTime", brodInfo[2].ToString());
                
                sohoUniLib.uniUtil.SetRegistry("brodCode", brodInfo[0].ToString());
                sohoUniLib.uniUtil.SetRegistry("centerId", brodInfo[3].ToString());

                didinfo.brodCode = brodInfo[0].ToString();
                didinfo.centerId = brodInfo[3].ToString();



                //명령 전송후 작업 
                if (!util.GetRegistry("MSG_SEQ").Equals("") && util.GetRegistry("XML_PROCESS_NAME").Equals("SP_DIDENDTIME") && util.GetRegistry("ORDER_SEND").Equals(""))
                {

                    didinfo.errorMessage = "OK";
                    didinfo.msgSeq = util.GetRegistry("MSG_SEQ");

                    string returnString1 = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe12), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);                    
                    if (sohoUniLib.ServerComm_json.jsonResult(returnString1).Equals("O"))
                    {
                        sohoUniLib.uniUtil.SetRegistry("ORDER_SEND", DateTime.Now.ToString("HH:mm:ss"));
                    }                    
                }
                
            }
        }
        /// <summary>
        /// 음원 관련 전문 처리 
        /// </summary>

        private void brodSoundPlayList()
        {

            //여기 부분 다시 변경 필요
            util.setLogFile("brodSoundPlayList 시작.");            
            
            try
            {
                if (  (Convert.ToInt32(DateTime.Now.ToString("HH:mm").Replace(":", "")) % 10) == 0                    
                && Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString()) >= Convert.ToInt32(util.GetRegistry("agent_startTime").ToString().Replace(":", "")+"00")
                && Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString()) <= Convert.ToInt32(util.GetRegistry("agent_endTime").ToString().Replace(":", "") + "00"))
                {

                    util.setLogFile("음원 편성표 생성");
                    waitFirstCheck = false;
                    playerDispos(Player);

                    if (util.fileEx(xml_path + agentConstInfo.xmlBrodFileName))
                    {
                        XDocument doc = XDocument.Load(xml_path + agentConstInfo.xmlBrodFileName);
                        var brodFiles = (from brodFile in doc.Descendants("brodList")
                                         where brodFile.Element("BROD_TIME").Value.Substring(0, 4) == DateTime.Now.ToString("HH:mm").Substring(0, 4)
                                         orderby brodFile.Element("BROD_TIME").Value ascending
                                         select new
                                         {
                                             BROD_TIME = brodFile.Element("BROD_TIME").Value,
                                             STRE_FILE_NM = brodFile.Element("STRE_FILE_NM").Value,
                                             FILE_INFO = "시간:" + brodFile.Element("BROD_TIME").Value + " 파일:" + brodFile.Element("STRE_FILE_NM").Value

                                         }).ToList();
                        if (brodFiles.Count > 0)
                        {
                            playList01.DataSource = null;
                            //편성표 생성 확인 
                            //시작이 00 이면 이 함수 호출 

                            playList01.DataSource = brodFiles;
                            playList01.DisplayMember = "BROD_TIME";
                            playList01.ValueMember = "FILE_INFO";
                            brodPlay();
                        }
                        else
                        {
                            playList01.Visible = false;
                            waitPlay();
                        }

                        if ( DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("110000") || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("133000"))
                        {
                            sendPlayInfo();
                        }
                        
                    }
                    else
                    {
                        util.setLogFile("음원 편성 파일이 없습니다");
                        lbl_agentInfo.Text = "음원 편성 파일이 없습니다.";
                        waitPlay();
                    }
                }
                else if (
                         Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString()) <= Convert.ToInt32(util.GetRegistry("agent_startTime").ToString().Replace(":", "") + "00")
                         && Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString()) >= Convert.ToInt32(util.GetRegistry("agent_endTime").ToString().Replace(":", "") + "00"))
                {
                    playList01.Visible = false;                    
                    waitPlay();
                }
                else if (playList01.Visible == false)
                {

                    if (waitFirstCheck == false)
                    {                    
                        waitPlay();
                    }

                }
                //업데이트 카운트 수정 
                if (DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("102500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("103500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("104500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("105500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("112500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("113500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("114500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("115500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("120500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("121500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("122500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("123500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("124500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("125530")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("130500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("131500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("132500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("133500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("134500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("135530")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("150500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("151500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("152500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("153500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("154500")
                    || DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString().Equals("155530")
                    )
                {
                    sendPlayInfo();
                }

                server_dateCheck();
            }
            catch (Exception ex)
            {
                util.setLogFile("brodSoundPlayList error:" + ex.ToString());
                playList01.Visible = false;
                waitPlay();
            }                    
        }
        private void brodPlay()
        {

            util.setLogFile("brodPlay 시작:");
            playList01.Visible = true;                 
            playList01.SelectedIndex = 0;            
            PlayFiles(listBoxFileConvert(playList01.SelectedItem.ToString()), mp3_path, "B");
                        
        }
        //최초 후 시간 기다리는 동안 처리 
        private void waitPlay()
        {

            waitFirstCheck = true;
            
            playerDispos(Player);


            //파일 리스트 임의로 축출 후 재생

            //string[] basicMp3 = util.dirFileLst(basic_mp3_path);
            ////시간때 확인 하기 
            //if (basicMp3.Length > 0)
            //{

            //    var rnd = new Random();
            //    string random_txt = basicMp3[rnd.Next(0, basicMp3.Length)].ToString();

            //    util.setLogFile("random_txt:" + random_txt + ":" + basicMp3.Length + ":" + pre_mp3PlayTxt);
            //    if (random_txt.Equals(pre_mp3PlayTxt))
            //        random_txt = basicMp3[rnd.Next(0, basicMp3.Length)].ToString();

            //    if (basicMp3.Length > 0)
            //    {
            //        //랜덤 관련 해서 재생 카운터 넣기 
            //        PlayFiles(random_txt, basic_mp3_path);
            //    }
            //    pre_mp3PlayTxt = random_txt;
            //}
            if (pre_mp3PlayTxt == null) pre_mp3PlayTxt = "0";
            //기초 음원 생성 만들기
            try
            {

                util.fileCreate(xml_path, agentConstInfo.xmlBasicBrodList_New, agentConstInfo.xmlText);

                string fileInfo = utilXml.basicPlayList(xml_path + agentConstInfo.xmlBasicBrodList_New, pre_mp3PlayTxt);
                if (fileInfo.Length > 0)
                {

                    string[] file_Info = fileInfo.Split('|');
                    pre_mp3PlayTxt = file_Info[1].ToString();
                    PlayFiles(file_Info[0].ToString(), basic_mp3_path, "G");
                }
            }
            catch(Exception e)
            {
                util.setLogFile("waitPlay error:" + e.ToString());
                //PlayFiles("emart_basic.mp3", System.Environment.CurrentDirectory, "E");
                PlayFiles("emart_basic.mp3", default_path, "E");
            }

            

            
             
        }
        private void playerDispos(WindowsMediaPlayer disPlayer)
        {

            if (disPlayer != null)
            {
                
                disPlayer.close();
                disPlayer = null;                
            }
            else
            {
                util.setLogFile("disPlayer 없음");
            }
            
        }

        //파일 변환
        private string listBoxFileConvert(string _itemValue)
        {
            string fileNm = _itemValue.Substring((playList01.SelectedItem.ToString().IndexOf(",") + 1), playList01.SelectedItem.ToString().LastIndexOf(",") - 1);
            fileNm = fileNm.Substring(0, fileNm.LastIndexOf(","));
            fileNm = fileNm.Substring(playList01.SelectedItem.ToString().IndexOf("=") + 3).Trim();
            return fileNm;
        }
        private void PlayFiles(string _url, string _basic_path, string _basicGubun)
        {
            util.setLogFile("음원 재생 :" + _url);
            Player = new WMPLib.WindowsMediaPlayer();

            Player.PlayStateChange += Player_PlayStateChange;
            Player.MediaError += Player_MediaError;
            Player.URL = _basic_path + _url;
            int sound_volume = util.GetRegistry("sound_volume").Equals("") ? 100 : Convert.ToInt32(util.GetRegistry("sound_volume"));
            Player.settings.volume = sound_volume; 
            Player.controls.play();
            util.setLogFile("음원 Play");

            //송출곡 확인 하기 09시 부터 11:59 까지 
            if (_basicGubun.Equals("G") && (
                    Convert.ToInt32(  DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString() )  >  85940 &&
                    Convert.ToInt32(DateTime.Now.ToString("HH:mm:ss").Replace(":", "").ToString()) < 235940
                   )
                )
            {
                utilXml.BasicPlayInfo(send_path, _url, agentConstInfo.xmlPlayText);
            }


        }

        private void Player_MediaError(object pMediaObject)
        {
            util.setLogFile("음원 Player_MediaError:" + pMediaObject.ToString());
            throw new NotImplementedException();
        }

        private void Player_PlayStateChange(int NewState)
        {

            
            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {

                Player.close();
                Player = null;
                
                if (playList01.SelectedIndex == playList01.Items.Count - 1)
                {
                    waitPlay();
                }
                else if ( playList01.Visible == false && (Convert.ToInt32(DateTime.Now.ToString("HH:mm").Replace(":", "")) % 10) != 0 && waitFirstCheck == true)
                {
                    waitPlay();
                }
                else if (waitFirstCheck != true && playList01.Visible == true && playList01.SelectedIndex <= playList01.Items.Count - 1)
                {
                    playList01.SelectedIndex = playList01.SelectedIndex + 1;
                    PlayFiles(listBoxFileConvert(playList01.SelectedItem.ToString()), mp3_path, "B");

                }
                else
                {
                    util.setLogFile("Player_PlayStateChange 조건값이 없을때");
                    waitPlay();
                }
            }
        }

        private void brodSchInfo(string _ReCheck)
        {            
            

            try
            {
                if (util.directory_del(mp3_path + "\\") == true)
                {
                    //여기 부분 어떻게 할지 생각  
                    //mp3를 받고 난 이후 이동 한다음 플레이 할지 안할지 
                    
                }

                sendTimer.Stop();
                string returnString = string.Empty;
                if (_ReCheck.Equals("S"))
                {
                    returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe03), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);
                }
                else
                {
                    returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe15), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);
                }
                
                if (returnString.Length > 50)
                {

                    if (playList01.Visible == true)
                    {
                        playList01.Visible = false;
                    }

                    string[] brodInfo = sohoUniLib.ServerComm_json.jsonResult(returnString).Split('|');

                    sohoUniLib.uniUtil.SetRegistry("agent_startTime", brodInfo[1].ToString());
                    sohoUniLib.uniUtil.SetRegistry("agent_endTime", brodInfo[2].ToString());
                    sohoUniLib.uniUtil.SetRegistry("brodCode", brodInfo[0].ToString());
                    sohoUniLib.uniUtil.SetRegistry("centerId", brodInfo[3].ToString());

                    didinfo.brodCode = brodInfo[0].ToString();
                    didinfo.centerId = brodInfo[3].ToString();

                    //xml생성 
                    sohoUniLib.uniUtil.SetRegistry("download", "F");

                    lbl_agentInfo.Text = "스케줄 확인.";
                    if (backgroundWorker.IsBusy != true)
                    {
                        toOpen();
                        backgroundWorker.RunWorkerAsync();
                    }
                    else
                    {
                        //Console.WriteLine("backgroundWorker IsBusy");
                        util.setLogFile("brodSchInfo backgroundWorker IsBusy");
                    }                    
                }
                else
                {
                    util.setLogFile("전문 전송 에러 다시 전문 요청");
                    sohoUniLib.uniUtil.Delay(30);
                    brodSchInfo(_ReCheck);
                }
                
            }
            catch(Exception e)
            {

                util.setLogFile("brodSchInfo error:" + e.ToString());
                lbl_agentInfo.Text = "스케줄 에러:" + e.ToString();
            }

            

        }        
        private void agent_setting()
        {
            gbSettingInfo.Visible = true;
            gbInfo.Visible = false;
            chk_RegStart.Visible = false;

            Dictionary<string, string> macIntercase = sohoUniLib.NetWorkClass.ShowNetworkInterFace();

            cbo_Mac.DataSource = new BindingSource(macIntercase, null);
            cbo_Mac.DisplayMember = "Key";
            cbo_Mac.ValueMember = "Value";
            cbo_Mac.SelectedItem = 0;
            if ( macIntercase.ContainsKey("Ethernet"))
            {                
                cbo_Mac.SelectedValue = macIntercase["Ethernet"];
            }
            // 프로그림 중복 실행 방지            
        }
        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                util.setLogFile("네트워크 정상 ");
            }
            else
            {
                util.setLogFile("네트워크 장애 발생");
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            //정보 변경 관련 내용 입력 
            if (btn_Save.Text.Equals("정보변경"))
            {
                //기본콤보 먼저 정의 
                agent_setting();
                gbSettingInfo.Visible = true;
                gbInfo.Visible = false;
                gbSettingInfo.BringToFront();
                sendTimer.Stop();
                //정보 변경 정보

                txt_ServerIp.Text = util.GetRegistry("server_url");
                txtAgetntId.Text = util.GetRegistry("did_id");                                
                cbo_Mac.SelectedValue = util.GetRegistry("license_info");
                //체크박스 
                chk_RegStart.Checked = util.GetRegistry("agent_info_start").Equals("Y") ? true : false;
                btn_Save.Text = "저장";

            }
            else
            {
                saveReg();
            }
            
        }
        private void saveReg()
        {

            try
            {
                if (util.txtEmpty_Check(txt_ServerIp, agentConstInfo.auth_ErrorMessage1) == false) { return; }
                if (util.txtEmpty_Check(txtAgetntId, agentConstInfo.auth_ErrorMessage2) == false) { return; }
                if (util.cboEmpty_Check(cbo_Mac, agentConstInfo.auth_ErrorMessage5) == false) { return; }

                didinfo.didId = txtAgetntId.Text;
                didinfo.didIpAddress = sohoUniLib.NetWorkClass.Client_IP;
                didinfo.didInterval = "1";
                didinfo.didMac = cbo_Mac.SelectedValue.ToString().Trim();
                didinfo.didType = agentConstInfo.tel_Const01;

                //사이트 접속 확인 
                if (sohoUniLib.NetWorkClass.UrlConnectionValidation(txt_ServerIp.Text) == false)
                {
                    MessageBox.Show("등록하신 서버 주소로 접속 하실수 없습니다. 확인하시고 다시 연결 바랍니다.");
                    return;
                }

                string returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe02), txt_ServerIp.Text + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);

                if (sohoUniLib.ServerComm_json.jsonResult(returnString).Equals("OK"))
                {

                    util.SetStartUp(chk_RegStart.Checked ? true : false);


                    MessageBox.Show("정상적으로 등록 되었습니다.");

                    //레지스트리 저장 
                    sohoUniLib.uniUtil.SetRegistry("license_info", didinfo.didMac);
                    sohoUniLib.uniUtil.SetRegistry("did_id", didinfo.didId);
                    sohoUniLib.uniUtil.SetRegistry("did_Interval", didinfo.didInterval);
                    sohoUniLib.uniUtil.SetRegistry("server_url", txt_ServerIp.Text);
                    sohoUniLib.uniUtil.SetRegistry("sound_volume", Convert.ToString(trackBar_info.Value));

                    gbSettingInfo.Visible = false;
                    //toTray();
                    // 서버와 시간 동기화 
                    server_date();
                    //mp3파일 생성
                    util.fileCreate(xml_path, agentConstInfo.xmlFileName, agentConstInfo.xmlText);
                    //음원 방송 편성표
                    util.fileCreate(xml_path, agentConstInfo.xmlBrodFileName, agentConstInfo.xmlText);
                    //음원 방송 시작
                    util.fileCreate(xml_path, agentConstInfo.xmlBrodBasicName, agentConstInfo.xmlText);
                    //명령어 리스트 
                    util.fileCreate(xml_path, agentConstInfo.xmlBrodOrderList, agentConstInfo.xmlText);
                    //기초음원 파일 리스트
                    util.fileCreate(xml_path, agentConstInfo.xmlBasicFileList, agentConstInfo.xmlText);
                    //기초편성표 파일 리스트
                    util.fileCreate(xml_path, agentConstInfo.xmlBasicBrodList_New, agentConstInfo.xmlText);
                    

                    //no sound 로 정보 변경 
                    util.program_start(System.Environment.CurrentDirectory, "noSound.bat");

                    btn_Save.Text = "정보변경";
                    brodAgentStart();
                }
                else
                {
                    MessageBox.Show("등록중 문제가 발생 하였습니다.");
                }
            }
            catch(Exception ex)
            {
                util.setLogFile("saveReg error:" + ex.ToString());
                MessageBox.Show("등록중 문제가 발생 하였습니다.");
            }
            
        }
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            program_exit();
        }        

        private void server_date()
        {
            try
            {     
                //서버 시간 알아오기 
                string returnString = util.webString( util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl02);                
                NetWorkClass netWork = new NetWorkClass();                
                netWork.Time_Sync(Convert.ToDateTime(returnString));
            }
            catch(Exception e)
            {
                util.setLogFile("server_date error:" + e.ToString());
            }
            
        }
        
        private void SMItem01_Click(object sender, EventArgs e)
        {
            toOpen();
        }
        private void SMItem02_Click(object sender, EventArgs e)
        {            
            program_exit();
        }
        private void toTray()
        {
            this.WindowState = FormWindowState.Minimized;            
            this.ShowInTaskbar = false;            
            this.Visible = false;            
            this.notCon.Visible = true;

        }
        private void toOpen()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.notCon.Visible = false;
        }
        private void program_exit()
        {
            notCon.Icon = null;
            notCon.Visible = false;            
            notCon.Dispose();

            System.Diagnostics.Process[] mPrmcess = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
            foreach (System.Diagnostics.Process p in mPrmcess)
            {
                p.Kill();
            }
            Application.Exit();
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                
                PictureBox picBox = new PictureBox();
                this.BeginInvoke(new Action(() =>
                {                    
                    picBox.Parent = pnl_loading;
                    picBox.Location = new Point(65, 3);
                    picBox.Size = new Size(127, 127);
                    picBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    picBox.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\img\\loading.gif");
                }));
                string brodMessage = null;
                DataTable dt = null;


              
                if (util.GetRegistry("download").Equals("B"))
                {
                    
                    

                    //brodMessage = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe16), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);
                    brodMessage = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe19), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);
                    utilXml.delNodeAll(xml_path + agentConstInfo.xmlBasicFileList, "fileList");                    
                }
                else
                {
                    brodMessage = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe04), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);                    
                    utilXml.delNodeAll(xml_path + agentConstInfo.xmlFileName, "fileList");                    
                }
                dt = sohoUniLib.ServerComm_json.jsonArrayList(brodMessage , "FILEINFO");
                DataRow[] properIDs = dt.Select("1=1", "");

                //Console.WriteLine(util.GetRegistry("download").ToString());

                string downloadText = "전체파일:[" + Convert.ToString(properIDs.Length) + "]";
                string downloadNow = string.Empty;
                for (int i = 0; i < properIDs.Length; i++)
                {
                    DataRow temp = properIDs[i];

                    if (util.GetRegistry("download").Equals("B"))
                    {


                        if (util.GetRegistry("centerId").ToString() == "")
                        {
                            sohoUniLib.uniUtil.SetRegistry("centerId", util.GetRegistry("did_id").Substring(0, (util.GetRegistry("did_id").ToString().Length - 3)));
                        }

                        if (utilXml.xmlCreateNodeMusic(xml_path + agentConstInfo.xmlBasicFileList, temp["FILESTRECOURS"].ToString(), temp["STREFILENM"].ToString()) == true)
                        //if (utilXml.xmlCreateNodeBasicMusic(xml_path + agentConstInfo.xmlBasicFileList, temp["FILESTRECOURS"].ToString(), temp["STREFILENM"].ToString()
                        //                                   , temp["BROD_STARTTIME"].ToString(), temp["BROD_ENDTIME"].ToString(), temp["GROUP_TIMEGUBUN"].ToString()) == true)
                        {
                            //파일 다운로드 

                            if (sohoUniLib.uniUtil.webFileDownload(util.GetRegistry("server_url") + "/upload/" + temp["FILESTRECOURS"].ToString() + temp["STREFILENM"].ToString(), temp["STREFILENM"].ToString(), basic_mp3_path) == true)
                            {
                                utilXml.xmlNodeUpdate(xml_path + agentConstInfo.xmlBasicFileList, temp["STREFILENM"].ToString(), "Y");
                                downloadNow = "/현재 다운로드" + i + "개";
                            }
                            else
                            {
                                utilXml.xmlNodeUpdate(xml_path + agentConstInfo.xmlBasicFileList, temp["STREFILENM"].ToString(), "N");
                            }

                        }
                    }
                    else
                    {
                        if (utilXml.xmlCreateNodeMusic(xml_path + agentConstInfo.xmlFileName, temp["FILESTRECOURS"].ToString(), temp["STREFILENM"].ToString()) == true)
                        {
                            //파일 다운로드 
                            if (sohoUniLib.uniUtil.webFileDownload(util.GetRegistry("server_url") + "/upload/" + temp["FILESTRECOURS"].ToString() + temp["STREFILENM"].ToString(), temp["STREFILENM"].ToString(), mp3_path) == true)
                            {
                                utilXml.xmlNodeUpdate(xml_path + agentConstInfo.xmlFileName, temp["STREFILENM"].ToString(), "Y");
                                downloadNow = "/현재 다운로드" + i + "개";
                            }
                            else
                            {
                                utilXml.xmlNodeUpdate(xml_path + agentConstInfo.xmlFileName, temp["STREFILENM"].ToString(), "N");
                            }
                        }
                    }
                    //if (utilXml.xmlCreateNodeMusic(xml_path + agentConstInfo.xmlFileName, temp["FILESTRECOURS"].ToString(), temp["STREFILENM"].ToString()) == true)
                    //{
                    //    //파일 다운로드 
                    //    if (sohoUniLib.uniUtil.webFileDownload(util.GetRegistry("server_url") + "/upload/" + temp["FILESTRECOURS"].ToString() + temp["STREFILENM"].ToString(), temp["STREFILENM"].ToString(), mp3_path) == true)
                    //    {
                    //        utilXml.xmlNodeUpdate(xml_path + agentConstInfo.xmlFileName, temp["STREFILENM"].ToString(), "Y");
                    //        downloadNow = "/현재 다운로드" + i + "개";
                    //    }
                    //    else
                    //    {
                    //        utilXml.xmlNodeUpdate(xml_path + agentConstInfo.xmlFileName, temp["STREFILENM"].ToString(), "N");
                    //    }
                    //}

                    this.BeginInvoke(new Action(() =>
                    {
                        lbl_agentInfo.Text = downloadText + downloadNow;
                    }));
                }

                this.BeginInvoke(new Action(() =>
                {
                    picBox.Dispose();
                }));
                
                didinfo.didDownCheck = "Y";
                //업데이트 이후 플레이 시작                 
                dt.Dispose();                
                
                
            }
            catch (Exception ex)
            {
                waitPlay();
                util.setLogFile("backgroundWorker_DoWork error:" + ex.ToString());
                didinfo.didDownCheck = "N";                
                sohoUniLib.uniUtil.Delay(30);
                brodSchInfo("R");

            }
            
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //리포트 받기    
            //음원 파일 생성 
            string brodReport = null;
            if (util.GetRegistry("download").Equals("B"))
            {
                if (e.Cancelled == true)
                {
                    lbl_agentInfo.Text = "기초 음원 다운로드 취소.";
                }
                else if (e.Error != null)
                {
                    lbl_agentInfo.Text = "기초 파일 다운로드 애러: " + e.Error.Message;
                }
                else
                {                    
                    if (util.GetRegistry("centerId").ToString() == "")
                    {
                        sohoUniLib.uniUtil.SetRegistry("centerId", util.GetRegistry("did_id").Substring(0, (util.GetRegistry("did_id").ToString().Length - 3)));
                    }

                    didinfo.centerId= util.GetRegistry("centerId").ToString();
                    //변경부분 
                    string returnMessage = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe18), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);
                    //다운로드 완료

                    JObject json = JObject.Parse(returnMessage);
                    string _returlMessage = json["result"].ToString();
                    if (_returlMessage.Equals("O"))
                    {
                        // 기초 음원 편성표 삭제
                        utilXml.delNodeAll(xml_path + agentConstInfo.xmlBasicBrodList_New, "fileList");
                        //기초음원 편성표 생성 
                        DataTable dt = sohoUniLib.ServerComm_json.jsonArrayList(returnMessage, "FILEINFO");
                        DataRow[] properIDs = dt.Select("1=1", "");
                        for (int i = 0; i < properIDs.Length; i++)
                        {
                            DataRow temp = properIDs[i];
                            if (utilXml.xmlCreateNodeBasicMusic(xml_path + agentConstInfo.xmlBasicBrodList_New,   temp["STREFILENM"].ToString(), temp["BROD_STARTTIME"].ToString(), temp["BROD_ENDTIME"].ToString(), temp["GROUP_TIMEGUBUN"].ToString(), Convert.ToString(i), temp["BASIC_CODE"].ToString()) == true)
                            {

                            }
                            //편성표 생성 
                        }
                    }
                    //returnMessage = sohoUniLib.ServerComm_json.jsonResult(returnMessage);



                    lbl_agentInfo.Text = _returlMessage.Equals("O") ? "기초  음원 다운로드 완료." : "서버 전송중 문제 발생.";
                    
                }
            }
            else
            {
                brodReport = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe05), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);
                utilXml.delNodeAll(xml_path + agentConstInfo.xmlBrodFileName, "brodList");

                DataTable dt = sohoUniLib.ServerComm_json.jsonArrayList(brodReport, "REPORTINFO");
                DataRow[] properIDs = dt.Select("1=1", "");
                for (int i = 0; i < properIDs.Length; i++)
                {
                    DataRow temp = properIDs[i];
                    if (utilXml.xmlReportCreateNode(xml_path + agentConstInfo.xmlBrodFileName, temp["BROD_TIME"].ToString(), temp["STRE_FILE_NM"].ToString(), temp["BROD_SEQ"].ToString()) != true)
                    {
                        util.setLogFile("노드 에러 생성:" + temp["BROD_SEQ"].ToString());
                        //Console.Write("brodXml Create error:" + temp["BROD_SEQ"].ToString());
                    }
                }

                if (e.Cancelled == true)
                {
                    lbl_agentInfo.Text = "음원 다운로드 취소.";
                }
                else if (e.Error != null)
                {
                    lbl_agentInfo.Text = "파일 다운로드 애러: " + e.Error.Message;
                }
                else
                {
                    string returnMessage = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, agentConstInfo.xmlMessageTyepe06), util.GetRegistry("server_url") + agentConstInfo.serverJsonUtrl01, agentConstInfo.contentType_01);
                    returnMessage = sohoUniLib.ServerComm_json.jsonResult(returnMessage);
                    lbl_agentInfo.Text = returnMessage.Equals("O") ? "음원 다운로드 완료." : "서버 전송중 문제 발생.";


                }
                dt.Dispose();
            }
            
            
            
            sendTimer.Start();
            sohoUniLib.uniUtil.SetRegistry("download", "");
            util.program_start(System.Environment.CurrentDirectory, "program_restart.bat");            
            //toTray();


        }

        private void emart_agent_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void notCon_Click(object sender, EventArgs e)
        {
            this.toOpen();
        }

        private void notCon_DoubleClick(object sender, EventArgs e)
        {
            this.toOpen();
        }

        private void txt_Volume_KeyPress(object sender, KeyPressEventArgs e)
        {

            if(!(char.IsDigit(e.KeyChar)  || e.KeyChar == Convert.ToChar(Keys.Back) ))
            {
                e.Handled = true;
            }
            
        }
        

        private void trackBar_info_Scroll(object sender, EventArgs e)
        {
            util.setLogFile("볼륨 변경" + trackBar_info.Value);
            sohoUniLib.uniUtil.SetRegistry("sound_volume", Convert.ToString( trackBar_info.Value));
            
            Player.settings.volume = trackBar_info.Value;
        }
    }
    
}
