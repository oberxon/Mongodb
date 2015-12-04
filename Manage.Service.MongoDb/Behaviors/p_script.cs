using Manage.Service.MongoDb.Behaviors.Entities;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb
{
    public partial class script
    {
        volatile bool isBuildUp;
        Script<IDictionary<string, object>> runableScript = null;


        public async Task ExecuteAsync(CommandContext context)
        {
            if (!isBuildUp)
            {
                runableScript = BuildRunableScript();
                isBuildUp = true;
            }
            context.AdvanceToNextFrame((await runableScript.RunAsync(context)).ReturnValue);
        }

        private Script<IDictionary<string, object>> BuildRunableScript()
        {
            //Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(this.body));

            var ret = CSharpScript.Create<IDictionary<string, object>>(this.body, this.imports.GetScriptOption(), typeof(CommandContext));
            ret.Compile();

            return ret;
        }
    }
}
