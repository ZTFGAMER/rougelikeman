namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Math;
    using System;

    public class ElGamalPublicKeyParameters : ElGamalKeyParameters
    {
        private readonly BigInteger y;

        public ElGamalPublicKeyParameters(BigInteger y, ElGamalParameters parameters) : base(false, parameters)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            this.y = y;
        }

        protected bool Equals(ElGamalPublicKeyParameters other) => 
            (this.y.Equals(other.y) && base.Equals((ElGamalKeyParameters) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            ElGamalPublicKeyParameters other = obj as ElGamalPublicKeyParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            (this.y.GetHashCode() ^ base.GetHashCode());

        public BigInteger Y =>
            this.y;
    }
}

