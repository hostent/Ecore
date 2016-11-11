using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecore.Sql4
{
    public class OptimismTran
    {

        public static ThreadLocal<Guid> ThreadLocal_Guid = new ThreadLocal<Guid>();

        ICache cache = Cache.MemoryCache; //在芒果，全局记录

        public static void Rollback()
        {

        }


        public class LogEntity
        {

        }
    }
}
