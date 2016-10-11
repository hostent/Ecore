using EMin.Sale.Model.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecore.Frame;
using EMin.Model.Collection;
using EMin.Sale.Logic.DataAccess;

namespace EMin.Sale.Logic
{
    public class CShoppingCart : IShoppingCart
    {
        public Result AddToCart(Sale_ShoppingCart cart)
        {
            var cartId = SaleDb.Get().Sale_ShoppingCart.Add(cart);

            return new Result(1, "", cartId);
        }

        public ShoppingCartInfo GetInfo(string menberId)
        {
            //目前只有一条记录
            var cartInfo = SaleDb.Get().Sale_ShoppingCart.Where(q => q.MenberId == menberId).First();

            ShoppingCartInfo info = new ShoppingCartInfo();

            info.Menber = SaleDb.Get().Sale_Menber.Where(q => q.Id == menberId).First();
            info.ItemList = new List<ShoppingCartItem>()
            {
                 new ShoppingCartItem()
                 {
                       
                 }
                  
            };


        }
    }
}
