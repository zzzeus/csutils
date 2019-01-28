using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils
{
    public class Command
    {
        public static string explorer = "explorer";
        public static string explorer_select = "/select,";
        #region 运行cmd
        public static void run()
        {
            try
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    // You can start any process, HelloWorld is a do-nothing example.
                    myProcess.StartInfo.FileName = "C:\\HelloWorld.exe";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    // This code assumes the process you are starting will terminate itself. 
                    // Given that is is started without a window so you cannot terminate it 
                    // on the desktop, it must terminate itself or you can do it programmatically
                    // from this application using the Kill method.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // Opens the path.
        public static void OpenApplication(string myFavoritesPath)
        {
            // Start Internet Explorer. Defaults to the home page.
            Process.Start(myFavoritesPath);

            // Display the contents of the favorites folder in the browser.
            //Process.Start(myFavoritesPath);
        }

        // Opens urls and .html documents using Internet Explorer.
        public static void OpenWithArguments()
        {
            // url's are not considered documents. They can only be opened
            // by passing them as arguments.
            Process.Start("explorer", "www.northwindtraders.com");

            // Start a Web page using a browser associated with .html and .asp files.
            //Process.Start("IExplore.exe", "C:\\myPath\\myFile.htm");
            //Process.Start("IExplore.exe", "C:\\myPath\\myFile.asp");
        }

        // Uses the ProcessStartInfo class to start new processes,
        // both in a minimized mode.
        public static Process OpenWithStartInfo(string exe,string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(exe);
            startInfo.WindowStyle = ProcessWindowStyle.Minimized;

            Process.Start(startInfo);

            startInfo.Arguments = arguments;

            return Process.Start(startInfo);
        }
        public static Process OpenWithWindowHide(string exe, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(exe);
            startInfo.UseShellExecute = false ;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

             //Process.Start(startInfo);

            startInfo.Arguments = arguments;

            return Process.Start(startInfo);
        }
        #endregion
        public static void openAndSelect(string path)
        {
            OpenWithWindowHide(explorer, explorer_select + path);
        }
        /// <summary>
        /// 获取环境路径
        /// </summary>
        /// <returns></returns>
        public static List<string> getEnvPath()
        {
            var d = Environment.GetEnvironmentVariables();
            foreach (var item in d.Keys)
            {
                if (item.ToString().Equals("Path"))
                {
                    var paths = d[item].ToString().Split(';');
                    return paths.ToList();
                }
            }
            
            return new List<string>();
        }
        /// <summary>
        /// 获取环境路径下所有exe文件
        /// </summary>
        /// <returns></returns>
        public static List<string> getExeList()
        {
            var paths = getEnvPath();
            List<string> list = new List<string>();
            foreach (var item in paths)
            {
                try
                {
                    var l1= from file in Directory.GetFiles(item)
                                    where file.EndsWith(".exe")
                                    select file;
                                    list.AddRange(l1);
                }
                catch (Exception)
                {
                    
                }
                
            }
            return list;

        }
        public static List<Exe> GetExes()
        {
            var list = getExeList();
            var l=from item in list
            select new Exe {name=Path.GetFileName(item),path=item,description="" };
            return l.ToList();
        }
    }
    public class Exe
    {
        public string name;
        public string path;
        public string description;
    }
}
