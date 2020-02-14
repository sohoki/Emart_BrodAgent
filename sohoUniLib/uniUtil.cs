using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Drawing;
using System.Media;

namespace sohoUniLib
{


    public delegate void StrIPAddressHandler(string str);



    public class uniUtil
    {
        private const int WmSyscommand = 0x0112;
        private const int ScMonitorpower = 0xF170;
        private const int HwndBroadcast = 0xFFFF;
        /// <summary>
        /// 모니터 슬립 문서
        /// </summary>
        private const int ShutOffDisplay = 2;
        private const int ShutONDisplay = -1;
        private const int ShutSLEEPDisplay = 1;

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, int Size, String filePat);

        [DllImport("Kernel32.dll")]
        private static extern long WritePrivateProfileString(String Section, String Key, String val, String filePath);

        //logoff 
        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        //Lock station 
        [DllImport("user32")]
        public static extern void LockWorkStation();

        //Hibernate (최대 절전 모드) or sleep
        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        // remote lgoin
        [DllImport("advapi32.dll")]
        public static extern bool LogonUser(string name, string domain, string pass, int logType, int logpv, out IntPtr pht);

        [DllImport("ADVAPI32.DLL")]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, out IntPtr DuplicateTokenHandle);

        [DllImport("kernel32.DLL")]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool BlockInput([In, MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

        //작업 표시줄 숨기기 표시하기 
        public const int SWP_HIDEWINDOW = 128;
        public const int SWP_SHOWWINDOW = 64;

        [DllImport("user32.dll")]
        public static extern int FindWindowA(string IpClassName, string IpWindowName);

        [DllImport("user32.dll")]
        public static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);


        //사운드 조절 
        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);





        public static event StrIPAddressHandler CLIP;
        /// <summary>
        /// 현재 디렉토리 찾기
        /// </summary>
        /// <returns></returns>
        public string returnPath()
        {
            string foler = null;
            return foler = Environment.CurrentDirectory;
        }
        /// <summary>
        /// 시간 delay
        /// </summary>
        /// <param name="MS">초</param>
        /// <returns></returns>
        public static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }
        /// <summary>
        /// IP 정규식 확인
        /// </summary>
        /// <param name="_ip"></param>
        /// <returns></returns>
        public bool ISIPv4 (string _ip)
        {
            var quads = _ip.Split('.');

            // if we do not have 4 quads, return false
            if (!(quads.Length == 4)) return false;

            // for each quad
            foreach (var quad in quads)
            {
                int q;
                if (!Int32.TryParse(quad, out q)
                    || !q.ToString().Length.Equals(quad.Length)
                    || q < 0
                    || q > 255) { return false; }

            }
            return true;

        }
        /// <summary>
        /// ini환경 파일 쓰기
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="avaPath"></param>
        public void IniWriteValue(String Section, String Key, String Value, string avaPath)
        {
            WritePrivateProfileString(Section, Key, Value, avaPath);
        }
        /// <summary>
        /// ini 환경 파일 읽기
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="avsPath"></param>
        /// <returns></returns>
        public String IniReadValue(String Section, String Key, string avsPath)
        {

            StringBuilder temp = new StringBuilder(255);
            try
            {
                int i = GetPrivateProfileString(Section, Key, "not_found", temp, 255, avsPath);
            }
            catch (Exception ex)
            {
                setLogFile(ex.ToString());
            }
            return temp.ToString();

        }
        //폼 열려 있는지 확인 
        public static Form GetForm(string _formNm)
        {
            foreach (Form frm in Application.OpenForms)
                if (frm.Name == _formNm)
                    return frm;
            return null;                        
        }
        // 현 실행 프로그램 이름 알아오기
        public string programNm()
        {
            return System.AppDomain.CurrentDomain.FriendlyName.ToString();
        }
        //form full 화면 
        public void form_fullView(Form frm, bool _topMost, int _screenWidth, int _screenHegith)
        {
            try
            {
                if ((frm.FormBorderStyle == FormBorderStyle.None) && (frm.WindowState == FormWindowState.Maximized))
                {
                    //frm.TopMost = true; 이 부분은 화면 전체를 맨 앞으로 가지고 오게 하기
                    frm.FormBorderStyle = FormBorderStyle.None;

                    if (GetRegistry("Toolbar_Status").Equals("0"))
                    {
                        frm.MaximumSize = new Size(_screenWidth, _screenHegith - 30);
                    }
                    frm.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    frm.WindowState = FormWindowState.Normal;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    if (GetRegistry("Toolbar_Status").Equals("0"))
                    {
                        frm.MaximumSize = new Size(_screenWidth, _screenHegith - 30);
                    }
                    frm.WindowState = FormWindowState.Maximized;
                }
                if (_topMost.Equals(true))
                {
                    frm.TopMost = true;
                }
                else
                {
                    frm.TopMost = false;
                }
            }
            catch (Exception e)
            {
                setLogFile("from_MaxView error:" + e.ToString());
            }
        }
        //작업 표시줄 true 표시 false 숨기기
        public static void taskBarControl(bool _task)
        {   
            foreach(var screen in Screen.AllScreens)
            {
                if (_task == true)
                {
                    int IRet;
                    
                    IRet = FindWindowA(screen.DeviceName, "");

                    if (IRet > 0)
                    {
                        IRet = SetWindowPos(IRet, 0, 0, 0, 0, 0, SWP_SHOWWINDOW);
                    }
                }
                else
                {
                    int IRet;
                    IRet = FindWindowA(screen.DeviceName, "");

                    if (IRet > 0)
                    {
                        IRet = SetWindowPos(IRet, 0, 0, 0, 0, 0, SWP_HIDEWINDOW);
                    }
                }

            }

                
        }
        /// <summary>
        /// 웹서버 파일 쓰기 
        /// </summary>
        /// <param name="_rfid"></param>
        /// <param name="_path"></param>
        /// <param name="_fileNm"></param>
        /// <param name="_delGubun"></param>
        public void setDataWeb( string _path, string _url, string _fileNm)
        {            
            WebClient wc = new WebClient();
            wc.UploadFileAsync(new Uri(_url), "POST", _path + _fileNm);
        }
        public void setData(string _rfid, string _path, string _fileNm, bool _delGubun)
        {

            string strDir = Path.GetDirectoryName(string.Format("{0}{1}", _path, _fileNm));
            DirectoryInfo diDir = new DirectoryInfo(strDir);
            if (!diDir.Exists)
            {
                diDir.Create();
                diDir = new DirectoryInfo(strDir);
            }
            if (diDir.Exists)
            {

                if (_delGubun == true)
                {
                    string text = System.IO.File.ReadAllText(_path + _fileNm);
                    if (text.Length > 5)
                    {

                        string[] rfidSplits = text.Substring(1).Split(',');
                        bool fileWrite = true;
                        if (rfidSplits.Length > 0)
                        {                            
                            if (fileWrite == true)
                            {
                                System.IO.StreamWriter swStream = File.AppendText(string.Format("{0}{1}", _path, _fileNm));
                                swStream.WriteLine(_rfid);
                                swStream.Close();
                                setLogFileS("_rfidInfo:" + _rfid);
                            }
                            //삭제 하기 
                            //Console.WriteLine("_rfid:" + _rfid);
                        }
                    }
                    else
                    {
                        setLogFileS("text 없음");
                        System.IO.StreamWriter swStream = File.AppendText(string.Format("{0}{1}", _path, _fileNm));
                        swStream.WriteLine("," + _rfid);
                        swStream.Close();
                    }
                }
                else
                {
                    File.WriteAllText(_path + _fileNm, String.Empty);
                }

            }


        }
        //특정 프로그램 실행  (프로그램 재시작 등등)
        public void program_start(string _programe_path, string _programeNm)
        {
            System.Diagnostics.ProcessStartInfo proInfo = new ProcessStartInfo();
            System.Diagnostics.Process ps = new Process();
            setLogFile("_programe_path:" + _programe_path);

            ps.StartInfo.FileName = _programeNm;
            ps.StartInfo.WorkingDirectory = _programe_path;
            ps.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

            
            ps.Start();
            ps.WaitForExit(100);

        }
        public bool directory_del(string _paht)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_paht);
                if (dir.Exists)
                {
                    System.IO.FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);

                    foreach (System.IO.FileInfo file in files)
                        file.Attributes = FileAttributes.Normal;

                    Directory.Delete(_paht, true);
                }                
                return true;
            }
            
            catch (Exception e)
            {
                setLogFile("directory_del error:" + e.ToString());
                return false;
            }

            
        }
        public static bool fileMove(string _exitFilePath, string _newFilePath)
        {
            bool fileCheck = false;
            try
            {
                if (!File.Exists(_exitFilePath))
                {
                    fileCheck = false;
                    setLogFileS("fileMove error: 파일이 존재 하지 않습니다.");
                    return fileCheck;
                }

                // Ensure that the target does not exist.
                if (File.Exists(_newFilePath))
                    File.Delete(_newFilePath);

                // Move the file.
                File.Move(_exitFilePath, _newFilePath);
                
                // See if the original exists now.
                if (File.Exists(_exitFilePath))
                {
                    fileCheck = false;
                    setLogFileS("fileMove error: 파일이 삭제시 문제가 발생 하였습니다.");
                    return fileCheck;
                }
                fileCheck = true;
            }
            catch (Exception e)
            {
                setLogFileS("fileMove error: "+ e.ToString());
            }
            return fileCheck;
        }
        public static bool directory_dels(string _paht)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_paht);
                if (dir.Exists)
                {
                    System.IO.FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);

                    foreach (System.IO.FileInfo file in files)
                    {
                        file.Attributes = FileAttributes.Normal;
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                        
                        
                        


                    //Directory.Delete(_paht, true);
                }
                return true;
            }

            catch (Exception e)
            {
                setLogFileS("directory_del error:" + e.ToString());
                return false;
            }


        }
        //사용자 계정 알아 오기
        public static string user_Info()
        {
            return WindowsIdentity.GetCurrent().ToString(); ;
        }
        public static WindowsPrincipal user_isRole()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent());
        }
        public string fileExit(string _fileFullName, string _split)
        {
            int index = _fileFullName.LastIndexOf(_split);
            return _fileFullName.Substring(index + 1);
        }
        public bool fileEx (string _fileNm)
        {
            return File.Exists(_fileNm) ? true : false;

        }
        //파일 삭제
        public bool fileDel(string _fileNm)
        {
            try
            {
                File.Delete(_fileNm);
                return true;
                    
            }
            catch(Exception ex)
            {
                setLogFile("fileDel:" + ex.ToString());
                return false;
            }
            
        }
        public bool mkdir(string _path)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                    dirInfo = new DirectoryInfo(_path);
                }                
                return true;
            }
            catch (Exception e)
            {
                setLogFile("mkdir error:" + e.ToString());
                return false;
            }
        }
        public bool fileCreate (string _path, string _fileNm, string _fileTxt)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                    dirInfo = new DirectoryInfo(_path);
                }
                if (!File.Exists(_path + "\\" + _fileNm) )
                { 
                    File.WriteAllText(_path + "\\" + _fileNm, _fileTxt);
                }
                return true;
            }
            catch(Exception e)
            {
                setLogFile("fileCreate error:" + e.ToString());
                return false;
            }
            
        }
        //디렉토리 파일 갯수 구하기
        //나중에 간단하게 수정 하기 
        public int getFileCnt (string _path)
        {
            int nReturn = 0;
            try
            {
                Console.WriteLine(_path);
                if (Directory.Exists(_path))
                {
                    string[] pszfileEntries = Directory.GetFiles( _path);                    
                    foreach (string szfileName in pszfileEntries)
                    {   
                        nReturn = nReturn + 1;
                    }
                }                
            }
            catch(Exception e)
            {
                setLogFile("getFileCnt Error:" + e.ToString());
            }
            return nReturn;


        }
        public void setLogFile(string _str)
        {
            try
            {                
                string m_strLogPrefix = System.Environment.CurrentDirectory + "\\log\\";  //mssqlExecuteField("select user_value from  tb_telephone where user_field = 'kiosk_log'");
                string m_strLogExt = @".LOG";
                DateTime dtNow = DateTime.Now;
                string strDate = dtNow.ToString("yyyy-MM-dd");
                string strPath = string.Format("{0}{1}{2}", m_strLogPrefix, strDate, m_strLogExt);
                string strDir = Path.GetDirectoryName(strPath);

                DirectoryInfo diDir = new DirectoryInfo(strDir);

                if (!diDir.Exists)
                {
                    diDir.Create();
                    diDir = new DirectoryInfo(strDir);
                }
                if (diDir.Exists)
                {
                    System.IO.StreamWriter swStream = File.AppendText(strPath);
                    string strLog = string.Format("{0}:{1}", dtNow.ToString(dtNow.Hour + "시" + dtNow.Minute + "분" + dtNow.Second + "초"), _str);
                    swStream.WriteLine(strLog);
                    swStream.Close();
                }

            }
            catch (System.Exception e)
            {
                setLogFile(e.ToString());
            }

        }
        public static string Client_IP
        {
            get
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string ClientIP = string.Empty;
                for (int i = 0; i < host.AddressList.Length; i++)
                {
                    if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ClientIP = host.AddressList[i].ToString();
                    }
                }
                return ClientIP;
            }

        }
        public static void IPSEND()
        {
            CLIP(Client_IP);
        }


        public static void setLogFileS(string _str)
        {
            try
            {



                string m_strLogPrefix = System.Environment.CurrentDirectory + "\\log\\"; //mssqlExecuteField("select user_value from  tb_telephone where user_field = 'kiosk_log'");
                string m_strLogExt = @".LOG";
                DateTime dtNow = DateTime.Now;
                string strDate = dtNow.ToString("yyyy-MM-dd");
                string strPath = string.Format("{0}{1}{2}", m_strLogPrefix, strDate, m_strLogExt);
                string strDir = Path.GetDirectoryName(strPath);

                DirectoryInfo diDir = new DirectoryInfo(strDir);

                if (!diDir.Exists)
                {
                    diDir.Create();
                    diDir = new DirectoryInfo(strDir);
                }
                if (diDir.Exists)
                {
                    System.IO.StreamWriter swStream = File.AppendText(strPath);
                    string strLog = string.Format("{0}:{1}", dtNow.ToString(dtNow.Hour + "시" + dtNow.Minute + "분" + dtNow.Second + "초"), _str);
                    swStream.WriteLine(strLog);
                    swStream.Close();
                }

            }
            catch (System.Exception e)
            {
                //Debug.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
            }

        }
        //시작시 자동 실행 
        public void SetStartUp(bool _check)
        {
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            
            if (_check == true)
            {
                SetRegistry("agent_info_start", "Y");
                rkey.SetValue("Agent", System.Windows.Forms.Application.ExecutablePath.ToString());
            }
            else
            {
                SetRegistry("agent_info_start", "N");
                rkey.SetValue("Agent", false);
            }            
        }
        #region  byte to string 
        public static string ByteToString(byte[] strByte)
        {
            string str = Encoding.Default.GetString(strByte);
            return str;
        }
        // String을 바이트 배열로 변환 
        public static byte[] StringToByte(string str)
        {
            byte[] StrByte = Encoding.UTF8.GetBytes(str);
            return StrByte;
        }
        #endregion 

        public static void SetRegistry(string _key, string _value)
        {
            //String을 암호화 한다.
            _value = EncryptString(_value);
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE");
            rk.CreateSubKey("agent_info").SetValue(_key, _value);
            rk.Close();
        }
        //레지스트리 삭제
        public static bool DetRegistry(string _key)
        {
            try
            {
                Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\" + _key);
                return true;
            }
            catch(Exception e)
            {
                setLogFileS("DetRegistry error" + e.ToString());
                return false;
            }
            

        }
        public bool txtEmpty_Check( TextBox txt, string alert_message)
        {
            bool ck_emptyTxt = false;
            if (string.IsNullOrEmpty(txt.Text))
            {
                ck_emptyTxt = false;
                MessageBox.Show(alert_message);
                txt.Focus();
            }
            else
            {
                ck_emptyTxt = true;
            }
            return ck_emptyTxt;
        }
        public void txtEmpty_CheckDefault(TextBox txt, string _defaultValue)
        {            
            if (string.IsNullOrEmpty(txt.Text))
                txt.Text = _defaultValue;                            
        }
        public void saveInformation(string _serverIP, string _deviceInofo, string _authType, string _PCReload, string _macAddress, bool _checkRadio)
        {
            SetRegistry("agent_info_s", "S");
            SetRegistry("agent_info_serverIP", _serverIP);
            SetRegistry("agent_info_deviceInofo", _deviceInofo);
            SetRegistry("agent_info_authType", _authType);
            SetRegistry("agent_info_saveReload", _PCReload);
            SetRegistry("agent_info_macAddress", _macAddress);

            SetStartUp(_checkRadio);
        }
        //PC 상태 병경
        public static void PCStateChage(string _mode)
        {
            switch (_mode)
            {
                case "SP_DIDLOGOFF":
                        ExitWindowsEx(0, 0);  //LOG OFF
                    break;
                case "SP_DIDREBOOT":
                    Process.Start("shutdown.exe", "/f /r /t 0");
                    break;
                case "SP_POWEROFF":
                    Process.Start("shutdown.exe", "/s /t 0");
                    break;
                case "SP_DIDLOCK":
                    LockWorkStation();
                    break;
                case "SP_HIBERNATE":
                    SetSuspendState(true, true, true);
                    break;
                case "SP_SLEEP":
                    SetSuspendState(false, true, true);
                    break;
            }
        }
        //에러 메세지
        public static void _ErrorMessage(string _errorCode, int _Time)
        {
            string _retunMessage, _retunAlert = null;

            uniUtil.TurnDisplay("O");
            uniUtil.Delay(3);

            switch (_errorCode)
            {
                case "Card":
                    _retunMessage = UtilConstant.auth_resultFasleC;
                    _retunAlert = UtilConstant.alert_Message2.ToString();
                    break;
                case "Finger":
                    _retunMessage = UtilConstant.auth_resultFasleF;
                    _retunAlert = UtilConstant.alert_Message2.ToString();
                    break;
                case "DeviceErr":
                    _retunMessage = UtilConstant.device_error;
                    _retunAlert = UtilConstant.alert_Message3.ToString();
                    break;
                default:
                    _retunMessage = UtilConstant.auth_resultOK;
                    _retunAlert = UtilConstant.alert_Message1.ToString();
                    break;
            }
            AutoClosingMessageBox.Show(_retunMessage, _retunAlert, _Time);
            uniUtil.Delay(2);
            BlockInput("OK");
            if (!_errorCode.Equals("OK"))
            {
                PCStateChage("SP_DIDLOCK");
                
            }
        }       
        public static bool dirExite (string _path)
        {
            DirectoryInfo diDir = new DirectoryInfo(_path);           
            return diDir.Exists ? true : false;
        }
        //file download 
        public static bool webFileDownload(string _fullFileUrl, string _fileNm)
        {
            try
            {



                //디렉토리 존제 여부 확인
                string mp3FilePath = System.Environment.CurrentDirectory + "\\mp3\\" ;
                string strDir = Path.GetDirectoryName(mp3FilePath);
                DirectoryInfo diDir = new DirectoryInfo(strDir);
                if (!diDir.Exists)
                {
                    diDir.Create();
                    diDir = new DirectoryInfo(strDir);
                }
                
                WebClient wb = new WebClient();
                wb.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.33 Safari/537.36");
                wb.DownloadFile(_fullFileUrl, mp3FilePath+ _fileNm);
                return true;

            }
            catch(Exception e)
            {
                setLogFileS("webFileDownload error:" + e.ToString());
                return false;
            }
            
        }

        public static bool webFileDownload(string _fullFileUrl, string _fileNm, string _path)
        {
            try
            {



                //디렉토리 존제 여부 확인
                string FilePath = _path;
                string strDir = Path.GetDirectoryName(FilePath);
                DirectoryInfo diDir = new DirectoryInfo(strDir);
                if (!diDir.Exists)
                {
                    diDir.Create();
                    diDir = new DirectoryInfo(strDir);
                }

                WebClient wb = new WebClient();
                wb.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.33 Safari/537.36");
                wb.DownloadFile(_fullFileUrl, FilePath + _fileNm);
                return true;

            }
            catch (Exception e)
            {
                setLogFileS("webFileDownload error:" + e.ToString());
                return false;
            }

        }
        public string webString (string _serverUrl)
        {
            string html = new WebClient().DownloadString(_serverUrl);
            return html; 
        }
        //디렉토리 파일 목록 가지고 오기
        public string[] dirFileLst(string _path)
        {
           
            DirectoryInfo diDir = new DirectoryInfo(_path);
            int i = 0;
            FileInfo[] listFiles = diDir.GetFiles();
            string[] filelist = new string[listFiles.Length];
            if (diDir.Exists)
            {
                foreach (var item in diDir.GetFiles())
                {
                    if (!item.Extension.Equals(".html") && !item.Extension.Equals(".js"))
                    {

                        filelist[i++] = item.Name;
                    }
                    
                }
                return filelist;
            }
            else
            {
                return null;
            }
        }

        public string[] dirFileLst(string _path, string _notSearchFile)
        {
            //
            DirectoryInfo diDir = new DirectoryInfo(_path);
            int i = 0;
            FileInfo[] listFiles = diDir.GetFiles();
            //Length 관련 내용 다시 정리 하기 
            string[] filelist = new string[listFiles.Length];
            if (diDir.Exists)
            {
                foreach (var item in diDir.GetFiles())
                {
                    if (!item.Extension.Equals(".html") && !item.Extension.Equals(".js") && !item.Name.Equals(_notSearchFile))
                    {
                        filelist[i++] = item.Name;
                    }

                }
                return filelist;
            }
            else
            {
                return null;
            }
        }
        public string isNullReplace(string _inputTxt, string _replaceTxt)
        {
            return string.IsNullOrEmpty(_inputTxt) ? _replaceTxt : _inputTxt;
        }
        public List<string> dirFileLists(string _path)
        {
            DirectoryInfo diDir = new DirectoryInfo(_path);
            
            FileInfo[] listFiles = diDir.GetFiles();
            List<string> fileList = new List<string>();

            if (diDir.Exists)
            {
                foreach (var item in diDir.GetFiles())
                {
                    if (!item.Extension.Equals(".html") && !item.Extension.Equals(".js"))
                    {
                        fileList.Add(item.Name);
                        //filelist[i++] = item.Name;
                    }

                }
                return fileList;
            }
            else
            {
                return null;
            }
        }
        // 기존 파일 삭제 
        public bool scheduleOterFileDel (string _path, string[] _dirFileLst, string[] _xmlFileLst)
        {
            try
            {
                IEnumerable<string> differenceQuery = _dirFileLst.Except(_xmlFileLst);
                
                
                if (differenceQuery != null && differenceQuery.Count() > 0)
                {                    
                    foreach (string s in differenceQuery)
                    {                             
                        if (s != null)
                        {
                            fileDel(_path + "\\" + s);
                            Console.WriteLine("file:" + s);
                        }                        
                    }
                }                
                return true;
            }
            catch(Exception ex)
            {
                setLogFile("scheduleOterFileDel error:" + ex.ToString());
                return false;
            }                
        }
        /// <summary>
        /// 확장자 찾기
        /// </summary>
        /// <param name="_fullTxt"></param>
        /// <param name="_split"></param>
        /// <returns></returns>
        public string endTxt(string _fullTxt, string _split)
        {
            return _fullTxt.Substring(_fullTxt.LastIndexOf(_split) + 1);
        }
        //뒤에 문자 나올때 까지 
        public string endTxt(string _fullTxt, string _split, string _split1)
        {
            string _fileTxt = _fullTxt.Substring(_fullTxt.LastIndexOf(_split) + 1);
            
            return _fileTxt.Contains(_split1) ?  _fileTxt.Substring(0, _fileTxt.LastIndexOf(_split1)) : _fileTxt;
        }
        //포스트 데이터 전송 
        public static string WebPostDataSend(string _postData, string _serverUrl, string contentType)
        {
            string _returnReader = "";
            try
            {

                setLogFileS("_serverUrl:" + _serverUrl);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_serverUrl);
                req.ContentType = contentType;
                req.Method = "POST";
                setLogFileS("postData:json=" + _postData);
                
                byte[] bytes = Encoding.Default.GetBytes("json=" + _postData);
                req.ContentLength = bytes.Length;
                
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                requestStream = null;

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK)
                {                    
                    Stream sm = resp.GetResponseStream();
                    StreamReader sr = new StreamReader(sm, Encoding.UTF8);

                    _returnReader = sr.ReadToEnd().Replace("\"","'");
                    
                    sr.Close();
                    sm.Close();
                    resp.Close();
                    sr = null;
                    sm = null;
                }
                else
                {
                    setLogFileS("접속 장애 ");
                }
                
            }
            catch (UriFormatException ex1)
            {

                setLogFileS(string.Format("WebPostDataSend UriFormatException error : {0} ", ex1.ToString()));
            }
            catch (Exception ex)
            {
                setLogFileS(string.Format("WebPostDataSend Exception error {0} ", ex.ToString()));
            }
            finally
            {
                _postData = null;
            }
            setLogFileS("returnReader:" + _returnReader);            
            return _returnReader;
        }

        public  string FromHex(string hex)
        {
            hex = hex.Replace(" ", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return Encoding.ASCII.GetString(raw);
        }
        /// <summary>
        /// 화면 정의
        /// </summary>
        /// <param name="_mode"></param>
        private static void BlockInput(string _mode)
        {
            try
            {

                if (_mode.Equals("OK"))
                {
                    BlockInput(false);
                }
                else
                {
                    BlockInput(true);
                }
            }
            catch (Exception ex)
            {
                setLogFileS("BlockInput:" + ex.ToString());
            }
        }
        public static void keyMouseProhibition()
        {
            //showDesktop("R");
            Delay(1);
            BlockInput(true);
        }
        //display mointer off
        public static void TurnDisplay(string _gubun)
        {
            if (_gubun.Equals("F"))
            {
                PostMessage((IntPtr)HwndBroadcast, (uint)WmSyscommand, (IntPtr)ScMonitorpower, (IntPtr)ShutOffDisplay);
            }
            else if (_gubun.Equals("O"))
            {
                PostMessage((IntPtr)HwndBroadcast, (uint)WmSyscommand, (IntPtr)ScMonitorpower, (IntPtr)ShutONDisplay);
            }
            else
            {
                PostMessage((IntPtr)HwndBroadcast, (uint)WmSyscommand, (IntPtr)ScMonitorpower, (IntPtr)ShutSLEEPDisplay);
            }
        }
        public void delInformation()
        {
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKey(UtilConstant.reg_path);

        }
        public bool cboEmpty_Check(ComboBox cboBox, string alert_message)
        {
            bool ck_emptyTxt = false;
            if (string.IsNullOrEmpty(cboBox.SelectedValue.ToString()))
            {
                ck_emptyTxt = false;
                MessageBox.Show(alert_message);
                cboBox.Focus();
            }
            else
            {
                ck_emptyTxt = true;
            }
            return ck_emptyTxt;
        }
        public bool cboEmpty_CheckIndex(ComboBox cboBox, string alert_message)
        {
            bool ck_emptyTxt = false;
            if (string.IsNullOrEmpty(Convert.ToString(cboBox.SelectedIndex)))
            {
                ck_emptyTxt = false;
                MessageBox.Show(alert_message);
                cboBox.Focus();
            }
            else
            {
                ck_emptyTxt = true;
            }
            return ck_emptyTxt;
        }
        //프로그램 중복 실행 방지
        public bool programeDubleProhibit(string _filePath)
        {
            string exeName = Path.GetFileNameWithoutExtension(_filePath);
            Process[] serviceProcess = Process.GetProcessesByName(exeName);

            return serviceProcess.Length > 0 ? false : true;            
        }
        //특정 프로그램 종료
        public bool programekill(string _processName)
        {
            Process[] processList = Process.GetProcessesByName(_processName);
            if (processList.Length > 0)
            {
                processList[0].Kill();
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetRegistry(String _key)
        {            
            try
            {
                Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE");
                string rstValue = rk.OpenSubKey("agent_info").GetValue(_key).ToString();
                rk.Close();
                return DecryptString(rstValue);                
            }
            catch (NullReferenceException e1)
            {
                return "";
            }
            catch (Exception ex)
            {
                setLogFile("GetRegistry error:" + _key + ":" + ex.ToString());
                return "";
            }
            
        }
        /// <summary>
        /// 데스크탑 
        /// </summary>
        /// <param name="_gubun"></param>
        
        public static string DecryptString(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(UtilConstant.EncryptPassString));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Results);
        }
        public static string EncryptString(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(UtilConstant.EncryptPassString));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return Convert.ToBase64String(Results);
        }
        /// <summary>
        /// return 값 List로 변경
        /// </summary>
        /// <param name="_retrunMessage">입력값</param>
        /// <param name="_regexTxt">구분값</param>
        /// <returns></returns>
        public static List<string> returnListMessage(string _retrunMessage, string _regexTxt)
        {

            //string[] _messages = System.Text.RegularExpressions.Regex.Split(_retrunMessage, _regexTxt);
            //string[] _messages = _retrunMessage.Split('|');

            List<string> lst = new List<string>(_retrunMessage.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));

            return lst;
        }
        public static string addToDay(string duration_type, int duration)
        {
            DateTime nowDay = System.DateTime.Now;
            string returnDay = "";
            switch (duration_type)
            {
                case "D":
                    returnDay = nowDay.AddDays(duration).ToString("yyyyMMdd");
                    break;
                case "M":
                    returnDay = nowDay.AddMonths(duration).ToString("yyyyMMdd");
                    break;
                case "Y":
                    returnDay = nowDay.AddYears(duration).ToString("yyyyMMdd");
                    break;
            }

            return returnDay;
        }


    }
    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;

        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;

            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed, null, (timeout * 1000), System.Threading.Timeout.Infinite);
            MessageBox.Show(text, caption);
        }

        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow(null, _caption);
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }




        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }    
    public static class wakeLenCard
    {
        public static void WakeUp(string macAddress, string ipAddress, string subnetMask)
        {


            try
            {
                UdpClient client = new UdpClient();

                Byte[] datagram = new byte[102];

                for (int i = 0; i <= 5; i++)
                {
                    datagram[i] = 0xff;
                }

                string[] macDigits = null;
                if (macAddress.Contains("-"))
                {
                    macDigits = macAddress.Split('-');
                }
                else
                {
                    macDigits = macAddress.Split(':');
                }

                if (macDigits.Length != 6)
                {
                    uniUtil.setLogFileS("Incorrect MAC address supplied!");
                }

                int start = 6;
                for (int i = 0; i < 16; i++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        datagram[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                    }
                }

                IPAddress address = IPAddress.Parse(ipAddress);
                IPAddress mask = IPAddress.Parse(subnetMask);
                IPAddress broadcastAddress = GetBroadcastAddress(address, mask);

                client.Send(datagram, datagram.Length, broadcastAddress.ToString(), 12287);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)

                uniUtil.setLogFileS("Lengths of IP address and subnet mask do not match.");
            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                uniUtil.setLogFileS("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }

    }
    //프로세스 죽이기 
    public static class Unkillable
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern void RtlSetProcessIsCritical(UInt32 v1, UInt32 v2, UInt32 v3);

        public static void MakeProcessUnkillable()
        {
            Process.EnterDebugMode();
            RtlSetProcessIsCritical(1, 0, 0);
        }

        public static void MakeProcessKillable()
        {
            RtlSetProcessIsCritical(0, 0, 0);
        }

    }

}
