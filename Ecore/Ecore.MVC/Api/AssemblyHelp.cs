using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ecore.MVC.Api
{
    public class AssemblyHelp
    {

        public static Assembly LoadAss(string impDllNameWeb)
        {
            if (!File.Exists(AppContext.BaseDirectory + @"bin\" + impDllNameWeb + ".dll"))
            {
                return null;
            }
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


        public static object GetImpObj(string apiKey)
        {
            var strList = apiKey.Split('.').ToList();

            string methodName = strList.Last();

            strList.RemoveAt(strList.Count - 1);

            string interfaceName = strList.Last();

            strList.RemoveAt(strList.Count - 1);

            string impDllNameWeb = strList[0] + "." + strList[1] + ".Web";
            string impInterfaceFullNameWeb = string.Join(".", strList).Replace(".Model.Service", ".Web.Controller") + ("." + interfaceName).Replace(".I", ".C");

            Assembly cAss = AssemblyHelp.LoadAss(impDllNameWeb);

            object resultObj = null;
            if (cAss != null && cAss.DefinedTypes.Any(q => q.FullName == impInterfaceFullNameWeb))
            {
                resultObj = cAss.CreateInstance(impInterfaceFullNameWeb);
                return resultObj;
            }
            else
            {
                impDllNameWeb = strList[0] + "." + strList[1] + ".Logic";
                impInterfaceFullNameWeb = string.Join(".", strList).Replace(".Model.Service", ".Logic") + ("." + interfaceName).Replace(".I", ".C");

                Assembly cAssLogic = AssemblyHelp.LoadAss(impDllNameWeb);
                if (cAssLogic != null && cAssLogic.DefinedTypes.Any(q => q.FullName == impInterfaceFullNameWeb))
                {
                    resultObj = cAssLogic.CreateInstance(impInterfaceFullNameWeb);
                    return resultObj;
                }
                else
                {
                    throw new Exception("Interface imp is not exist");
                }
            }


        }

        public static string GetMethodName(string apiKey)
        {
            return apiKey.Split('.').Last();
        }



        public static string GetErrorMsg(Exception e)
        {
            string msg = "";

            Exception tag = e;
            for (int i = 0; i < 5; i++)
            {
                if (tag == null)
                {
                    break;
                }
                msg = msg + tag.Message;
                msg = msg + "\r\n ---------------------------堆栈信息------------------------------- \r\n ";
                msg = msg + tag.StackTrace;

                msg = msg + "\r\n --------------------------------------------------------------------------------------- \r\n ";
                tag = tag.InnerException;
            }

            return msg;

        }
    }
}
