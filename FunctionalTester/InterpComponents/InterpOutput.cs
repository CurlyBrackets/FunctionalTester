using FunctionalTester.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpOutput : InterpBase
    {
        public InterpBase Value { get; private set; }

        public InterpOutput(InterpBase value)
        {
            Value = value;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var proc = Value.Interp(environment);
            if (proc.Type != ValueType.Process)
                throw new WrongTypeException(proc.Type, ValueType.Process);

            return new InterpValue(proc.ProcessValue.Output);
        }

        public override string ToString()
        {
            return "output " + Value;
        }
    }
}
