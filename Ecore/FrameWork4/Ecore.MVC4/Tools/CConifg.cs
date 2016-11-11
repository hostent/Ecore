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


        public T GetConfigFile<T>(string fileKey) where T : class, new()
        {
            string cacheKey = "CConifg.ConfigFile." + fileKey;
            if (Cache.ContainsKey(cacheKey))
            {
                return (T)Cache[cacheKey];
            }
        
            T result = default(T);

            lock (_lock)
            {
                if (!Cache.ContainsKey(cacheKey))
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"Config\" + fileKey + ".json";

                    if (File.Exists(path))
                    {
                        string json = File.ReadAllText(path);
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                    }

                    if (result == null)
                    {
                        result = new T();
                    }
                    Cache.Add(cacheKey, result);
                }
            }
            return result;

        }

        public string GetConfigFile(string fileKey)
        {

            string cacheKey = "CConifg.ConfigFile." + fileKey;
            if (Cache.ContainsKey(cacheKey))
            {
                return (string)Cache[cacheKey];
            }

            string result = "";

            lock (_lock)
            {
                if (!Cache.ContainsKey(cacheKey))
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"Config\" + fileKey + ".json";
                    if (File.Exists(path))
                    {
                        result = File.ReadAllText(path);
                    }
                    path = AppDomain.CurrentDomain.BaseDirectory + @"Config\" + fileKey + ".xml";
                    if (File.Exists(path))
                    {
                        result = File.ReadAllText(path);
                    }

                    Cache.Add(cacheKey, result);
                }
            }
            return result;
        }
    }
}
