using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.Behaviors.Entities;

namespace Manage.Service.MongoDb
{
    public partial class rules
    {
        public async Task<IEnumerable<KeyValuePair<string, object>>> RunAsync(string cmdName, CommandContext context)
        {
            //Contract.Requires(!string.IsNullOrWhiteSpace(cmdName));

            command cmd;
            if (!this.commands.IsSupport(cmdName, out cmd))
                throw new NotSupportedException(cmdName + " not found.");

            var rawResult =await cmd.ExecuteAsync(context);

            output transformer;
            if (this.outputs == null || !this.outputs.CanTransform(cmd, out transformer))
                return rawResult;

            return await transformer.TransformAsync(context);
        }

        public IEnumerable<string> GetCommandRequiredKeys(string cmdName)
        {
            //Contract.Requires(!string.IsNullOrWhiteSpace(cmdName));

            command cmd;
            if (this.commands.IsSupport(cmdName, out cmd))
                return cmd.GetAggregateRequiredKeys();

            throw new NotSupportedException(cmdName + " not found.");
        }
    }
}
