using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{
    public class IDGenerator
    {
        public static IIDGenerator Default { get; set; }
    }

    public interface IIDGenerator
    {
        string NewID();
    }
}
