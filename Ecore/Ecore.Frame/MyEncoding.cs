using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{


    public class MyEncoding
    {
        public static string UrlEncode(string context)
        {
            return System.Net.WebUtility.UrlEncode(context);
        }

        public static string UrlDecode(string context)
        {
            return System.Net.WebUtility.UrlDecode(context);
             
        }


        public static string HtmlEncode(string context)
        {
            return System.Net.WebUtility.HtmlEncode(context);
        }

        public static string HtmlDecode(string context)
        {
            return System.Net.WebUtility.HtmlDecode(context);
        }
    }
}
