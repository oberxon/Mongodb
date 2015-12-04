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
    class OffsetVisitor : TSqlFragmentVisitor, IMongodbQueryable
    {
        string skipCount;

        string takecount;

        public override void Visit(OffsetClause node)
        {
            takecount = node.FetchExpression.As<Literal>().Value;
            skipCount = node.OffsetExpression.As<Literal>().Value;
        }

        public QueryClip ToQuery()
        {
            if (takecount == null || takecount == "0")
                return new QueryClip();

            return string.Format(".limit({0}).skip({1})", takecount, skipCount);
        }
    }
}
