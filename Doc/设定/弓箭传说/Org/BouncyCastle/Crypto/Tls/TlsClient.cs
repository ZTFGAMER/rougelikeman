namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface TlsClient : TlsPeer
    {
        TlsAuthentication GetAuthentication();
        int[] GetCipherSuites();
        IDictionary GetClientExtensions();
        IList GetClientSupplementalData();
        byte[] GetCompressionMethods();
        TlsKeyExchange GetKeyExchange();
        TlsSession GetSessionToResume();
        void Init(TlsClientContext context);
        void NotifyNewSessionTicket(NewSessionTicket newSessionTicket);
        void NotifySelectedCipherSuite(int selectedCipherSuite);
        void NotifySelectedCompressionMethod(byte selectedCompressionMethod);
        void NotifyServerVersion(ProtocolVersion selectedVersion);
        void NotifySessionID(byte[] sessionID);
        void ProcessServerExtensions(IDictionary serverExtensions);
        void ProcessServerSupplementalData(IList serverSupplementalData);

        List<string> HostNames { get; set; }

        ProtocolVersion ClientHelloRecordLayerVersion { get; }

        ProtocolVersion ClientVersion { get; }

        bool IsFallback { get; }
    }
}

