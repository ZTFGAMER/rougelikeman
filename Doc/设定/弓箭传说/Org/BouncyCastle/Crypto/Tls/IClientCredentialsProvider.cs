namespace Org.BouncyCastle.Crypto.Tls
{
    public interface IClientCredentialsProvider
    {
        TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest);
    }
}

