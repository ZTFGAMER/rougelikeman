namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class NamedCurve
    {
        public const int sect163k1 = 1;
        public const int sect163r1 = 2;
        public const int sect163r2 = 3;
        public const int sect193r1 = 4;
        public const int sect193r2 = 5;
        public const int sect233k1 = 6;
        public const int sect233r1 = 7;
        public const int sect239k1 = 8;
        public const int sect283k1 = 9;
        public const int sect283r1 = 10;
        public const int sect409k1 = 11;
        public const int sect409r1 = 12;
        public const int sect571k1 = 13;
        public const int sect571r1 = 14;
        public const int secp160k1 = 15;
        public const int secp160r1 = 0x10;
        public const int secp160r2 = 0x11;
        public const int secp192k1 = 0x12;
        public const int secp192r1 = 0x13;
        public const int secp224k1 = 20;
        public const int secp224r1 = 0x15;
        public const int secp256k1 = 0x16;
        public const int secp256r1 = 0x17;
        public const int secp384r1 = 0x18;
        public const int secp521r1 = 0x19;
        public const int brainpoolP256r1 = 0x1a;
        public const int brainpoolP384r1 = 0x1b;
        public const int brainpoolP512r1 = 0x1c;
        public const int arbitrary_explicit_prime_curves = 0xff01;
        public const int arbitrary_explicit_char2_curves = 0xff02;

        protected NamedCurve()
        {
        }

        public static bool IsValid(int namedCurve) => 
            (((namedCurve >= 1) && (namedCurve <= 0x1c)) || ((namedCurve >= 0xff01) && (namedCurve <= 0xff02)));

        public static bool RefersToASpecificNamedCurve(int namedCurve) => 
            ((namedCurve != 0xff01) && (namedCurve != 0xff02));
    }
}

