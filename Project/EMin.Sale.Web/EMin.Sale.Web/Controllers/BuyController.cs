using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EMin.Sale.Model.Service;
using Ecore.Frame;

namespace EMin.Sale.Web.Controllers
{
    public class BuyController : Controller
    {
        IShoppingCart shoppingCart;

        public BuyController()
        {
            shoppingCart = UContainer.Get<IShoppingCart>();
        }

        public IActionResult Index()
        {
            string cartId = Request.Query["cartId"];

            ViewBag.ItemInfo = ItemInfoService.GetItemInfo(id);

            return View();
        }



    }
}
