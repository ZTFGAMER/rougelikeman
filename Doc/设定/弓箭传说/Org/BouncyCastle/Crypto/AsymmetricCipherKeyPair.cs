namespace Org.BouncyCastle.Crypto
{
    using System;

    public class AsymmetricCipherKeyPair
    {
        private readonly AsymmetricKeyParameter publicParameter;
        private readonly AsymmetricKeyParameter privateParameter;

        public AsymmetricCipherKeyPair(AsymmetricKeyParameter publicParameter, AsymmetricKeyParameter privateParameter)
        {
            if (publicParameter.IsPrivate)
            {
                throw new ArgumentException("Expected a public key", "publicParameter");
            }
            if (!privateParameter.IsPrivate)
            {
                throw new ArgumentException("Expected a private key", "privateParameter");
            }
            this.publicParameter = publicParameter;
            this.privateParameter = privateParameter;
        }

        public AsymmetricKeyParameter Public =>
            this.publicParameter;

        public AsymmetricKeyParameter Private =>
            this.privateParameter;
    }
}

