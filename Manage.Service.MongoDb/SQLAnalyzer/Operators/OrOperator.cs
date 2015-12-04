using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;

namespace Manage.Service.MongoDb.SQLAnalyzer.Operators
{
    class OrOperator : BooleanOperator
    {
        public override QueryClip ToQuery()
        {
            var sb = new StringBuilder("$or : [");
            sb.Join(", ", operators, op => string.Format("{{{0}}}", op.ToQuery().Query));
            sb.Append("]");
            return sb;
        }
    }
}
