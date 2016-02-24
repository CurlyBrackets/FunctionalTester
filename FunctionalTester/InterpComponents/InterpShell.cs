using FunctionalTester.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpShell : InterpBase
    {
        public InterpBase Value { get; private set; }

        public InterpShell(InterpBase value)
        {
            Value = value;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var value = Value.Interp(environment);
            if (value.Type != ValueType.String)
                throw new WrongTypeException(value.Type, ValueType.String);

            var p = StartShell(value.StringValue);
            p.WaitForExit();

            return new InterpValue();
        }

        private Process StartShell(string val)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                var split = val.IndexOf(' ');
                return Process.Start(val.Substring(0, split), val.Substring(split + 1));
            }
            else
            {
                return Process.Start("cmd", "/c " + val);
            }
        }

        public override string ToString()
        {
            return "shell " + Value;
        }
    }
}
