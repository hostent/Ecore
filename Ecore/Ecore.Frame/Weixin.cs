using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{

    public class Weixin
    {



        public static IWeixin Account { get; set; }
    }

    public interface IWeixin
    {
        string AppId { get; }

        string Secret { get; }

        string OpenIdCallbackUrl { get; }

        string GetAccessToken();

        string GetOpenId();

        string GetOpenIdByCode(string code);
    }
}
