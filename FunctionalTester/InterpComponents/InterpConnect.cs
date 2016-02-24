using FunctionalTester.Exceptions;
using FunctionalTester.Wrapper;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpConnect : InterpBase
    {
        public InterpBase Address { get; private set; }
        public InterpBase Prepend { get; private set; }

        private SshAuthManager m_authManager;

        public InterpConnect(SshAuthManager authManager, InterpBase address, InterpBase prepend)
        {
            Address = address;
            m_authManager = authManager;
            Prepend = prepend;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var addrVal = Address.Interp(environment);
            AssertType(addrVal.Type, ValueType.String);

            string prepend = string.Empty;
            if(Prepend != null)
            {
                var prependVal = Prepend.Interp(environment);
                AssertType(prependVal.Type, ValueType.String);

                prepend = prependVal.StringValue;
            }

            AuthInfo auth;
            if (m_authManager.HasInfo(addrVal.StringValue))
            {
                auth = m_authManager[addrVal.StringValue];
            }
            else {
                // mode check
                // prompt
                var ret = m_authManager.Prompt(addrVal.StringValue);
                if (ret == null || !ret.HasValue)
                    throw new Exception("Bad ssh thing");

                auth = ret.Value;
            }

            var info = new ConnectionInfo(addrVal.StringValue, auth.Username, new PasswordAuthenticationMethod(auth.Username, auth.Password));
            return new InterpValue(new ConnectionWrapper(info, prepend));
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
