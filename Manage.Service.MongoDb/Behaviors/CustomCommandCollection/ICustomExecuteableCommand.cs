using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.Behaviors.CustomCommandCollection
{
    interface ICustomExecuteableCommand : INamedObject
    {
        Task<IDictionary<string, object>> ExecuteAsync(IDictionary<string, object> input, singleCommand command);
    }
}
