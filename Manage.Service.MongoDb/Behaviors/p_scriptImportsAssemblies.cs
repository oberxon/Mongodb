using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb
{
    public partial class scriptImportsAssemblies
    {
        public Assembly[] GetAssemblies()
        {
            var currentAssembly = new Assembly[] { this.GetType().Assembly };
            return this.add == null || this.add.Length == 0
                ? currentAssembly
                : (from locator in this.add select locator.GetAssembly()).Union(currentAssembly).ToArray();
        }
    }
}
