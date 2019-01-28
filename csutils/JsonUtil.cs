using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace csutils
{
    /// <summary>
    /// Deal with json
    /// https://www.newtonsoft.com/json
    /// </summary>
    //JSON转换类
    public class ConvertJson
    {
       

        public static string listToJson<T>(List<T> list)
        {
            return JsonConvert.SerializeObject(list);
        }
    }
}
