namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using System;

    public class DHPrivateKeyParameters : DHKeyParameters
    {
        private readonly BigInteger x;

        public DHPrivateKeyParameters(BigInteger x, DHParameters parameters) : base(true, parameters)
        {
            this.x = x;
        }

        public DHPrivateKeyParameters(BigInteger x, DHParameters parameters, DerObjectIdentifier algorithmOid) : base(true, parameters, algorithmOid)
        {
            this.x = x;
        }

        protected bool Equals(DHPrivateKeyParameters other) => 
            (this.x.Equals(other.x) && base.Equals((DHKeyParameters) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DHPrivateKeyParameters other = obj as DHPrivateKeyParameters;
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

