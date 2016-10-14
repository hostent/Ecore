using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class MyProxy
    {
        public static IProxyClient Default { get; set; }
    }


    public interface IProxyClient
    {
        T Call<T>(string apiKey, params object[] par);

        Task CallAsync(string apiKey, params object[] par);


    }

    public class ProxyModel
    {
        public string Name { get; set; }

        public string RestAddress { get; set; }

        public string WsAddress { get; set; }


    }
}
