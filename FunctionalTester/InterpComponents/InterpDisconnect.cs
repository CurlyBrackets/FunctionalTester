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
        public InterpBase Cleanup { get; private set; }

        public InterpDisconnect(InterpBase value, InterpBase cleanup)
        {
            Value = value;
            Cleanup = cleanup;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var conn = Value.Interp(environment);
            AssertType(conn.Type, ValueType.SshConnection);

            bool cleanup = true;
            if (Cleanup != null)
            {
                var cleanupVal = Cleanup.Interp(environment);
                AssertType(cleanupVal.Type, ValueType.Boolean);

                cleanup = cleanupVal.BoolValue;
            }

            conn.SshValue.Disconnect(cleanup);

            return new InterpValue();
        }

        public override string ToString()
        {
            return $"disconnect {Value}";
        }
    }
}
