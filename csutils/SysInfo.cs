using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace csutils
{
    public class SysInfo
    {
        public static IEnumerable<string> getSomeInfo()
        {
            var list = from p in Process.GetProcesses()
                       select p.ProcessName;

            return list;
        }
    }
}
