using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Redis
{
    public class CCache : ICache
    {
        int DatabaseName
        {
            get
            {
                return 0;
            }
        }

        public void Add(string key, object data)
        {
            var redisClient = new Manager().RedisManager;
            var db = redisClient.GetDatabase(DatabaseName);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            db.StringSet(key, json);

            redisClient.CloseAsync();

        }

        public void Add(string key, object data, DateTime limitTime)
        {
            var redisClient = new Manager().RedisManager;
            var db = redisClient.GetDatabase(DatabaseName);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            db.StringSet(key, json, limitTime - DateTime.Now);

            redisClient.CloseAsync();
        }

        public void Add(string key, object data, int second)
        {
            var redisClient = new Manager().RedisManager;
            var db = redisClient.GetDatabase(DatabaseName);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            db.StringSet(key, json, TimeSpan.FromSeconds(second));

            redisClient.CloseAsync();
        }

        public List<string> FindKeys(string prefix)
        {
            
            throw new NotImplementedException();
        }

        public object Get(string key)
        {
            var redisClient = new Manager().RedisManager;
            var db = redisClient.GetDatabase(DatabaseName);

            var item= db.StringGet(key);

            redisClient.CloseAsync();

            return item.HasValue ? item.ToString() : null;
        }

        public T Get<T>(string key)
        {
            var redisClient = new Manager().RedisManager;
            var db = redisClient.GetDatabase(DatabaseName);

            var  json = db.StringGet(key);

            redisClient.CloseAsync();

            if (!json.HasValue)
            {
                return default(T);
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json.ToString());
        }

        public void Remove(string key)
        {
            var redisClient = new Manager().RedisManager;
            var db = redisClient.GetDatabase(DatabaseName);

            db.KeyDelete(key);

            redisClient.CloseAsync();
        }
    }
}
