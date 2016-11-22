using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Redis
{
    public class CMessageQueue : IMessageQueue
    {

        int DatabaseName
        {
            get
            {
                return 2;
            }
        }


        public T Pop<T>(string queueKey)
        {
            var redisClient = new Manager().RedisManager;
            var val = redisClient.GetDatabase(DatabaseName).ListRightPop(queueKey);
            redisClient.CloseAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(val);
        }
        public long Push<T>(string queueKey, T value)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            var redisClient = new Manager().RedisManager;
            var index = redisClient.GetDatabase(DatabaseName).ListLeftPush(queueKey, json);
            redisClient.CloseAsync();
            return index;
        }


        //这个模式有问题，测试不通过，后续再搞
        public void Publish(string channel, string message)
        {
            var redisClient = new Manager().RedisManager;
            var sub = redisClient.GetSubscriber();
            sub.PublishAsync(channel, message);
            redisClient.CloseAsync();
        }



        public void Subscribe(string channel, Action<string, string> act)
        {
            var redisClient = new Manager().RedisManager;
            var sub = redisClient.GetSubscriber();
            sub.Subscribe("message", (chl, msg) =>
            {
                act(chl, msg);
            });

            redisClient.CloseAsync();
        }

        public long Push(string queueKey, string value)
        {
            var redisClient = new Manager().RedisManager;
            var index = redisClient.GetDatabase(DatabaseName).ListLeftPush(queueKey, value);
            redisClient.CloseAsync();
            return index;
        }

        public string Pop(string queueKey)
        {
            var redisClient = new Manager().RedisManager;
            var val = redisClient.GetDatabase(DatabaseName).ListRightPop(queueKey);
            redisClient.CloseAsync();

            return val;
        }
    }
}
