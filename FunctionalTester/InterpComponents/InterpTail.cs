using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpTail : InterpBase
    {
        public InterpBase Amount { get; private set; }
        public InterpBase Value { get; private set; }

        public InterpTail(InterpBase amount, InterpBase value)
        {
            Amount = amount;
            Value = value;
        }
        
        public override InterpValue Interp(InterpEnvironment environment)
        {
            var amVal = Amount.Interp(environment);
            AssertType(amVal.Type, ValueType.Integer);

            var stringVal = Value.Interp(environment);
            AssertType(stringVal.Type, ValueType.String);

            var lines = stringVal.StringValue.Split('\n');
            if (lines.Count() <= amVal.IntValue)
                return new InterpValue(string.Empty);
            else
                return new InterpValue(lines.Skip(amVal.IntValue).Aggregate((l,r) => l + "\n" + r));
        }

        public override string ToString()
        {
            return $"tail {Amount} {Value}";
        }
    }
}
