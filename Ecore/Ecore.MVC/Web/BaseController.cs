using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ecore.Frame;
using Ecore.MVC.Api;

namespace Ecore.MVC.Web
{
    public class BaseController
    {
        public HttpContext CurrentContext { get; set; }

        public ICookie Cookie
        {
            get
            {
                return new CookieHelp(CurrentContext);
            }
        }


        public virtual string Render(string templateName, object model)
        {
            return ViewEngine.RazorEngine.Render(templateName, model);
        }

        public virtual string Render(string templateName, object model, IDictionary<string, object> viewBag)
        {
            return ViewEngine.RazorEngine.Render(templateName, model, viewBag);
        }

        public virtual void OnControllerCreate(HttpContext CurrentContext)
        {

        }

        public virtual PageResult OnActionExecting(HttpContext CurrentContext)
        {
            return null;
        }

        public virtual void OnResultExecting(HttpContext CurrentContext, PageResult result)
        {

        }

    }
}
