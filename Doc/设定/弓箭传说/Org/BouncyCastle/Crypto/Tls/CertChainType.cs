namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class CertChainType
    {
        public const byte individual_certs = 0;
        public const byte pkipath = 1;

        protected CertChainType()
        {
        }

        public static bool IsValid(byte certChainType) => 
            ((certChainType >= 0) && (certChainType <= 1));
    }
}

