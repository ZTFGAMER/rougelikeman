namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IStreamCipher
    {
        void Init(bool forEncryption, ICipherParameters parameters);
        void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff);
        void Reset();
        byte ReturnByte(byte input);

        string AlgorithmName { get; }
    }
}

