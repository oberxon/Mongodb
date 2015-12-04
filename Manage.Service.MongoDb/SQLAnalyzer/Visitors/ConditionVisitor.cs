using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manage.Service.MongoDb.SQLAnalyzer.Ext;
using Manage.Service.MongoDb.SQLAnalyzer.Ext.Warps;
using Manage.Service.MongoDb.SQLAnalyzer.Operators;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Manage.Service.MongoDb.SQLAnalyzer.Visitors
{
    class ConditionVisitor : TSqlFragmentVisitor, IMongodbQueryable
    {
        Operator _op = null;

        public override void Visit(WhereClause node)
        {

            _op = ProcessExpression(node.SearchCondition, _op);
        }

        Operator ProcessExpression(BooleanExpression expr, Operator op)
        {

            new Switch(expr)
                .Case<BooleanBinaryExpression>(exp =>
                {
                    Operator currentOp = null;

                    bool isAndOper = exp.BinaryExpressionType == BooleanBinaryExpressionType.And;

                    currentOp = isAndOper ? new AndOperator().As<Operator>() : new OrOperator();
                    if (op == null)
                        op = currentOp;
                    else
                        op.As<BooleanOperator>().AddOperator(currentOp);

                    ProcessExpression(exp.FirstExpression, currentOp);
                    ProcessExpression(exp.SecondExpression, currentOp);
                })
                .Case<LikePredicate>(exp =>
                {
                    var columnName = exp.FirstExpression.As<ColumnReferenceExpression>().MultiPartIdentifier.GetFullName();
                    var value = exp.SecondExpression.As<Literal>().Value;

                    var oper = new LikeOperator
                    {
                        CompairValue = value,
                        FieldInfo = new FieldWarp
                        {
                            FieldName = columnName,
                            NeedQuotationMark = true
                        }
                    };

                    if (op == null)
                        op = oper;
                    else
                        op.As<BooleanOperator>().AddOperator(oper);
                })
                .Case<InPredicate>(exp =>
                {
                    var columnName = exp.Expression.As<ColumnReferenceExpression>().MultiPartIdentifier.GetFullName();
                    var value = from vRaw in exp.Values
                                let v = vRaw.As<Literal>()
                                select new FieldWarp { FieldName = v.Value, NeedQuotationMark = v.LiteralType == LiteralType.String };

                    var oper = exp.NotDefined
                    ? new NotInOperator { CompairValues = value.ToList(), FieldInfo = new FieldWarp { FieldName = columnName } }.As<Operator>()
                    : new InOperator { CompairValues = value.ToList(), FieldInfo = new FieldWarp { FieldName = columnName } };

                    if (op == null)
                        op = oper;
                    else
                        op.As<BooleanOperator>().AddOperator(oper);
                })
                .Case<BooleanParenthesisExpression>(exp =>
                {
                    op = ProcessExpression(exp.Expression, op);
                })
                .Case<BooleanComparisonExpression>(exp =>
                {
                    Operator oper = null;
                    switch (exp.ComparisonType)
                    {
                        case BooleanComparisonType.Equals:
                            oper = new EqualsOperator
                            {
                                FieldInfo = GetFieldInfo(exp),
                                CompairValue = exp.SecondExpression.As<Literal>().Value,

                            };
                            break;
                        case BooleanComparisonType.GreaterThan:
                            oper = new GreaterThanOperator
                            {
                                FieldInfo = GetFieldInfo(exp),
                                CompairValue = exp.SecondExpression.As<Literal>().Value,

                            };
                            break;
                        case BooleanComparisonType.GreaterThanOrEqualTo:
                            oper = new GreaterThanOrEqualsOperator
                            {
                                FieldInfo = GetFieldInfo(exp),
                                CompairValue = exp.SecondExpression.As<Literal>().Value,
                            };
                            break;
                        case BooleanComparisonType.LessThan:
                            oper = new LessThanOperator
                            {
                                FieldInfo = GetFieldInfo(exp),
                                CompairValue = exp.SecondExpression.As<Literal>().Value,
                            };
                            break;
                        case BooleanComparisonType.LessThanOrEqualTo:
                            oper = new LessThanOrEqualsOperator
                            {
                                FieldInfo = GetFieldInfo(exp),
                                CompairValue = exp.SecondExpression.As<Literal>().Value,
                            };
                            break;
                        case BooleanComparisonType.NotEqualToBrackets:
                        case BooleanComparisonType.NotEqualToExclamation:
                            oper = new NotEqualsOperator
                            {
                                FieldInfo = GetFieldInfo(exp),
                                CompairValue = exp.SecondExpression.As<Literal>().Value,
                            };
                            break;
                        default:
                            //  "".Dump("WTF");
                            break;
                    }
                    if (op == null)
                        op = oper;
                    else
                        op.As<BooleanOperator>().AddOperator(oper);
                })
                .Case<BooleanTernaryExpression>(exp =>
                {
                    if (exp.TernaryExpressionType != BooleanTernaryExpressionType.Between)
                        throw new NotSupportedException("only support between");

                    var oper = new BetweenOperator
                    {
                        FieldInfo = new FieldWarp { FieldName = exp.FirstExpression.As<ColumnReferenceExpression>().MultiPartIdentifier.GetFullName() },
                        CompairValue1 = exp.SecondExpression.As<Literal>().Value,
                        CompairValue2 = exp.ThirdExpression.As<Literal>().Value,
                    };

                    oper.FieldInfo.NeedQuotationMark = exp.SecondExpression.As<Literal>().LiteralType == LiteralType.String;

                    if (op == null)
                        op = oper;
                    else
                        op.As<BooleanOperator>().AddOperator(oper);
                });
            return op;
        }

        FieldWarp GetFieldInfo(BooleanComparisonExpression exp)
        {
            return new FieldWarp
            {
                FieldName = exp.FirstExpression.As<ColumnReferenceExpression>().MultiPartIdentifier.GetFullName(),
                NeedQuotationMark = exp.SecondExpression.As<Literal>().LiteralType == LiteralType.String
            };
        }

        public QueryClip ToQuery()
        {
            return _op == null ? new QueryClip() : _op.ToQuery();
        }
    }
}
