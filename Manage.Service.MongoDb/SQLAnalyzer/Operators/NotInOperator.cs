using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;

namespace Manage.Service.MongoDb.SQLAnalyzer.Operators
{
    class NotInOperator : TemiOperator
    {
        public List<FieldWarp> CompairValues { get; set; }

        public override QueryClip ToQuery()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} : {{ $nin : [", this.FieldInfo.FieldName);         

            sb.Join(", ", CompairValues, v => v.ConvertValue(v.FieldName));
            return sb.Append("]}");
        }
    }
}
