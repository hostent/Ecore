using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4.Api
{
    public class RestApiRouter
    {
        public void Exec(object context)
        {

            HttpContext httpContent = context as HttpContext;
            Response result = null;
            string resultJson = "";
            string requestjson = "";
            try
            {

                StreamReader streamReader = new StreamReader(httpContent.Request.InputStream, Encoding.UTF8);
                requestjson = streamReader.ReadToEnd();

                RpcAuth rpcAuth = null;
                if (!Config.IsDebug)
                {
                    rpcAuth = new RpcAuth();
                    rpcAuth.Key = httpContent.Request.QueryString["key"];
                    rpcAuth.Sign = httpContent.Request.QueryString["sign"];
                    rpcAuth.Timestamp = Convert.ToInt64(httpContent.Request.QueryString["ts"]);
                }

                result = Response.GetResponse(requestjson, rpcAuth);

                resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                Log.Default.Msg("请求：" + httpContent.Request.Url.ToString() + "\r\n" + requestjson + "\r\n响应：" + resultJson);
            }
            catch (Exception ee)
            {
                result = new Response();
                result.Error = ee.Message;
                resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                Log.Default.Error(requestjson, ee);
            }

            httpContent.Response.Charset = "utf-8";
            httpContent.Response.ContentType = "application/json";

            httpContent.Response.Write(resultJson);
            httpContent.Response.End();



        }
    }
}
