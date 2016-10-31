using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Redis4
{
    public class CLock : ILock
    {
        int DatabaseName
        {
            get
            {
                return 0;
            }
        }


        public bool Lock(string tag)
        {
            var redisClient = new Manager().RedisManager;
            var db = redisClient.GetDatabase(DatabaseName);

            if (db.StringGet(tag).HasValue)
            {
                return false;
            }
            db.StringSet(tag, tag);

            redisClient.CloseAsync();

            return true;
        }

        public void Release(string tag)
        {
            var redisClient = new Manager().RedisManager;
            var db = redisClient.GetDatabase(DatabaseName);

            db.KeyDelete(tag);

            redisClient.CloseAsync();


        }
    }
}
