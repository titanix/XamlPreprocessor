using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamlPreprocessor.Evaluator
{
    public abstract class Expression
    {
        public abstract bool Evaluate(string arg);
        public abstract bool Evaluate(string[] args);
    }
}
