using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace Ecore.Mongodb
{
    public class DbQuery<T> : IQuery<T> where T : class, new()
    {

        protected string Constr { get; set; }

        protected string DbName { get; set; }

        public DbQuery(string constr, string dbName)
        {
            Constr = constr;

            DbName = dbName;
        }


        protected IMongoDatabase GetMongoDb(string constrName, string dataBaseName)
        {

            string connectionString = Config.Default.GetConnString(constrName);
            if (String.IsNullOrWhiteSpace(connectionString)) connectionString = "mongodb://115.159.67.29:27017";

            MongoClient client = new MongoClient(connectionString);


            IMongoDatabase database = client.GetDatabase(dataBaseName);

            return database;

        }


        IMongoDatabase _db
        {
            get
            {
                return GetMongoDb(Constr, DbName);
            }
        }

        //先用一次where 后续再扩展list
        Expression<Func<T, bool>> whereExp { get; set; }

        Expression<Func<T, object>> orderExp { get; set; }

        string order = string.Empty;

        int? limitForm = null;
        int? limitLength = null;

        IQuery<T> IQuery<T>.Where(Expression<Func<T, bool>> exp)
        {
            whereExp = exp;

            return this;
        }

        IQuery<T> IQuery<T>.OrderBy(Expression<Func<T, object>> exp)
        {
            orderExp = exp;

            return this;
        }

        IQuery<T> IQuery<T>.OrderByDesc(Expression<Func<T, object>> exp)
        {
            order = "desc";
            orderExp = exp;

            return this;
        }

        IQuery<T> IQuery<T>.Limit(int form, int length)
        {
            limitForm = form;
            limitLength = length;

            return this;
        }

        T IQuery<T>.First()
        {
            IFindFluent<T, T> finder = BuildQueryable();

            return finder.FirstOrDefault();
        }

        private IFindFluent<T, T> BuildQueryable()
        {
            IFindFluent<T, T> finder = null;
            var collection = _db.GetCollection<T>(typeof(T).Name);
            if (whereExp != null)
            {
                finder = collection.Find<T>(whereExp);
            }
            else
            {
                FilterDefinition<T> filter = FilterDefinition<T>.Empty;
                finder = collection.Find<T>(filter);
            }
            if (orderExp != null)
            {
                if (order == string.Empty)
                {
                    finder = finder.SortBy(orderExp);
                }
                finder = finder.SortByDescending(orderExp);
            }
            if (limitLength != null)
            {
                finder = finder.Skip(limitForm).Limit(1);
            }

            return finder;
        }

        List<T> IQuery<T>.ToList()
        {
            IFindFluent<T, T> finder = BuildQueryable();

            return finder.ToList();
        }

        long IQuery<T>.Count()
        {

            IFindFluent<T, T> finder = null;
            var collection = _db.GetCollection<T>(typeof(T).Name);
            if (whereExp != null)
            {
                finder = collection.Find<T>(whereExp);
            }
            else
            {
                FilterDefinition<T> filter = FilterDefinition<T>.Empty;
                finder = collection.Find<T>(filter);
            }

            return finder.Count();
        }

        bool IQuery<T>.Exist()
        {
            IFindFluent<T, T> finder = null;
            var collection = _db.GetCollection<T>(typeof(T).Name);
            if (whereExp != null)
            {
                finder = collection.Find<T>(whereExp);
            }
            else
            {
                FilterDefinition<T> filter = FilterDefinition<T>.Empty;
                finder = collection.Find<T>(filter);
            }

            return finder.Any();
        }

        public IQuery<T> Distinct()
        {
            //do nothing
            return this;
        }

        public R FirstAs<R>(Expression<Func<T, R>> singleSelector)
        {
            IFindFluent<T, T> finder = BuildQueryable();

            return finder.ToList().Select(singleSelector.Compile()).FirstOrDefault();
        }

        public R FirstAs<R>() where R : class, new()
        {
            IFindFluent<T, T> finder = BuildQueryable();

            return finder.As<R>().FirstOrDefault();
        }

        public List<R> ToListAs<R>(Expression<Func<T, R>> singleSelector)
        {
            IFindFluent<T, T> finder = BuildQueryable();

            return finder.ToList().Select(singleSelector.Compile()).ToList();
        }

        public List<R> ToListAs<R>() where R : class, new()
        {
            IFindFluent<T, T> finder = BuildQueryable();

            return finder.As<R>().ToList();
        }
    }
}
