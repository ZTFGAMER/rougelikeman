namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Math;
    using System;

    public class DsaPrivateKeyParameters : DsaKeyParameters
    {
        private readonly BigInteger x;

        public DsaPrivateKeyParameters(BigInteger x, DsaParameters parameters) : base(true, parameters)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            this.x = x;
        }

        protected bool Equals(DsaPrivateKeyParameters other) => 
            (this.x.Equals(other.x) && base.Equals((DsaKeyParameters) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DsaPrivateKeyParameters other = obj as DsaPrivateKeyParameters;
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

