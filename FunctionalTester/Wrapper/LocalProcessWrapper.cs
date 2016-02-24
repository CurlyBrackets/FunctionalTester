using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FunctionalTester.Wrapper
{
    class LocalProcessWrapper : IProcessWrapper
    {
        public Process Process { get; private set; }

        private StringBuilder m_core;
        public string Output
        {
            get
            {
                return m_core.ToString();
            }
        }

        public LocalProcessWrapper(Process p)
        {
            Process = p;

            m_core = new StringBuilder();
            p.OutputDataReceived += DataReceived;
            p.ErrorDataReceived += DataReceived;

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            if(e.Data != null)
                m_core.Append(e.Data + "\n");
        }

        public void Wait()
        {
            Process.WaitForExit();
        }

        public bool Kill()
        {
            Process.Kill();
            return Process.HasExited;
        }
    }
}
