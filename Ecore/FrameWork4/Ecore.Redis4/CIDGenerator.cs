using Ecore.Frame;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Redis4
{
    public class CIDGenerator : IIDGenerator
    {

        static long Index = 0;

        static DateTime LastTime = DateTime.Now;

        static object _lock = new object();


        string TimeHost
        {
            get
            {
                string set = Config.Default.GetAppSetting("Redis.IDGenerator.TimeHost");
                if (string.IsNullOrEmpty(set))
                {
                    set = "127.0.0.1:6379";
                }

                return set;
            }
        }

        string ServerFx
        {
            get
            {
                string set = Config.Default.GetAppSetting("Redis.IDGenerator.ServerFx");
                if (string.IsNullOrEmpty(set))
                {
                    set = "0";
                }

                return set;
            }
        }

        public string NewID()
        {
            var redisClient = new Manager().RedisManager;
            //time
            DateTime now = redisClient.GetServer(TimeHost).Time(CommandFlags.None);//这里的时间从redis中读取
            redisClient.CloseAsync();

            long ms = Convert.ToInt64((now - new DateTime(2016, 1, 1)).TotalMilliseconds);

            string result = "";

            Change36(ms, ref result);

            result = result.PadLeft(8, '0');

            lock (_lock)
            {
                if ((now - LastTime).TotalMilliseconds == 0)
                {
                    Index = Index + 1;
                }
                else
                {
                    Index = 0;
                }

                LastTime = now;
            }

            string indexStr = "";

            Change36(Index, ref indexStr);

            result = result.ToString() + ServerFx + indexStr;

            return result;


        }


        readonly string[] array = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "s", "y", "z" };

        public void Change36(long number, ref string result)
        {
            long remainder = number % 36;
            result = array[remainder] + result;
            long a = number / 36;
            if (a >= 36)
            {
                Change36(a, ref result);
            }
            else if (a > 0)
            {
                result = array[a] + result;
            }
        }

    }
}
