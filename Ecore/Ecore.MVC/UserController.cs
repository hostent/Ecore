using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Ecore.Frame;

namespace Ecore.MVC
{
    public class UserController: VisitorController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if(!LoginContext.Default.IsLogin())
            {
                context.Result = Redirect("");
            }

        }
    }
}
