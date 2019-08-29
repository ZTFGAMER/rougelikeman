namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.IO;

    public interface TlsKeyExchange
    {
        void GenerateClientKeyExchange(Stream output);
        byte[] GeneratePremasterSecret();
        byte[] GenerateServerKeyExchange();
        void Init(TlsContext context);
        void ProcessClientCertificate(Certificate clientCertificate);
        void ProcessClientCredentials(TlsCredentials clientCredentials);
        void ProcessClientKeyExchange(Stream input);
        void ProcessServerCertificate(Certificate serverCertificate);
        void ProcessServerCredentials(TlsCredentials serverCredentials);
        void ProcessServerKeyExchange(Stream input);
        void SkipClientCredentials();
        void SkipServerCredentials();
        void SkipServerKeyExchange();
        void ValidateCertificateRequest(CertificateRequest certificateRequest);

        bool RequiresServerKeyExchange { get; }
    }
}

