namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Math;
    using System;

    public class DsaPublicKeyParameters : DsaKeyParameters
    {
        private readonly BigInteger y;

        public DsaPublicKeyParameters(BigInteger y, DsaParameters parameters) : base(false, parameters)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            this.y = y;
        }

        protected bool Equals(DsaPublicKeyParameters other) => 
            (this.y.Equals(other.y) && base.Equals((DsaKeyParameters) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DsaPublicKeyParameters other = obj as DsaPublicKeyParameters;
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

