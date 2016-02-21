using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.Exceptions
{
    class UndefinedIdentifierException : Exception
    {
        public string Name { get; private set; }

        public UndefinedIdentifierException(string name)
            : base("Undefined identifier: " + name)
        {
            Name = name;
        }
    }
}
