using Ecore.Frame;
using Ecore.Frame.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Ecore.MVC4.Api
{
    public class RpcAuth
    {
        public string Key { get; set; }

        public string Sign { get; set; }

        public long Timestamp { get; set; }

        string Secret
        {
            get
            {
                var s = Config.Default.GetAppSetting(Key + "-Secret");
                return s;
            }
        }

        /// <summary>
        ///  md5(json + key + secret + ts)
        /// </summary>
        public Result Check(string json)
        {

            var dt = MyFormat.UnixTimestampToDateTime(Timestamp);

            if (dt > DateTime.Now)
            {
                Result.Failure("时间戳错误，无法调用接口");
            }
            if (dt.AddMinutes(2) < DateTime.Now)
            {
                Result.Failure("参数时间过期，无法调用接口");
            }

            var calcSign = MD5Helper.Encrypt_MD5(json + Key + Secret + Timestamp.ToString());

            if (string.IsNullOrEmpty(Secret))
            {
                Result.Failure("账号不存在，无法调用接口");
            }

            if (Sign != calcSign)
            {
                Result.Failure("签名错误，无法调用接口");
            }
            return Result.Succeed(); ;
        }
    }
}
