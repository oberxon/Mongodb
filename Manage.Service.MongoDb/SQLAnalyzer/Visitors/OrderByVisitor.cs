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
    class OrderByVisitor : TSqlFragmentVisitor, IMongodbQueryable
    {
        List<Tuple<string, bool>> nodes = new List<Tuple<string, bool>>();
        public override void Visit(ExpressionWithSortOrder node)
        {
            //default to asc
            bool isDesc = node.SortOrder == SortOrder.Descending;

            var n = node.Expression.As<ColumnReferenceExpression>();
            if (n != null)
            {
                nodes.Add(Tuple.Create(n.MultiPartIdentifier.GetFullName(), isDesc));
            }
        }

        public QueryClip ToQuery()
        {
            if (nodes.Count == 0)
                return new QueryClip();
            //desc:-1 asc:1
            return string.Format(".sort({{{0}}})", new StringBuilder().Join(",", nodes, tp => string.Format("{0}:{1}", tp.Item1, tp.Item2 ? "-1" : "1")));
        }
    }
}
