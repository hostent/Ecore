using Ecore.Frame;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Weixin
{
    public class WxBasic : Controller
    {
        string Token
        {
            get
            {
                return Config.Default.GetAppSetting("Weixin.Token");
            }
        }

        public ActionResult CheckTokenCallback()
        {
            string signature = Request.Query["signature"];
            string timestamp = Request.Query["signature"];
            string nonce = Request.Query["signature"];

            string echostr = Request.Query["echostr"];

            string tempStr = Frame.Security.Sha1Help.Encrypt_Sha1(Token + timestamp + nonce);
            if (tempStr == signature)
            {
                return Content(echostr);
            }
            return Content("");


        }


        public ActionResult WebAuthCallback()
        {
            string code = Request.Query["code"];

            if (string.IsNullOrEmpty(code))
            {
                //todo, 未授权，跳转的错误页面

            }

            string openId = Ecore.Frame.Weixin.Account.GetOpenIdByCode(code);

            Cookie.Default.SetCookie("weixin.openid", Frame.Security.AESHelper.AESEncrypt(openId));

            string pageurl = Request.Query["pageurl"];

            return Redirect(Frame.Security.AESHelper.AESDecrypt(pageurl));

        }

    }
}
