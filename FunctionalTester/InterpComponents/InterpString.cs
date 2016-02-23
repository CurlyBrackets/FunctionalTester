using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FunctionalTester.InterpComponents
{
    class InterpString : InterpBase
    {
        public string Value { get; private set; }

        public InterpString(string value)
        {
            Value = value;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            return new InterpValue(Value);
        }

        public override string ToString()
        {
            return "\"" + Regex.Escape(Value) + "\"";
        }
    }
}
