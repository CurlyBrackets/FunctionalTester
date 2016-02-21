using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpBool : InterpBase
    {
        public bool Value { get; private set; }

        public InterpBool(bool value)
        {
            Value = value;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            return new InterpValue(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
