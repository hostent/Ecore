using Ecore.Frame;
using Ecore.MVC;
using Ecore.MVC.Api;
using Ecore.MVC.Web;
using EMin.Oms.Model.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMin.Oms.Web.Controller
{
    public class Demo01 : BaseController
    {
        [Mapping("Demo01.html")]
        public StringResult Action01()
        {

            string result = UContainer.Get<IDemo>().test01("你猜");

            return new StringResult(result);
        }
    }
}
