namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Asn1.X509;
    using System;

    public class AlwaysValidVerifyer : ICertificateVerifyer
    {
        public bool IsValid(Uri targetUri, X509CertificateStructure[] certs) => 
            true;
    }
}

