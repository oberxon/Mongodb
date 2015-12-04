using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.Misc;
using Manage.Service.MongoDb.SQLAnalyzer;
using Manage.Service.MongoDb.SQLAnalyzer.Ext;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Manage.Service.MongoDb.Behaviors.CustomCommandCollection
{
    class Query : ICustomExecuteableCommand
    {
        public string Name
        {
            get
            {
                return "Query";
            }
        }

        public async Task<IDictionary<string, object>> ExecuteAsync(IDictionary<string, object> input, singleCommand command)
        {
            var connectionString = ToStr(input.TryGet("connectionString"));
            if (connectionString == null)
                throw new ArgumentNullException("ConnectionString is empty.");

            var dbName = ToStr(input.TryGet("dbName"));
            if (dbName == null)
                throw new ArgumentNullException("dbName is empty");

            var tsqlQuery = ToStr(input.TryGet("query"));
            if (string.IsNullOrWhiteSpace(tsqlQuery))
                throw new ArgumentException("query is empty");

            var mongodbQuery = TSQLTranslator.TranslateQuery(tsqlQuery);

            var runningArgs = JsonConvert.DeserializeObject<IDictionary<string, object>>(command.template.Replace("###", mongodbQuery));

            var ret = await MongoDbHelper.GetDatabase(connectionString, dbName).ExecuteCommandAsync(runningArgs);
            ret.As<IDictionary<string, object>>().Add("mSql", mongodbQuery);
            return ret;
        }

        private string ToStr(object o)
        {
            return o == null ? null : o.ToString();
        }
    }
}
