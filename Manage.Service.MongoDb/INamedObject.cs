using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb
{
    interface INamedObject
    {
        string Name { get; }
    }
}
