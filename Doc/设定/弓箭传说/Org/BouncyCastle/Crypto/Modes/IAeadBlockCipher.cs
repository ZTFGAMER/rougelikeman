namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using System;

    public interface IAeadBlockCipher
    {
        int DoFinal(byte[] outBytes, int outOff);
        int GetBlockSize();
        byte[] GetMac();
        int GetOutputSize(int len);
        IBlockCipher GetUnderlyingCipher();
        int GetUpdateOutputSize(int len);
        void Init(bool forEncryption, ICipherParameters parameters);
        void ProcessAadByte(byte input);
        void ProcessAadBytes(byte[] inBytes, int inOff, int len);
        int ProcessByte(byte input, byte[] outBytes, int outOff);
        int ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff);
        void Reset();

        string AlgorithmName { get; }
    }
}

