using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils
{
    public class DataUtil
    {
        public static string ListToString<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }
        public static string ArrayToString(object[] list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }
    }
}
