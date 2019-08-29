namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;

    public class ElGamalKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly ElGamalParameters parameters;

        public ElGamalKeyGenerationParameters(SecureRandom random, ElGamalParameters parameters) : base(random, GetStrength(parameters))
        {
            this.parameters = parameters;
        }

        internal static int GetStrength(ElGamalParameters parameters) => 
            ((parameters.L == 0) ? parameters.P.BitLength : parameters.L);

        public ElGamalParameters Parameters =>
            this.parameters;
    }
}

