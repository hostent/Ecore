using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;

namespace Ecore.MVC4.Tools
{
    public class CConifg : IConfig
    {
        static Dictionary<string, object> _Cache = null;

        static Dictionary<string, object> Cache
        {
            get
            {
                if (_Cache == null)
                {
                    _Cache = new Dictionary<string, object>();
                }

                return _Cache;
            }
        }

        static object _lock = new object();

        public string GetAppSetting(string setKey)
        {
            return ConfigurationManager.AppSettings[setKey];
        }

        public string GetConnString(string connName)
        {
            return ConfigurationManager.ConnectionStrings[connName].ConnectionString;
        }

        public LangItem GetLangItem(string keyLang)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"bin\langSource.json";

            List<LangItem> list = null;

            if (Cache.ContainsKey("GetLangItem"))
            {
                list = Cache["GetLangItem"] as List<LangItem>;
            }
            else
            {
                lock (_lock)
                {
                    if (!Cache.ContainsKey("GetLangItem"))
                    {
                        if(File.Exists(filePath))
                        {
                            string json = File.ReadAllText(filePath);
                            list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LangItem>>(json);
                        }                       
                        if(list==null)
                        {
                            list = new List<LangItem>();
                        }
                        Cache.Add("GetLangItem", list);
                    }
                }
            }

            LangItem item = list.Where(q => q.zh_CN == keyLang).FirstOrDefault();
            if (item == null)
            {
                return new LangItem() { zh_CN = keyLang };
            }

            return item;

        }
    }
}
