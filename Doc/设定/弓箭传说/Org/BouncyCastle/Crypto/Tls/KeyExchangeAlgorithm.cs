namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class KeyExchangeAlgorithm
    {
        public const int NULL = 0;
        public const int RSA = 1;
        public const int RSA_EXPORT = 2;
        public const int DHE_DSS = 3;
        public const int DHE_DSS_EXPORT = 4;
        public const int DHE_RSA = 5;
        public const int DHE_RSA_EXPORT = 6;
        public const int DH_DSS = 7;
        public const int DH_DSS_EXPORT = 8;
        public const int DH_RSA = 9;
        public const int DH_RSA_EXPORT = 10;
        public const int DH_anon = 11;
        public const int DH_anon_EXPORT = 12;
        public const int PSK = 13;
        public const int DHE_PSK = 14;
        public const int RSA_PSK = 15;
        public const int ECDH_ECDSA = 0x10;
        public const int ECDHE_ECDSA = 0x11;
        public const int ECDH_RSA = 0x12;
        public const int ECDHE_RSA = 0x13;
        public const int ECDH_anon = 20;
        public const int SRP = 0x15;
        public const int SRP_DSS = 0x16;
        public const int SRP_RSA = 0x17;
        public const int ECDHE_PSK = 0x18;

        protected KeyExchangeAlgorithm()
        {
        }
    }
}

