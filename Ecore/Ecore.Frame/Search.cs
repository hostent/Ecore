using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Frame
{

    public interface ISearch<T>: IQuery<T> where T : class, new()
    {
        IQuery<T> QueryString(string queryString, DefaultOperator op);

        
    }

    public enum DefaultOperator
    {
        Or,
        And
    }
}
