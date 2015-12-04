using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.Misc;
using Manage.Service.MongoDb.SQLAnalyzer.Ext;

namespace Manage.Service.MongoDb.Behaviors.CustomCommandCollection.Cache
{
    static class CustomCommandCache
    {
        readonly static IDictionary<string, ICustomExecuteableCommand> dic = new Dictionary<string, ICustomExecuteableCommand>();

        static CustomCommandCache()
        {
            var commands = from tp in Assembly.GetExecutingAssembly().GetTypes()
                           where tp.IsClass && !tp.IsAbstract && typeof(ICustomExecuteableCommand).IsAssignableFrom(tp)
                           select Activator.CreateInstance(tp).As<ICustomExecuteableCommand>();
            commands.ToList().ForEach(c => dic.Add(c.Name, c));
        }

        public static ICustomExecuteableCommand Get(string commandName)
        {
            return dic.TryGet(commandName);
        }
    }
}
