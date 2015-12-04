using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;

namespace Manage.Service.MongoDb.SQLAnalyzer.Operators
{
    class AndOperator : BooleanOperator
    {
        public override QueryClip ToQuery()
        {
            return new StringBuilder().Join(", ", operators, op => op.ToQuery().Query);
        }
    }
}
