namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    using System;

    public interface IGcmExponentiator
    {
        void ExponentiateX(long pow, byte[] output);
        void Init(byte[] x);
    }
}

