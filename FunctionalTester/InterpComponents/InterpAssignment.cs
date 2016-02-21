using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpAssignment : InterpBase
    {
        public string Identifier { get; private set; }
        public InterpBase Value { get; private set; }

        public InterpAssignment(string ident, InterpBase value)
        {
            Identifier = ident;
            Value = value;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var val = Value.Interp(environment);
            environment[Identifier] = val;

            return new InterpValue();
        }

        public override string ToString()
        {
            return Identifier + " = " + Value;
        }
    }
}
