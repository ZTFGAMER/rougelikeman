namespace Org.BouncyCastle.Crypto
{
    using System;

    public interface ISignerWithRecovery : ISigner
    {
        byte[] GetRecoveredMessage();
        bool HasFullMessage();
        void UpdateWithRecoveredMessage(byte[] signature);
    }
}

