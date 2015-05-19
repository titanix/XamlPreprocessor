using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlPreprocessor.Evaluator
{
    class NotExpression : Expression
    {
        Expression Expr;

        public NotExpression(Expression expr)
        {
            Expr = expr;
        }

        public override bool Evaluate(string arg)
        {
            return !Expr.Evaluate(arg);
        }

        public override bool Evaluate(string[] args)
        {
            return !Expr.Evaluate(args);
        }

        public override string ToString()
        {
            return String.Format("(NOT {0})", Expr.ToString());
        }
    }
}
