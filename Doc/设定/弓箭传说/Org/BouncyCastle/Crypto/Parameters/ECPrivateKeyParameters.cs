namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using System;

    public class ECPrivateKeyParameters : ECKeyParameters
    {
        private readonly BigInteger d;

        [Obsolete("Use version with explicit 'algorithm' parameter")]
        public ECPrivateKeyParameters(BigInteger d, DerObjectIdentifier publicKeyParamSet) : base("ECGOST3410", true, publicKeyParamSet)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }
            this.d = d;
        }

        public ECPrivateKeyParameters(BigInteger d, ECDomainParameters parameters) : this("EC", d, parameters)
        {
        }

        public ECPrivateKeyParameters(string algorithm, BigInteger d, DerObjectIdentifier publicKeyParamSet) : base(algorithm, true, publicKeyParamSet)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }
            this.d = d;
        }

        public ECPrivateKeyParameters(string algorithm, BigInteger d, ECDomainParameters parameters) : base(algorithm, true, parameters)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }
            this.d = d;
        }

        protected bool Equals(ECPrivateKeyParameters other) => 
            (this.d.Equals(other.d) && base.Equals((ECKeyParameters) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            ECPrivateKeyParameters other = obj as ECPrivateKeyParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            (this.d.GetHashCode() ^ base.GetHashCode());

        public BigInteger D =>
            this.d;
    }
}

