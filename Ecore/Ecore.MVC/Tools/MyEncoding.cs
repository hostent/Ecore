using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.MVC.Tools
{
    public class MyEncoding : IEncoding
    {
        public string UrlEncode(string context)
        {
            return System.Net.WebUtility.UrlEncode(context);
        }

        public string UrlDecode(string context)
        {
            return System.Net.WebUtility.UrlDecode(context);
        }

        public string HtmlEncode(string context)
        {
            return System.Net.WebUtility.HtmlEncode(context);
        }

        public string HtmlDecode(string context)
        {
            return System.Net.WebUtility.HtmlDecode(context);
        }
    }
}
