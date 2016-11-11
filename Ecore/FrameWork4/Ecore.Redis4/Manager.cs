using Ecore.Frame;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Redis4
{
    public class Manager
    {

       static  ConnectionMultiplexer _redis = null;

        public ConnectionMultiplexer RedisManager
        {
            get
            {
                if (_redis == null)
                {
                    _redis = GetManager();
                }

                return _redis;
            }
        }

        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = Config.Default.GetConnString("ConnString.redis.default");

                //redis0:6379,redis1:6379,allowAdmin=true"

                //配置选项
                //ConfigurationOptions 包含大量的配置选项，一些常用的配置如下：

                //abortConnect ： 当为true时，当没有可用的服务器时则不会创建一个连接
                //allowAdmin ： 当为true时 ，可以使用一些被认为危险的命令
                //channelPrefix：所有pub / sub渠道的前缀
                //connectRetry ：重试连接的次数
                //connectTimeout：超时时间
                //configChannel： Broadcast channel name for communicating configuration changes
                //defaultDatabase ： 默认0到 - 1
                //keepAlive ： 保存x秒的活动连接
                //name:ClientName
                //password:password
                //proxy:代理 比如 twemproxy
                //resolveDns : 指定dns解析
                //serviceName ： Not currently implemented(intended for use with sentinel)
                //                        ssl ={ bool} ： 使用sll加密
                //   sslHost = { string } ： 强制服务器使用特定的ssl标识
                //       syncTimeout = { int } ： 异步超时时间
                //           tiebreaker = { string }：Key to use for selecting a server in an ambiguous master scenario
                //               version ={ string} ： Redis version level(useful when the server does not make this available)
                //writeBuffer ={ int} ： 输出缓存区的大小

                //各配置项用逗号分割


            }

            

            return ConnectionMultiplexer.Connect(connectionString);
        }
    }
}
