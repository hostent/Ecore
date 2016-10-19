using Ecore.MVC;
using Ecore.MVC.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Manager.Web.Controller
{
    public class DemoController : BaseController
    {
        [Mapping("Demo/list")]
        public StringResult List1()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            data["UserName"] = "fdsajfkds3893";

            string username = "fdsaffdsafdfdfs222";

            var html = Ecore.Razor.RazorEngine.Render("Demo/Index", username);

            Cookie.SetCookie("lkl", "fdsafjdsk2");

            return new StringResult(html);
        }
    }
}
