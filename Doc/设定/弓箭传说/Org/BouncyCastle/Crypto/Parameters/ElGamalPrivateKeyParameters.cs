namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Math;
    using System;

    public class ElGamalPrivateKeyParameters : ElGamalKeyParameters
    {
        private readonly BigInteger x;

        public ElGamalPrivateKeyParameters(BigInteger x, ElGamalParameters parameters) : base(true, parameters)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            this.x = x;
        }

        protected bool Equals(ElGamalPrivateKeyParameters other) => 
            (other.x.Equals(this.x) && base.Equals((ElGamalKeyParameters) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            ElGamalPrivateKeyParameters other = obj as ElGamalPrivateKeyParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            (this.x.GetHashCode() ^ base.GetHashCode());

        public BigInteger X =>
            this.x;
    }
}

