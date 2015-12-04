using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Manage.Service.MongoDb.SQLAnalyzer.Visitors
{
    class TopCountVisitor : TSqlFragmentVisitor,IMongodbQueryable
    {
        string count;
        public override void Visit(TopRowFilter node)
        {
            if (node.Percent || node.WithTies)
                throw new NotSupportedException("top percent not support");
            count =node.Expression.As<Literal>().Value;
        }

        public QueryClip ToQuery()
        {
            if (count==null||count == "0")
                return new QueryClip();

            return string.Format(".limit({0})", count);
        }
    }
}
