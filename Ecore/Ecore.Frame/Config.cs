using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class Config
    {
        public static IConfig Default { get; set; }

        public static bool IsDebug
        {
            get
            {
                string isdebug = Default.GetAppSetting("IsDebug");

                return isdebug == "1";
            }
        }

        public static string Version
        {
            get
            {
                string version = Default.GetAppSetting("Version");
                if (string.IsNullOrEmpty(version))
                {
                    version = "0.0.0";
                }

                return version;
            }
        }

        public static string LogicClassify
        {
            get
            {
                return Default.GetAppSetting("LogicClassify");
            }           
        }
    }

    public interface IConfig
    {
        string GetConnString(string connName);

        string GetAppSetting(string setKey);

        T GetConfigFile<T>(string fileKey) where T : class, new();

        string GetConfigFile(string fileKey);

    }




}
