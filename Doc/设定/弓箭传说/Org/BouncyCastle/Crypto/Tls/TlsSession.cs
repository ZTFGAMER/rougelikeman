namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public interface TlsSession
    {
        SessionParameters ExportSessionParameters();
        void Invalidate();

        byte[] SessionID { get; }

        bool IsResumable { get; }
    }
}

