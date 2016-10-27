using Ecore.Frame;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.MVC.Tools
{
    public class CConifg : IConfig
    {

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

        public LangItem GetLangItem(string keyLang)
        {
            throw new NotImplementedException();
        }
    }
}
