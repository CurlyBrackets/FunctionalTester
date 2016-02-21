using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpIdentifier : InterpBase
    {
        public string Identifier { get; private set; }

        public InterpIdentifier(string identifier)
        {
            Identifier = identifier;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            return environment[Identifier];
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
