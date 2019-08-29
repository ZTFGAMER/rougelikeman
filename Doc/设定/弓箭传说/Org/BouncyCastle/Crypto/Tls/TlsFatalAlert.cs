namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.IO;

    public class TlsFatalAlert : IOException
    {
        private readonly byte alertDescription;

        public TlsFatalAlert(byte alertDescription) : this(alertDescription, null)
        {
        }

        public TlsFatalAlert(byte alertDescription, Exception alertCause) : base(Org.BouncyCastle.Crypto.Tls.AlertDescription.GetText(alertDescription), alertCause)
        {
            this.alertDescription = alertDescription;
        }

        public virtual byte AlertDescription =>
            this.alertDescription;
    }
}

