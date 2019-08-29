namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math.EC;
    using System;

    public class ECPublicKeyParameters : ECKeyParameters
    {
        private readonly ECPoint q;

        [Obsolete("Use version with explicit 'algorithm' parameter")]
        public ECPublicKeyParameters(ECPoint q, DerObjectIdentifier publicKeyParamSet) : base("ECGOST3410", false, publicKeyParamSet)
        {
            if (q == null)
            {
                throw new ArgumentNullException("q");
            }
            this.q = q.Normalize();
        }

        public ECPublicKeyParameters(ECPoint q, ECDomainParameters parameters) : this("EC", q, parameters)
        {
        }

        public ECPublicKeyParameters(string algorithm, ECPoint q, DerObjectIdentifier publicKeyParamSet) : base(algorithm, false, publicKeyParamSet)
        {
            if (q == null)
            {
                throw new ArgumentNullException("q");
            }
            this.q = q.Normalize();
        }

        public ECPublicKeyParameters(string algorithm, ECPoint q, ECDomainParameters parameters) : base(algorithm, false, parameters)
        {
            if (q == null)
            {
                throw new ArgumentNullException("q");
            }
            this.q = q.Normalize();
        }

        protected bool Equals(ECPublicKeyParameters other) => 
            (this.q.Equals(other.q) && base.Equals((ECKeyParameters) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            ECPublicKeyParameters other = obj as ECPublicKeyParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            (this.q.GetHashCode() ^ base.GetHashCode());

        public ECPoint Q =>
            this.q;
    }
}

