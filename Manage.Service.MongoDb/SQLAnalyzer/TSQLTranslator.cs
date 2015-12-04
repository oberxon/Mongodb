using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext;
using Manage.Service.MongoDb.SQLAnalyzer.Visitors;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Manage.Service.MongoDb.SQLAnalyzer
{
    public static class TSQLTranslator
    {
        public static TSqlParser Parser { private get; set; } = new TSql110Parser(false);

        public static string TranslateQuery(string tsqlQuery)
        {
            if (string.IsNullOrWhiteSpace(tsqlQuery))
                throw new ArgumentNullException("query is empty");

            var tbVisitor = new TbVisitor();
            var fieldVisitor = new FieldVisitor();
            var orderVisitor = new OrderByVisitor();
            var whereVisitor = new ConditionVisitor();
            var topCountVisitor = new TopCountVisitor();
            var offsetVisitor = new OffsetVisitor();


            IList<ParseError> errors;
            var fragments = Parser.Parse(new StringReader(tsqlQuery), out errors);
            if (errors.Count > 0)
            {
                throw new ArgumentException(new StringBuilder().Join(";", errors, error => string.Format("{0} - {1} - position: {2}", error.Number, error.Message, error.Offset)).ToString());
            }

            var selectStatement = (fragments as TSqlScript).Batches[0].Statements[0].As<SelectStatement>();

            if (selectStatement == null)
                throw new ArgumentException("not a select query: " + tsqlQuery);

            fragments.Accept(tbVisitor);
            fragments.Accept(fieldVisitor);
            fragments.Accept(orderVisitor);
            fragments.Accept(topCountVisitor);
            fragments.Accept(offsetVisitor);
            fragments.Accept(whereVisitor);

            var fieldResult = fieldVisitor.ToQuery();
            var format = fieldResult.IsAvailable ? "db.{0}.find({{{1}}},{{{2}}})" : "db.{0}.find({{{1}}})";

            var sb = new StringBuilder();
            if (fieldResult.IsAvailable)
                sb.AppendFormat(format, tbVisitor.ToQuery(), whereVisitor.ToQuery(), fieldResult);
            else
                sb.AppendFormat(format, tbVisitor.ToQuery(), whereVisitor.ToQuery());

            var orderByResult = orderVisitor.ToQuery();
            if (orderByResult.IsAvailable)
                sb.Append(orderByResult);

            var topCountResult = topCountVisitor.ToQuery();
            var offsetResult = offsetVisitor.ToQuery();
            if (topCountResult.IsAvailable && offsetResult.IsAvailable)
                throw new NotSupportedException("top count and offset can't appears together.");

            if (topCountResult.IsAvailable)
                sb.Append(topCountResult);

            if (offsetResult.IsAvailable)
                sb.Append(offsetResult);

            return sb.ToString();
        }
    }
}
