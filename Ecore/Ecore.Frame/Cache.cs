using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class Cache
    {      
        public static ICache Default { get; set; }

    }

    public interface ICache
    {
        object Get(string key);

        T Get<T>(string key);

        void Remove(string key);

        void Add(string key, object data);

        void Add(string key, object data, int second);

        void Add(string key, object data, DateTime limitTime);

        List<string> FindKeys(string prefix);
    }
}
