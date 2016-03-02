using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpNot : InterpBase
    {
        public InterpBase Value { get; private set; }

        public InterpNot(InterpBase value)
        {
            Value = value;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var val = Value.Interp(environment);
            AssertType(val.Type, ValueType.Boolean);

            return new InterpValue(!val.BoolValue);
        }

        public override string ToString()
        {
            return "!" + Value;
        }
    }
}
