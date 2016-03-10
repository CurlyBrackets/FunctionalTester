using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpKill : InterpBase
    {
        public InterpBase Target { get; private set; }

        public InterpKill(InterpBase target)
        {
            Target = target;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var tval = Target.Interp(environment);
            AssertType(tval.Type, ValueType.Process);

            return new InterpValue(tval.ProcessValue.Kill());
        }

        public override string ToString()
        {
            return $"kill {Target}";
        }
    }
}
