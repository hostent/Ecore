using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.MsSql
{
    public enum ExecWay
    {
        Transaction = 1,
        UnitOfWork = 2
    }

    public enum DbType
    {
        Sql = 1,
        Mysql = 2,
        Mongo = 4
    }
}
