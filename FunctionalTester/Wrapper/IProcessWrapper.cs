using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.Wrapper
{
    interface IProcessWrapper
    {
        string Output { get; }
        void Wait();
        bool Kill();
    }
}
