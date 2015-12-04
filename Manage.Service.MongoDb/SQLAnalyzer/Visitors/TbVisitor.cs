using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Manage.Service.MongoDb.SQLAnalyzer.Visitors
{
    class TbVisitor: TSqlFragmentVisitor,IMongodbQueryable
    {
        string collectionName;
        public override void Visit(NamedTableReference node)
        {
            collectionName = node.SchemaObject.BaseIdentifier.Value;
            base.Visit(node);
        }

        public QueryClip ToQuery()
        {
            return collectionName;
        }

        
    }
}
