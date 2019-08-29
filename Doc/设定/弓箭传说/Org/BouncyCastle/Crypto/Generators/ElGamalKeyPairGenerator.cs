namespace Org.BouncyCastle.Crypto.Generators
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using System;

    public class ElGamalKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private ElGamalKeyGenerationParameters param;

        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
            ElGamalParameters parameters = this.param.Parameters;
            DHParameters dhParams = new DHParameters(parameters.P, parameters.G, null, 0, parameters.L);
            BigInteger x = instance.CalculatePrivate(dhParams, this.param.Random);
            return new AsymmetricCipherKeyPair(new ElGamalPublicKeyParameters(instance.CalculatePublic(dhParams, x), parameters), new ElGamalPrivateKeyParameters(x, parameters));
        }

        public void Init(KeyGenerationParameters parameters)
        {
            this.param = (ElGamalKeyGenerationParameters) parameters;
        }
    }
}

