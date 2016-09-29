using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class Log
    {
        public static ILog Default { get; set; }
    }

    public interface ILog
    {
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        void Error(string msg, Exception ex);


        /// <summary>
        /// 记录跟踪消息
        /// </summary>
        /// <param name="msg"></param>
        void Msg(string msg);

        /// <summary>
        /// 记录跟踪消息,带标题
        /// </summary>
        /// <param name="category"></param>
        /// <param name="msg"></param>
        void Msg(string category, string msg);

        /// <summary>
        /// 记录调试信息，在Release模式下，不会记录.
        /// </summary>
        /// <param name="msg"></param>
        void DebugMsg(string msg);

        /// <summary>
        /// 自定义日志信息
        /// </summary>
        /// <typeparam name="T">日志信息类型</typeparam>
        /// <param name="msg">日志信息</param>
        void Msg<T>(T msg) where T : class;
    }
}
