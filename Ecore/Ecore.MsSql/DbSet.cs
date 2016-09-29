using Ecore.Frame;
using Ecore.MsSql.XmlSql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ecore.MsSql
{
    public class DbSet<T> : DbQuery<T>, Frame.ISet<T> where T : class, new()
    {

        public DbSet(IDbConnection conn, DbType sqlType) : base(conn, sqlType)
        {
        }


        string GetKey()
        {
            var t = typeof(T);
            PropertyInfo[] minfos = t.GetProperties();

            string key = minfos.Where(q =>
            {
                var keyAttr = q.GetCustomAttribute<KeyAttribute>();
                if (keyAttr != null)
                {
                    return true;
                }
                return false;
            }).Select(q => string.Format("{0}", q.Name)).FirstOrDefault();

            return key;
        }

        private string[] GetColumns(bool isIncludeID)
        {
            var t = typeof(T);
            PropertyInfo[] minfos = t.GetProperties();

            if (isIncludeID == false)
            {
                return minfos.Where(q =>
                {
                    var keyAttr = q.GetCustomAttribute<KeyAttribute>();
                    if (keyAttr != null)
                    {
                        return false;
                    }
                    return true;
                }).Select(q => q.Name).ToArray();
            }
            else
            {
                return minfos.Select(q => q.Name).ToArray();
            }

        }

        private string GetCacheTag()
        {
            return "table:" + typeof(T).FullName;
        }

        private bool CanCache()
        {
            var t = typeof(T);
            CacheAttribute ca = t.GetTypeInfo().GetCustomAttribute<CacheAttribute>();
            if (ca == null)
            {
                return false;
            }
            return true;
        }



        object ICommand<T>.Add(T t)
        {
            string sql = "insert into {table} ( {columns} ) values ( {values} );"; // select @@IDENTITY;


            string[] columns = null;

            if (t is IEntity)
            {
                ((IEntity)t).Id = IDGenerator.Default.NewID();
                columns = GetColumns(true);
            }
            else
            {
                sql = sql + " select @@IDENTITY; ";
                columns = GetColumns(false);
            }

            if (t is IEntityRecord)
            {
                ((IEntityRecord)t).CreateDate = DateTime.Now;
                ((IEntityRecord)t).CreateBy = "Not implemented";
            }

            sql = sql.Replace("{table}", typeof(T).Name);
            if (SqlType == DbType.Sql)
            {
                sql = sql.Replace("{columns}", "[" + string.Join("],[", columns) + "]");
            }
            else
            {
                sql = sql.Replace("{columns}", "`" + string.Join("`,`", columns) + "`");
            }

            sql = sql.Replace("{values}", "@" + string.Join(",@", columns));

            if (BaseModule.ThreadLocal_Tag.Value == ExecWay.UnitOfWork)
            {
                UnitOfWork.AddToUnit(t, sql);
                if (t is IEntity)
                {
                    return ((IEntity)t).Id;
                }
                return 0;
            }

            var result = Dapper.SqlMapper.ExecuteScalar(base.Conn, sql, t, BaseModule.GetTran());


            return result;


        }



        int ICommand<T>.Delete(object id)
        {
            string sql = "";
            if (SqlType == DbType.Sql)
            {
                sql = "delete from [{table}] where [{key}]=@key";
            }
            else
            {
                sql = "delete from `{table}` where `{key}`=@key";
            }



            sql = sql.Replace("{table}", typeof(T).Name);
            sql = sql.Replace("{key}", GetKey());

            object par = new { key = id };

            if (BaseModule.ThreadLocal_Tag.Value == ExecWay.UnitOfWork)
            {
                UnitOfWork.AddToUnit(par, sql);
                return 0;
            }

            var result = Dapper.SqlMapper.Execute(Conn, sql, par, BaseModule.GetTran());


            return result;

        }

        int ICommand<T>.Update(T t)
        {
            if (t is IEntityRecord)
            {
                ((IEntityRecord)t).UpdateDate = DateTime.Now;
                ((IEntityRecord)t).UpdateBy = "Not implemented";
            }

            string sql = "update {table} set {updateStr} where {key}=@{key}";

            sql = sql.Replace("{table}", typeof(T).Name);

            string[] strs = GetColumns(false).Select(q => string.Format("{0}=@{0}", q)).ToArray();
            sql = sql.Replace("{updateStr}", string.Join(",", strs));

            sql = sql.Replace("{key}", GetKey());

            if (BaseModule.ThreadLocal_Tag.Value == ExecWay.UnitOfWork)
            {
                UnitOfWork.AddToUnit(t, sql);
                return 0;
            }
            var result = Dapper.SqlMapper.Execute(Conn, sql, t, BaseModule.GetTran());

            return result;


        }



        public IList<T> QueryXml(string reportName, IDictionary<string, object> par)
        {
            int totalCount = 0;
            return ComplexSqlHelp.GetReportData<T>(Conn, reportName, 0, 0, "", par, false, out totalCount);
        }

        public PageData<T> QueryXml(string reportName, PagePars param)
        {
            PageData<T> result = new PageData<T>();
            int totalCount = 0;
            result.CurrentPage = ComplexSqlHelp.GetReportData<T>(Conn, reportName, param.PageSize, param.PageIndex, param.Order, param.Where, true, out totalCount);
            result.TotalCounts = totalCount;
            return result;
        }

        public IList<T> QuerySql(string sql, IDictionary<string, object> par)
        {
            throw new NotImplementedException();
        }

        public void ExecXml(string reportName, IDictionary<string, object> par)
        {
            throw new NotImplementedException();
        }

        public void ExecSql(string sql, IDictionary<string, object> par)
        {
            throw new NotImplementedException();
        }


    }
}
