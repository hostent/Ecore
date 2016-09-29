using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ecore.MsSql.XmlSql
{
    public class XmlConfigManager
    {
        private static XElement RepsConfig { get; set; }

        /// <summary>
        /// 获取sql语句xml
        /// </summary>
        /// <param name="moduleTag"></param>
        /// <returns></returns>
        public static XElement GetRepsConfig()
        {
            //if (RepsConfig == null)
            //{
            var files = Directory.GetFiles(AppContext.BaseDirectory.TrimEnd('\\') + "\\XmlReport\\", "*.Reps.xml");
            var obj = new XElement("Reps");
            foreach (var file in files)
            {
                obj.Add(XElement.Load(file).Elements("Rep"));
            }
            RepsConfig = obj;
            //}
            return RepsConfig;
        }
    }
}
