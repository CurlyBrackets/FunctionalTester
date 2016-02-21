using FunctionalTester.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpEnvironment
    {
        private Dictionary<string, InterpValue> m_core;

        public InterpEnvironment()
        {
            m_core = new Dictionary<string, InterpValue>();
        }

        private InterpEnvironment(IDictionary<string, InterpValue> other)
        {
            m_core = other.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        public InterpValue this[string id]
        {
            get
            {
                if (!m_core.ContainsKey(id))
                    throw new UndefinedIdentifierException(id);

                return m_core[id];
            }
            set
            {
                if (!m_core.ContainsKey(id))
                    m_core.Add(id, value);
                else
                    m_core[id] = value;
            }
        }

        public InterpEnvironment Clone()
        {
            return new InterpEnvironment(m_core);
        }
    }
}
