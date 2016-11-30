using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
                byte[] byteArray = Encoding.UTF8.GetBytes(body); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
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
            throw new NotImplementedException();
        }
    }
}
