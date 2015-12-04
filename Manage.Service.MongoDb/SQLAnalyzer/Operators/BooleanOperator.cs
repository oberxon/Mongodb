using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Service.MongoDb.SQLAnalyzer.Operators
{
    abstract class BooleanOperator : Operator
    {
        protected List<Operator> operators = new List<Operator>();
        public void AddOperator(Operator o)
        {
            operators.Add(o);
        }
    }
}
