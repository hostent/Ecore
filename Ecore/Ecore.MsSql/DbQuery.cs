using Ecore.Frame;
using Ecore.Frame.LinqExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Ecore.Frame.Security;
using System.Data;

namespace Ecore.MsSql
{
    public class DbQuery<T> : IQuery<T> where T : class, new()
    {

        protected IDbConnection Conn = null;

        protected DbType SqlType { get; set; }

        protected DbQuery(IDbConnection conn, DbType sqlType)
        {
            Conn = conn;
            SqlType = sqlType;

            trackSql = trackSql.Replace("{table}", typeof(T).Name);
        }


        //先用一次where 后续再扩展list
        private Expression<Func<T, bool>> whereExp { get; set; }

        private Expression<Func<T, object>> orderExp { get; set; }

        private string order = string.Empty;

        private int? limitForm = null;
        private int? limitLength = null;

        private string distinct = "";

        private string trackSql = "select {column} from {table} {where} {order} {limit}";

        private DynamicParameters args = new DynamicParameters();



        public IQuery<T> OrderBy(Expression<Func<T, object>> exp)
        {
            orderExp = exp;
            order = "asc";

            return this;
        }

        public IQuery<T> OrderByDesc(Expression<Func<T, object>> exp)
        {
            order = "desc";
            orderExp = exp;

            return this;
        }

        private void BuildColumns(string cols = null)
        {
            BuildColumns(typeof(T), cols);

        }

        private void BuildColumns(Type t, string cols = null)
        {
            if (!string.IsNullOrEmpty(distinct))
            {
                trackSql = trackSql.Replace("{column}", distinct + " {column}");
            }

            if (cols != null)
            {
                trackSql = trackSql.Replace("{column}", cols);
                return;
            }
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] minfos = t.GetProperties();

            if (!t.Equals(typeof(T)))
            {
                List<string> pinfoList = typeof(T).GetProperties().Select(q => q.Name).ToList();
                minfos = minfos.Where(q => pinfoList.Contains(q.Name)).ToArray();
            }

            string[] columns = null;
            if (SqlType == DbType.Sql)
            {
                columns = minfos.Select(q => string.Format("[{0}]", q.Name)).ToArray();
            }
            else
            {
                columns = minfos.Select(q => string.Format("`{0}`", q.Name)).ToArray();
            }

            string columnStr = string.Join(",", columns);

            trackSql = trackSql.Replace("{column}", columnStr);



        }

        private void BuildWhere(string where = null)
        {
            if (where != null)
            {
                trackSql = trackSql.Replace("{where}", where);
                return;
            }
            if (whereExp == null)
            {
                trackSql = trackSql.Replace("{where}", "");
                return;
            }

            ConditionBuilder conditionBuilder = new ConditionBuilder();
            conditionBuilder.Build(whereExp.Body);

            //arg
            for (int i = 0; i < conditionBuilder.Arguments.Count(); i++)
            {
                args.Add("@q__" + i.ToString(), conditionBuilder.Arguments[i]);
            }
            //sql
            trackSql = trackSql.Replace("{where}", " where " + conditionBuilder.Condition);



        }

        private void BuildOrder(string order = null)
        {
            if (order != null)
            {
                trackSql = trackSql.Replace("{order}", order);
                return;
            }
            if (orderExp == null)
            {
                trackSql = trackSql.Replace("{order}", " order by 1 ");
                return;
            }

            ConditionBuilder conditionBuilder = new ConditionBuilder();
            conditionBuilder.Build(orderExp.Body);
            if (SqlType == DbType.Sql)
            {
                trackSql = trackSql.Replace("{order}", string.Format(" order by [{0}] {1} ", conditionBuilder.Condition, order));
            }
            else
            {
                trackSql = trackSql.Replace("{order}", string.Format(" order by `{0}` {1} ", conditionBuilder.Condition, order));
            }
 
        }

        private void BuildLimit(string limit = null)
        {
            if (limit != null)
            {
                trackSql = trackSql.Replace("{limit}", limit);
                return;
            }

            if (limitLength == null)
            {
                trackSql = trackSql.Replace("{limit}", "");
                return;
            }

            if (SqlType == DbType.Sql)
            {
                trackSql = trackSql.Replace("{limit}", string.Format(" offset {0} rows fetch next {1} rows only ", limitForm ?? 0, limitLength ?? 1));
            }
            else
            {
                trackSql = trackSql.Replace("{limit}", string.Format(" limit {0}, {1} ", limitForm ?? 0, limitLength ?? 1));
            }


        }

        private string GetCacheKey(string method)
        {
            string keyFormat = "Table.{table}:{sql.par.method}";

            keyFormat = keyFormat.Replace("{table}", typeof(T).Name);

            string str = trackSql;
            foreach (var item in args.ParameterNames)
            {
                str += item;
                str += args.Get<object>(item).ToString();
            }
            str += method;

            keyFormat = keyFormat.Replace("{where.order.orderWay.limitForm.limitLength.method}", Frame.Security.MD5Helper.Encrypt_MD5(str));

            return keyFormat;


        }


        private string GetCacheTag()
        {
            return "table:" + typeof(T).FullName;
        }

        public IQuery<T> Where(Expression<Func<T, bool>> exp)
        {
            whereExp = exp;

            return this;
        }

        public IQuery<T> Limit(int form, int length)
        {
            limitForm = form;
            limitLength = length;

            return this;
        }


        public T First()
        {
            if (CanCache())
            {
                return this.ToQueryable().FirstOrDefault();
            }

            limitForm = 0;
            limitLength = 1;

            BuildColumns();
            BuildWhere();
            BuildOrder();
            BuildLimit();

            var obj = Dapper.SqlMapper.QueryFirstOrDefault<T>(Conn, trackSql, args, BaseModule.GetTran());

            return obj;
        }

        public List<T> ToList()
        {
            if (CanCache())
            {
                return this.ToQueryable().ToList();
            }

            BuildColumns();
            BuildWhere();
            BuildOrder();
            BuildLimit();

            var list = Dapper.SqlMapper.Query<T>(Conn, trackSql, args, BaseModule.GetTran()).ToList();

            return list;

        }

        public long Count()
        {
            if (CanCache())
            {
                return this.ToQueryable().Count();
            }

            BuildColumns(" count(0) ");
            BuildWhere();
            BuildOrder("");
            BuildLimit("");

            var count = Dapper.SqlMapper.ExecuteScalar<long>(Conn, trackSql, args, BaseModule.GetTran());

            return count;

        }

        public bool Exist()
        {
            if (CanCache())
            {
                return this.ToQueryable().Any();
            }

            BuildColumns(" count(0) ");
            BuildWhere();
            BuildOrder("");
            BuildLimit("");

            var exist = Dapper.SqlMapper.ExecuteScalar<int>(Conn, trackSql, args, BaseModule.GetTran());

            return exist > 0;
        }

        internal List<T> ToAll()
        {
            BuildColumns("*");
            BuildWhere("");
            BuildOrder("");
            BuildLimit("");

            var list = Dapper.SqlMapper.Query<T>(Conn, trackSql, args, BaseModule.GetTran()).ToList();

            return list;
        }

        internal IEnumerable<T> ToQueryable()
        {
            List<T> data = Cache.Default.Get<List<T>>(GetCacheTag());
            if (data == null)
            {
                data = this.ToAll();
                Cache.Default.Add(GetCacheTag(), data, DateTime.Now.AddMinutes(20));
            }            

           var query = data.Where(whereExp.Compile());

            if (whereExp != null)
            {
                query = query.Where(whereExp.Compile());
            }

            if (order == "asc")
            {
                query.OrderBy(orderExp.Compile());
            }
            else if (order == "desc")
            {
                query.OrderByDescending(orderExp.Compile());
            }

            if (limitLength != null)
            {
                query.Skip(limitForm ?? 0).Take(limitLength.Value);
            }

            return query;

        }

        private bool CanCache()
        {
            var t = typeof(T);
            CacheAttribute ca = t.DeclaringType.GetTypeInfo().GetCustomAttribute<CacheAttribute>();
            if (ca == null)
            {
                return false;
            }
            return true;
        }

        public IQuery<T> Distinct()
        {
            this.distinct = "distinct";

            return this;
        }

        public R FirstAs<R>(Expression<Func<T, R>> singleSelector)
        {
            if (CanCache())
            {
                return this.ToQueryable().Select(singleSelector.Compile()).FirstOrDefault();
            }

            ConditionBuilder conditionBuilder = new ConditionBuilder();
            conditionBuilder.Build(singleSelector.Body);
            string column = conditionBuilder.Condition;

            limitForm = 0;
            limitLength = 1;

            BuildColumns(column);
            BuildWhere();
            BuildOrder();
            BuildLimit();

            var obj = Dapper.SqlMapper.QueryFirstOrDefault<R>(Conn, trackSql, args, BaseModule.GetTran());

            return obj;
        }

        public R FirstAs<R>() where R : class, new()
        {
            if (CanCache())
            {
                var data = this.ToQueryable().FirstOrDefault();
                if (data == null)
                {
                    return null;
                }
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<R>(json);
            }

            limitForm = 0;
            limitLength = 1;

            BuildColumns(typeof(R));
            BuildWhere();
            BuildOrder();
            BuildLimit();

            var obj = Dapper.SqlMapper.QueryFirstOrDefault<R>(Conn, trackSql, args, BaseModule.GetTran());

            return obj;


        }

        public List<R> ToListAs<R>(Expression<Func<T, R>> singleSelector)
        {
            if (CanCache())
            {
                return this.ToQueryable().Select(singleSelector.Compile()).ToList();
            }

            ConditionBuilder conditionBuilder = new ConditionBuilder();
            conditionBuilder.Build(singleSelector.Body);
            string column = conditionBuilder.Condition;

            BuildColumns(column);
            BuildWhere();
            BuildOrder();
            BuildLimit();

            var list = Dapper.SqlMapper.Query<R>(Conn, trackSql, args, BaseModule.GetTran()).ToList();

            return list;
        }

        public List<R> ToListAs<R>() where R : class, new()
        {
            if (CanCache())
            {
                var data = this.ToQueryable().ToList();
                if (data == null)
                {
                    return null;
                }
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<R>>(json);
            }


            BuildColumns(typeof(R));
            BuildWhere();
            BuildOrder();
            BuildLimit();

            var list = Dapper.SqlMapper.Query<R>(Conn, trackSql, args, BaseModule.GetTran()).ToList();

            return list;
        }


    }
}
