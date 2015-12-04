using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.SQLAnalyzer.Ext
{
    public static class TypeExt
    {
        public static T As<T>(this object o) where T : class
        {
            return o as T;
        }
    }
}
