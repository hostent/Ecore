using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ecore.MVC.Api
{
    public class RestApiRouter : IRouterExec
    {
        public Task Exec(object context)
        {
            return Task.Run(() =>
            {
                var httpContent = context as HttpContext;

                Response result = null;
                string resultJson = "";
                string requestjson = "";
                try
                {
                    StreamReader streamReader = new StreamReader(httpContent.Request.Body, Encoding.UTF8);
                    string json = streamReader.ReadToEnd();

                    result = Response.GetResponse(json);
                    resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

                    Log.Default.Msg("请求：\r\n" + requestjson + "\r\n响应：" + resultJson);
                }
                catch (Exception ee)
                {
                    result = new Response();
                    result.Error = ee.Message;
                    resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    Log.Default.Error(requestjson, ee);
                }
                httpContent.Response.ContentType = "application/json; charset=utf-8";

                httpContent.Response.WriteAsync(resultJson, Encoding.UTF8);
            });
        }
    }
}
