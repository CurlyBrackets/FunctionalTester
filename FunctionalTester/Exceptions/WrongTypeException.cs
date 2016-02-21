using System.Collections.Generic;
using System.Linq;
using System.Text;
using FunctionalTester.InterpComponents;

namespace FunctionalTester.Exceptions
{
    class WrongTypeException : System.Exception
    {
        public ValueType Actual { get; private set; }
        public ValueType Expected { get; private set; }

        public WrongTypeException(ValueType actual, ValueType expected)
            : base("Wrong type. Got: " + actual +" Expected: " + expected)
        {
            Actual = actual;
            Expected = expected;
        }
    }
}
