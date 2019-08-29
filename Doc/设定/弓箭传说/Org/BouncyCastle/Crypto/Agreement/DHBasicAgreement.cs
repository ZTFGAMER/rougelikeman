namespace Org.BouncyCastle.Crypto.Agreement
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using System;

    public class DHBasicAgreement : IBasicAgreement
    {
        private DHPrivateKeyParameters key;
        private DHParameters dhParams;

        public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
        {
            if (this.key == null)
            {
                throw new InvalidOperationException("Agreement algorithm not initialised");
            }
            DHPublicKeyParameters parameters = (DHPublicKeyParameters) pubKey;
            if (!parameters.Parameters.Equals(this.dhParams))
            {
                throw new ArgumentException("Diffie-Hellman public key has wrong parameters.");
            }
            return parameters.Y.ModPow(this.key.X, this.dhParams.P);
        }

        public virtual int GetFieldSize() => 
            ((this.key.Parameters.P.BitLength + 7) / 8);

        public virtual void Init(ICipherParameters parameters)
        {
            if (parameters is ParametersWithRandom)
            {
                parameters = ((ParametersWithRandom) parameters).Parameters;
            }
            if (!(parameters is DHPrivateKeyParameters))
            {
                throw new ArgumentException("DHEngine expects DHPrivateKeyParameters");
            }
            this.key = (DHPrivateKeyParameters) parameters;
            this.dhParams = this.key.Parameters;
        }
    }
}

