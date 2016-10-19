using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ecore.Frame;
using Ecore.MVC.Tools;

namespace Ecore.MVC
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


    }
}
