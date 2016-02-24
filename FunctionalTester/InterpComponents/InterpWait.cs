using FunctionalTester.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpWait : InterpBase
    {
        public IList<InterpBase> Children { get; private set; }

        public InterpWait(IList<InterpBase> children)
        {
            Children = children;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var evaled = new List<InterpValue>();
            foreach(var child in Children)
            {
                var res = child.Interp(environment);
                if (res.Type != ValueType.Process)
                    throw new WrongTypeException(res.Type, ValueType.Process);

                evaled.Add(res);
            }

            foreach (var val in evaled)
                val.ProcessValue.Wait();

            return new InterpValue();
        }

        public override string ToString()
        {
            return "wait " + Children.Aggregate(string.Empty, (acc, val) => acc + " " + val);
        }
    }
}
