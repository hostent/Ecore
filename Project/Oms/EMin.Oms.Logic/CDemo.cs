using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMin.Oms.Model.Service;

namespace EMin.Oms.Logic
{
    public class CDemo : IDemo
    {
        public string test01(string input)
        {
            return "这个就是你要的结果吧：" + input;
        }
    }
}
