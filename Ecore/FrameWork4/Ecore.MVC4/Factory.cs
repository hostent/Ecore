using Autofac;
using Ecore.Frame;
using Ecore.MVC4.Web;
using Ecore.Proxy4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ecore.MVC4
{
    public class Factory : IFactory
    {

        IContainer container = null;
        public Factory()
        {
        }



        public T Get<T>() where T : class
        {
            if (container != null && container.IsRegistered<T>())
            {
                return container.Resolve<T>();
            }

            Type tImp = new Analyze<T>().GetInstanceType();
            if (tImp == null)
            {
                throw new Exception("Imp is null");
            }
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<T>();
            var register = containerBuilder.RegisterType(tImp).As<T>();

            if (container == null)
            {
                container = containerBuilder.Build();
            }
            else
            {
                containerBuilder.Update(container);
            }

            return container.Resolve<T>();

        }

        //先从本地接口，如果发现本地没有，在读取远程接口。


        public static void Init()
        {
            Ecore.Frame.Cache.Default = new Ecore.Redis4.CCache();
            //Ecore.Frame.Cookie.Default = new Tools.CookieHelp();
            Ecore.Frame.IDGenerator.Default = new Ecore.Redis4.CIDGenerator();
            //LockUser.Default = new Ecore.Redis.CLock();
            //Log.Default = new Ecore.Mongodb.CLog();
            Log.Default = new Tools.CLog4net();
            //LoginContext.Default = new CLoginContext();
            MessageQueue.Default = new Ecore.Redis4.CMessageQueue();
            MyHttpClient.Default = new Ecore.MVC4.Tools.HttpClientHelp();
            UContainer.Factory = new Factory();
            //Ecore.Frame.Weixin.Account = new Ecore.MVC.Weixin.Manager();
            Config.Default = new Tools.CConifg();
            MyEncoding.Default = new Ecore.MVC4.Tools.MyEncoding();

            //Router.MvcHandle = new MvcRouter();
            //Router.RestApiHandle = new RestApiRouter();
            //Router.WebSocketHandle = new WebSocketRouter();
            MyProxy.Default = new Client();

            Ecore.Frame.ViewEngine.RazorEngine = new MyRazorEngine();

            XmlSerializerHelp.Default = new Tools.XmlHelp();

        }
    }




    public class Analyze<T>
    {
        public Analyze()
        {
            string interfaceFullName = typeof(T).FullName;
            string interfaceName = typeof(T).Name;
            string[] interfaceFullNameArr = interfaceFullName.Split('.');

            Module = interfaceFullNameArr[1];
            if (Module != "Model")
            {

                var arr = interfaceFullName.Replace(string.Format("EMin.{0}.Model.Service.", Module), "").Split('.').ToList();
                arr.RemoveAt(arr.Count - 1);
                var subClass = (string.Join(".", arr) + ("." + interfaceName).Replace(".I", ".C")).Trim('.');


                //Controller
                ControllerDll = string.Format("EMin.{0}.Web", Module);
                ControllerClass = string.Format("{0}.Controller.{1}", ControllerDll, subClass);

                //Logic
                LogicDll = string.Format("EMin.{0}.Logic", Module);
                LogicClass = string.Format("{0}.{1}", LogicDll, subClass);

                //Proxy
                ProxyDll = string.Format("EMin.{0}.Model", Module);
                ProxyClass = string.Format("{0}.Proxy.{1}", ProxyDll, subClass);


            }
            else
            {
                var arr = interfaceFullName.Replace(string.Format("EMin.Model.Service"), "").Split('.').ToList();
                arr.RemoveAt(arr.Count - 1);
                var subClass = (string.Join(".", arr) + ("." + Module).Replace(".I", ".C")).Trim('.');


                //Logic
                LogicDll = "EMin.Model.Logic";
                LogicClass = string.Format("{0}.{1}", LogicDll, subClass);

            }



        }

        public static string BaseDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(Config.Default.GetAppSetting("BaseDirectory")))
                {
                    return AppDomain.CurrentDomain.BaseDirectory + "bin\\";
                }
                return Config.Default.GetAppSetting("BaseDirectory");

            }
        }

        public T CreateInstance()
        {
            //controller           

            if (File.Exists(BaseDirectory + ControllerDll + ".dll"))
            {
                Assembly cAss = LoadAss(this.ControllerDll);
                Type classType = cAss.GetType(this.ControllerClass);
                if (classType != null && classType.GetInterfaces().Contains(typeof(T)))
                {
                    CurrentDll = ControllerDll;
                    CurrentClass = ControllerClass;

                    return (T)cAss.CreateInstance(CurrentClass);
                }
            }

            //logic
            if (File.Exists(BaseDirectory + LogicDll + ".dll"))
            {
                Assembly cAss = LoadAss(this.LogicDll);
                Type classType = cAss.GetType(this.LogicClass);
                if (classType != null && classType.GetInterfaces().Contains(typeof(T)))
                {
                    CurrentDll = LogicDll;
                    CurrentClass = LogicClass;

                    return (T)cAss.CreateInstance(CurrentClass);
                }
            }

            //Proxy
            if (File.Exists(BaseDirectory + ProxyDll + ".dll"))
            {
                Assembly cAss = LoadAss(this.ProxyDll);
                Type classType = cAss.GetType(this.ProxyClass);
                if (classType != null && classType.GetInterfaces().Contains(typeof(T)))
                {
                    CurrentDll = ProxyDll;
                    CurrentClass = ProxyClass;

                    return (T)cAss.CreateInstance(CurrentClass);
                }
            }

            return default(T);


        }

        public Type GetInstanceType()
        {
            //controller           

            if (File.Exists(BaseDirectory + ControllerDll + ".dll"))
            {
                Assembly cAss = LoadAss(this.ControllerDll);
                Type classType = cAss.GetType(this.ControllerClass);
                if (classType != null && classType.GetInterfaces().Contains(typeof(T)))
                {
                    return classType;
                }
            }

            //logic
            if (File.Exists(BaseDirectory + LogicDll + ".dll"))
            {
                Assembly cAss = LoadAss(this.LogicDll);
                Type classType = cAss.GetType(this.LogicClass);
                if (classType != null && classType.GetInterfaces().Contains(typeof(T)))
                {
                    return classType;
                }
            }

            //Proxy
            if (File.Exists(BaseDirectory + ProxyDll + ".dll"))
            {
                Assembly cAss = LoadAss(this.ProxyDll);
                Type classType = cAss.GetType(this.ProxyClass);
                if (classType != null && classType.GetInterfaces().Contains(typeof(T)))
                {
                    return classType;
                }
            }

            return null;
        }

        private static Assembly LoadAss(string impDllNameWeb)
        {

            if (Type.GetType(impDllNameWeb) == null)
            {
                Assembly cAssLogic = Assembly.Load(new AssemblyName(impDllNameWeb));
                return cAssLogic;
            }
            else
            {
                Assembly ass = Assembly.GetExecutingAssembly();//...GetAssembly(Type.GetType(impDllNameWeb));
                return ass;
            }


        }



        string Module { get; set; }

        string ControllerDll { get; set; }

        string ControllerClass { get; set; }


        string LogicDll { get; set; }

        string LogicClass { get; set; }


        string ProxyDll { get; set; }

        string ProxyClass { get; set; }


        public string CurrentDll { get; set; }

        public string CurrentClass { get; set; }


    }
}
