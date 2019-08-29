namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class AbstractTlsPeer : TlsPeer
    {
        protected AbstractTlsPeer()
        {
        }

        public abstract TlsCipher GetCipher();
        public abstract TlsCompression GetCompression();
        public virtual void NotifyAlertRaised(byte alertLevel, byte alertDescription, string message, Exception cause)
        {
        }

        public virtual void NotifyAlertReceived(byte alertLevel, byte alertDescription)
        {
        }

        public virtual void NotifyHandshakeComplete()
        {
        }

        public virtual void NotifySecureRenegotiation(bool secureRenegotiation)
        {
            if (!secureRenegotiation)
            {
                throw new TlsFatalAlert(40);
            }
        }

        public virtual bool ShouldUseGmtUnixTime() => 
            false;
    }
}

