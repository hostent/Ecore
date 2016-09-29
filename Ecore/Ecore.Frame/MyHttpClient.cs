using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class MyHttpClient
    {
        public static IHttpClient Default { get; set; }

        

    }

    public interface IHttpClient
    {
        string Get(string url, IDictionary<string, string> par, IDictionary<string, string> head = null);


        string Post(string url, IDictionary<string, string> body, IDictionary<string, string> head = null);

        string Post(string url, string jsonBody, IDictionary<string, string> head = null);

    }

 
}
