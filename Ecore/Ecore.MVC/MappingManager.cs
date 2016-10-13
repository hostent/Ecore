using Ecore.Frame;
using Ecore.Frame.Security;
using Ecore.MVC.Api;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ecore.MVC
{
    public class MappingManager
    {
        public static List<MappingStore> Store = null;

        static object _Lock = new object();

        public static void AddMvc(string controllerPath, string dllName)
        {
            lock (_Lock)
            {
                if (Store == null)
                {
                    Store = new List<MappingStore>();
                }
                else
                {
                    return;
                }
                Assembly ass = AssemblyHelp.LoadAss(dllName);

                Type[] controllerTypes = ass.GetTypes();

                foreach (var controllerItem in controllerTypes.Where(q => q.FullName.Contains(controllerPath)))
                {
                    foreach (var action in controllerItem.GetMethods())
                    {
                        MappingAttribute map = action.GetCustomAttribute<MappingAttribute>();

                        if (map == null)
                        {
                            continue;
                        }
                        Store.Add(new MappingStore()
                        {
                            Url = map.Url,
                            Index = map.Index,
                            CacheSecond = map.CacheSecond,
                            Controller = (BaseController)ass.CreateInstance(controllerItem.FullName),
                            Action = action
                        });
                    }
                }
            }
        }
    }

    public class MappingStore : MappingAttribute
    {
        public BaseController Controller { get; set; }

        public MethodInfo Action { get; set; }


        public void Exec()
        {
            bool canCache = false;
            string key = "";

            HttpContext Context = new DefaultHttpContext();
            if (Context.Request.Method.ToLower() == "get" && base.CacheSecond > 0)
            {
                canCache = true;
                key = "pageCache:" + MD5Helper.Encrypt_MD5(Context.Request.Path);
            }

            PageResult result = null;
            if (canCache)
            {
                result = Cache.Default.Get<PageResult>(key);
                if (result == null)
                {
                    result = (PageResult)Action.Invoke(Controller, null);

                    Cache.Default.Add(key, result, DateTime.Now.AddSeconds(CacheSecond));
                }

                result.RenderResult();
                return;
            }

            result = (PageResult)Action.Invoke(Controller, null);

            result.RenderResult();

        }

    }


    [AttributeUsage(AttributeTargets.Method)]
    public class MappingAttribute : Attribute
    {
        public MappingAttribute()
        {

        }
        public MappingAttribute(string url)
        {
            Url = url;
        }
        public string Url { get; set; }

        public int Index { get; set; }

        /// <summary>
        /// 0:表示不缓存，默认0
        /// </summary>
        public int CacheSecond { get; set; }



    }
}
