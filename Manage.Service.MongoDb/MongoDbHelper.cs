using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb
{
    public static class MongoDbHelper
    {
        public static async Task<ExpandoObject> ExecuteCommandAsync(this IMongoDatabase database, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            //Contract.Requires<ArgumentException>(database != null);
            //Contract.Requires<ArgumentException>(parameters != null);

            return await database.RunCommandAsync(new BsonDocumentCommand<ExpandoObject>(new BsonDocument(parameters)));
        }

        public static IMongoDatabase GetDatabase(string connectionString, string dbName)
        {
            //Contract.Requires<ArgumentException>(dbName != null);
            return new MongoClient(connectionString).GetDatabase(dbName);
        }
    }
}
