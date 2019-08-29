namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class AbstractTlsCredentials : TlsCredentials
    {
        protected AbstractTlsCredentials()
        {
        }

        public abstract Org.BouncyCastle.Crypto.Tls.Certificate Certificate { get; }
    }
}

