using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Ecore.Frame
{



    public class UContainer
    {
        public static IFactory Factory { get; set; }

        public static T Get<T>() where T : class
        {
            return Factory.Get<T>();
        }

    }

    public interface IFactory
    {
        T Get<T>() where T : class;
    }


}
