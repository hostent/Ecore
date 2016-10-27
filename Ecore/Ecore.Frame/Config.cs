using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class Config
    {
        public static IConfig Default { get; set; }
    }

    public interface IConfig
    {
        string GetConnString(string connName);

        string GetAppSetting(string setKey);

        LangItem GetLangItem(string keyLang);
    }

    public class LangItem 
    {
        public string zh_CN { get; set; }
        public string en { get; set; }
        public string ja_JP { get; set; }
        public string ko_KR { get; set; }
    }


}
