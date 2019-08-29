namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Utilities;
    using System;

    public sealed class ProtocolVersion
    {
        public static readonly ProtocolVersion SSLv3 = new ProtocolVersion(0x300, "SSL 3.0");
        public static readonly ProtocolVersion TLSv10 = new ProtocolVersion(0x301, "TLS 1.0");
        public static readonly ProtocolVersion TLSv11 = new ProtocolVersion(770, "TLS 1.1");
        public static readonly ProtocolVersion TLSv12 = new ProtocolVersion(0x303, "TLS 1.2");
        public static readonly ProtocolVersion DTLSv10 = new ProtocolVersion(0xfeff, "DTLS 1.0");
        public static readonly ProtocolVersion DTLSv12 = new ProtocolVersion(0xfefd, "DTLS 1.2");
        private readonly int version;
        private readonly string name;

        private ProtocolVersion(int v, string name)
        {
            this.version = v & 0xffff;
            this.name = name;
        }

        public bool Equals(ProtocolVersion other) => 
            ((other != null) && (this.version == other.version));

        public override bool Equals(object other) => 
            ((this == other) || ((other is ProtocolVersion) && this.Equals((ProtocolVersion) other)));

        public static ProtocolVersion Get(int major, int minor)
        {
            if (major != 3)
            {
                if (major != 0xfe)
                {
                    throw new TlsFatalAlert(0x2f);
                }
            }
            else
            {
                switch (minor)
                {
                    case 0:
                        return SSLv3;

                    case 1:
                        return TLSv10;

                    case 2:
                        return TLSv11;

                    case 3:
                        return TLSv12;
                }
                return GetUnknownVersion(major, minor, "TLS");
            }
            if (minor != 0xff)
            {
                if (minor == 0xfe)
                {
                    throw new TlsFatalAlert(0x2f);
                }
                if (minor == 0xfd)
                {
                    return DTLSv12;
                }
                return GetUnknownVersion(major, minor, "DTLS");
            }
            return DTLSv10;
        }

        public ProtocolVersion GetEquivalentTLSVersion()
        {
            if (!this.IsDtls)
            {
                return this;
            }
            if (this == DTLSv10)
            {
                return TLSv11;
            }
            return TLSv12;
        }

        public override int GetHashCode() => 
            this.version;

        private static ProtocolVersion GetUnknownVersion(int major, int minor, string prefix)
        {
            TlsUtilities.CheckUint8(major);
            TlsUtilities.CheckUint8(minor);
            int v = (major << 8) | minor;
            string str = Platform.ToUpperInvariant(Convert.ToString((int) (0x10000 | v), 0x10).Substring(1));
            return new ProtocolVersion(v, prefix + " 0x" + str);
        }

        public bool IsEqualOrEarlierVersionOf(ProtocolVersion version)
        {
            if (this.MajorVersion != version.MajorVersion)
            {
                return false;
            }
            int num = version.MinorVersion - this.MinorVersion;
            return (!this.IsDtls ? (num >= 0) : (num <= 0));
        }

        public bool IsLaterVersionOf(ProtocolVersion version)
        {
            if (this.MajorVersion != version.MajorVersion)
            {
                return false;
            }
            int num = version.MinorVersion - this.MinorVersion;
            return (!this.IsDtls ? (num < 0) : (num > 0));
        }

        public override string ToString() => 
            this.name;

        public int FullVersion =>
            this.version;

        public int MajorVersion =>
            (this.version >> 8);

        public int MinorVersion =>
            (this.version & 0xff);

        public bool IsDtls =>
            (this.MajorVersion == 0xfe);

        public bool IsSsl =>
            (this == SSLv3);

        public bool IsTls =>
            (this.MajorVersion == 3);
    }
}

