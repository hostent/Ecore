using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecore.MVC.Api
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
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(Obj);
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

        public override Task RenderResult(HttpContext httpContent)
        {
            return Task.Run(()=>
            {
                httpContent.Response.Redirect(Url);
            });            
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

        public virtual Task RenderResult(HttpContext httpContent)
        {
            httpContent.Response.ContentType = ContentType;
            httpContent.Response.StatusCode = 200;

            return httpContent.Response.WriteAsync(Data, Encoding.UTF8);

        }


    }

}
