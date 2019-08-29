namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class KeyUsage : DerBitString
    {
        public const int DigitalSignature = 0x80;
        public const int NonRepudiation = 0x40;
        public const int KeyEncipherment = 0x20;
        public const int DataEncipherment = 0x10;
        public const int KeyAgreement = 8;
        public const int KeyCertSign = 4;
        public const int CrlSign = 2;
        public const int EncipherOnly = 1;
        public const int DecipherOnly = 0x8000;

        private KeyUsage(DerBitString usage) : base(usage.GetBytes(), usage.PadBits)
        {
        }

        public KeyUsage(int usage) : base(usage)
        {
        }

        public static KeyUsage GetInstance(object obj)
        {
            if (obj is KeyUsage)
            {
                return (KeyUsage) obj;
            }
            if (obj is X509Extension)
            {
                return GetInstance(X509Extension.ConvertValueToObject((X509Extension) obj));
            }
            return new KeyUsage(DerBitString.GetInstance(obj));
        }

        public override string ToString()
        {
            byte[] bytes = this.GetBytes();
            if (bytes.Length == 1)
            {
                int num = bytes[0] & 0xff;
                return ("KeyUsage: 0x" + num.ToString("X"));
            }
            int num2 = ((bytes[1] & 0xff) << 8) | (bytes[0] & 0xff);
            return ("KeyUsage: 0x" + num2.ToString("X"));
        }
    }
}

