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

        public RpcAuth Auth { get; set; }
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


        public static Response GetResponse(string json)
        {
            Request req = Newtonsoft.Json.JsonConvert.DeserializeObject<Request>(json);

            List<object> objList = new List<object>();

            object obj = AssemblyHelp.GetImpObj(req.Method);

            MethodInfo methodInfo = obj.GetType().GetMethod(AssemblyHelp.GetMethodName(req.Method));

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
