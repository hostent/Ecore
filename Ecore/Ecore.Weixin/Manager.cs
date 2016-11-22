using Ecore.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Weixin
{
    public class Manager : IWeixin
    {
        public string AppId
        {
            get
            {
                return Config.Default.GetAppSetting("weixin.appid");
            }
        }

        public string OpenIdCallbackUrl
        {
            get
            {
                return Config.Default.GetAppSetting("weixin.openidcallbackurl");
            }
        }

        public string Secret
        {
            get
            {
                return Config.Default.GetAppSetting("weixin.secret");
            }
        }




        public string GetAccessToken()
        {
            AccessTokenResult accessTokenResult = Cache.Default.Get<AccessTokenResult>("Weixin.AccessToken");
            if (accessTokenResult == null)
            {

                string url = @"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={appId}&secret={secret}";

                url = url.Replace("{appId}", AppId).Replace("{secret}", Secret);

                string result = MyHttpClient.Default.Get(url, null);

                accessTokenResult = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessTokenResult>(result);

                Cache.Default.Add("Weixin.AccessToken", accessTokenResult, DateTime.Now.AddHours(1));
            }
            return accessTokenResult.access_token;

        }

        public string GetOpenId()
        {
            var cookieValue = Cookie.Default.GetCookieValue("weixin.openid");
            if(string.IsNullOrEmpty(cookieValue))
            {
                return null;
            }
            return Frame.Security.AESHelper.AESDecrypt(cookieValue);
        }

        public string GetOpenIdByCode(string code)
        {
            string url = @"https://api.weixin.qq.com/sns/oauth2/access_token?appid={appId}&secret={secret}&code={code}&grant_type=authorization_code";

            url = url.Replace("{appId}", AppId).Replace("{secret}", Secret).Replace("{code}", code);
            string result = MyHttpClient.Default.Get(url, null);
            var openIdResult = Newtonsoft.Json.JsonConvert.DeserializeObject<OpenIdResult>(result);

            return openIdResult.openid;

        }









        public class AccessTokenResult
        {
            public string access_token { get; set; }

            public string expires_in { get; set; }
        }
        public class OpenIdResult
        {
            public string access_token { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string openid { get; set; }
            public string scope { get; set; }
        }
    }


}
