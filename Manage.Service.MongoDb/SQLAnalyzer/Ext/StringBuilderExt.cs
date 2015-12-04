using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.SQLAnalyzer.Ext
{
    public static class StringBuilderExt
    {
        public static StringBuilder Join(this StringBuilder sb, string seperator, IEnumerable<string> target)
        {
            var _sep = string.Empty;
            return target.Aggregate(sb, (_sb, _target) =>
            {
                _sb.Append(_sep).Append(_target);
                _sep = seperator;
                return _sb;
            });
        }

        public static StringBuilder Join<T>(this StringBuilder sb, string seperator, IEnumerable<T> target, Func<T, string> targetFetcher)
        {
            var _sep = string.Empty;
            return target.Aggregate(sb, (_sb, _target) =>
            {
                _sb.Append(_sep).Append(targetFetcher(_target));
                _sep = seperator;
                return _sb;
            });
        }
    }
}
