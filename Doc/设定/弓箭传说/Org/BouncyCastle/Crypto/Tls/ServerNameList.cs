namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public class ServerNameList
    {
        protected readonly IList mServerNameList;

        public ServerNameList(IList serverNameList)
        {
            if (serverNameList == null)
            {
                throw new ArgumentNullException("serverNameList");
            }
            this.mServerNameList = serverNameList;
        }

        private static byte[] CheckNameType(byte[] nameTypesSeen, byte nameType)
        {
            if (NameType.IsValid(nameType) && !Arrays.Contains(nameTypesSeen, nameType))
            {
                return Arrays.Append(nameTypesSeen, nameType);
            }
            return null;
        }

        public virtual void Encode(Stream output)
        {
            MemoryStream stream = new MemoryStream();
            byte[] emptyBytes = TlsUtilities.EmptyBytes;
            IEnumerator enumerator = this.ServerNames.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ServerName current = (ServerName) enumerator.Current;
                    emptyBytes = CheckNameType(emptyBytes, current.NameType);
                    if (emptyBytes == null)
                    {
                        throw new TlsFatalAlert(80);
                    }
                    current.Encode(stream);
                }
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    IDisposable disposable;
                    disposable.Dispose();
                }
            }
            TlsUtilities.CheckUint16(stream.Length);
            TlsUtilities.WriteUint16((int) stream.Length, output);
            stream.WriteTo(output);
        }

        public static ServerNameList Parse(Stream input)
        {
            int length = TlsUtilities.ReadUint16(input);
            if (length < 1)
            {
                throw new TlsFatalAlert(50);
            }
            MemoryStream stream = new MemoryStream(TlsUtilities.ReadFully(length, input), false);
            byte[] emptyBytes = TlsUtilities.EmptyBytes;
            IList serverNameList = Platform.CreateArrayList();
            while (stream.Position < stream.Length)
            {
                ServerName name = ServerName.Parse(stream);
                emptyBytes = CheckNameType(emptyBytes, name.NameType);
                if (emptyBytes == null)
                {
                    throw new TlsFatalAlert(0x2f);
                }
                serverNameList.Add(name);
            }
            return new ServerNameList(serverNameList);
        }

        public virtual IList ServerNames =>
            this.mServerNameList;
    }
}

