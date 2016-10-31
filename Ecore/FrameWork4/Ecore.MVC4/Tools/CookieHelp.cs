using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4.Tools
{
    public class CookieHelp : ICookie
    {
        HttpContext CurrentContext { get; set; }

        public CookieHelp(HttpContext context)
        {
            CurrentContext = context;
        }

        public void SetCookie(string key, string value)
        {
            CurrentContext.Response.Cookies.Add(new HttpCookie(key, value)
            {
                // Domain = SiteAppConfig.Domain,
                Path = "/",
                HttpOnly = true,

            });

        }
        public void SetCookie(string key, string value, DateTime Expires)
        {
            CurrentContext.Response.Cookies.Add(new HttpCookie(key, value)
            {
                // Domain = SiteAppConfig.Domain,
                Path = "/",
                HttpOnly = true,
                Expires = Expires,
            });

        }
        public void SetCookie(string key, string value, DateTime Expires, string domain)
        {
            CurrentContext.Response.Cookies.Add(new HttpCookie(key, value)
            {
                Domain = domain,
                Path = "/",
                HttpOnly = true,
                Expires = Expires,
            });
        }

        public string GetCookieValue(string key)
        {
            var cookie = CurrentContext.Request.Cookies.Get(key);
            if (cookie != null)
            {
                return cookie.Value;
            }
            return "";

        }

        public void RemoveCookie(string key)
        {
            CurrentContext.Response.Cookies.Remove(key);
        }
    }
}
