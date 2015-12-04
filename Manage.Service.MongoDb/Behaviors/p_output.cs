using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.Behaviors.Entities;

namespace Manage.Service.MongoDb
{
    public partial class output
    {
        public async Task<IEnumerable<KeyValuePair<string, object>>> TransformAsync(CommandContext context)
        {
            await this.transformScript.ExecuteAsync(context);
            return context.Current.Input;
        }

        public bool CanTransform(command command)
        {
            return this.supportCommands.commandName.Contains(command.name, StringComparer.OrdinalIgnoreCase);
        }
    }
}
