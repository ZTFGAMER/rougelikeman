namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using System;

    public class ElGamalKeyParameters : AsymmetricKeyParameter
    {
        private readonly ElGamalParameters parameters;

        protected ElGamalKeyParameters(bool isPrivate, ElGamalParameters parameters) : base(isPrivate)
        {
            this.parameters = parameters;
        }

        protected bool Equals(ElGamalKeyParameters other) => 
            (object.Equals(this.parameters, other.parameters) && base.Equals((AsymmetricKeyParameter) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            ElGamalKeyParameters other = obj as ElGamalKeyParameters;
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

        public ElGamalParameters Parameters =>
            this.parameters;
    }
}

