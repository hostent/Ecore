using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ecore.MVC.Tools
{
    public class HttpClientHelp : IHttpClient
    {
        public string Get(string url, IDictionary<string, string> par, IDictionary<string, string> head = null)
        {

            var client = new System.Net.Http.HttpClient();
            if (head != null)
            {
                foreach (var item in head)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
                }
            }
            if (par != null)
            {
                if (url.Contains('?'))
                {
                    foreach (var item in par)
                    {
                        url = url + "&" + item.Key + "=" + item.Value;
                    }
                }
                else
                {
                    var parStr = "";
                    foreach (var item in par)
                    {
                        parStr = parStr + "&" + item.Key + "=" + item.Value;
                    }
                    parStr = "?" + parStr.Trim('&');
                    url = url + parStr;
                }
            }

            Task<HttpResponseMessage> msg = client.GetAsync(url);

            return msg.Result.Content.ReadAsStringAsync().Result;

        }

        public string Post(string url, string jsonBody, IDictionary<string, string> head = null)
        {
            var client = new System.Net.Http.HttpClient();
            if (head != null)
            {
                foreach (var item in head)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
                }
            }

            StringContent stringContent = new StringContent(jsonBody, System.Text.UTF8Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> msg = client.PostAsync(url, stringContent);

            return msg.Result.Content.ReadAsStringAsync().Result;
        }

        public string Post(string url, IDictionary<string, string> body, IDictionary<string, string> head = null)
        {
            var client = new System.Net.Http.HttpClient();
            if (head != null)
            {
                foreach (var item in head)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
                }
            }

            var parStr = "";

            foreach (var item in body)
            {
                parStr = parStr + "&" + item.Key + "=" + item.Value;
            }
            parStr = parStr.Trim('&');

            StringContent stringContent = new StringContent(parStr, System.Text.UTF8Encoding.UTF8);

            Task<HttpResponseMessage> msg = client.PostAsync(url, stringContent);

            return msg.Result.Content.ReadAsStringAsync().Result;
        }
    }
}
