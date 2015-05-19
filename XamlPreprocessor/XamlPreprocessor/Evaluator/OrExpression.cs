using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlPreprocessor.Evaluator
{
    class OR : Expression
    {
        Expression Expr0;
        Expression Expr1;

        public OR(Expression arg0, Expression arg1)
        {
            Expr0 = arg0;
            Expr1 = arg1;
        }

        public override bool Evaluate(string arg)
        {
            return Expr0.Evaluate(arg) || Expr1.Evaluate(arg);
        }

        public override bool Evaluate(string[] args)
        {
            return Expr0.Evaluate(args) || Expr1.Evaluate(args);
        }

        public override string ToString()
        {
            return String.Format("(OR {0} {1})", Expr0.ToString(), Expr1.ToString());
        }
    }
}
