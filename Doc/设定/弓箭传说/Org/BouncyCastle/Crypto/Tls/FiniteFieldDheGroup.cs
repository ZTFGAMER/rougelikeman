namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class FiniteFieldDheGroup
    {
        public const byte ffdhe2432 = 0;
        public const byte ffdhe3072 = 1;
        public const byte ffdhe4096 = 2;
        public const byte ffdhe6144 = 3;
        public const byte ffdhe8192 = 4;

        protected FiniteFieldDheGroup()
        {
        }

        public static bool IsValid(byte group) => 
            ((group >= 0) && (group <= 4));
    }
}

