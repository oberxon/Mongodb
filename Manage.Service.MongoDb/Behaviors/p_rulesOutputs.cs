using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb
{
    public partial class rulesOutputs
    {
        public bool CanTransform(command cmd, out output candidate)
        {
            candidate = null;

            var candidates = this.caseField.Where(o => o.CanTransform(cmd)).ToList();
            //only return true when one candidate is available
            bool ret = candidates.Count == 1;
            if (ret)
                candidate = candidates[0];
            return ret;
        }
    }
}
