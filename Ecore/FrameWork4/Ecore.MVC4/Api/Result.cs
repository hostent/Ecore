using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4.Api
{

    public class JsonResult : PageResult
    {
        object Obj { get; set; }

        public JsonResult() : base()
        {
            ContentType = "application/json";
        }

        public JsonResult(object jsonObj) : base()
        {
            Obj = jsonObj;

            Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            Data = Newtonsoft.Json.JsonConvert.SerializeObject(Obj, Newtonsoft.Json.Formatting.Indented, timeConverter);

            ContentType = "application/json";

        }


    }


    public class StringResult : PageResult
    {

        public StringResult() : base()
        {
            ContentType = "text/html";
        }

        public StringResult(string stringContext) : base()
        {
            Data = stringContext;
            ContentType = "text/html";
        }

    }

    public class RedictionResult : PageResult
    {
        public dynamic Url { get; set; }

        public RedictionResult(string url) : base()
        {
            Url = url;
        }

        public override void RenderResult(HttpContext httpContent)
        {
            httpContent.Response.Redirect(Url);
        }
    }


    public class PageResult
    {

        public PageResult()
        {

        }

        public string Data { get; set; }

        public string ContentType { get; set; }


        public virtual void AddHead(HttpContext httpContent, KeyValuePair<string, string> kv)
        {
            httpContent.Response.Headers.Add(kv.Key, kv.Value);
        }

        public virtual void RenderResult(HttpContext httpContent)
        {
            string match = httpContent.Request.Headers["If-None-Match"] ?? Guid.NewGuid().ToString();
            string etag = httpContent.Response.Headers["Etag"] ?? Guid.NewGuid().ToString();
            if (match == etag)
            {
                httpContent.Response.StatusCode = 304;
            }
            else
            {
                httpContent.Response.StatusCode = 200;
            }

            httpContent.Response.ContentType = ContentType;

            httpContent.Response.ContentEncoding = Encoding.UTF8;
            httpContent.Response.Write(Data);


        }


    }

}
