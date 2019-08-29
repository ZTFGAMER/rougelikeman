namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Math;
    using System;

    public class RsaBlindingParameters : ICipherParameters
    {
        private readonly RsaKeyParameters publicKey;
        private readonly BigInteger blindingFactor;

        public RsaBlindingParameters(RsaKeyParameters publicKey, BigInteger blindingFactor)
        {
            if (publicKey.IsPrivate)
            {
                throw new ArgumentException("RSA parameters should be for a public key");
            }
            this.publicKey = publicKey;
            this.blindingFactor = blindingFactor;
        }

        public RsaKeyParameters PublicKey =>
            this.publicKey;

        public BigInteger BlindingFactor =>
            this.blindingFactor;
    }
}

