using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;
using Microsoft.Win32;
using System.ServiceProcess;
using System.IO;
using System.Threading;


namespace psExeStart
{
    class Program
    {
        static string _svcName = "Emart_BrodAgent";

        static void Main(string[] args)
        {
            if (Environment.UserInteractive == true)
            {
                //일반 프로그램으로 실행된 경우
                DanceWithSCM();
            }
            else
            {
                //Local System 권한의 NT 서비스로 실행된 경우
                if (Environment.UserName == "SYSTEM")
                {

                    //DoSystemRights();
                }
            }
        }
        private static void DoSystemRights()
        {
            using (RegistryKey regKey =
                Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services", true))
            {
                RegistryKey created = regKey.CreateSubKey("EmartBrodService");
                created.SetValue("User", Environment.UserName);
            }
        }
        private static void DanceWithSCM()
        {
            Process process = Process.GetCurrentProcess();
            string filePath = System.Environment.CurrentDirectory + "\\" + _svcName + ".exe"; //typeof(Program).Assembly.Location;



            if (ServiceInstaller.ServiceIsInstalled(_svcName) == false)
            {
                ServiceInstaller.InstallAndStart(_svcName, _svcName, filePath);
            }
            else
            {
                ServiceInstaller.Uninstall(_svcName);                
                ServiceInstaller.InstallAndStart(_svcName, _svcName, filePath);
            }

            //string exeName = Path.GetFileNameWithoutExtension(typeof(Program).Assembly.Location);
            string exeName = Path.GetFileNameWithoutExtension(filePath);
            Process[] serviceProcess = Process.GetProcessesByName(exeName);

            int maxSecond = 100;

            if (serviceProcess.Length == 2)
            {
                foreach (Process targetProcess in serviceProcess)
                {
                    if (targetProcess.Id == process.Id)
                    {
                        continue;
                    }

                    System.Diagnostics.Trace.WriteLine("Exceuted");

                    targetProcess.WaitForExit(maxSecond * 1000);
                }
            }

            ServiceInstaller.StopService(_svcName);

            while (ServiceInstaller.GetServiceStatus(_svcName) == ServiceState.Stop)
            {
                if (maxSecond-- <= 0)
                {
                    break;
                }

                Thread.Sleep(200);
            }

            ServiceInstaller.Uninstall(_svcName);
        }
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (null != identity)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }
    }

}
