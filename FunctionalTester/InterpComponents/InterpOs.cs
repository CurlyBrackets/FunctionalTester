using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpOs : InterpBase
    {
        public InterpBase Unix { get; private set; }
        public InterpBase Windows { get; private set; }

        public InterpOs(InterpBase unix, InterpBase windows)
        {
            Unix = unix;
            Windows = windows;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            InterpValue val = null;

            var plat = Environment.OSVersion.Platform;
            if (plat == PlatformID.Unix)
                val = Unix.Interp(environment);
            else
                val = Windows.Interp(environment);

            return val;
        }

        public override string ToString()
        {
            var plat = Environment.OSVersion.Platform;
            if (plat == PlatformID.Unix)
                return Unix.ToString();
            else
                return Windows.ToString();
        }
    }
}
