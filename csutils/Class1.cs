using System;
using System.Diagnostics;

namespace csutils
{
    public class Class1
    {
        public static void showInfo()
        {
            var ps=SysInfo.getSomeInfo();
            foreach (var p in ps)
            {
                Debug.WriteLine(p);
            }
            SystemSleepManagement.PreventSleep();
        }
    }
}
