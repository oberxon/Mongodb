using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Manage.Service.MongoDb.SQLAnalyzer.Ext
{
    public static class MultiPartIdentifierExt
    {
        public static string GetFullName(this MultiPartIdentifier id)
        {
            if (id.Identifiers.Count == 1)
                return id.Identifiers[0].Value;

            var sb = new StringBuilder();

            return sb.Join(".", id.Identifiers, (_id) => _id.Value).ToString();
        }
    }
}
