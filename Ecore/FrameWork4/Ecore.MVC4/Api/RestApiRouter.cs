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
            try
            {


                StreamReader streamReader = new StreamReader(httpContent.Request.InputStream, Encoding.UTF8);
                string json = streamReader.ReadToEnd();

                result = Response.GetResponse(json);
            }
            catch (Exception ee)
            {
                result = new Response();
                result.Error = ee.Message;
            }
            httpContent.Response.Charset = "utf-8";
            httpContent.Response.ContentType = "application/json";

            httpContent.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result));
            httpContent.Response.End();



        }
    }
}
