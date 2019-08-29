namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;

    public class DsaKeyGenerationParameters : KeyGenerationParameters
    {
        private readonly DsaParameters parameters;

        public DsaKeyGenerationParameters(SecureRandom random, DsaParameters parameters) : base(random, parameters.P.BitLength - 1)
        {
            this.parameters = parameters;
        }

        public DsaParameters Parameters =>
            this.parameters;
    }
}

