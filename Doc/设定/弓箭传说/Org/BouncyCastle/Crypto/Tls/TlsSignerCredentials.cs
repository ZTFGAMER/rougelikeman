namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public interface TlsSignerCredentials : TlsCredentials
    {
        byte[] GenerateCertificateSignature(byte[] hash);

        Org.BouncyCastle.Crypto.Tls.SignatureAndHashAlgorithm SignatureAndHashAlgorithm { get; }
    }
}

