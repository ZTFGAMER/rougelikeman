namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Crypto;
    using System;

    public class DHKeyParameters : AsymmetricKeyParameter
    {
        private readonly DHParameters parameters;
        private readonly DerObjectIdentifier algorithmOid;

        protected DHKeyParameters(bool isPrivate, DHParameters parameters) : this(isPrivate, parameters, PkcsObjectIdentifiers.DhKeyAgreement)
        {
        }

        protected DHKeyParameters(bool isPrivate, DHParameters parameters, DerObjectIdentifier algorithmOid) : base(isPrivate)
        {
            this.parameters = parameters;
            this.algorithmOid = algorithmOid;
        }

        protected bool Equals(DHKeyParameters other) => 
            (object.Equals(this.parameters, other.parameters) && base.Equals((AsymmetricKeyParameter) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DHKeyParameters other = obj as DHKeyParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            if (this.parameters != null)
            {
                hashCode ^= this.parameters.GetHashCode();
            }
            return hashCode;
        }

        public DHParameters Parameters =>
            this.parameters;

        public DerObjectIdentifier AlgorithmOid =>
            this.algorithmOid;
    }
}

