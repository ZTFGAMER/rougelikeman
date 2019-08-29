namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface IMac
    {
        void BlockUpdate(byte[] input, int inOff, int len);
        int DoFinal(byte[] output, int outOff);
        int GetMacSize();
        void Init(ICipherParameters parameters);
        void Reset();
        void Update(byte input);

        string AlgorithmName { get; }
    }
}

