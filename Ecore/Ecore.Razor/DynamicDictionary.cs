using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecore.Razor
{
    public class DynamicDictionary : System.Dynamic.DynamicObject
    {
        private IDictionary<string, object> _dictionary = new Dictionary<string, object>();

        public override bool TrySetMember(System.Dynamic.SetMemberBinder binder, object value)
        {

            if (_dictionary.ContainsKey(binder.Name))
                _dictionary[binder.Name] = value;
            else
                _dictionary.Add(binder.Name, value);
            return true;
        }


        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        public IDictionary<string, object> ToDictionary()
        {
            return _dictionary;
        }

    }
}
