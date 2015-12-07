using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.Behaviors
{
    static class CommandExt
    {
        public static async Task<IDictionary<string, object>> ExecuteMongoCommandAsync(string connectionString, string dbName, IDictionary<string, object> input)
        {
            var db = MongoDbHelper.GetDatabase(connectionString, dbName);
            return await db.ExecuteCommandAsync(input);
        }
    }
}
