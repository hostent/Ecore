using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4.Web
{
    public class MvcRouter
    {
        public void Exec(object context)
        {
            HttpContext httpContent = context as HttpContext;

            string rawUrl = httpContent.Request.Path.Trim('/').ToLower();

            if (string.IsNullOrEmpty(rawUrl))
            {//未指明具体地址
                var defaultHomePage = Config.Default.GetAppSetting("DefaultHomePage") ;
                if (string.IsNullOrEmpty(defaultHomePage))
                {
                    defaultHomePage = "index.html";
                }

                rawUrl = defaultHomePage;
            }

            var list = MvcMapFactory.Store.OrderBy(q => q.Index).ToList();
            //container
            foreach (var item in list)
            {
                if (rawUrl == item.Url.ToLower())
                {
                    item.Exec(httpContent);
                    return;
                }
            }

            //Regex
            foreach (var item in list)
            {
                Regex reg = new Regex(item.Url);
                if (reg.IsMatch(rawUrl))
                {
                    item.Exec(httpContent);
                    return;
                }
            }

            httpContent.Response.Write("地址错误");
            httpContent.Response.ContentType = "text/html";
            httpContent.Response.ContentEncoding = Encoding.UTF8;

        }
    }
}
