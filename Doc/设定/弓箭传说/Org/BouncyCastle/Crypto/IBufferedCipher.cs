namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IBufferedCipher
    {
        byte[] DoFinal();
        byte[] DoFinal(byte[] input);
        int DoFinal(byte[] output, int outOff);
        byte[] DoFinal(byte[] input, int inOff, int length);
        int DoFinal(byte[] input, byte[] output, int outOff);
        int DoFinal(byte[] input, int inOff, int length, byte[] output, int outOff);
        int GetBlockSize();
        int GetOutputSize(int inputLen);
        int GetUpdateOutputSize(int inputLen);
        void Init(bool forEncryption, ICipherParameters parameters);
        byte[] ProcessByte(byte input);
        int ProcessByte(byte input, byte[] output, int outOff);
        byte[] ProcessBytes(byte[] input);
        byte[] ProcessBytes(byte[] input, int inOff, int length);
        int ProcessBytes(byte[] input, byte[] output, int outOff);
        int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff);
        void Reset();

        string AlgorithmName { get; }
    }
}

