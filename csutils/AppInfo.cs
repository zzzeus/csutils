using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace csutils
{
    class AppInfo
    {
        /// <summary>
        /// To get the location of the Assembly.
        /// </summary>
     
        /// <returns></returns>
        public static string getAssemblyLocation(Type type)
        {
            //Assembly assem=Assembly.GetCallingAssembly();
            Assembly assem = type.Assembly;

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("Assembly Full Name:");
            //    sb.AppendLine(assem.FullName);

            // The AssemblyName type can be used to parse the full name.
            //    AssemblyName assemName = assem.GetName();
            //sb.AppendFormat("\nName: {0}", assemName.Name);
            //    sb.AppendFormat("Version: {0}.{1}",
            //        assemName.Version.Major, assemName.Version.Minor);

            //sb.AppendFormat("\nAssembly CodeBase:");
            //sb.AppendFormat(assem.CodeBase);
            return assem.Location;
            //Uri uriAddress = new Uri(assem.CodeBase);
            //return uriAddress.AbsolutePath;
        }
        /// <summary>
        /// To get the location of the current Assembly.
        /// </summary>
        /// <returns></returns>
        public static string getCurrentAssemblyLocation()
        {
            Assembly assem = Assembly.GetCallingAssembly();
            //Assembly assem = type.Assembly;

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("Assembly Full Name:");
            //    sb.AppendLine(assem.FullName);

            // The AssemblyName type can be used to parse the full name.
            //    AssemblyName assemName = assem.GetName();
            //sb.AppendFormat("\nName: {0}", assemName.Name);
            //    sb.AppendFormat("Version: {0}.{1}",
            //        assemName.Version.Major, assemName.Version.Minor);

            //sb.AppendFormat("\nAssembly CodeBase:");
            //sb.AppendFormat(assem.CodeBase);

            //Uri uriAddress = new Uri(assem.CodeBase);
            //return uriAddress.AbsolutePath;
            return assem.Location;
        }


    }
}
