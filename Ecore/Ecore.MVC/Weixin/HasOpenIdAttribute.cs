using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.MVC.Weixin
{
    public class OpenIdAttribute : ActionFilterAttribute
    {
        bool AuthUserInfo { get; set; }

        public OpenIdAttribute()
        {
            AuthUserInfo = false;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);



            string openid = Ecore.Frame.Weixin.Account.GetOpenId();
            if (!string.IsNullOrEmpty(openid))
            {
                return;
            }

            string pageUrl = Ecore.Frame.Weixin.Account.OpenIdCallbackUrl + "?pageurl="+ Frame.Security.AESHelper.AESEncrypt(context.HttpContext.Request.Path.Value);  // 这里少了 url coding


            string authUrlFormat = @"https://open.weixin.qq.com/connect/oauth2/authorize?appid={appid}&redirect_uri={redirect_uri}&response_type=code&scope={scope}&state=STATE#wechat_redirect";

            var authUrl = authUrlFormat.Replace("{appid}", Ecore.Frame.Weixin.Account.AppId).Replace("{redirect_uri}", pageUrl);

            if (AuthUserInfo)
            {
                //SCOPE
                authUrl = authUrl.Replace("{scope}", "scope");
            }
            else
            {
                //snsapi_userinfo
                authUrl = authUrl.Replace("{scope}", "snsapi_userinfo");
            }

            context.Result = new RedirectResult(authUrl);



        }
    }
}
