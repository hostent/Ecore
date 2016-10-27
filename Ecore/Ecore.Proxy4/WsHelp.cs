using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WebSocketSharp;
using Ecore.Frame;

namespace Ecore.Proxy4
{
    public class WsHelp
    {


        static WebSocket wsClient = null;
        static object _lock = new object();

        class MsgPool
        {
            public string MessageID { get; set; }

            public ManualResetEvent Waiter { get; set; }

            public string Result { get; set; }

        }


        public static string Send(string msgid, string json, ProxyModel proxyModel)
        {

            if (wsClient == null || wsClient.ReadyState != WebSocketState.Open)
            {
                lock (_lock)
                {
                    if (wsClient == null || wsClient.ReadyState != WebSocketState.Open)
                    {
                        wsClient = new WebSocket(proxyModel.WsAddress + "/wsapi");
                        wsClient.OnMessage += WsClient_OnMessage;

                        wsClient.Connect();
                    }
                }
            }
            if (wsClient.ReadyState != WebSocketState.Open)
            {
                return Client.Post(json, proxyModel);
            }

            MsgPool tag = new MsgPool();
            tag.Waiter = new ManualResetEvent(false);
            tag.MessageID = msgid;
            tag.Result = "";


            Cache.MemoryCache.Add("ws:" + tag.MessageID, tag, 30000);


            wsClient.Send(json);
            tag.Waiter.WaitOne(20000);

            // log todo


            string result = tag.Result;
            Cache.MemoryCache.Remove("ws:" + tag.MessageID);

            return result;

        }


        private static void WsClient_OnMessage(object sender, MessageEventArgs e)
        {
            string json = e.Data;

            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(json);

            var tag = (MsgPool)Cache.MemoryCache.Get("ws:" + res.Id);
            if (tag == null)
            {
                throw new Exception("No request or time out");
            }

            tag.Result = json;

            tag.Waiter.Set();

        }

    }

}
