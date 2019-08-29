namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IBlockCipher
    {
        int GetBlockSize();
        void Init(bool forEncryption, ICipherParameters parameters);
        int ProcessBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff);
        void Reset();

        string AlgorithmName { get; }

        bool IsPartialBlockOkay { get; }
    }
}

