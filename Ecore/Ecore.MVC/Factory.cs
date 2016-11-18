using Autofac;
using Ecore.Frame;
using Ecore.MVC.Api;
using Ecore.MVC.Tools;
using Ecore.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace Ecore.MVC
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
            Ecore.Frame.Cache.Default = new Ecore.Redis.CCache();
           // Ecore.Frame.Cookie.Default = new Tools.CookieHelp();
            Ecore.Frame.IDGenerator.Default = new Ecore.Redis.CIDGenerator();
            LockUser.Default = new Ecore.Redis.CLock();
            Log.Default = new Ecore.Mongodb.CLog();
            LoginContext.Default = new CLoginContext();
            MessageQueue.Default = new Ecore.Redis.CMessageQueue();
            MyHttpClient.Default = new Ecore.MVC.Tools.HttpClientHelp();
            UContainer.Factory = new Factory();
            Ecore.Frame.Weixin.Account = new Ecore.MVC.Weixin.Manager();
            Config.Default = new Tools.CConifg();
            Frame.MyEncoding.Default = new Ecore.MVC.Tools.MyEncoding();

            Router.MvcHandle = new MvcRouter();
            Router.RestApiHandle = new RestApiRouter();
            Router.WebSocketHandle = new WebSocketRouter();

            MyProxy.Default = new Client();

        }
    }


    public class ImpRelation
    {

        public string InterfaceDllName { get; set; }

        public string LogicDllName { get; set; }

        public string InterfaceName { get; set; }

        public string LogicName { get; set; }

        public static List<ImpRelation> GetConifig()
        {
            List<ImpRelation> result = new List<ImpRelation>();

            result.Add(new ImpRelation() { InterfaceDllName = "EMin.Model", LogicDllName = "EMin.Logic" });
            result.Add(new ImpRelation() { InterfaceDllName = "EMin.Menber.Model", LogicDllName = "EMin.Menber.Logic" });
            result.Add(new ImpRelation() { InterfaceDllName = "EMin.Item.Model", LogicDllName = "EMin.Item.Logic" });
            result.Add(new ImpRelation() { InterfaceDllName = "EMin.SiteBackOffice.Model", LogicDllName = "EMin.SiteBackOffice.Logic" });
            result.Add(new ImpRelation() { InterfaceDllName = "EMin.Order.Model", LogicDllName = "EMin.Order.Logic" });
            result.Add(new ImpRelation() { InterfaceDllName = "EMin.SiteSetting.Model", LogicDllName = "EMin.SiteSetting.Logic" });


            //特殊配置 ,填写4个参数
            //todo
            result.Add(new ImpRelation()
            {
                InterfaceName = "EMin.SiteSetting.Model.Service.MM.Icc.dd",
                LogicName = "EMin.SiteSetting.Web.Controller.MM.Ccc.dd",
                InterfaceDllName = "EMin.SiteSetting.Model",
                LogicDllName = "EMin.SiteSetting.Web"
            });

            return result;
        }

        public static object GetImpObj(string interfaceKey)
        {
            var configItem = ImpRelation.GetConifig().Where(q => interfaceKey == q.InterfaceName).FirstOrDefault();
            if (configItem == null)
            {
                configItem = ImpRelation.GetConifig().Where(q => interfaceKey.Contains(q.InterfaceDllName)).FirstOrDefault();
                if (configItem == null)
                {
                    throw new Exception("该接口没有相关的实现配置");
                }
                //fill
                var impSegArray = interfaceKey.Replace(configItem.InterfaceName, "").Replace(".Service.", "").Split('.');
                impSegArray[impSegArray.Length - 2] = impSegArray[impSegArray.Length - 2].Replace(".I", ".C");

                configItem.InterfaceName = interfaceKey;
                configItem.LogicName = String.Join(".", impSegArray);

            }

            Assembly cAss = LoadAss(configItem.LogicDllName);
            return cAss.CreateInstance(configItem.LogicName);



        }

        private static Assembly LoadAss(string impDllNameWeb)
        {
            //// 获取所引用的程序集
            //AssemblyName[] imports = Assembly.GetEntryAssembly().GetReferencedAssemblies();



            //if (Type.GetType(impDllNameWeb) == null)
            //{
            //    Assembly cAssLogic = Assembly.Load(impDllNameWeb);
            //    return cAssLogic;
            //}
            //else
            //{
            //    Assembly ass = Assembly.GetAssembly(Type.GetType(impDllNameWeb));
            //    return ass;
            //}

            return null;


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

        public T CreateInstance()
        {
            //controller           

            if (File.Exists(AppContext.BaseDirectory + "\\" + ControllerDll + ".dll"))
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
            if (File.Exists(AppContext.BaseDirectory + "\\" + LogicDll + ".dll"))
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
            if (File.Exists(AppContext.BaseDirectory + "\\" + ProxyDll + ".dll"))
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

            if (File.Exists(AppContext.BaseDirectory + "\\" + ControllerDll + ".dll"))
            {
                Assembly cAss = LoadAss(this.ControllerDll);
                Type classType = cAss.GetType(this.ControllerClass);
                if (classType != null && classType.GetInterfaces().Contains(typeof(T)))
                {
                    return classType;
                }
            }

            //logic
            if (File.Exists(AppContext.BaseDirectory + "\\" + LogicDll + ".dll"))
            {
                Assembly cAss = LoadAss(this.LogicDll);
                Type classType = cAss.GetType(this.LogicClass);
                if (classType != null && classType.GetInterfaces().Contains(typeof(T)))
                {
                    return classType;
                }
            }

            //Proxy
            if (File.Exists(AppContext.BaseDirectory + "\\" + ProxyDll + ".dll"))
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
            // 获取所引用的程序集
            AssemblyName[] imports = Assembly.GetEntryAssembly().GetReferencedAssemblies();



            if (Type.GetType(impDllNameWeb) == null)
            {
                Assembly cAssLogic = Assembly.Load(new AssemblyName(impDllNameWeb));
                return cAssLogic;
            }
            else
            {
                Assembly ass = Assembly.GetEntryAssembly();//...GetAssembly(Type.GetType(impDllNameWeb));
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
