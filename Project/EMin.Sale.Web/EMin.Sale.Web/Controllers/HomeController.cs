using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ecore.Frame;
using EMin.Sale.Model.Service;

namespace EMin.Sale.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //NewMethod();

            return View();
        }

        private static void NewMethod()
        {
            ItemInfo info = new ItemInfo();

            info.Item = new EMin.Model.Collection.Pro_Item()
            {
                Detail = "aaaaaaaaaa",
                IsOnSale = true,
                UnitPrice = 37.5M,
                Title = "ababababa",

            };

            info.ItemSkuList = new List<EMin.Model.Collection.Pro_ItemSku>()
            {
                 new EMin.Model.Collection.Pro_ItemSku()
                 {
                       IsOnSale = true,
                       Pattern="",
                       UnitPrice =37.5M,
                 }
            };

            info.ImageList = new List<EMin.Model.Collection.Pro_ItemImage>()
            {
                new EMin.Model.Collection.Pro_ItemImage()
                {
                     Url="http://image.51daigou.com//ProductImage/2014/4/24/20140424175739.jpg"
                },
                new EMin.Model.Collection.Pro_ItemImage()
                {
                     Url="http://image.51daigou.com//ProductImage/japanamazon/9d8e5446490f5e3601e3f66c0d87aeda.jpg"
                },
                new EMin.Model.Collection.Pro_ItemImage()
                {
                     Url="http://image.51daigou.com//ProductImage/japanamazon/0ad7351e141f59d4f75507de68a535e9.jpg"
                }
            };




            UContainer.Get<IItemInfo>().SaveItem(info);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
