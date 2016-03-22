using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FunctionalTester.InterpComponents
{
    class InterpRemove : InterpBase
    {
        public InterpBase Value { get; private set; }

        public InterpRemove(InterpBase value)
        {
            Value = value;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var glob = Value.Interp(environment);
            AssertType(glob.Type, ValueType.String);

            var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
            foreach (var file in dirInfo.EnumerateFiles(glob.StringValue))
                file.Delete();

            return new InterpValue();
        }

        public override string ToString()
        {
            return $"remove {Value}";
        }
    }
}
