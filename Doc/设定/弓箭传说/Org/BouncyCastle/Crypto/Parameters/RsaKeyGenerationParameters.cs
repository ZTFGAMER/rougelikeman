namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;

    public class RsaKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly BigInteger publicExponent;
        private readonly int certainty;

        public RsaKeyGenerationParameters(BigInteger publicExponent, SecureRandom random, int strength, int certainty) : base(random, strength)
        {
            this.publicExponent = publicExponent;
            this.certainty = certainty;
        }

        public override bool Equals(object obj)
        {
            RsaKeyGenerationParameters parameters = obj as RsaKeyGenerationParameters;
            if (parameters == null)
            {
                return false;
            }
            return ((this.certainty == parameters.certainty) && this.publicExponent.Equals(parameters.publicExponent));
        }

        public override int GetHashCode() => 
            (this.certainty.GetHashCode() ^ this.publicExponent.GetHashCode());

        public BigInteger PublicExponent =>
            this.publicExponent;

        public int Certainty =>
            this.certainty;
    }
}

