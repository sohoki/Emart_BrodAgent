using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using sohoUniLib;



namespace programe_Update
{
    public partial class frmUpdate : Form
    {
        uniUtil util = new uniUtil();
        didInfo didinfo = new didInfo();

        

        public frmUpdate()
        {
            
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void frmUpdate_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void frmUpdate_Load(object sender, EventArgs e)
        {
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            
        }
        private bool versionCheck()
        {
            bool versionCheck = false;

            try
            {
                //서버 접속이 디면 
                

                if (sohoUniLib.NetWorkClass.UrlConnectionValidation(util.GetRegistry("server_url")) == true)
                {
                    
                    string returnString = sohoUniLib.uniUtil.WebPostDataSend(sohoUniLib.ServerComm_json.sp_JsonString(didinfo, sohoUniLib.didConstInfo.xmlMessageTyepe01_1), 
                                                                             util.GetRegistry("server_url") + sohoUniLib.didConstInfo.serverJsonUtrl01,
                                                                             sohoUniLib.didConstInfo.contentType_01);

                    string[] didState = sohoUniLib.ServerComm_json.jsonResult(returnString).Split('/');

                    //버전 확인
                    if (didState[4].ToString().Equals(util.GetRegistry("version")) || string.IsNullOrEmpty(util.GetRegistry("version")))
                    {
                        //프로그램 다운로드 목록 만들기

                        //프로그램 다운로드 확인

                    }
                    else
                    {
                        //프로그램 닫고 음원 시작
                    }
                }
                else
                {

                    util.setLogFile("서버 접속 안됨");

                    
                    //프로그램 닫고 음원 시작
                    
                }
            }
            catch (Exception e)
            {

            }


            //if ( string.IsNullOrEmpty( util.GetRegistry("version_info")) || 

            return versionCheck;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbDownload.Value = e.ProgressPercentage;
        }
    }
}
