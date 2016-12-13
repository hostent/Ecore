using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Ecore.MVC4.Tools
{
    public class XmlHelp : IXmlSerializer
    {

        public string XmlObjectToString(object sourceObj, Encoding encoding = null, string namespacesKey = null, string namespacesValue = null)
        {
            if (sourceObj == null)
                throw new ArgumentNullException("sourceObj");
            if (encoding == null)
                encoding = UTF8Encoding.Default;

            string xml = "";
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Encoding = encoding;

                    //强制指定命名空间，覆盖默认的命名空间，可以添加多个，如果要在xml节点上添加指定的前缀，
                    //可以在跟节点的类上面添加[XmlRoot(Namespace = "http://www.w3.org/2001/XMLSchema-instance", IsNullable = false)]，Namespace指定哪个值，xml节点添加的前缀就是哪个命名空间(这里会添加ceb)
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    if (!string.IsNullOrEmpty(namespacesKey))
                    {
                        namespaces.Add(namespacesKey, namespacesValue);
                        namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    }
                    else
                    {
                        //OmitXmlDeclaration表示不生成声明头，默认是false，OmitXmlDeclaration为true，会去掉<?xml version="1.0" encoding="UTF-8"?>
                        //settings.OmitXmlDeclaration = true;
                        settings.OmitXmlDeclaration = true;

                        namespaces.Add(string.Empty, string.Empty);
                    }

                    XmlWriter writer = XmlWriter.Create(stream, settings);

                    XmlSerializer serializer = new XmlSerializer(sourceObj.GetType());
                    serializer.Serialize(writer, sourceObj, namespaces);
                    writer.Close();

                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        xml = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return xml;

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

        public IXmlSerializer Get()
        {
            return new XmlHelp();
        }
    }
}
