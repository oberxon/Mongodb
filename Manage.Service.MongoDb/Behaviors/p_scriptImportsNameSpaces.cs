using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb
{
    public partial class scriptImportsNameSpaces
    {
        public string[] GetNameSpaces()
        {
            return this.add == null || this.add.Length == 0 ? null : this.add;
        }
    }
}
