using FunctionalTester. Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpAssertion : InterpBase
    {
        public InterpBase Value { get; private set; }

        public InterpAssertion(InterpBase value)
        {
            Value = value;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var val = Value.Interp(environment);
            if (val.Type != ValueType.Boolean)
                throw new WrongTypeException(val.Type, ValueType.Boolean);

            if (!val.BoolValue)
            {
                if (Value is InterpEqual)
                {
                    var eq = Value as InterpEqual;
                    var lval = eq.Left.Interp(environment);
                    var rval = eq.Right.Interp(environment);

                    throw new AssertFailException(Value, lval, rval);
                }
                else {
                    throw new AssertFailException(Value);
                }
            }

            return new InterpValue();
        }

        public override string ToString()
        {
            return "assert " + Value;
        }
    }
}
