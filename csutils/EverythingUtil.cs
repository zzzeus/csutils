using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace csutils
{
    public class EverythingUtil
    {
        public static bool checkEverything()
        {
            var ps=Process.GetProcessesByName("Everything");
            if (ps.Length > 0)
                return true;
            else
                return false;
        }
    }
}
