using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Renci.SshNet;

namespace FunctionalTester.Wrapper
{
    class RemoteProcessWrapper : IProcessWrapper
    {
        private SshCommand m_command;
        private IAsyncResult m_result;

        public string Output
        {
            get
            {
                if (!string.IsNullOrEmpty(m_command.Error))
                    return m_command.Error;
                return m_command.Result;
            }
        }

        public RemoteProcessWrapper(SshCommand command)
        {
            m_command = command;
            m_result = command.BeginExecute();
        }

        public bool Kill()
        {
            // can't kill?
            return false;
        }

        public void Wait()
        {
            m_result.AsyncWaitHandle.WaitOne();
        }
    }
}
