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


        private static Dictionary<string, object> objCache = new Dictionary<string, object>();

        public static Response GetResponse(string json, RpcAuth auth = null)
        {
            Request req = Newtonsoft.Json.JsonConvert.DeserializeObject<Request>(json);

            if (string.IsNullOrEmpty(req.Method))
            {
                throw new Exception("参数错误");
            }

            if (auth != null)
            {
                Ecore.Frame.Result authResult = auth.Check(json);
                if (authResult.Tag != 1)
                {
                    throw new Exception(authResult.Message);
                }
            }

            List<object> objList = new List<object>();

            object obj = null;
            if (objCache.ContainsKey(req.Method))
            {
                obj = objCache[req.Method];
            }
            else
            {
                obj = AssemblyHelp.GetImpObj(req.Method);
                objCache.Add(req.Method, obj);
            }

            MethodInfo methodInfo = obj.GetType().GetMethod(AssemblyHelp.GetMethodName(req.Method));

            if (methodInfo == null)
            {
                throw (new Exception("参数错误，找不到方法"));
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

            Response result = new Response();
            result.Id = req.Id;

            try
            {
                result.Result = methodInfo.Invoke(obj, objList.ToArray());
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
