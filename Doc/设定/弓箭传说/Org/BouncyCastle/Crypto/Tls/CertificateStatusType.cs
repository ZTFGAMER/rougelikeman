namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class CertificateStatusType
    {
        public const byte ocsp = 1;

        protected CertificateStatusType()
        {
        }
    }
}

