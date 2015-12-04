using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;

namespace Manage.Service.MongoDb.SQLAnalyzer.Operators
{
    class LikeOperator : TemiOperator
    {
        public string CompairValue { get; set; }
        public override QueryClip ToQuery()
        {
            var headIsWildcard = CompairValue[0] == '%';
            var tailIsWildcard = CompairValue[CompairValue.Length - 1] == '%';
            var sb = new StringBuilder();
            return sb.AppendFormat("{0} : {{$regex: '{1}{2}{3}'}}", this.FieldInfo.FieldName, headIsWildcard ? null : "^", GetRealValue(headIsWildcard, tailIsWildcard), tailIsWildcard ? null : "$");
        }

        private string GetRealValue(bool headIsWildcard, bool tailIsWildcard)
        {
            string output = CompairValue;
            if (headIsWildcard)
                output = output.Substring(1);
            if (tailIsWildcard)
                output = output.Substring(0, output.Length - 2);
            return output;
        }
    }
}
