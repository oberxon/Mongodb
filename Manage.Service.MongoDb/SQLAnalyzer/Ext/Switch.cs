using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.SQLAnalyzer.Ext
{
    public class Switch
    {
        public object Object
        {
            get; private set;
        }
        public Switch(object o)
        {
            Object = o;
        }
    }
}
