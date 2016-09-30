using EMin.Sale.Model.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMin.Model.Collection;
using EMin.Sale.Logic.DataAccess;

namespace EMin.Sale.Logic
{
    public class CItemInfo : IItemInfo
    {
        public List<Pro_ItemImage> GetItemImages(string itemId)
        {
            return ProductDb.Get().Pro_ItemImage.ToList();
        }

        public ItemInfo GetItemInfo(string id)
        {

            return new ItemInfo()
            {
                Item = ProductDb.Get().Pro_Item.Where(q => q.Id == id).First(),
                ItemSkuList = ProductDb.Get().Pro_ItemSku.Where(q => q.ItemId == id && q.IsOnSale == true).ToList()

            };
        }
    }
}
