namespace Org.BouncyCastle.Asn1.Misc
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class NetscapeCertType : DerBitString
    {
        public const int SslClient = 0x80;
        public const int SslServer = 0x40;
        public const int Smime = 0x20;
        public const int ObjectSigning = 0x10;
        public const int Reserved = 8;
        public const int SslCA = 4;
        public const int SmimeCA = 2;
        public const int ObjectSigningCA = 1;

        public NetscapeCertType(DerBitString usage) : base(usage.GetBytes(), usage.PadBits)
        {
        }

        public NetscapeCertType(int usage) : base(usage)
        {
        }

        public override string ToString()
        {
            byte[] bytes = this.GetBytes();
            int num = bytes[0] & 0xff;
            return ("NetscapeCertType: 0x" + num.ToString("X"));
        }
    }
}

