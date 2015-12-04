using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps
{
    class QueryClip
    {
        public QueryClip(string query = null)
        {
            Query = query;
            IsAvailable = !string.IsNullOrEmpty(query);
        }
        public string Query { get; set; }

        public bool IsAvailable { get; set; }


        public static implicit operator QueryClip(string str)
        {
            return new QueryClip(str);
        }

        public static implicit operator QueryClip(StringBuilder sb)
        {
            return new QueryClip(sb.ToString());
        }

        public override string ToString()
        {
            return Query;
        }
    }
}
