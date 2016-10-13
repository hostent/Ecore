using Ecore.Frame;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecore.MVC.Api
{
    public class RestApiRouter : IRouterExec
    {
        public Task Exec(object context)
        {
            return Task.Run(() =>
            {
                HttpContext httpContent = context as HttpContext;

                StreamReader streamReader = new StreamReader(httpContent.Request.Body, Encoding.UTF8);
                string json = streamReader.ReadToEnd();

                Response result = Response.GetResponse(json);

                httpContent.Response.ContentType = "application/json";

                httpContent.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8);
            });
        }
    }
}
