using Ecore.Frame;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ecore.MVC
{
    public class MvcRouter : IRouterExec
    {
        public Task Exec(object context)
        {
            HttpContext httpContent = context as HttpContext;

            string rawUrl = httpContent.Request.Path.Value.Trim('/').ToLower();

            var list = MvcMapFactory.Store.OrderBy(q => q.Index).ToList();
            //container
            foreach (var item in list)
            {
                if (rawUrl == item.Url.ToLower())
                {
                    return item.Exec(httpContent);
                }
            }

            //Regex
            foreach (var item in list)
            {
                Regex reg = new Regex(item.Url);
                if (reg.IsMatch(rawUrl))
                {
                    return item.Exec(httpContent);
                }
            }

            httpContent.Response.ContentType = "text/html; charset=utf-8";
 
            return httpContent.Response.WriteAsync("地址错误", Encoding.UTF8);

        }
    }
}
