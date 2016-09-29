using Ecore.Frame;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Mongodb
{
    public class Manager
    {
        string _ConnectionString = null;

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_ConnectionString))
                {
                    _ConnectionString = Config.Default.GetConnString("mongoDbConfig");

                    if (string.IsNullOrEmpty(_ConnectionString))
                    {
                        _ConnectionString = "mongodb://127.0.0.1:27017";
                    }
                }
                return _ConnectionString;
            }
        }

        public Manager(string connstr = "")
        {
            _ConnectionString = connstr;
        }


        public MongoClient Client
        {
            get
            {
                MongoClient client = new MongoClient(ConnectionString);

                return client;
            }
        }

        public IMongoDatabase GetMongoDb(string dataBaseName)
        {
            IMongoDatabase database = Client.GetDatabase(dataBaseName);
            return database;
        }
    }
}
