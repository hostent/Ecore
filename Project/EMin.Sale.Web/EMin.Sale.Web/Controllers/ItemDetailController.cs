using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EMin.Sale.Model.Service;
using Ecore.Frame;

namespace EMin.Sale.Web.Controllers
{
    public class ItemDetailController : Controller
    {
        IItemInfo itemInfoService;

        IShoppingCart shoppingCartService;

        public ItemDetailController()
        {
            itemInfoService = UContainer.Get<IItemInfo>();
            shoppingCartService = UContainer.Get<IShoppingCart>();
        }

        public IActionResult Index()
        {
            string id = Request.Query["id"];

            ViewBag.ItemInfo = itemInfoService.GetItemInfo(id);

            return View();
        }

        public IActionResult AddToCart()
        {
            string itemId = Request.Query["itemId"];

            string skuId = Request.Query["skuId"];

            string menberId = "aaaaaa11111";

            int number = Convert.ToInt32(Request.Query["number"]);

            var result = shoppingCartService.AddToCart(new EMin.Model.Collection.Sale_ShoppingCart()
            {
                ItemId = itemId,
                ItemSkuId = skuId,
                MenberId = menberId,
                Number = number
            });

            return Json(result);

        }

    }
}
