namespace Org.BouncyCastle.Crypto
{
    using Org.BouncyCastle.Security;
    using System;

    public class KeyGenerationParameters
    {
        private SecureRandom random;
        private int strength;

        public KeyGenerationParameters(SecureRandom random, int strength)
        {
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }
            if (strength < 1)
            {
                throw new ArgumentException("strength must be a positive value", "strength");
            }
            this.random = random;
            this.strength = strength;
        }

        public SecureRandom Random =>
            this.random;

        public int Strength =>
            this.strength;
    }
}

