namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class NameType
    {
        public const byte host_name = 0;

        protected NameType()
        {
        }

        public static bool IsValid(byte nameType) => 
            (nameType == 0);
    }
}

