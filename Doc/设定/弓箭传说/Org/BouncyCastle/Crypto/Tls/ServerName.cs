namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class ServerName
    {
        protected readonly byte mNameType;
        protected readonly object mName;

        public ServerName(byte nameType, object name)
        {
            if (!IsCorrectType(nameType, name))
            {
                throw new ArgumentException("not an instance of the correct type", "name");
            }
            this.mNameType = nameType;
            this.mName = name;
        }

        public virtual void Encode(Stream output)
        {
            TlsUtilities.WriteUint8(this.mNameType, output);
            if (this.mNameType != 0)
            {
                throw new TlsFatalAlert(80);
            }
            byte[] buf = Strings.ToAsciiByteArray((string) this.mName);
            if (buf.Length < 1)
            {
                throw new TlsFatalAlert(80);
            }
            TlsUtilities.WriteOpaque16(buf, output);
        }

        public virtual string GetHostName()
        {
            if (!IsCorrectType(0, this.mName))
            {
                throw new InvalidOperationException("'name' is not a HostName string");
            }
            return (string) this.mName;
        }

        protected static bool IsCorrectType(byte nameType, object name)
        {
            if (nameType != 0)
            {
                throw new ArgumentException("unsupported value", "name");
            }
            return (name is string);
        }

        public static ServerName Parse(Stream input)
        {
            byte nameType = TlsUtilities.ReadUint8(input);
            if (nameType != 0)
            {
                throw new TlsFatalAlert(50);
            }
            byte[] bytes = TlsUtilities.ReadOpaque16(input);
            if (bytes.Length < 1)
            {
                throw new TlsFatalAlert(50);
            }
            return new ServerName(nameType, Strings.FromAsciiByteArray(bytes));
        }

        public virtual byte NameType =>
            this.mNameType;

        public virtual object Name =>
            this.mName;
    }
}

