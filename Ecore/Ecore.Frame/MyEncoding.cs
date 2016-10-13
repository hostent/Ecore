using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class MyEncoding
    {
        public static IEncoding Default { get; set; }
    }


    public interface IEncoding
    {
        string UrlEncode(string context);
        string UrlDecode(string context);
        string HtmlEncode(string context);
        string HtmlDecode(string context);
    }
}
