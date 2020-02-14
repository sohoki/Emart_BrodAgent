using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace sohoUniLib
{
    public class StateObject
    {


        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();

    }
    public class SocketServer
    {
        delegate void socketConnection();
        socketConnection myConnection;

        public delegate void _SetSocketEventHandler(string strMsg);
        public static event _SetSocketEventHandler _SetHandlerMsg;

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static Socket serverSocket;



        public SocketServer()
        {
        }
        public static void StartListening()
        {
            //자신 아이피 가지고 오기 
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);
                while (true)
                {
                    allDone.Reset();
                    serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                uniUtil.setLogFileS("StartListening error:" + e.ToString());
            }
        }
        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    uniUtil.setLogFileS(string.Format("Read {0} bytes from socket. \n Data : {1}", content.Length, content));                    
                    _SetHandlerMsg(content);
                    Send(handler, content);
                }
                else
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }
        private static void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);            
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                uniUtil.setLogFileS("SendCallback error:" + e.ToString());                
            }
        }
        public static void CloseAll()
        {
            try
            {
                serverSocket.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
