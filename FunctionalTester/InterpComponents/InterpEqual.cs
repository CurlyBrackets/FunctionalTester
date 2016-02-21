using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpEqual : InterpBase
    {
        public InterpBase Left { get; private set; }
        public InterpBase Right { get; private set; }

        public InterpEqual(InterpBase left, InterpBase right)
        {
            Left = left;
            Right = right;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var leftValue = Left.Interp(environment);
            var rightValue = Right.Interp(environment);

            return new InterpValue(leftValue.Equals(rightValue));
        }

        public override string ToString()
        {
            return "equal " + Left + " " + Right;
        }
    }
}
