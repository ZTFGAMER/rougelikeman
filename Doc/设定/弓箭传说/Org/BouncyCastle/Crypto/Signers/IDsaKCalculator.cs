namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;

    public interface IDsaKCalculator
    {
        void Init(BigInteger n, SecureRandom random);
        void Init(BigInteger n, BigInteger d, byte[] message);
        BigInteger NextK();

        bool IsDeterministic { get; }
    }
}

