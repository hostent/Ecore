using Ecore.Frame;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecore.MVC.Api
{
    public class WebSocketRouter : IRouterExec
    {
        public Task Exec(object context)
        {
            HttpContext httpContent = context as HttpContext;
            return Task.Run(() =>
            {
                var webSocket = httpContent.WebSockets.AcceptWebSocketAsync().Result;

                while (webSocket.State == WebSocketState.Open)
                {
                    var token = CancellationToken.None;
                    var buffer = new ArraySegment<Byte>(new Byte[4096]);
                    var received = webSocket.ReceiveAsync(buffer, token).Result;

                    switch (received.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            var json = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);

                            Response rep = Response.GetResponse(json);

                            var data = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(rep));
                            buffer = new ArraySegment<Byte>(data);
                            webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, token);

                            break;
                    }
                }

            });


        }
    }
}
