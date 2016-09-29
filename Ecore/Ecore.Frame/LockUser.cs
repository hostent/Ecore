using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class LockUser
    {
        public static ILock Default { get; set; }
    }

    public interface ILock
    {
        /// <summary>
        /// 根据tag值，锁住代码块，如果有冲突，返回false
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        bool Lock(string tag);

        void Release(string tag);
    }

}
