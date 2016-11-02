using Ecore.Frame;
using Ecore.MVC4.Api;
using Ecore.MVC4.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4
{
    public class Router : IHttpModule
    {
        public void Dispose()
        {
             
        }

        public void Init(HttpApplication context)
        {

            context.BeginRequest += Context_BeginRequest;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            HttpContext context = application.Context;
            if (AssRequest(context) == RequestWay.ApiKey)
            {
                ((IRouterExec)new RestApiRouter()).Exec(context).Wait();
            }
            else if (AssRequest(context) == RequestWay.heartbeat)
            {
                context.Response.Write("OK");
                context.Response.End();
            }
            else
            {
                ((IRouterExec)new MvcRouter()).Exec(context).Wait();

            }


        }



        RequestWay AssRequest(HttpContext context)
        {
            string rawUrl = context.Request.RawUrl.Trim('/').ToLower();

            if (rawUrl.StartsWith("heartbeat"))
            {
                return RequestWay.heartbeat;
            }

            if (rawUrl.StartsWith("restapi"))
            {
                return RequestWay.ApiKey;
            }
            else
            {
                return RequestWay.MVC;
            }

        }

        enum RequestWay
        {
            heartbeat = 0,
            ApiKey = 1,
            MVC = 2
        }

    }
}
