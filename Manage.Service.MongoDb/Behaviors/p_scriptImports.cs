using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb
{
    public partial class scriptImports
    {
        public ScriptOptions GetScriptOption()
        {
            var options = ScriptOptions.Default;
            if (this.assemblies != null)
                options= options.AddReferences(this.assemblies.GetAssemblies());
            if (this.nameSpaces != null)
                options = options.AddImports(this.nameSpaces.GetNameSpaces());
            return options;
        }
    }
}
