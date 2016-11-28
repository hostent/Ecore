using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class XmlSerializerHelp
    {
        public static IXmlSerializer Default { get; set; }
    }

    public interface IXmlSerializer
    {
        T StringToXmlObject<T>(string xml);
        string XmlObjectToString(object sourceObj, Encoding encoding = null, string namespacesKey = null, string namespacesValue = null);

        IXmlSerializer Get();

    }
}
