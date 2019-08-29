namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface ISigner
    {
        void BlockUpdate(byte[] input, int inOff, int length);
        byte[] GenerateSignature();
        void Init(bool forSigning, ICipherParameters parameters);
        void Reset();
        void Update(byte input);
        bool VerifySignature(byte[] signature);

        string AlgorithmName { get; }
    }
}

