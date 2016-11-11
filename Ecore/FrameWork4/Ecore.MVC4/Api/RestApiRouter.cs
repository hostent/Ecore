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

                result = Response.GetResponse(requestjson);

                resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                Log.Default.Msg("请求：\r\n" + requestjson + "\r\n响应：" + resultJson);
            }
            catch (Exception ee)
            {
                result = new Response();
                result.Error = ee.Message;

                Log.Default.Error(requestjson, ee);
            }

            httpContent.Response.Charset = "utf-8";
            httpContent.Response.ContentType = "application/json";

            httpContent.Response.Write(resultJson);
            httpContent.Response.End();



        }
    }
}
