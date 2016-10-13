# Ecore
Net Core 项目

#集成网站开发各种技术:
1. Aop
2. cache
3. Search
4. MVC
5. MessageQuery
6. IDGenerator
7. LoginContext
8. Weixin
9. Orm
10. JsonRpc api（支持websocket 和 http rest）
11. 自定义MVC

以及各种工具类。


##以下是容器代码
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

            MappingManager.AddMvc("EMin.Oms.Web.Controller", "EMin.Oms.Web");



            app.Run(async (context) =>
            {

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

#框架全部采用注入方式，无侵入。

#模块划分，全部面向接口，有共享模块，有微服务模块。
