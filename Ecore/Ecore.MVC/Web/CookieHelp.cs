﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ecore.Frame;

namespace Ecore.MVC.Web
{
    public class CookieHelp: ICookie
    {
        HttpContext CurrentContext { get; set; }

        public CookieHelp(HttpContext context)
        {
            CurrentContext = context;
        }

        public void SetCookie(string key, string value)
        {
            CurrentContext.Response.Cookies.Append(key, value, new CookieOptions()
            {
                // Domain = SiteAppConfig.Domain,
                Path = "/",
                HttpOnly = true,
            });

        }
        public void SetCookie(string key, string value, DateTime Expires)
        {
            CurrentContext.Response.Cookies.Append(key, value, new CookieOptions()
            {
                // Domain = SiteAppConfig.Domain,
                Path = "/",
                HttpOnly = true,
                Expires = Expires,
            });

        }
        public void SetCookie(string key, string value, DateTime Expires, string domain)
        {
            CurrentContext.Response.Cookies.Append(key, value, new CookieOptions()
            {
                Domain = domain,
                Path = "/",
                HttpOnly = true,
                Expires = Expires,
            });
        }

        public string GetCookieValue(string key)
        {
            return CurrentContext.Request.Cookies[key];

        } 

        public void RemoveCookie(string key)
        {
            CurrentContext.Response.Cookies.Delete(key);
        }
    }
}
