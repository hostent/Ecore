using Ecore.Frame;
using Ecore.Frame.Security;
using Ecore.MVC4.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4.Web
{
    public class MvcMapFactory
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
                        MappingAttribute map = (MappingAttribute)action.GetCustomAttributes(typeof(MappingAttribute), false).FirstOrDefault();//.GetCustomAttribute<MappingAttribute>();

                        if (map == null)
                        {
                            continue;
                        }
                        Store.Add(new MappingStore()
                        {
                            Url = map.Url,
                            Index = map.Index,
                            CacheSecond = map.CacheSecond,
                            Controller = controllerItem,//(BaseController)ass.CreateInstance(controllerItem.FullName),
                            Action = action
                        });
                    }
                }
            }
        }
    }

    public class MappingStore : MappingAttribute
    {
        public Type Controller { get; set; }

        public MethodInfo Action { get; set; }


        public void Exec(HttpContext httpContent)
        {
            try
            {
                bool canCache = false;
                string key = "";

                if (httpContent.Request.HttpMethod.ToLower() == "get" && base.CacheSecond > 0)
                {
                    canCache = true;
                    key = "pageCache:" + MD5Helper.Encrypt_MD5(httpContent.Request.Path);
                }

                PageResult result = null;
                BaseController controllerObj = null;
                if (canCache)
                {
                    result = Cache.Default.Get<PageResult>(key);
                    if (result == null)
                    {
                        controllerObj = (BaseController)Assembly.GetAssembly(Controller).CreateInstance(Controller.FullName);

                        controllerObj.CurrentContext = httpContent;
                        controllerObj.OnControllerCreate(httpContent);

                        result = (PageResult)Action.Invoke(controllerObj, null);

                        Cache.Default.Add(key, result, DateTime.Now.AddSeconds(CacheSecond));
                    }

                    result.RenderResult(httpContent);
                }


                controllerObj = (BaseController)Assembly.GetAssembly(Controller).CreateInstance(Controller.FullName);

                controllerObj.CurrentContext = httpContent;
                controllerObj.OnControllerCreate(httpContent);

                result = controllerObj.OnActionExecting(httpContent);
                if (result == null)
                {
                    result = (PageResult)Action.Invoke(controllerObj, null);
                }

                result.RenderResult(httpContent);
            }
            catch (Exception ee)
            {
                Exception temp = null;

                if (ee.InnerException != null)
                {
                    temp = ee.InnerException;
                }
                else
                {
                    temp = ee;
                }

                httpContent.Response.Write("错误信息：" + ee.Message + "\r\n <p/>");

                httpContent.Response.Write("堆栈信息：" + ee.StackTrace + "\r\n <p/>");

                httpContent.Response.Write("所有信息：" + ee.ToString() + "\r\n <p/>");
                httpContent.Response.Charset = "utf-8";
                httpContent.Response.ContentEncoding = Encoding.UTF8;
                httpContent.Response.ContentType = "text/html";
                httpContent.Response.StatusCode = 200;
               
            }

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
