namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;

    public class ParametersWithRandom : ICipherParameters
    {
        private readonly ICipherParameters parameters;
        private readonly SecureRandom random;

        public ParametersWithRandom(ICipherParameters parameters) : this(parameters, new SecureRandom())
        {
        }

        public ParametersWithRandom(ICipherParameters parameters, SecureRandom random)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            if (random == null)
            {
                throw new ArgumentNullException("random");
            }
            this.parameters = parameters;
            this.random = random;
        }

        [Obsolete("Use Random property instead")]
        public SecureRandom GetRandom() => 
            this.Random;

        public SecureRandom Random =>
            this.random;

        public ICipherParameters Parameters =>
            this.parameters;
    }
}

