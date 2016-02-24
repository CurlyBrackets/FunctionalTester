﻿using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionalTester.Wrapper
{
    class ConnectionWrapper
    {
        public ConnectionInfo Info { get; private set; }

        private SshClient m_ssh;
        public SshClient SshClient
        {
            get
            {
                if(m_ssh == null)
                {
                    m_ssh = new SshClient(Info);
                    m_ssh.Connect();
                    //m_ssh.RunCommand("source ~/.bash_profile");
                }

                return m_ssh;
            }
        }

        private ScpClient m_scp;
        public ScpClient ScpClient
        {
            get
            {
                if(m_scp == null)
                {
                    m_scp = new ScpClient(Info);
                    m_scp.Connect();
                }

                return m_scp;
            }
        }

        public string Prepend { get; private set; }

        public ConnectionWrapper(ConnectionInfo info, string prepend)
        {
            Info = info;
            Prepend = prepend;
        }

        public void Disconnect()
        {
            if (m_ssh != null)
            {
                m_ssh.Disconnect();
                m_ssh.Dispose();
                m_ssh = null;
            }

            if (m_scp != null)
            {
                m_scp.Disconnect();
                m_scp.Dispose();
                m_scp = null;
            }
        }
    }
}