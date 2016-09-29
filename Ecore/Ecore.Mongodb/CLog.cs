using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace Ecore.Mongodb
{
    public class CLog : ILog
    {
        public CLog()
        {
        }

        string logDbName = "MayanLogDb";

        public void DebugMsg(string msg)
        {
            if (CurrentLevel == LogLevel.Debug)
            {
                MessageEntity entity = new MessageEntity();
                FillBase(entity);
                entity.Message = msg;

                Log(entity);
            }
        }

        public void Error(Exception ex)
        {
            ErrorEntity entity = new ErrorEntity();
            FillBase(entity);
            entity.ErrorMessage = GetErrorMsg(ex);
            Log(entity);
        }

        public void Error(string msg, Exception ex)
        {
            ErrorEntity entity = new ErrorEntity();
            FillBase(entity);
            entity.ErrorMessage = GetErrorMsg(ex);
            entity.Message = msg;
            Log(entity);

        }

        public void Msg(string msg)
        {
            MessageEntity entity = new MessageEntity();
            FillBase(entity);
            entity.Category = "";
            entity.Message = msg;

            Log(entity);


        }

        public void Msg(string category, string msg)
        {
            MessageEntity entity = new MessageEntity();
            FillBase(entity);

            entity.Category = category;
            entity.Message = msg;

            Log(entity);

        }

        public void Msg<T>(T msg) where T : class
        {
            MessageEntity<T> cMsg = new MessageEntity<T>();

            FillBase(cMsg);

            cMsg.CustomerMsg = msg;

            Log(cMsg);


        }




        private string GetErrorMsg(Exception e)
        {
            string msg = "";

            Exception tag = e;
            for (int i = 0; i < 5; i++)
            {
                if (tag == null)
                {
                    break;
                }
                msg = msg + tag.Message;
                msg = msg + "\r\n ---------------------------堆栈信息------------------------------- \r\n ";
                msg = msg + tag.StackTrace;

                msg = msg + "\r\n --------------------------------------------------------------------------------------- \r\n ";
                tag = tag.InnerException;
            }

            return msg;

        }

        private static LogLevel CurrentLevel
        {
            get
            {
                //if (Convert.ToBoolean(AppConfig.IsDebugLog()))
                //{
                //    return LogLevel.Debug;
                //}
                return LogLevel.Release;
            }
        }

        private void FillBase(BaseEntity entity)
        {
            //HttpContext httpContext = HttpContext.Current;

            entity.AppPath = AppContext.BaseDirectory;
            //if (httpContext != null)
            //{
            //    entity.Browser = httpContext.Request.Browser.Type + "," + httpContext.Request.Browser.Version;

            //    entity.UserId = OpUserID;
            //    entity.UserName = OpUserName;

            //    entity.UserIP = HttpUtils.GetIp();
            //    entity.PageUrl = httpContext.Request.Url.ToString();
            //}

            entity.Createtime = DateTime.Now;


            // entity.MachineName = new DnsEndPoint()..Host;
            entity.ThreadName = Thread.CurrentThread.Name;
            entity.Track = "";

        }

        private void LogError(object errorEntity)
        {
            ErrorEntity entity = (ErrorEntity)errorEntity;
            Log(entity);

        }

        private void LogMessage(object messageEntity)
        {
            MessageEntity entity = (MessageEntity)messageEntity;
            Log(entity);

        }

        private void Log<T>(T t) where T : class
        {
            var database = new Manager().Client.GetDatabase(logDbName);



            try
            {

                var collection = database.GetCollection<T>(t.GetType().Name);

                collection.InsertOneAsync(t);

            }
            catch
            {
                // Don't do anything.
            }

        }
    }
}
