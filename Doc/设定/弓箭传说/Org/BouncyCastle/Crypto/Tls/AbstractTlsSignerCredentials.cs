namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class AbstractTlsSignerCredentials : AbstractTlsCredentials, TlsSignerCredentials, TlsCredentials
    {
        protected AbstractTlsSignerCredentials()
        {
        }

        public abstract byte[] GenerateCertificateSignature(byte[] hash);

        public virtual Org.BouncyCastle.Crypto.Tls.SignatureAndHashAlgorithm SignatureAndHashAlgorithm
        {
            get
            {
                throw new InvalidOperationException("TlsSignerCredentials implementation does not support (D)TLS 1.2+");
            }
        }
    }
}

