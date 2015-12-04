using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manage.Service.MongoDb.SQLAnalyzer.Ext;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Manage.Service.API.Models
{
    public class ExecuteCommand
    {
        public string Name { get; set; }

        public object CommandMessage { get; set; }

        public IDictionary<string, object> GetCommandArgs()
        {
            if (this.CommandMessage == null)
                return null;
            var tmp = this.CommandMessage.As<string>();
            if (tmp != null)
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(tmp);

            var tmp2 = this.CommandMessage.As<JObject>().ToList<KeyValuePair<string, JToken>>();
            if (tmp2 != null)
            {
                var dic = new Dictionary<string, object>();
                tmp2.ForEach(kvp => dic.Add(kvp.Key, kvp.Value));
                return dic;
            }
            return null;
        }
    }

    public class ExecuteResult<T>
    {
        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public IEnumerable<T> Details { get; set; }
    }
}