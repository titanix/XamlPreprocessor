using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlPreprocessor.Evaluator
{
    class ValueExpression : Expression
    {
        string Value;

        public ValueExpression(string value)
        {
            Value = value;
        }

        public override bool Evaluate(string arg)
        {
            return String.Compare(Value, arg, true) == 0;
        }

        public override bool Evaluate(string[] args)
        {
            bool Contains = false;
            foreach (string str in args)
            {
                if (String.Compare(str, Value, true) == 0)
                    Contains = true;
            }
            return Contains;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
