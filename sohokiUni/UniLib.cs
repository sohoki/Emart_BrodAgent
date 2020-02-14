using System;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Security.Cryptography;


namespace sohokiUni
{


    public delegate void StrIPAddressHandler(string str);

    public class UniLib
    {
        //네트워크 확인         
        private static string EncryptPassString = "sohoki_agent";



        private static object GetGetIsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        public bool IsInternetConnected()
        {
            const string NCSI_TEST_URL = "http://www.msftncsi.com/ncsi.txt";
            const string NCSI_TEST_RESULT = "Microsoft NCSI";
            const string NCSI_DNS = "dns.msftncsi.com";
            const string NCSI_DNS_IP_ADDRESS = "131.107.255.255";

            try
            {
                // Check NCSI test link
                var webClient = new WebClient();
                string result = webClient.DownloadString(NCSI_TEST_URL);
                if (result != NCSI_TEST_RESULT)
                {
                    return false;
                }

                // Check NCSI DNS IP
                var dnsHost = Dns.GetHostEntry(NCSI_DNS);
                if (dnsHost.AddressList.Count() < 0 || dnsHost.AddressList[0].ToString() != NCSI_DNS_IP_ADDRESS)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                setLogFileS(ex.ToString());
                return false;
            }

            return true;
        }
        public static void setLogFileS(string _str)
        {
            try
            {



                string m_strLogPrefix = Application.StartupPath+"\\log\\";  //mssqlExecuteField("select user_value from  tb_telephone where user_field = 'kiosk_log'");
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
            }

        }
        //시작시 자동 실행 
        private void SetStartUp(bool _check)
        {
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);


            if (_check == true)
            {
                SetRegistry("agent_info_start", "Y");
                rkey.SetValue("fingerAgent", Application.ExecutablePath.ToString());
            }
            else
            {
                SetRegistry("agent_info_start", "N");
                rkey.SetValue("fingerAgent", false);
            }

        }
        public static void SetRegistry(string _key, string _value)
        {
            //String을 암호화 한다.
            _value = EncryptString(_value);
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE");
            rk.CreateSubKey("atensys_demon_info").SetValue(_key, _value);
            rk.Close();
        }
        public static string EncryptString(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(EncryptPassString));

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
        public static string addToDays(string duration_type, int duration)
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
    public class uniJsonComm
    {


    }
}
