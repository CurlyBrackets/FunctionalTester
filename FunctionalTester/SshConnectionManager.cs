using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester
{
    struct AuthInfo
    {
        public string Username;
        public byte[] Password;

        public AuthInfo(string username, byte[] password)
        {
            Username = username;
            Password = password;
        }
    }

    class SshAuthManager
    {
        private static readonly byte[] Entropy = new byte[] { 1, 4, 3, 2, 1 };

        private Dictionary<string, AuthInfo> m_exists;
        private ISet<string> m_needed;

        public SshAuthManager()
        {
            m_exists = new Dictionary<string, AuthInfo>();
            m_needed = new HashSet<string>();
        }

        public AuthInfo this[string addr]
        {
            get
            {
                return default(AuthInfo);
            }
            set
            {

            }
        }

        public IEnumerable<string> GetNeeded()
        {
            return m_needed;
        }
    }
}
