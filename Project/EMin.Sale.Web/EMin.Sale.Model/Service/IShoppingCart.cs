using Ecore.Frame;
using EMin.Model.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Sale.Model.Service
{
    public interface IShoppingCart
    {
        Result AddToCart(Sale_ShoppingCart cart);

        ShoppingCartInfo GetInfo(string menberId);


    }

    public class ShoppingCartInfo
    {
        public Sale_Menber Menber { get; set; }

        public List<ShoppingCartItem> ItemList { get; set; }

    }

    public class ShoppingCartItem : Pro_Item
    {
        public Pro_ItemSku Sku { get; set; }

        public Pro_ItemImage MainImage { get; set; }
    }

}
