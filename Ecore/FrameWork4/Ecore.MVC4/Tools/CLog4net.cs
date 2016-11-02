using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;

namespace Ecore.MVC4.Tools
{
    public class CLog4net : Frame.ILog
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CLog4net()
        {

        }

        #region ILog
        public void DebugMsg(string msg)
        {
            log.Info(msg);
        }

        public void Error(Exception ex)
        {
            log.Error(null, ex);
        }

        public void Error(string msg, Exception ex)
        {
            log.Error(msg, ex);
        }

        public void Msg(string msg)
        {
            log.Info(msg);
        }

        public void Msg(string category, string msg)
        {
            log.Info("category:" + category + "Msg:" + msg);
        }

        public void Msg<T>(T msg) where T : class
        {
            log.Info(Newtonsoft.Json.JsonConvert.SerializeObject(msg));
        }

        #endregion




    }
}
