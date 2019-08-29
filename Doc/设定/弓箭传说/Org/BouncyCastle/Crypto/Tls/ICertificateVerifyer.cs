namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1.X509;
    using System;

    public interface ICertificateVerifyer
    {
        bool IsValid(Uri targetUri, X509CertificateStructure[] certs);
    }
}

