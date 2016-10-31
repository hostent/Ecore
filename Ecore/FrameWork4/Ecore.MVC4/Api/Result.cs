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
            return Task.Factory.StartNew(() =>
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


        public virtual void OverHead() { }

        public virtual Task RenderResult(HttpContext httpContent)
        {
            return Task.Factory.StartNew(() =>
            {
                httpContent.Response.ContentType = ContentType;
                httpContent.Response.StatusCode = 200;

                httpContent.Response.ContentEncoding = Encoding.UTF8;
                httpContent.Response.Write(Data);
                httpContent.Response.End();
            });           

        }


    }

}
