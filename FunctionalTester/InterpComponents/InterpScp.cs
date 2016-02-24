using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FunctionalTester.InterpComponents
{
    class InterpScp : InterpBase
    {
        public InterpBase Connection { get; private set; }
        public InterpBase LocalFile { get; private set; }
        public InterpBase RemoteFile { get; private set; }

        public InterpScp(InterpBase conn, InterpBase localFile, InterpBase remoteFile)
        {
            Connection = conn;
            LocalFile = localFile;
            RemoteFile = remoteFile;
        }

        public override InterpValue Interp(InterpEnvironment environment)
        {
            var connVal = Connection.Interp(environment);
            AssertType(connVal.Type, ValueType.SshConnection);

            var localVal = LocalFile.Interp(environment);
            AssertType(localVal.Type, ValueType.String);

            string remoteName = string.Empty;
            if(RemoteFile == null)
            {
                remoteName = Path.GetFileName(localVal.StringValue);
            }
            else
            {
                var remoteVal = RemoteFile.Interp(environment);
                AssertType(remoteVal.Type, ValueType.String);

                remoteName = remoteVal.StringValue;
            }

            using (var localStream = File.OpenRead(localVal.StringValue))
                connVal.SshValue.ScpClient.Upload(localStream, "./" + connVal.SshValue.DirName + "/" + remoteName);

            return new InterpValue();
        }

        public override string ToString()
        {
            return $"scp {Connection} {LocalFile} {RemoteFile}";
        }
    }
}
