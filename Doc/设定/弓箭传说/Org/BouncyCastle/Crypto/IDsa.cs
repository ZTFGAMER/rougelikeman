namespace Org.BouncyCastle.Crypto
{
    using Org.BouncyCastle.Math;
    using System;

    public interface IDsa
    {
        BigInteger[] GenerateSignature(byte[] message);
        void Init(bool forSigning, ICipherParameters parameters);
        bool VerifySignature(byte[] message, BigInteger r, BigInteger s);

        string AlgorithmName { get; }
    }
}

