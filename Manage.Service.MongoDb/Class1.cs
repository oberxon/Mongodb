using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Manage.Service.MongoDb
{
    public class Class1
    {
        public class G
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class ExpandoObjectWapper
        {
            public ExpandoObject Target { get; set; }

            public Func<ExpandoObject, StringBuilder> Del { get; set; }

        }

        public class TestStatic
        {
            public static int Count { get; set; }
        }

        public void GetDbStats()
        {
            MongoClient c = new MongoClient("mongodb://localhost");

            var db = c.GetDatabase("test");

            

            var r = ExecuteCommand(db, new Dictionary<string, object> { { "dbStats", 1 } }).Result;

            var sb = new StringBuilder();
           
             var ret = CSharpScript.EvaluateAsync<string>(@"
                    var dic=new Dictionary<string,object>();
                    dic.Add(""s"",1);
                    var x=Del(Target);
                    Class1.TestStatic.Count++;
                    return x.ToString();
                ", globals: new ExpandoObjectWapper
            {
                Target = r,
                Del = o =>
                {
                    Dump(sb, o);
                    return sb;
                }
            }, options: ScriptOptions.Default.WithImports("Manage.Service.MongoDb", "System.Collections.Generic").WithReferences(this.GetType().Assembly));

            

            var r2 = ExecuteCommand(db, new Dictionary<string, object> { { "collStats", "test" } }).Result;

            sb = new StringBuilder();
            Dump(sb, r);

            var r3 = ExecuteCommand(c.GetDatabase("test"), new Dictionary<string, object> { { "serverStatus", 1 } }).Result;

            sb = new StringBuilder();

            Dump(sb, r3);


            var x = c.GetDatabase("test").ListCollectionsAsync().ContinueWith(async tk =>
            {
                var y = await tk;
                return await y.ToListAsync();
            });
            var x2 = x.Unwrap().Result;
        }

        public async Task<ExpandoObject> Test()
        {
            MongoClient c = new MongoClient("mongodb://localhost");

            var db = c.GetDatabase("test");

            return await ExecuteCommand(db, new Dictionary<string, object> { { "collStats", "test" } });
        }


        private void Dump(StringBuilder sb, IEnumerable<KeyValuePair<string, object>> target, int level = 0)
        {
            foreach (var i in target)
            {
                if (i.Value is ExpandoObject)
                {
                    sb.Append(" ".PadLeft(level * 5)).AppendLine("[" + i.Key + "]");
                    Dump(sb, i.Value as ExpandoObject, level + 1);
                }
                else
                {
                    sb.Append(" ".PadLeft(level * 5)).AppendLine("[" + i.Key + "] " + i.Value);
                }
            }
        }

        private async Task<ExpandoObject> ExecuteCommand(IMongoDatabase database, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return await database.RunCommandAsync(new BsonDocumentCommand<ExpandoObject>(new BsonDocument(parameters)));
        }


    }

}
