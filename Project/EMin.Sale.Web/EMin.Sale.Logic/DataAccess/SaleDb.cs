
using Ecore.Frame;
using Ecore.Mongodb;
using EMin.Model.Collection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Sale.Logic.DataAccess
{
    public class SaleDb
    {
        string dbName = "SaleDb";

        string connName = "mongodb.SaleDb";

        public static SaleDb Get()
        {
            return new SaleDb();
        }


        public ISet<Sale_Order> Sale_Order
        {
            get
            {
                return new MongoSet<Sale_Order>(connName, dbName);
            }
        }

        public ISet<Sale_Menber> Sale_Menber
        {
            get
            {
                return new MongoSet<Sale_Menber>(connName, dbName);
            }
        }

        public ISet<Sale_Trade> Sale_Trade
        {
            get
            {
                return new MongoSet<Sale_Trade>(connName, dbName);
            }
        }

        public ISet<Sale_TradePayment> Sale_TradePayment
        {
            get
            {
                return new MongoSet<Sale_TradePayment>(connName, dbName);
            }
        }

        public ISet<Sale_TradeRecord> Sale_TradeRecord
        {
            get
            {
                return new MongoSet<Sale_TradeRecord>(connName, dbName);
            }
        }
    }
}
