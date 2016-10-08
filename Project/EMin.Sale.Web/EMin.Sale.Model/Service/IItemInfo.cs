using EMin.Model.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Sale.Model.Service
{
    public interface IItemInfo
    {
        ItemInfo GetItemInfo(string id);

        void SaveItem(ItemInfo info);
    }

    public class ItemInfo 
    {
        public Pro_Item Item { get; set; }

        public List<Pro_ItemSku> ItemSkuList { get; set; }

        public List<Pro_ItemImage> ImageList { get; set; }
    }

}
