using Ecore.Frame;
using Ecore.MVC4.Api;
using Ecore.MVC4.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4.Web
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


        protected string Render(string templateName, object model)
        {

            return ViewEngine.RazorEngine.Render(templateName, model);


        }

        public virtual void OnControllerCreate(HttpContext CurrentContext)
        {

        }

        public virtual PageResult OnActionExecting(HttpContext CurrentContext)
        {
            return null;
        }

    }
}
