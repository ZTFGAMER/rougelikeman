namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IWrapper
    {
        void Init(bool forWrapping, ICipherParameters parameters);
        byte[] Unwrap(byte[] input, int inOff, int length);
        byte[] Wrap(byte[] input, int inOff, int length);

        string AlgorithmName { get; }
    }
}

