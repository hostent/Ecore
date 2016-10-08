using Ecore.Frame;
using Ecore.Mongodb;
using EMin.Model.Collection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Sale.Logic.DataAccess
{
    public class ProductDb
    {

        string dbName = "ProductDb";

        string connName = "mongodb.SaleDb";

        public static ProductDb Get()
        {
            return new ProductDb();
        }


        public ISet<Pro_Item> Pro_Item
        {
            get
            {
                return new MongoSet<Pro_Item>(connName, dbName);
            }
        }

        public ISet<Pro_ItemImage> Pro_ItemImage
        {
            get
            {
                return new MongoSet<Pro_ItemImage>(connName, dbName);
            }
        }

        public ISet<Pro_ItemSku> Pro_ItemSku
        {
            get
            {
                return new MongoSet<Pro_ItemSku>(connName, dbName);
            }
        }


    }
}
