using Ecore.Frame;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecore.RabbitMQ
{
    public class RabbitMessageQueue : IMessageQueue
    {

        public RabbitMessageQueue()
        {
            MqUrl = Config.Default.GetAppSetting("RabbitMessageQueue.Url");
        }

        public RabbitMessageQueue(string mqUrl)
        {
            MqUrl = mqUrl;
        }

        string MqUrl { get; set; }

        static object lockObj = new object();
        static IConnection _Conn = null;
        IConnection Conn
        {
            get
            {
                if (_Conn == null && _Conn.IsOpen == false)
                {
                    lock (lockObj)
                    {
                        if (_Conn == null && _Conn.IsOpen == false)
                        {
                            ConnectionFactory factory = new ConnectionFactory();
                            factory.Uri = MqUrl;//"amqp://user:pass@hostName:port/vhost";

                            IConnection conn = factory.CreateConnection();

                            _Conn = conn;
                        }
                    }
                }

                return _Conn;
            }
        }


        public T Pop<T>(string queueKey)
        {
            IModel channel = Conn.CreateModel();
            channel.QueueDeclare(queueKey, false, false, false, null);
            BasicGetResult result = channel.BasicGet(queueKey, true);

            channel.Close();
            if (result != null)
            {
                string message = Encoding.UTF8.GetString(result.Body);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(message);
            }

            return default(T);
        }


        public long Push<T>(string queueKey, T value)
        {

            IModel channel = Conn.CreateModel();

            channel.QueueDeclare(queueKey, false, false, false, null);
            byte[] message = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(value));
            channel.BasicPublish(string.Empty, queueKey, null, message);

            channel.Close();

            return 1;
        }


        public void Publish(string channel, string message)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string channel, Action<string, string> act)
        {
            throw new NotImplementedException();
        }

        public long Push(string queueKey, string value)
        {
            IModel channel = Conn.CreateModel();

            channel.QueueDeclare(queueKey, false, false, false, null);
            byte[] message = Encoding.UTF8.GetBytes(value);
            channel.BasicPublish(string.Empty, queueKey, null, message);

            channel.Close();

            return 1;
        }

        public string Pop(string queueKey)
        {
            IModel channel = Conn.CreateModel();
            channel.QueueDeclare(queueKey, false, false, false, null);
            BasicGetResult result = channel.BasicGet(queueKey, true);

            channel.Close();
            if (result != null)
            {
                string message = Encoding.UTF8.GetString(result.Body);

                return message;
            }

            return "";
        }
    }
}
