using Ecore.Frame;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Mongodb
{
    public class MongoSet<T> : DbQuery<T>, Frame.ISet<T> where T : class, IEntity, new()
    {


        public MongoSet(string constr, string dbName) : base(constr, dbName)
        {
            Constr = constr;

            DbName = dbName;
        }

        IMongoDatabase _db
        {
            get
            {
                return GetMongoDb(Constr, DbName);
            }
        }


        public object Add(T t)
        {
            t.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            _db.GetCollection<T>(t.GetType().Name).InsertOne(t);
            return t.Id;
        }

        public int Delete(object id)
        {
            var result = _db.GetCollection<T>(typeof(T).Name).DeleteOne(q => q.Id == id.ToString());

            return (int)result.DeletedCount;
        }

        public int Update(T t)
        {
            var result = _db.GetCollection<T>(typeof(T).Name).ReplaceOne(q => q.Id == t.Id, t);
            return (int)result.MatchedCount;
        }

        public IQuery<T> Query()
        {
            return new DbQuery<T>(Constr, DbName);
        }

        public void ExecSql(string sql, IDictionary<string, object> par)
        {
            throw new NotImplementedException();
        }

        public void ExecXml(string reportName, IDictionary<string, object> par)
        {
            throw new NotImplementedException();
        }



        public IList<T> QuerySql(string sql, IDictionary<string, object> par)
        {
            throw new NotImplementedException();
        }

        public PageData<T> QueryXml(string reportName, PagePars param)
        {
            throw new NotImplementedException();
        }

        public IList<T> QueryXml(string reportName, IDictionary<string, object> par)
        {
            throw new NotImplementedException();
        }


    }
}
