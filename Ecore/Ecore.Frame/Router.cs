using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class Router
    {
        public static IRouterExec MvcHandle { get; set; }

        public static IRouterExec RestApiHandle { get; set; }

        public static IRouterExec WebSocketHandle { get; set; }

        public static RequestWay AssHttpRequest(string path)
        {
            string rawUrl = path.Trim('/').ToLower();

            if (rawUrl.StartsWith("heartbeat"))
            {
                return RequestWay.Heartbeat;
            }

            if (rawUrl.StartsWith("restapi"))
            {
                return RequestWay.RestApi;
            }
            else
            {
                return RequestWay.MVC;
            }
        }
    }

    public interface IRouterExec
    {
        Task Exec(object context);
    }

    public enum RequestWay
    {
        Heartbeat = 0,
        RestApi = 1,
        WebSocketApi = 2,
        MVC = 3
    }

}
