using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;

namespace Manage.Service.MongoDb.SQLAnalyzer.Operators
{
    class BetweenOperator : TemiOperator
    {
        public string CompairValue1 { get; set; }
        public string CompairValue2 { get; set; }

        public override QueryClip ToQuery()
        {
            var lowerBound = string.Compare(CompairValue1, CompairValue2) > 0 ? CompairValue2 : CompairValue1;
            var higherBound = CompairValue1 == lowerBound ? CompairValue2 : CompairValue1;

            var sb = new StringBuilder();
            return sb.AppendFormat("{0} : {{ $gte : {1}, $lte : {2} }}", this.FieldInfo.FieldName, this.FieldInfo.ConvertValue(lowerBound), this.FieldInfo.ConvertValue(higherBound));
        }
    }
}
