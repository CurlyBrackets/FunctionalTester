using FunctionalTester.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpDisconnect : InterpBase
    {
        public InterpBase Value { get; private set; }

        public InterpDisconnect(InterpBase value)
        {
            Value = value;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var conn = Value.Interp(environment);
            if (conn.Type != ValueType.SshConnection)
                throw new WrongTypeException(conn.Type, ValueType.SshConnection);

            conn.SshValue.Disconnect();

            return new InterpValue();
        }

        public override string ToString()
        {
            return $"disconnect {Value}";
        }
    }
}
