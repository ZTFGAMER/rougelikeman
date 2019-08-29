namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using System;

    public class Iso18033KdfParameters : IDerivationParameters
    {
        private byte[] seed;

        public Iso18033KdfParameters(byte[] seed)
        {
            this.seed = seed;
        }

        public byte[] GetSeed() => 
            this.seed;
    }
}

