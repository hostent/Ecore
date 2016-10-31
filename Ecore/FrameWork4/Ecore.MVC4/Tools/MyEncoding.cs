using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4.Tools
{
    public class MyEncoding : IEncoding
    {
        public string UrlEncode(string context)
        {
            return HttpUtility.UrlEncode(context);
             
        }

        public string UrlDecode(string context)
        {
            return HttpUtility.UrlDecode(context);
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
