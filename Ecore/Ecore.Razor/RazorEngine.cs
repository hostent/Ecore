using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Ecore.Razor
{
    public class RazorEngine
    {
        static IDictionary<string, RazorViewTemplate> viewCache = new ConcurrentDictionary<string, RazorViewTemplate>();

        public static string Render(string viewName, object model)
        {
            RazorViewTemplate razorViewTemplate = null;

            string templateName = viewName.GetHashCode().ToString();

            if (!viewCache.TryGetValue(templateName, out razorViewTemplate))
            {
                string viewTemplate = LoadTemplateContent(viewName);  //"User/List"

                CodeGenerateService codeGenerater = new CodeGenerateService();
                var generateResult = codeGenerater.Generate(model.GetType(), viewTemplate);


                RoslynCompileService service = new RoslynCompileService();
                var type = service.Compile(generateResult.GeneratedCode);

                if (type == null) return "";

                razorViewTemplate = (RazorViewTemplate)Activator.CreateInstance(type);


                viewCache.Add(templateName, razorViewTemplate);

            }

            razorViewTemplate.SetModel(model);
            //razorViewTemplate.setViewBag(null);

            razorViewTemplate.Execute().Wait();

            string html = razorViewTemplate.Result;

            return html;


        }

        static string LoadTemplateContent(string viewName)
        {
            return System.IO.File.ReadAllText(
                @"D:\code\MyProject\Ecore\Project\EMin.Manager.Web\src\EMin.Manager.Web\View\Demo\Index.cshtml");
        }


    }
}
