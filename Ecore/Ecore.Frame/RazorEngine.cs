using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class ViewEngine
    {
        public static IViewEngine RazorEngine { get; set; }
    }


    public interface IViewEngine
    {
        string Render(string templateName, object model);
    }


    
}
