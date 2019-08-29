namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class EncryptionAlgorithm
    {
        public const int NULL = 0;
        public const int RC4_40 = 1;
        public const int RC4_128 = 2;
        public const int RC2_CBC_40 = 3;
        public const int IDEA_CBC = 4;
        public const int DES40_CBC = 5;
        public const int DES_CBC = 6;
        public const int cls_3DES_EDE_CBC = 7;
        public const int AES_128_CBC = 8;
        public const int AES_256_CBC = 9;
        public const int AES_128_GCM = 10;
        public const int AES_256_GCM = 11;
        public const int CAMELLIA_128_CBC = 12;
        public const int CAMELLIA_256_CBC = 13;
        public const int SEED_CBC = 14;
        public const int AES_128_CCM = 15;
        public const int AES_128_CCM_8 = 0x10;
        public const int AES_256_CCM = 0x11;
        public const int AES_256_CCM_8 = 0x12;
        public const int CAMELLIA_128_GCM = 0x13;
        public const int CAMELLIA_256_GCM = 20;
        public const int CHACHA20_POLY1305 = 0x66;
        [Obsolete]
        public const int AEAD_CHACHA20_POLY1305 = 0x66;
        public const int AES_128_OCB_TAGLEN96 = 0x67;
        public const int AES_256_OCB_TAGLEN96 = 0x68;

        protected EncryptionAlgorithm()
        {
        }
    }
}

