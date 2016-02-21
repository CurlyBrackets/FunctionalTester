using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    abstract class InterpBase
    {
        public abstract InterpValue Interp(InterpEnvironment environment);
        public abstract override string ToString();
    }
}
