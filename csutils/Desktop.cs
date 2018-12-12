using PInvoke;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace csutils
{
    public class Desktop
    {
        static IntPtr sdview = IntPtr.Zero;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SystemParametersInfo(uint uAction, uint uParam, StringBuilder lpvParam, uint init);

        const uint SPI_GETDESKWALLPAPER = 0x0073;



        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        public static string getWallpaper()
        {
            StringBuilder wallPaperPath = new StringBuilder(200);

            if (SystemParametersInfo(SPI_GETDESKWALLPAPER, 200, wallPaperPath, 0))
            {
                return (wallPaperPath.ToString());
            }
            return "none";
        }

        //findwindow
        public static IntPtr findFolderView()
        {
           
            User32.EnumWindows(new User32.WNDENUMPROC(isWorkerW), IntPtr.Zero);
            
            return sdview;
        }
        public static bool isWorkerW(IntPtr intPtr, IntPtr para)
        {

          
            if (User32.GetClassName(intPtr) == "WorkerW")
            {
                var sd = User32.FindWindowEx(intPtr, IntPtr.Zero, "SHELLDLL_DefView", "");
                if (sd != IntPtr.Zero)
                {
                    sdview = sd;
                    return false;
                }
            }
            return true;
        }
    }
}
