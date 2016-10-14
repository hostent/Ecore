using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using WebSocketSharp;
using Ecore.Frame;
using System.Threading.Tasks;

namespace Ecore.Proxy4
{


    public class Client : IProxyClient
    {
        #region private
        static Dictionary<string, ProxyModel> webServiceDict = new Dictionary<string, ProxyModel>();


        internal static string Post(string json, ProxyModel proxyModel)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            if (proxyModel.RestAddress.StartsWith("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }


            request = (HttpWebRequest)WebRequest.Create(proxyModel.RestAddress + "/restapi");
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            //向请求添加表单数据
            byte[] postdatabyte = Encoding.UTF8.GetBytes(json);
            request.ContentLength = postdatabyte.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length); //设置请求主体的内容
            stream.Close();

            response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);

            var result = responseReader.ReadToEnd();

            return result;
        }

        private static object Lock_GeApiAddress = new object();

        static ProxyModel GeApiAddress(string apiKey)
        {
            string[] strList = apiKey.Split('.');

            string moduleName = strList[1];

            lock (Lock_GeApiAddress)
            {

                if (webServiceDict == null)
                {
                    webServiceDict = new Dictionary<string, ProxyModel>();
                }
            }

            if (webServiceDict.ContainsKey(moduleName))
            {
                return webServiceDict[moduleName];
            }

            lock (Lock_GeApiAddress)
            {

                string module = string.Format("EMin.{0}.Model", strList[1]);

                var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "EMin.Module.xml", SearchOption.AllDirectories);

                if (files.Count() == 0)
                {
                    throw new Exception("模块配置信息不存在");
                }

                XElement xe = XElement.Load(files[0]).Elements("add").Where(q => q.Attribute("Name").Value == strList[1]).FirstOrDefault();

                webServiceDict.Add(moduleName, new ProxyModel()
                {
                    Name = xe.Attribute("Name").Value,
                    RestAddress = xe.Attribute("RestAddress").Value,
                    WsAddress = xe.Attribute("WsAddress").Value
                });

                return webServiceDict[moduleName];

            }


        }



        static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }


        #endregion





        public T Call<T>(string apiKey, params object[] par)
        {

            Request request = new Request();
            request.Id = Guid.NewGuid().ToString();
            request.Method = apiKey;
            request.Params = par;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            ProxyModel proxyModel = GeApiAddress(apiKey);

            string resultJson = "";
            if (false)
            {

                resultJson = Post(json, proxyModel);

            }
            else
            {
                resultJson = WsHelp.Send(request.Id, json, proxyModel);

            }



            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<Response<T>>(resultJson);

            if (res == null)
            {
                //throw new Exception("time out");
                Console.Write("time out:");
                return default(T);
            }

            if (!string.IsNullOrEmpty(res.Error))
            {
                throw new Exception(res.Error);
            }

            return res.Result;

        }


        public Task CallAsync(string apiKey, params object[] par)
        {
            throw new NotImplementedException();
        }


    }



}
