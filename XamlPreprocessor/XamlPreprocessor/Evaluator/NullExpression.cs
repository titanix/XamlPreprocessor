using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlPreprocessor.Evaluator
{
    class NullExpression : Expression
    {
        public NullExpression() { }

        public override bool Evaluate(string arg)
        {
            return false;
        }

        public override bool Evaluate(string[] args)
        {
            return false;
        }

        public override string ToString()
        {
            return String.Format("nil_exp");
        }
    }
}
