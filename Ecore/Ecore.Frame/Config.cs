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
    }


}
