using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class Cookie
    {
        public static ICookie Default { get; set; }
    }


    public interface ICookie
    {
        void SetCookie(string key, string value);
        void SetCookie(string key, string value, DateTime Expires);
        void SetCookie(string key, string value, DateTime Expires, string domain);
        string GetCookieValue(string key);
        void RemoveCookie(string key);
    }
}
