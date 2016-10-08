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
                ItemSkuList = ProductDb.Get().Pro_ItemSku.Where(q => q.ItemId == id && q.IsOnSale == true).ToList(),
                ImageList = ProductDb.Get().Pro_ItemImage.Where(q => q.ItemId == id).ToList()

            };
        }

        public void SaveItem(ItemInfo info)
        {
            string id = (string)ProductDb.Get().Pro_Item.Add(info.Item);
            foreach (var item in info.ItemSkuList)
            {
                item.ItemId = id;
                ProductDb.Get().Pro_ItemSku.Add(item);
            }
            foreach (var item in info.ImageList)
            {
                item.ItemId = id;
                ProductDb.Get().Pro_ItemImage.Add(item);
            }
        }
    }
}
