namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.Collections;

    public interface TlsServer : TlsPeer
    {
        CertificateRequest GetCertificateRequest();
        CertificateStatus GetCertificateStatus();
        TlsCredentials GetCredentials();
        TlsKeyExchange GetKeyExchange();
        NewSessionTicket GetNewSessionTicket();
        int GetSelectedCipherSuite();
        byte GetSelectedCompressionMethod();
        IDictionary GetServerExtensions();
        IList GetServerSupplementalData();
        ProtocolVersion GetServerVersion();
        void Init(TlsServerContext context);
        void NotifyClientCertificate(Certificate clientCertificate);
        void NotifyClientVersion(ProtocolVersion clientVersion);
        void NotifyFallback(bool isFallback);
        void NotifyOfferedCipherSuites(int[] offeredCipherSuites);
        void NotifyOfferedCompressionMethods(byte[] offeredCompressionMethods);
        void ProcessClientExtensions(IDictionary clientExtensions);
        void ProcessClientSupplementalData(IList clientSupplementalData);
    }
}

