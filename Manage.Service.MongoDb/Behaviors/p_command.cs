using Manage.Service.MongoDb.Behaviors.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Manage.Service.MongoDb.Behaviors.CustomCommandCollection.Cache;
using Manage.Service.MongoDb.Misc;
using Manage.Service.MongoDb.Behaviors;

namespace Manage.Service.MongoDb
{
    public abstract partial class command
    {
        public abstract Task<IDictionary<string, object>> ExecuteAsync(CommandContext context);

        protected static void SaveResult(CommandContext context, IDictionary<string, object> result)
        {
            context.Current.ExecuteResult = result;
            context.SaveResult();
        }

        public bool IsSupport(string cmdName)
        {
            return string.Equals(this.name, cmdName, StringComparison.OrdinalIgnoreCase);
        }

        public abstract IList<string> GetAggregateRequiredKeys();

    }

    public partial class composedCommand
    {
        public override async Task<IDictionary<string, object>> ExecuteAsync(CommandContext context)
        {
            IDictionary<string, object> result = null;

            this.commands.command.ToList().ForEach(async cmdName =>
          {
              var targetCommand = RuleSettings.Rules.commands.@case.FirstOrDefault(c => string.Equals(c.name, cmdName.name, StringComparison.OrdinalIgnoreCase));
              result = await targetCommand.ExecuteAsync(context);

              SaveResult(context, result);

              if (cmdName.linkNextScript != null)
                  await cmdName.linkNextScript.ExecuteAsync(context);

          });

            return await Task.FromResult(result);
        }

        public override IList<string> GetAggregateRequiredKeys()
        {
            var q = from cmd in this.commands.command
                    select RuleSettings.Rules.commands.@case.FirstOrDefault(c => string.Equals(c.name, cmd.name, StringComparison.OrdinalIgnoreCase));

            var ret = new List<string>();

            Func<IList<string>, command, IList<string>> func = (dataClip, cmd) =>
            {
                var tmp = cmd.GetAggregateRequiredKeys();
                return dataClip.Union(tmp).ToList();
            };

            return q.Aggregate(ret, func);
        }
    }

    public partial class singleCommand
    {
        internal const string DB_NAME_KEY = "dbName";
        internal const string CONN_NAME_KEY = "connStrName";

        public override async Task<IDictionary<string, object>> ExecuteAsync(CommandContext context)
        {
            string runningTemplate = GetRawCommand(context);
            var commandArgs = JsonConvert.DeserializeObject<IDictionary<string, object>>(runningTemplate);

            var connectionString = context.Current.Input.TryGet(CONN_NAME_KEY);
            if (connectionString == null)
                throw new ArgumentNullException("connStrName should not be empty");

            var dbName = context.Current.Input.TryGet(DB_NAME_KEY, "admin");


            var result =await CommandExt.ExecuteMongoCommandAsync(connectionString.ToString(), dbName.ToString(), commandArgs);
            SaveResult(context, result);
            return result;
        }

        private string GetRawCommand(CommandContext context)
        {
            var runningTemplate = this.template;
            if (this.requires != null)
            {
                this.requires.key.ToList().ForEach(k =>
                {
                    runningTemplate = Regex.Replace(runningTemplate, string.Format("#{0}#", k), context.Current.Input[k].ToString());
                });
            }

            return runningTemplate;
        }

        public override IList<string> GetAggregateRequiredKeys()
        {
            return this.requires == null ? null : this.requires.key;
        }
    }

    public partial class customCommand
    {
        public override Task<IDictionary<string, object>> ExecuteAsync(CommandContext context)
        {
            var realCommand = CustomCommandCache.Get(this.customCommandName);
            return realCommand.ExecuteAsync(context.Current.Input, this);
        }
    }
}
