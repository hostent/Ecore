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
    public class RestApiRouter : IRouterExec
    {
        public Task Exec(object context)
        {
            return Task.Factory.StartNew(() =>
            {
                HttpContext httpContent = context as HttpContext;

                StreamReader streamReader = new StreamReader(httpContent.Request.InputStream, Encoding.UTF8);
                string json = streamReader.ReadToEnd();

                Response result = Response.GetResponse(json);

                httpContent.Response.Charset = "utf-8";
                httpContent.Response.ContentType = "application/json";

                httpContent.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
                httpContent.Response.End();
            });
        }
    }
}
