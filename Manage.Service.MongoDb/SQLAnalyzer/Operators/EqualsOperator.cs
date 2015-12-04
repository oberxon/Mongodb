using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;

namespace Manage.Service.MongoDb.SQLAnalyzer.Operators
{
    class EqualsOperator : TemiOperator
    {
        public string CompairValue { get; set; }

        public override QueryClip ToQuery()
        {
            return string.Format("{0} : {{ $eq : {1} }}", this.FieldInfo.FieldName, this.FieldInfo.ConvertValue(CompairValue));
        }
    }
}
