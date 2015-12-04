using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.Behaviors.Entities
{
    public class ScriptExecuteResult
    {
        public IDictionary<string, object> Input { get; set; }

        public IDictionary<string, object> ExecuteResult { get; set; }
    }

    public class CommandContext
    {
        public CommandContext(IDictionary<string, object> input)
        {
            Current = new ScriptExecuteResult { Input = input };            
        }

        public ScriptExecuteResult Current { get; private set; }

        public List<ScriptExecuteResult> ExecuteResultCollection { get; } = new List<ScriptExecuteResult>();

        public void SaveResult()
        {  
            ExecuteResultCollection.Add(Current);
        }

        public void AdvanceToNextFrame(IDictionary<string, object> input)
        {
            Current= new ScriptExecuteResult { Input = input };
        }
    }
}
