using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb
{
    public partial class rulesCommands
    {
        public bool IsSupport(string cmdName, out command command)
        {
            command = null;

            var q = (from cmd in this.@case
                     where cmd.IsSupport(cmdName)
                     select cmd).ToList();
            var ret = q.Count == 1;
            if (ret)
                command = q[0];
            return ret;
        }
    }
}
