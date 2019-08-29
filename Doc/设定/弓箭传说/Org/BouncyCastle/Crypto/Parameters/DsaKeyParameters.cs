namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using System;

    public abstract class DsaKeyParameters : AsymmetricKeyParameter
    {
        private readonly DsaParameters parameters;

        protected DsaKeyParameters(bool isPrivate, DsaParameters parameters) : base(isPrivate)
        {
            this.parameters = parameters;
        }

        protected bool Equals(DsaKeyParameters other) => 
            (object.Equals(this.parameters, other.parameters) && base.Equals((AsymmetricKeyParameter) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DsaKeyParameters other = obj as DsaKeyParameters;
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

        public DsaParameters Parameters =>
            this.parameters;
    }
}

