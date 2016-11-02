using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4.Web
{
    public class MvcRouter : IRouterExec
    {
        public Task Exec(object context)
        {
            HttpContext httpContent = context as HttpContext;

            string rawUrl = httpContent.Request.RawUrl.Trim('/').ToLower();

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

            throw new Exception("url error");

        }
    }
}
