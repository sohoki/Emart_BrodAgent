using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sohoUniLib;

namespace Uninstall
{
    static class Program
    {

        

        [STAThread]
        static void Main(string[] args)
        {
            System.Diagnostics.Process _proc = new System.Diagnostics.Process();

            //레지스트리 삭제 
            sohoUniLib.uniUtil.DetRegistry("agent_info");
            //파일삭제
            sohoUniLib.uniUtil.directory_dels(Environment.CurrentDirectory);

            _proc.EnableRaisingEvents = false;
            _proc.StartInfo.FileName = "msiexec.exe";
            _proc.StartInfo.Arguments = "/x {02809471-0B82-45FF-9279-E2D7A56AE596}";
            _proc.Start();

        }
    }
}
