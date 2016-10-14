using System;
using System.Collections.Generic;
using System.Linq;
 

namespace Ecore.Proxy4
{
    [Serializable]
    public class Request
    {

        public string Id { get; set; }

        public string Method { get; set; }

        public object[] Params { get; set; }


    }
}
