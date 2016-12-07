using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ecore.MVC4.Api
{
    public class BaseApiController
    {
        public virtual void OnControllerCreate(Request req)
        {

        }

        public virtual Response OnActionExecting(Request req, List<object> par)
        {
            return null;
        }

        public virtual void OnResultExecting(Request req, Response result)
        {

        }
    }
}
