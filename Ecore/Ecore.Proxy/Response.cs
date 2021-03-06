﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Proxy
{

    [Serializable]
    public class Response<T>
    {
        public string Id { get; set; }

        public T Result { get; set; }

        public string Error { get; set; }

    }

    [Serializable]
    public class Response
    {
        public string Id { get; set; }

        public object Result { get; set; }

        public string Error { get; set; }

    }
}
