namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using System;

    public abstract class AbstractTlsAgreementCredentials : AbstractTlsCredentials, TlsAgreementCredentials, TlsCredentials
    {
        protected AbstractTlsAgreementCredentials()
        {
        }

        public abstract byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey);
    }
}

