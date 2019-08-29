namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public interface TlsPeer
    {
        TlsCipher GetCipher();
        TlsCompression GetCompression();
        void NotifyAlertRaised(byte alertLevel, byte alertDescription, string message, Exception cause);
        void NotifyAlertReceived(byte alertLevel, byte alertDescription);
        void NotifyHandshakeComplete();
        void NotifySecureRenegotiation(bool secureRenegotiation);
        bool ShouldUseGmtUnixTime();
    }
}

