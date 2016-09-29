using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class MessageQueue
    {
        public static IMessageQueue Default { get; set; }
    }

    public interface IMessageQueue
    {

        #region 生产者消费者模式

        long Push<T>(string queueKey, T value);

        T Pop<T>(string queueKey);

        #endregion

        #region 发布订阅模式
        /// <summary>
        /// 发布订阅模式
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="message"></param>
        void Publish(string channel, string message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="act">参数：channel，message</param>
        void Subscribe(string channel, Action<string, string> act);

        #endregion

    }
}
