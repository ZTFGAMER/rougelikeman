namespace Org.BouncyCastle.Crypto.Generators
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DsaKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private static readonly BigInteger One = BigInteger.One;
        private DsaKeyGenerationParameters param;

        private static BigInteger CalculatePublicKey(BigInteger p, BigInteger g, BigInteger x) => 
            g.ModPow(x, p);

        public AsymmetricCipherKeyPair GenerateKeyPair()
        {
            DsaParameters parameters = this.param.Parameters;
            BigInteger x = GeneratePrivateKey(parameters.Q, this.param.Random);
            return new AsymmetricCipherKeyPair(new DsaPublicKeyParameters(CalculatePublicKey(parameters.P, parameters.G, x), parameters), new DsaPrivateKeyParameters(x, parameters));
        }

        private static BigInteger GeneratePrivateKey(BigInteger q, SecureRandom random)
        {
            BigInteger integer;
            int num = q.BitLength >> 2;
            do
            {
                integer = BigIntegers.CreateRandomInRange(One, q.Subtract(One), random);
            }
            while (WNafUtilities.GetNafWeight(integer) < num);
            return integer;
        }

        public void Init(KeyGenerationParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            this.param = (DsaKeyGenerationParameters) parameters;
        }
    }
}

