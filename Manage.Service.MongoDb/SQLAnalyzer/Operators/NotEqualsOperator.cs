using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;

namespace Manage.Service.MongoDb.SQLAnalyzer.Operators
{
    class NotEqualsOperator : TemiOperator
    {
        public string CompairValue { get; set; }
        public override QueryClip ToQuery()
        {
            var sb = new StringBuilder();
            return sb.AppendFormat("{0} : {{ $ne : {1}}}", this.FieldInfo.FieldName, this.FieldInfo.ConvertValue(CompairValue));
        }
    }
}
