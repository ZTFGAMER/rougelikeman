namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    using System;

    public interface IGcmMultiplier
    {
        void Init(byte[] H);
        void MultiplyH(byte[] x);
    }
}

