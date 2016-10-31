using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecore.Sql4
{
    public class UnitOfWork
    {
        public static ThreadLocal<List<UnitStore>> ThreadLocal_Tag = new ThreadLocal<List<UnitStore>>();


        public static void Exec(BaseModule module)
        {
            module.Transaction(() =>
            {
                if (ThreadLocal_Tag.Value == null)
                {
                    return;
                }
                foreach (var item in ThreadLocal_Tag.Value)
                {
                    Dapper.SqlMapper.Execute(module.GetConnection(), item.Sql, item.Par, BaseModule.GetTran());
                }
            });
        }



        public class UnitStore
        {
            public string Sql { get; set; }

            public string ConnStr { get; set; }

            public object Par { get; set; }

        }


        public static void AddToUnit<T>(T t, string sql)
        {
            if (UnitOfWork.ThreadLocal_Tag.Value == null)
            {
                UnitOfWork.ThreadLocal_Tag.Value = new List<UnitStore>();
            }
            UnitOfWork.ThreadLocal_Tag.Value.Add(new UnitOfWork.UnitStore()
            {
                Sql = sql,
                Par = t
            });
        }


    }
}
