namespace Org.BouncyCastle.Crypto.Generators
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using System;

    public class DHBasicKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private DHKeyGenerationParameters param;

        public virtual AsymmetricCipherKeyPair GenerateKeyPair()
        {
            DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
            DHParameters dhParams = this.param.Parameters;
            BigInteger x = instance.CalculatePrivate(dhParams, this.param.Random);
            return new AsymmetricCipherKeyPair(new DHPublicKeyParameters(instance.CalculatePublic(dhParams, x), dhParams), new DHPrivateKeyParameters(x, dhParams));
        }

        public virtual void Init(KeyGenerationParameters parameters)
        {
            this.param = (DHKeyGenerationParameters) parameters;
        }
    }
}

