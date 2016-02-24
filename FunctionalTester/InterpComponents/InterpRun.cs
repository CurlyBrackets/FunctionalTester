using FunctionalTester.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpRun : InterpBase
    {
        public InterpBase Name { get; private set; }
        public InterpBase Args { get; private set; }

        public InterpRun(InterpBase name, InterpBase args)
        {
            Name = name;
            Args = args;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var name = Name.Interp(environment);
            if (name.Type != ValueType.String)
                throw new WrongTypeException(name.Type, ValueType.String);

            string procName = null;
            if (Environment.OSVersion.Platform != PlatformID.MacOSX && Environment.OSVersion.Platform != PlatformID.Unix)
                procName = name.StringValue + ".exe";
            else
                procName = name.StringValue;

            var startInfo = new ProcessStartInfo(procName)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            if (Args != null) {
                var args = Args.Interp(environment);
                if (args.Type != ValueType.String)
                    throw new WrongTypeException(args.Type, ValueType.String);

                startInfo.Arguments = args.StringValue;
            }

            return new InterpValue(
                Process.Start(startInfo));
        }

        public override string ToString()
        {
            if (Args == null)
                return "run " + Name;
            else
                return "run " + Name + " " + Args;
        }
    }
}
