using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class XmlSerializer
    {
        public static IXmlSerializer Default { get; set; }
    }

    public interface IXmlSerializer
    {
        T StringToXmlObject<T>(string xml);
        string XmlObjectToString(object sourceObj, string xmlRootName = "");
    }
}
