using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ecore.MVC4.Tools
{
    //public class HttpClientHelp : IHttpClient
    //{
    //    public string Get(string url, IDictionary<string, string> par, IDictionary<string, string> head = null)
    //    {

    //        var client = new System.Net.Http.HttpClient();
    //        if (head != null)
    //        {
    //            foreach (var item in head)
    //            {
    //                client.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
    //            }
    //        }
    //        if (par != null)
    //        {
    //            if (url.Contains('?'))
    //            {
    //                foreach (var item in par)
    //                {
    //                    url = url + "&" + item.Key + "=" + item.Value;
    //                }
    //            }
    //            else
    //            {
    //                var parStr = "";
    //                foreach (var item in par)
    //                {
    //                    parStr = parStr + "&" + item.Key + "=" + item.Value;
    //                }
    //                parStr = "?" + parStr.Trim('&');
    //                url = url + parStr;
    //            }
    //        }

    //        Task<HttpResponseMessage> msg = client.GetAsync(url);

    //        return msg.Result.Content.ReadAsStringAsync().Result;

    //    }

    //    public string Post(string url, string jsonBody, IDictionary<string, string> head = null)
    //    {
    //        var client = new System.Net.Http.HttpClient();
    //        if (head != null)
    //        {
    //            foreach (var item in head)
    //            {
    //                client.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
    //            }
    //        }

    //        StringContent stringContent = new StringContent(jsonBody, System.Text.UTF8Encoding.UTF8, "application/json");
    //        Task<HttpResponseMessage> msg = client.PostAsync(url, stringContent);

    //        return msg.Result.Content.ReadAsStringAsync().Result;
    //    }

    //    public string Post(string url, IDictionary<string, string> body, IDictionary<string, string> head = null)
    //    {
    //        var client = new System.Net.Http.HttpClient();
    //        if (head != null)
    //        {
    //            foreach (var item in head)
    //            {
    //                client.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
    //            }
    //        }

    //        var parStr = "";

    //        foreach (var item in body)
    //        {
    //            parStr = parStr + "&" + item.Key + "=" + item.Value;
    //        }
    //        parStr = parStr.Trim('&');

    //        StringContent stringContent = new StringContent(parStr, System.Text.UTF8Encoding.UTF8);

    //        Task<HttpResponseMessage> msg = client.PostAsync(url, stringContent);

    //        return msg.Result.Content.ReadAsStringAsync().Result;
    //    }
    //}

    public class HttpClientHelp : IHttpClient
    {
        public string Get(string url, IDictionary<string, string> par, IDictionary<string, string> head = null)
        {
            throw new NotImplementedException();
        }

        public string Post(string url, string body, IDictionary<string, string> head = null)
        {
            string ret = string.Empty;
            try
            {

                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                byte[] bytes = Encoding.UTF8.GetBytes(body); //转化
                var text = Convert.ToBase64String(bytes);
                text = Frame.MyEncoding.Default.UrlEncode(text);
                text = "data=" + text;
                bytes = Encoding.UTF8.GetBytes(text);

                webReq.ContentLength = bytes.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                Ecore.Frame.Log.Default.Error(ex);
                throw ex;
            }
            return ret;
        }

        public string Post(string url, IDictionary<string, string> body, IDictionary<string, string> head = null)
        {
            string ret = string.Empty;
            try
            {
                var parStr = "";

                foreach (var item in body)
                {
                    parStr = parStr + "&" + item.Key + "=" + item.Value;
                }
                parStr = parStr.Trim('&');


                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                if (head != null)
                {//不为null
                    foreach (var item in head)
                    {
                        webReq.Headers.Add(item.Key, item.Value);
                    }
                }

                byte[] bytes = Encoding.UTF8.GetBytes(parStr); //转化

                webReq.ContentLength = bytes.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(bytes, 0, bytes.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                Ecore.Frame.Log.Default.Error(ex);
                throw ex;
            }
            return ret;
        }

        public string PostJson(string url, string json)
        {
            try
            {
                HttpWebRequest request = null;
                HttpWebResponse response = null;

                if (url.ToLower().StartsWith("https://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                request = (HttpWebRequest)WebRequest.Create(url);
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
                return responseReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Ecore.Frame.Log.Default.Error(e);         
                return "";
            }
        }


        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //直接确认，否则打不开    
            return true;
        }
    }
}
