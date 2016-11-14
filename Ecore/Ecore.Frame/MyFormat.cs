using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public static class MyFormat
    {
        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTime.Now.Kind);

            return Convert.ToInt64((DateTime.Now.ToUniversalTime() - start).TotalSeconds);

        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTime.Now.Kind);
            start = start.Add(DateTime.Now.TimeOfDay - DateTime.UtcNow.TimeOfDay);

            return start.AddSeconds(timestamp);
        }
    }
}
