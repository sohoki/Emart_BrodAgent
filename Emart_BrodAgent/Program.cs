using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using sohoUniLib;
using System.Net.NetworkInformation;

namespace Emart_BrodAgent
{
   
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        /// static string _svcName = "psexec2";
        /// 
        
        [STAThread]
        static void Main()
        {

            string mtxNmae = "A0C147E3-323E-40E9-B011-712570E02D6A";


           
            bool bnew;
            Mutex mutex = new Mutex(true, mtxNmae, out bnew);
            if (bnew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new emart_agent());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("프로그램이 실행중입니다.");
                Application.Exit();
            }

        }

    }
    
   

    
    
}
