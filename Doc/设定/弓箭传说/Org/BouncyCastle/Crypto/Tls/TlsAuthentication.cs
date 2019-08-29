namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public interface TlsAuthentication
    {
        TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest);
        void NotifyServerCertificate(Certificate serverCertificate);
    }
}

