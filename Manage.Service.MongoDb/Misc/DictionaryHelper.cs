using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.Misc
{
    public static class DictionaryHelper
    {
        public static V TryGet<T, V>(this IDictionary<T, V> target, T key, V defaultValue = default(V))
        {
            V value;
            if (target.TryGetValue(key, out value))
                return value;
            return defaultValue;
        }
    }
}
