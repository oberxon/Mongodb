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
    class FieldVisitor : TSqlFragmentVisitor,IMongodbQueryable
    {
        List<string> nodes=new List<string>();

        public QueryClip ToQuery()
        {
            if (nodes.Count == 0)
                return new QueryClip();

            var q = (from n in nodes
                    select string.Format("{0}:1", n)).ToList();
            //remove _id from showing
            q.Add("_id:0");
            return new StringBuilder().Join(",", q);
        }

        public override void Visit(SelectScalarExpression node)
        {
            var n = node.Expression.As<ColumnReferenceExpression>();

            if (n != null)
            {
                //c.bbb two identifiers
                //mongodb is document
                nodes.Add(n.MultiPartIdentifier.GetFullName());                
            }

        }


    }
}
