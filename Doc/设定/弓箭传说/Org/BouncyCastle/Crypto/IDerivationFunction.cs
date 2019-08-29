namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IDerivationFunction
    {
        int GenerateBytes(byte[] output, int outOff, int length);
        void Init(IDerivationParameters parameters);

        IDigest Digest { get; }
    }
}

