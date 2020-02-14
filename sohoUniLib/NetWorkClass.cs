using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace sohoUniLib
{
    public class NetWorkClass
    {
        [DllImport("kernel32.dll")]
        private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        

        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }
        //시간 동기화
        public void Time_Sync(DateTime o_Time)
        {
            SYSTEMTIME tmpTime = new SYSTEMTIME();
            string i_Time = o_Time.AddHours(-9).ToString("yyyyMMddHHmmss");

            tmpTime.wYear = (ushort)int.Parse(i_Time.Substring(0, 4));
            tmpTime.wMonth = (ushort)int.Parse(i_Time.Substring(4, 2));
            tmpTime.wDay = (ushort)int.Parse(i_Time.Substring(6, 2));
            tmpTime.wHour = (ushort)int.Parse(i_Time.Substring(8, 2));
            tmpTime.wMinute = (ushort)int.Parse(i_Time.Substring(10, 2));
            tmpTime.wSecond = (ushort)int.Parse(i_Time.Substring(12, 2));
            tmpTime.wMilliseconds = (ushort)100;

            SetSystemTime(ref tmpTime);
        }

        private void SetTime()
        {
            uniUtil util = new uniUtil();
            string Url = util.GetRegistry("server_url").ToString();// "http://www.naver.com";
            System.Net.HttpWebRequest wReqFirst = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Url);
            DateTime dtBefore = DateTime.Now;
            System.Net.HttpWebResponse wRespFirst = (System.Net.HttpWebResponse)wReqFirst.GetResponse();
            
            DateTime dtAfter = DateTime.Now;
            DateTime dtNosp = Convert.ToDateTime(wRespFirst.Headers["Date"].ToString());            
            dtNosp = dtNosp.AddTicks(dtAfter.Ticks - dtBefore.Ticks);
            Time_Sync(dtNosp);

            util = null;
        }
        ///
        /// 클라이언트 IP 주소 얻어오기...
        ///
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
        // telnet 연결
        public static string TelnetConnect(string ip, int port, string _message)
        {
            TcpClient oClient = new TcpClient();
            
            try
            {
                oClient.Connect(ip, port);
                if (oClient.Connected)
                {
                    NetworkStream ns = oClient.GetStream();
                    read(ns).ToString();//처음읽으면 이상한값이... 나온다.. 어째서일까..
                    read(ns).ToString();//로그인 입력하라는 메세지가 읽어진다.
                    write(ns, _message);//아이디입력
                    read(ns).ToString();//아이디 기록 되는지 확인                
                    string pc = read(ns);//접속되었는지 확인
                    //pc = pc.Substring(pc.Length - 2, 1);
                    //Console.WriteLine("pc:"+pc);
                    if (pc.Length > 0) //내가 한 기계에 연결결과 로그인하고 나면 맨 마지막 커맨드에 #이붙어서 이것을 기준으로 접속 되는지 안되는지 확인함. 접속하는것 마다 다를거라고 생각함.
                    {
                        return "OK";
                    }
                    else
                    {
                        return "NOT CONNECT";
                    }
                    ns.Close();
                }
                else
                {
                    return "NOT CONNECT1";
                }                
                oClient.Close();
                Console.WriteLine("oClient colse");
            }
            catch (Exception e)
            {                
                if (oClient != null) { oClient.Close(); Console.WriteLine("catch oClient colse"); }
                uniUtil.setLogFileS("TelnetConnect errror:" + e.ToString());
                return "ERROR";
            }
            
        }
        private static void write(NetworkStream ns, string message)
        {            
            byte[] msg = Encoding.ASCII.GetBytes(message + Environment.NewLine);
            ns.Write(msg, 0, msg.Length);
        }
        private static string read(NetworkStream ns)
        {
            StringBuilder sb = new StringBuilder();
            if (ns.CanRead)
            {                
                byte[] readBuffer = new byte[1024];
                int numBytesRead = 0;                
                do
                {

                    numBytesRead = ns.Read(readBuffer, 0, readBuffer.Length);
                    sb.AppendFormat("{0}", Encoding.ASCII.GetString(readBuffer, 0, numBytesRead));
                    sb.Replace(Convert.ToChar(24), ' ');
                    sb.Replace(Convert.ToChar(255), ' ');
                    sb.Replace('?', ' ');
                }                
                while (ns.DataAvailable);
            }            
            return sb.ToString();

        }
        //
        public static bool ConnectTest(string ip, int port)
        {
            bool result = false;
            Socket socket = null;
            try {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);
                IAsyncResult ret = socket.BeginConnect(ip, port, null, null);
                result = ret.AsyncWaitHandle.WaitOne(100, true);
            }
            catch { }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }
            return result;
        }
        public static bool PingTest(string ip)
        {
            bool result = false;
            try {
                Ping pp = new Ping();
                PingOptions po = new PingOptions();
                po.DontFragment = true;
                byte[] buf = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaa");
                PingReply reply = pp.Send(IPAddress.Parse(ip), 10, buf, po);
                if (reply.Status == IPStatus.Success)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }
        
        public static bool UrlConnectionValidation(string strUrl)
        {
            try
            {
                WebClient wc = new WebClient();
                // 연결 시도  
                string strReturn = wc.DownloadString(strUrl);
                wc.Dispose();
                return true;
            }
            catch (System.Exception ex)
            {
                //uniUtil.setLogFileS(string.Format("UrlConnectionValidation error : {0} ", ex.ToString()));
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        
        public static Dictionary<string, string> ShowNetworkInterFace()
        {
            Dictionary<string, string> macAddress = new Dictionary<string, string>();
            try
            {

                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                if (nics == null || nics.Length < 1)
                {
                    return macAddress;
                }
                string mac_address = "";
                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        mac_address += string.Format("{0}", bytes[i].ToString("X2"));
                        if (i != bytes.Length - 1)
                        {
                            mac_address += "-";
                        }
                    }
                    try
                    {
                        macAddress.Add(adapter.NetworkInterfaceType.ToString(), mac_address.Replace("-", ""));
                    }
                    catch (ArgumentException ex1)
                    {
                        //UniLib.setLogFileS(string.Format("ShowNetworkInterFace error : {0} ", ex1.ToString()));
                        Console.WriteLine(ex1.ToString());
                    }

                    mac_address = "";
                }
            }
            catch (Exception ex)
            {
                uniUtil.setLogFileS(string.Format("ShowNetworkInterFace error : {0} ", ex.ToString()));
            }
            return macAddress;
        }
    }
}
