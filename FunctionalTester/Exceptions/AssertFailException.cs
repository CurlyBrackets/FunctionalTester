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

        public AssertFailException(InterpBase value)
            : base("Assertion failed for: " + value)
        {
            Value = value;
        }
    }
}
