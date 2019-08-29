namespace Org.BouncyCastle.Crypto
{
    using Org.BouncyCastle.Math;
    using System;

    public interface IBasicAgreement
    {
        BigInteger CalculateAgreement(ICipherParameters pubKey);
        int GetFieldSize();
        void Init(ICipherParameters parameters);
    }
}

