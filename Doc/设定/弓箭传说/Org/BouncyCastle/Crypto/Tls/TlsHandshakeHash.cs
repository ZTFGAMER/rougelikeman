namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using System;

    public interface TlsHandshakeHash : IDigest
    {
        IDigest ForkPrfHash();
        byte[] GetFinalHash(byte hashAlgorithm);
        void Init(TlsContext context);
        TlsHandshakeHash NotifyPrfDetermined();
        void SealHashAlgorithms();
        TlsHandshakeHash StopTracking();
        void TrackHashAlgorithm(byte hashAlgorithm);
    }
}

