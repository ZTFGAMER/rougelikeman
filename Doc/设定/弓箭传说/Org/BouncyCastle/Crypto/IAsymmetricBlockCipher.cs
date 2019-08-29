namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IAsymmetricBlockCipher
    {
        int GetInputBlockSize();
        int GetOutputBlockSize();
        void Init(bool forEncryption, ICipherParameters parameters);
        byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen);

        string AlgorithmName { get; }
    }
}

