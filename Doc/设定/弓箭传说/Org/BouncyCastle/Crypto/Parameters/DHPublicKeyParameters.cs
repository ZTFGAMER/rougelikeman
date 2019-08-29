namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using System;

    public class DHPublicKeyParameters : DHKeyParameters
    {
        private readonly BigInteger y;

        public DHPublicKeyParameters(BigInteger y, DHParameters parameters) : base(false, parameters)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            this.y = y;
        }

        public DHPublicKeyParameters(BigInteger y, DHParameters parameters, DerObjectIdentifier algorithmOid) : base(false, parameters, algorithmOid)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            this.y = y;
        }

        protected bool Equals(DHPublicKeyParameters other) => 
            (this.y.Equals(other.y) && base.Equals((DHKeyParameters) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DHPublicKeyParameters other = obj as DHPublicKeyParameters;
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

