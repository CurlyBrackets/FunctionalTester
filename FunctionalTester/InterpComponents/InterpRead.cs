using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FunctionalTester.InterpComponents
{
    class InterpRead : InterpBase
    {
        public InterpBase Filename { get; private set; }

        public InterpRead(InterpBase filename)
        {
            Filename = filename;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var filename = Filename.Interp(environment);
            AssertType(filename.Type, ValueType.String);

            var text = File.ReadAllText(filename.StringValue);
            return new InterpValue(text);
        }

        public override string ToString()
        {
            return $"read {Filename}";
        }
    }
}
