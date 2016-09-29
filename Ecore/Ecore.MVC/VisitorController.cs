using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Ecore.Frame;

namespace Ecore.MVC
{
    public class VisitorController : Controller
    {
        public IFactory Factory = new Factory();



    }
}
