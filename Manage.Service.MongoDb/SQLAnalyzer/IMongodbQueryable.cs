using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;

namespace Manage.Service.MongoDb.SQLAnalyzer
{
    interface IMongodbQueryable
    {
        QueryClip ToQuery();
    }
}
