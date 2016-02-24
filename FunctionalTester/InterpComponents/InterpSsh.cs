using FunctionalTester.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpSsh : InterpBase
    {
        public InterpBase Connection { get; private set; }
        public InterpBase Command { get; private set; }

        public InterpSsh(InterpBase conn, InterpBase command)
        {
            Connection = conn;
            Command = command;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var connVal = Connection.Interp(environment);
            AssertType(connVal.Type, ValueType.SshConnection);

            var commandVal = Command.Interp(environment);
            AssertType(commandVal.Type, ValueType.String);

            var command = connVal.SshValue.SshClient.CreateCommand(connVal.SshValue.Prepend + commandVal.StringValue);

            return new InterpValue(new RemoteProcessWrapper(command));
        }

        public override string ToString()
        {
            return $"ssh {Connection} {Command}";
        }
    }
}
