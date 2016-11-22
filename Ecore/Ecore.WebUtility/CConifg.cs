using Ecore.Frame;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.WebUtility
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


        public IConfigurationRoot Configuration { get; set; }

        public CConifg()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(AppContext.BaseDirectory + "/config.json");

            Configuration = builder.Build();
        }



        public string GetAppSetting(string setKey)
        {
            return Configuration["AppSetting:" + setKey];
        }

        public string GetConnString(string connName)
        {
            return Configuration["ConnString:" + connName];
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
                    string path = AppContext.BaseDirectory + @"Config\" + fileKey + ".json";

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
                    string path = AppContext.BaseDirectory + @"Config\" + fileKey + ".json";
                    if (File.Exists(path))
                    {
                        result = File.ReadAllText(path);
                    }
                    path = AppContext.BaseDirectory + @"Config\" + fileKey + ".xml";
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
