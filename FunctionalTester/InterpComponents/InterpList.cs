using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpList : InterpBase
    {
        public IList<InterpBase> Values { get; private set; }

        public InterpList(IList<InterpBase> values)
        {
            Values = values;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            InterpValue last = new InterpValue();
            foreach(var val in Values)
            {
                last = val.Interp(environment);
            }

            return last;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach(var val in Values)
            {
                sb.AppendLine(val.ToString());
            }

            return sb.ToString();
        }
    }
}
