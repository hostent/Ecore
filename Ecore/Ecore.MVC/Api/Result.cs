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
            ContentType = "application/json; charset=utf-8";
        }

        public JsonResult(object jsonObj) : base()
        {
            Obj = jsonObj;
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(Obj);
            ContentType = "application/json; charset=utf-8";

        }


    }


    public class StringResult : PageResult
    {

        public StringResult() : base()
        {
            ContentType = "text/html; charset=utf-8";
        }

        public StringResult(string stringContext) : base()
        {
            Data = stringContext;
            ContentType = "text/html; charset=utf-8";
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
