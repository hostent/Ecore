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

        public override void RenderResult()
        {
            new DefaultHttpContext().Response.Redirect(Url);

            GetCurrentContext().Response.Redirect(Url);

        }
    }


    public class PageResult
    {

        public PageResult()
        {
            Context = new DefaultHttpContext();
        }

        public string Data { get; set; }


        DefaultHttpContext Context;

        public DefaultHttpContext GetCurrentContext()
        {
            if (Context == null)
            {
                Context = new DefaultHttpContext();
            }
            return Context;
        }

        public string ContentType { get; set; }


        public virtual void OverHead() { }

        public virtual void RenderResult()
        {
            Context.Response.WriteAsync(Data, Encoding.UTF8);
            Context.Response.ContentType = ContentType;
            Context.Response.StatusCode = 200;
        }


    }

}
