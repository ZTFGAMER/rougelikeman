namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using System;

    public interface TlsAgreementCredentials : TlsCredentials
    {
        byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey);
    }
}

