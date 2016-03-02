using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FunctionalTester.InterpComponents
{
    class InterpWrite : InterpBase
    {
        public InterpBase Filename { get; private set; }
        public InterpBase Text { get; private set; }

        public InterpWrite(InterpBase filename, InterpBase text)
        {
            Filename = filename;
            Text = text;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var filenameVal = Filename.Interp(environment);
            AssertType(filenameVal.Type, ValueType.String);

            var textVal = Text.Interp(environment);
            AssertType(textVal.Type, ValueType.String);

            File.WriteAllText(filenameVal.StringValue, textVal.StringValue);
            return new InterpValue();
        }

        public override string ToString()
        {
            return $"write {Filename} {Text}";
        }
    }
}
