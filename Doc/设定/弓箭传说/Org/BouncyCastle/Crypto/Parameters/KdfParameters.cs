namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using System;

    public class KdfParameters : IDerivationParameters
    {
        private byte[] iv;
        private byte[] shared;

        public KdfParameters(byte[] shared, byte[] iv)
        {
            this.shared = shared;
            this.iv = iv;
        }

        public byte[] GetIV() => 
            this.iv;

        public byte[] GetSharedSecret() => 
            this.shared;
    }
}

