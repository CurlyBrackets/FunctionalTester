using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        private const string CacheName = ".sshauth";
        private const int ESeed = 152735685;
        private const int ECount = 64;

        private Dictionary<string, AuthInfo> m_exists;
        private ISet<string> m_needed, m_nocache;

        private Func<string, string> m_userPrompt;
        private Func<string, byte[]> m_passwordPrompt;
        private Func<string, string, bool> m_repeatPrompt;
        private Func<bool> m_cachePrompt;

        public SshAuthManager()
        {
            m_exists = new Dictionary<string, AuthInfo>();
            m_needed = new HashSet<string>();
            m_nocache = new HashSet<string>();

            Load();
        }

        #region Prompts

        public void SetUserPrompt(Func<string, string> func)
        {
            m_userPrompt = func;
        }

        public void SetPasswordPrompt(Func<string, byte[]> func)
        {
            m_passwordPrompt = func;
        }

        public void SetRepeatPrompt(Func<string, string, bool> func)
        {
            m_repeatPrompt = func;
        }

        public void SetCachePrompt(Func<bool> func)
        {
            m_cachePrompt = func;
        }

        public AuthInfo? Prompt(string addr)
        {
            var host = SameHost(addr);
            if (host != null && m_repeatPrompt != null)
            {
                if(m_repeatPrompt(addr, host))
                {
                    m_exists[addr] = m_exists[host];
                    if (m_cachePrompt != null && !m_nocache.Contains(addr))
                    {
                        if (!m_cachePrompt())
                            m_nocache.Add(addr);
                        else if (m_needed.Count == 0)
                            Save();
                    }
                    return m_exists[addr];
                }
            }

            if(m_userPrompt != null && m_passwordPrompt != null)
            {
                var info = new AuthInfo(
                    m_userPrompt(addr),
                    m_passwordPrompt(addr));
                m_exists[addr] = info;

                if (m_cachePrompt != null && !m_nocache.Contains(addr))
                {
                    if (!m_cachePrompt())
                        m_nocache.Add(addr);
                    else if (m_needed.Count == 0)
                        Save();
                }

                return info;
            }

            return null;
        }

        private string SameHost(string addr)
        {
            return null;
        }

        #endregion

        public bool HasInfo(string addr)
        {
            return m_exists.ContainsKey(addr);
        }

        public AuthInfo this[string addr]
        {
            get
            {
                return m_exists[addr];
            }
            set
            {
                if (m_needed.Contains(addr))
                    m_needed.Remove(addr);

                if (m_exists.ContainsKey(addr))
                    m_exists[addr] = value;
                else
                    m_exists.Add(addr, value);
            }
        }

        public IEnumerable<string> GetNeeded()
        {
            return m_needed;
        }

        #region Save/Load

        private const int NoCacheFrame = 0x55443322, CacheFrame = 0x77889900;

        private void Save()
        {
            var random = new Random(ESeed);
            byte[] uentropy = new byte[ECount], pentropy = new byte[ECount];
            random.NextBytes(uentropy);
            random.NextBytes(pentropy);

            using (var fs = File.OpenWrite(CacheName))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(NoCacheFrame);
                bw.Write(m_nocache.Count);

                foreach (var el in m_nocache)
                    bw.Write(el);

                bw.Write(CacheFrame);
                bw.Write(m_exists.Count);

                foreach(var kvp in m_exists)
                {
                    bw.Write(kvp.Key);

                    var udata = ProtectedData.Protect(
                            Encoding.ASCII.GetBytes(kvp.Value.Username),
                            uentropy,
                            DataProtectionScope.CurrentUser);

                    bw.Write(udata.Length);
                    bw.Write(udata);

                    var pdata = ProtectedData.Protect(
                            kvp.Value.Password,
                            pentropy,
                            DataProtectionScope.CurrentUser);

                    bw.Write(pdata.Length);
                    bw.Write(pdata);
                }
            }
        }

        private void Load()
        {
            if (!File.Exists(CacheName))
                return;

            var random = new Random(ESeed);
            byte[] uentropy = new byte[ECount], pentropy = new byte[ECount];
            random.NextBytes(uentropy);
            random.NextBytes(pentropy);

            using (var fs = File.OpenRead(CacheName))
            using (var br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    int frameid = br.ReadInt32();
                    int size = br.ReadInt32();

                    if (frameid == NoCacheFrame)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            m_nocache.Add(br.ReadString());
                        }
                    }
                    else if (frameid == CacheFrame)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            var host = br.ReadString();

                            var ulength = br.ReadInt32();
                            var udata = br.ReadBytes(ulength);
                            var plength = br.ReadInt32();
                            var pdata = br.ReadBytes(plength);

                            m_exists.Add(
                                host,
                                new AuthInfo(
                                    Encoding.ASCII.GetString(
                                        ProtectedData.Unprotect(udata, uentropy, DataProtectionScope.CurrentUser)),
                                    ProtectedData.Unprotect(pdata, pentropy, DataProtectionScope.CurrentUser)));
                        }
                    }
                }
            }
        }

        #endregion
    }
}
