namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;

    public class DHKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly DHParameters parameters;

        public DHKeyGenerationParameters(SecureRandom random, DHParameters parameters) : base(random, GetStrength(parameters))
        {
            this.parameters = parameters;
        }

        internal static int GetStrength(DHParameters parameters) => 
            ((parameters.L == 0) ? parameters.P.BitLength : parameters.L);

        public DHParameters Parameters =>
            this.parameters;
    }
}

