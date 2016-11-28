using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ecore.MVC4.Tools
{
    public class XmlHelp: IXmlSerializer
    {

        public string XmlObjectToString(object sourceObj, string xmlRootName = "")
        {
            MemoryStream ms = new MemoryStream();

            Type type = sourceObj.GetType();

            XmlSerializer xmlSerializer = string.IsNullOrWhiteSpace(xmlRootName) ?
                new XmlSerializer(type) :
                new XmlSerializer(type, new XmlRootAttribute(xmlRootName));

            xmlSerializer.Serialize(ms, sourceObj);

            string s = System.Text.Encoding.Default.GetString(ms.ToArray());

            xmlSerializer = null;
            ms.Dispose();

            return s;

        }


        public T StringToXmlObject<T>(string xml)
        {
            byte[] bs2 = System.Text.Encoding.UTF8.GetBytes(xml);

            MemoryStream ms2 = new MemoryStream(bs2);

            Type type = typeof(T);

            XmlSerializer xmlSerializer = new XmlSerializer(type);
            object result = xmlSerializer.Deserialize(ms2);

            return (T)result;


        }


    }
}
