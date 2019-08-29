namespace Org.BouncyCastle.Crypto
{
    using Org.BouncyCastle.Security;
    using System;

    public class CipherKeyGenerator
    {
        protected internal SecureRandom random;
        protected internal int strength;
        private bool uninitialised;
        private int defaultStrength;

        public CipherKeyGenerator()
        {
            this.uninitialised = true;
        }

        internal CipherKeyGenerator(int defaultStrength)
        {
            this.uninitialised = true;
            if (defaultStrength < 1)
            {
                throw new ArgumentException("strength must be a positive value", "defaultStrength");
            }
            this.defaultStrength = defaultStrength;
        }

        protected virtual byte[] engineGenerateKey() => 
            SecureRandom.GetNextBytes(this.random, this.strength);

        protected virtual void engineInit(KeyGenerationParameters parameters)
        {
            this.random = parameters.Random;
            this.strength = (parameters.Strength + 7) / 8;
        }

        public byte[] GenerateKey()
        {
            if (this.uninitialised)
            {
                if (this.defaultStrength < 1)
                {
                    throw new InvalidOperationException("Generator has not been initialised");
                }
                this.uninitialised = false;
                this.engineInit(new KeyGenerationParameters(new SecureRandom(), this.defaultStrength));
            }
            return this.engineGenerateKey();
        }

        public void Init(KeyGenerationParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            this.uninitialised = false;
            this.engineInit(parameters);
        }

        public int DefaultStrength =>
            this.defaultStrength;
    }
}

