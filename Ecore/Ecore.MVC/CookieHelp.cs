using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ecore.Frame;

namespace Ecore.MVC
{
    public class CookieHelp: ICookie
    {

        public void SetCookie(string key, string value)
        {
            var context = new DefaultHttpContext();

            context.Response.Cookies.Append(key, value, new CookieOptions()
            {
                // Domain = SiteAppConfig.Domain,
                Path = "/",
                HttpOnly = true,
            });

        }
        public void SetCookie(string key, string value, DateTime Expires)
        {
            var context = new DefaultHttpContext();

            context.Response.Cookies.Append(key, value, new CookieOptions()
            {
                // Domain = SiteAppConfig.Domain,
                Path = "/",
                HttpOnly = true,
                Expires = Expires,
            });

        }
        public void SetCookie(string key, string value, DateTime Expires, string domain)
        {
            var context = new DefaultHttpContext();

            context.Response.Cookies.Append(key, value, new CookieOptions()
            {
                Domain = domain,
                Path = "/",
                HttpOnly = true,
                Expires = Expires,
            });
        }

        public string GetCookieValue(string key)
        {
            var context = new DefaultHttpContext();
            return context.Request.Cookies[key];

        } 

        public void RemoveCookie(string key)
        {
            var context = new DefaultHttpContext();

            context.Response.Cookies.Delete(key);
        }
    }
}
