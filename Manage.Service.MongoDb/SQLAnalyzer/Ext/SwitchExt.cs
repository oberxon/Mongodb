using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.SQLAnalyzer.Ext
{
    public static class SwitchExt
    {
        public static Switch Case<T>(this Switch s, Action<T> a) where T : class
        {
            return Case(s, o => true, a, false);
        }

        public static Switch Case<T>(this Switch s, Action<T> a, bool fallThrough) where T : class
        {
            return Case(s, o => true, a, fallThrough);
        }

        public static Switch Case<T>(this Switch s, Func<T, bool> c, Action<T> a) where T : class
        {
            return Case(s, c, a, false);
        }

        public static Switch Case<T>(this Switch s, Func<T, bool> c, Action<T> a, bool fallThrough) where T : class
        {
            if (s == null)
            {
                return null;
            }
            else
            {
                T t = s.Object.As<T>();
                if (t != null)
                {
                    if (c(t))
                    {
                        a(t);
                        return fallThrough ? s : null;
                    }
                }
            }

            return s;
        }
    }
}
