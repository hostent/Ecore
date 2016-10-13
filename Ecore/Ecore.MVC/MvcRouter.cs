using Ecore.Frame;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ecore.MVC
{
    public class MvcRouter : IRouterExec
    {
        public Task Exec(object context)
        {
             HttpContext httpContent = context as HttpContext;

            return Task.Run(() =>
            {
                string rawUrl = httpContent.Request.Path.Value.Trim('/').ToLower();

                var list = MappingManager.Store.OrderBy(q => q.Index).ToList();
                //container
                foreach (var item in list)
                {
                    if (rawUrl == item.Url.ToLower())
                    {
                        item.Exec();
                        return;
                    }
                }

                //Regex
                foreach (var item in list)
                {
                    Regex reg = new Regex(item.Url);
                    if (reg.IsMatch(rawUrl))
                    {
                        item.Exec();
                        return;
                    }
                }

                throw new Exception("url error");
            });
        }
    }
}
