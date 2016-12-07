using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ecore.MVC4.Api
{
    public class Request
    {

        public string Id { get; set; }

        public string Method { get; set; }

        public object[] Params { get; set; }

    }


    public class Response<T>
    {
        public string Id { get; set; }

        public T Result { get; set; }

        public string Error { get; set; }

    }


    public class Response
    {
        public string Id { get; set; }

        public object Result { get; set; }

        public string Error { get; set; }


        private static Dictionary<string, Type> objCache = new Dictionary<string, Type>();

        public static Response GetResponse(string json, RpcAuth auth = null)
        {
            Request req = Newtonsoft.Json.JsonConvert.DeserializeObject<Request>(json);

            if (string.IsNullOrEmpty(req.Method))
            {
                throw new Exception("参数错误");
            }
            if (!req.Method.StartsWith("EMin.Api.Model.Service."))
            {
                req.Method = "EMin.Api.Model.Service." + req.Method;
            }
            Response result = new Response();
            try
            {
                result.Id = req.Id;

                if (auth != null)
                {
                    Ecore.Frame.Result authResult = auth.Check(json);
                    if (authResult.Tag != 1)
                    {
                        result.Error = authResult.Message;
                        return result;
                    }
                }

                List<object> objList = new List<object>();

                Type objType = null;
                if (objCache.ContainsKey(req.Method))
                {
                    objType = objCache[req.Method];
                }
                else
                {
                    objType = AssemblyHelp.GetImpObjType(req.Method);
                    objCache.Add(req.Method, objType);
                }

                var instance = objType.Assembly.CreateInstance(objType.FullName);

                if (objType.IsSubclassOf(typeof(BaseApiController)))
                {
                    ((BaseApiController)instance).OnControllerCreate(req);
                }

                MethodInfo methodInfo = objType.GetMethod(AssemblyHelp.GetMethodName(req.Method));

                if (methodInfo == null)
                {
                    result.Error = "参数错误，找不到方法";
                    return result;

                }

                ParameterInfo[] tArr = methodInfo.GetParameters();

                for (int i = 0; i < tArr.Length; i++)
                {
                    object par = null;

                    if (req.Params[i] is Newtonsoft.Json.Linq.JToken)
                    {
                        par = ((Newtonsoft.Json.Linq.JToken)req.Params[i]).ToObject(tArr[i].ParameterType);
                    }
                    else
                    {
                        par = req.Params[i];
                    }

                    objList.Add(par);
                }

                Response responseBefour = null;

                if (objType.IsSubclassOf(typeof(BaseApiController)))
                {
                    responseBefour = ((BaseApiController)instance).OnActionExecting(req, objList);
                }
                if (responseBefour != null)
                {
                    result = responseBefour;
                }
                else
                {
                    result.Result = methodInfo.Invoke(instance, objList.ToArray());
                }
                if (objType.IsSubclassOf(typeof(BaseApiController)))
                {
                    ((BaseApiController)instance).OnResultExecting(req, result);
                }
            }
            catch (Exception ee)
            {
                Log.Default.Error(ee);
                result.Error = AssemblyHelp.GetErrorMsg(ee);
            }

            return result;
        }
    }
}
