using Ecore.MVC.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.MVC
{
    public class RouterStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"lib")),
                RequestPath = new PathString("/lib")
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.Run(async (context) =>
            {
                if (AssRequest(context) == RequestWay.ApiKey)
                {
                    await new RestApiRouter().Exec(context);
                }
                else if (AssRequest(context) == RequestWay.heartbeat)
                {
                    await context.Response.WriteAsync("OK");
                }
                else if (AssRequest(context) == RequestWay.StaticFile)
                {
                    return;
                }
                else
                {
                    await new MvcRouter().Exec(context);
                }
            });
        }

        RequestWay AssRequest(HttpContext context)
        {
            string rawUrl = context.Request.Path.Value.Trim('/').ToLower();

            if (rawUrl.StartsWith("heartbeat"))
            {
                return RequestWay.heartbeat;
            }

            if (rawUrl.StartsWith("restapi"))
            {
                return RequestWay.ApiKey;
            }
            if (rawUrl.StartsWith(@"lib/"))
            {
                return RequestWay.StaticFile;
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
            MVC = 2,
            StaticFile = 3
        }
    }
}
