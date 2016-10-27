using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.MVC.Api
{
    public class RpcAuth
    {
        public string Key { get; set; }

        /// <summary>
        ///  md5(Id + Method + Key + AccessToken)
        /// </summary>
        public string Sign { get; set; }

        public int Timestamp { get; set; }
    }
}
