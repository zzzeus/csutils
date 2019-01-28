using PInvoke;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace csutils
{
    public class Desktop
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        static IntPtr sdview = IntPtr.Zero;
        static IntPtr folderview = IntPtr.Zero;
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

        /// <summary>
        /// 
        /// Get the Folder window which contains the desktop folder icon
        /// </summary>
        /// <returns>return the window handle of  FolderView</returns>
        public static IntPtr findFolderView()
        {

            User32.EnumWindows(new User32.WNDENUMPROC(isWorkerW), IntPtr.Zero);
            folderview = User32.FindWindowEx(sdview, IntPtr.Zero, "SysListView32", "FolderView");
            return folderview;
        }
        /// <summary>
        /// judge is the ture WorkerW window that contains the folderview
        /// </summary>
        /// <param name="intPtr"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private static bool isWorkerW(IntPtr intPtr, IntPtr para)
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
        public static void changeFolderViewState()
        {
            var winfo = new User32.WINDOWINFO();
            User32.GetWindowInfo(folderview,ref winfo);
        
        }
        public static void showDesktopIcon()
        {
            if (folderview == IntPtr.Zero)
                findFolderView();
            User32.SetWindowPos(folderview, IntPtr.Zero, 0, 0, 0, 0, User32.SetWindowPosFlags.SWP_SHOWWINDOW);
        }
        public static void HideDesktopIcon()
        {
            if (folderview == IntPtr.Zero)
                findFolderView();
            User32.SetWindowPos(folderview, IntPtr.Zero, 0, 0, 0, 0, User32.SetWindowPosFlags.SWP_HIDEWINDOW);
        }

        public static void drawPictureOnDesktop(string path)
        {
            findFolderView();
            //var dc=User32.GetDC(sdview);
            //// Create image.
            //Image newImage = Image.FromFile(path);

            //// Create point for upper-left corner of image.
            //PointF ulCorner = new PointF(100.0F, 100.0F);

            //var g = Graphics.FromHdc(dc.DangerousGetHandle());

            //// Draw image to screen.
            //g.DrawImage(newImage, ulCorner);

            //g.Dispose();
            //dc.DangerousRelease();
            //dc.Dispose();

            IntPtr desktopPtr = GetDC(sdview);
            Graphics g = Graphics.FromHdc(desktopPtr);

            SolidBrush b = new SolidBrush(Color.White);
            g.FillRectangle(b, new Rectangle(0, 0, 1920, 1080));

            g.Dispose();
            ReleaseDC(IntPtr.Zero, desktopPtr);
        }

        public static void setToolWindow(IntPtr win)
        {
            User32.SetWindowLong(win, User32.WindowLongIndexFlags.GWL_EXSTYLE, User32.SetWindowLongFlags.WS_EX_TOOLWINDOW);
        }
        public static void ShowWindow(IntPtr win)
        {
            User32.ShowWindow(win,User32.WindowShowStyle.SW_SHOW);
        }
        public static void ShowWindowForeword(IntPtr win)
        {
            User32.SetWindowPos(win,findFolderView(),0,0,0,0,User32.SetWindowPosFlags.SWP_NOACTIVATE);
        }
    }
}
