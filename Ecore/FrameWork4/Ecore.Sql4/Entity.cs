using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ecore.Sql4
{
    public class Entity<T> where T : class, new()
    {

        protected string[] GetColumns(bool isIncludeID)
        {
            var t = typeof(T);
            PropertyInfo[] minfos = t.GetProperties();

            if (isIncludeID == false)
            {
                return minfos.Where(q =>
                {
                    var keyAttr = (KeyAttribute)q.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();
                    //q.GetCustomAttribute<KeyAttribute>();
                    if (keyAttr != null)
                    {
                        return false;
                    }
                    return true;
                }).Select(q => q.Name).ToArray();
            }
            else
            {
                return minfos.Select(q => q.Name).ToArray();
            }

        }

        protected string GetCacheTag()
        {
            return "table:" + typeof(T).FullName;
        }

        protected bool CanCache()
        {
            var t = typeof(T);
            CacheAttribute ca = (CacheAttribute)t.GetCustomAttributes(typeof(CacheAttribute), false).FirstOrDefault();
            if (ca == null)
            {
                return false;
            }
            return true;
        }

        protected string GetKey()
        {
            var t = typeof(T);
            PropertyInfo[] minfos = t.GetProperties();

            string key = minfos.Where(q =>
            {
                var keyAttr = (KeyAttribute)q.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();// q.GetCustomAttribute<KeyAttribute>();
                if (keyAttr != null)
                {
                    return true;
                }
                return false;
            }).Select(q => string.Format("{0}", q.Name)).FirstOrDefault();

            return key;
        }


        protected string GetUniqueKey()
        {
            var t = typeof(T);
            PropertyInfo[] minfos = t.GetProperties();

            string key = minfos.Where(q =>
            {
                var keyAttr = (UniqueAttribute)q.GetCustomAttributes(typeof(UniqueAttribute), false).FirstOrDefault();// q.GetCustomAttribute<KeyAttribute>();
                if (keyAttr != null)
                {
                    return true;
                }
                return false;
            }).Select(q => string.Format("{0}", q.Name)).FirstOrDefault();

            return key;
        }



    }
}
