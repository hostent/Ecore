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
    public class MyRazorEngine: IViewEngine
    {
        public string Render(string templateName, object model)
        {
            templateName = AppDomain.CurrentDomain.BaseDirectory + @"View\" + templateName + ".cshtml";


            string template = File.ReadAllText(templateName);
           
           var result =
             Engine.Razor.RunCompile(template, "templateKey" + template.GetHashCode(), null, model);

            return result;


        }
    }
}
