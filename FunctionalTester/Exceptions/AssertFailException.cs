using FunctionalTester.InterpComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.Exceptions
{
    class AssertFailException : Exception
    {
        public InterpBase Value { get; private set; }

        public bool IsDetailed { get; private set; }
        public InterpValue Left { get; private set; }
        public InterpValue Right { get; private set; }

        public AssertFailException(InterpBase value)
            : base("Assertion failed for: " + value)
        {
            Value = value;
            IsDetailed = false;
        }

        public AssertFailException(InterpBase value, InterpValue left, InterpValue right)
            : base("Assertion failed for: " + value + Environment.NewLine + "Left: " + left + Environment.NewLine + "Right: " + right)
        {
            IsDetailed = true;
            Value = value;
            Left = left;
            Right = right;
        }
    }
}
