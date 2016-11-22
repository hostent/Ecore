using Ecore.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Oms.Logic.DataAccess
{
    public class _Db : BaseModule
    {
        public _Db(string connName) : base(connName, DbType.Mysql)
        {
        }

        public static _Db Get()
        {
            return new _Db("Oms");
        }




        //tables




    }
}
