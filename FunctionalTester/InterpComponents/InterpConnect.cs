using FunctionalTester.Exceptions;
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

        public InterpConnect(InterpBase address)
        {
            Address = address;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var addrVal = Address.Interp(environment);
            if (addrVal.Type != ValueType.String)
                throw new WrongTypeException(addrVal.Type, ValueType.String);

            
            //var conn = new SshClient()

            return new InterpValue();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
