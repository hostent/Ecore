using Ecore.Frame;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.MVC4.Web
{
    public class MyRazorEngine : IViewEngine
    {
        public string Render(string templateName, object model)
        {

            return Render(templateName, model, null);

        }

        public string Render(string templateName, object model, IDictionary<string, object> viewBag)
        {
            templateName = AppDomain.CurrentDomain.BaseDirectory + @"View\" + templateName + ".cshtml";


            string template = File.ReadAllText(templateName);

            if (viewBag == null)
            {
                viewBag = new Dictionary<string, object>();
            }
            DynamicViewBag bag = new DynamicViewBag(viewBag);

            var result =
              Engine.Razor.RunCompile(template, "templateKey" + template.GetHashCode(), null, model, bag);

            return result;
        }
    }
}
