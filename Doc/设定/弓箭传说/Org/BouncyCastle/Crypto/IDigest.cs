namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IDigest
    {
        void BlockUpdate(byte[] input, int inOff, int length);
        int DoFinal(byte[] output, int outOff);
        int GetByteLength();
        int GetDigestSize();
        void Reset();
        void Update(byte input);

        string AlgorithmName { get; }
    }
}

