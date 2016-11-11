using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using System.IO;

namespace Ecore.MVC4.Tools
{
    public class CLog4net : Frame.ILog
    {

     

        public CLog4net()
        {

        }

        #region ILog
        public void DebugMsg(string msg)
        {
            Log.Debug(msg);

            
        }

        public void Error(Exception ex)
        {
            Log.Error(null, ex);
        }

        public void Error(string msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public void Msg(string msg)
        {
            Log.Info(msg);
        }

        public void Msg(string category, string msg)
        {
            Log.Info("category:" + category + "Msg:" + msg);
        }

        public void Msg<T>(T msg) where T : class
        {
            Log.Info(Newtonsoft.Json.JsonConvert.SerializeObject(msg));
        }

        #endregion




    }

    public class Log
    {

        private const string SError = "Error";

        private const string SDebug = "Debug";

        private const string DefaultName = "Info";



        static Log()
        {

            var path = AppDomain.CurrentDomain.BaseDirectory + @"\log4net.xml";

            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));

        }



        public static log4net.ILog GetLog(string logName)
        {

            var log = log4net.LogManager.GetLogger(logName);

            return log;

        }



        public static void Debug(string message)
        {

            var log = log4net.LogManager.GetLogger(SDebug);

            if (log.IsDebugEnabled)

                log.Debug(message);

        }



        public static void Debug(string message, Exception ex)
        {

            var log = log4net.LogManager.GetLogger(SDebug);

            if (log.IsDebugEnabled)

                log.Debug(message, ex);

        }



        public static void Error(string message)
        {

            var log = log4net.LogManager.GetLogger(SError);

            if (log.IsErrorEnabled)

                log.Error(message);

        }



        public static void Error(string message, Exception ex)
        {

            var log = log4net.LogManager.GetLogger(SError);

            if (log.IsErrorEnabled)

                log.Error(message, ex);

        }



        public static void Fatal(string message)
        {

            var log = log4net.LogManager.GetLogger(DefaultName);

            if (log.IsFatalEnabled)

                log.Fatal(message);

        }



        public static void Info(string message)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(DefaultName);

            if (log.IsInfoEnabled)

                log.Info(message);

        }



        public static void Warn(string message)
        {

            var log = log4net.LogManager.GetLogger(DefaultName);

            if (log.IsWarnEnabled)

                log.Warn(message);

        }

    }
}
