namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class ECCurveType
    {
        public const byte explicit_prime = 1;
        public const byte explicit_char2 = 2;
        public const byte named_curve = 3;

        protected ECCurveType()
        {
        }
    }
}

