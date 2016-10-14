using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Proxy
{
    [Serializable]
    public class Request
    {

        public string Id { get; set; }

        public string Method { get; set; }

        public object[] Params { get; set; }


    }
}
