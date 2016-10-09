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
        IItemInfo ItemInfoService;

        public BuyController()
        {
            ItemInfoService = UContainer.Get<IItemInfo>();
        }

        public IActionResult Index()
        {
            string id = Request.Query["id"];

            ViewBag.ItemInfo = ItemInfoService.GetItemInfo(id);

            return View();
        }



    }
}
