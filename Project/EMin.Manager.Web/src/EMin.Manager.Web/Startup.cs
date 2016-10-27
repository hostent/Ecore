using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ecore.MVC;
using Ecore.Frame;

namespace EMin.Manager.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseWebSockets();

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            Factory.Init();

            MvcMapFactory.AddMvc("EMin.Manager.Web.Controller", "EMin.Manager.Web");

            //Pipe.ServerFeaturesStatic = app.ServerFeatures;


            app.Run(async (context) =>
            {
                Pipe.ServerFeaturesStatic = context.Features;

                if (context.WebSockets.IsWebSocketRequest)
                {
                    await Router.WebSocketHandle.Exec(context);
                }
                else
                {
                    switch (Router.AssHttpRequest(context.Request.Path.Value))
                    {
                        case RequestWay.Heartbeat:
                            await context.Response.WriteAsync("Hello World!");
                            break;
                        case RequestWay.RestApi:
                            await Router.RestApiHandle.Exec(context);
                            break;
                        case RequestWay.MVC:
                            await Router.MvcHandle.Exec(context);
                            break;

                    }
                }


            });
        }
    }
}
