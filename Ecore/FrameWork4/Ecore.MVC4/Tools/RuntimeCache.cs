using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Ecore.MVC4.Tools
{
    public class RuntimeCache : ICache
    {
        private int ticketDefault = 20 * 60;


        private CacheItemPolicy GetPolicy(int second = 0)
        {
            if (second == 0)
            {
                second = ticketDefault;
            }
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy()
            {
                SlidingExpiration = new TimeSpan(0, 0, second)
            };

            return cacheItemPolicy;
        }

        private CacheItemPolicy GetPolicy(DateTime limitTime)
        {
            var cacheItemPolicy = new CacheItemPolicy()
            {
                AbsoluteExpiration = new DateTimeOffset(limitTime)
            };

            return cacheItemPolicy;
        }

        public void Add(string key, object data)
        {
            key = key.ToLower();
            if (MemoryCache.Default.Contains(key))
            {
                MemoryCache.Default[key] = data;
            }
            else
            {
                MemoryCache.Default.Add(key, data, GetPolicy());
            }
        }

        public void Add(string key, object data, DateTime limitTime)
        {
            key = key.ToLower();
            MemoryCache.Default.Add(key, data, GetPolicy(limitTime));
        }

        /// <summary>
        /// 新增缓存
        /// </summary>
        public void Add(string key, object data, int second)
        {
            key = key.ToLower();
            MemoryCache.Default.Add(key, data, GetPolicy(second));
        }


        /// <summary>
        /// 根据key获取单个缓存
        /// </summary>
        public object Get(string key)
        {
            key = key.ToLower();
            if (MemoryCache.Default.Contains(key))
            {
                return (object)MemoryCache.Default[key];
            }
            return null;
        }


        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            key = key.ToLower();
            if (MemoryCache.Default.Contains(key))
            {
                MemoryCache.Default.Remove(key);
            }
        }

        public T Get<T>(string key)
        {
            key = key.ToLower();
            if (MemoryCache.Default.Contains(key))
            {
                return (T)MemoryCache.Default[key];
            }
            return default(T);
        }

        public List<string> FindKeys(string prefix)
        {
            throw new NotImplementedException();
        }
    }
}
