using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlPreprocessor.Evaluator
{
    class AndExpression : Expression
    {
        Expression LeftExpression;
        Expression RightExpression;

        public AndExpression(Expression arg0, Expression arg1)
        {
            LeftExpression = arg0;
            RightExpression = arg1;
        }

        public override bool Evaluate(string arg)
        {
            return LeftExpression.Evaluate(arg) && RightExpression.Evaluate(arg);
        }

        public override bool Evaluate(string[] args)
        {
            return LeftExpression.Evaluate(args) && RightExpression.Evaluate(args);
        }

        public override string ToString()
        {
            return String.Format("(AND {0} {1})", LeftExpression.ToString(), RightExpression.ToString());
        }
    }
}
