using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Manage.Service.MongoDb
{
    public static class RuleSettings
    {
        public static readonly rules Rules;
        static RuleSettings()
        {
            var serializer = new XmlSerializer(typeof(rules));
            using (var file = File.OpenRead(ConfigurationManager.AppSettings["rulePath"]))
            {
                Rules = serializer.Deserialize(file) as rules;
            }
        }


    }
}
