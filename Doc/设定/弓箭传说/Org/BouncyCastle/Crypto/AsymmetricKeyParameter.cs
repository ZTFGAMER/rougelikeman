namespace Org.BouncyCastle.Crypto
{
    using System;

    public abstract class AsymmetricKeyParameter : ICipherParameters
    {
        private readonly bool privateKey;

        protected AsymmetricKeyParameter(bool privateKey)
        {
            this.privateKey = privateKey;
        }

        protected bool Equals(AsymmetricKeyParameter other) => 
            (this.privateKey == other.privateKey);

        public override bool Equals(object obj)
        {
            AsymmetricKeyParameter other = obj as AsymmetricKeyParameter;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            this.privateKey.GetHashCode();

        public bool IsPrivate =>
            this.privateKey;
    }
}

