using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps
{
    class FieldWarp
    {
        public string FieldName { get; set; }

        public bool NeedQuotationMark { get; set; }

        public string ConvertValue(string value)
        {
            return NeedQuotationMark ? string.Format("'{0}'", value) : value;
        }
    }
}
