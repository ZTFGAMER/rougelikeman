namespace Org.BouncyCastle.Crypto.Parameters
{
    using System;

    public class IesWithCipherParameters : IesParameters
    {
        private int cipherKeySize;

        public IesWithCipherParameters(byte[] derivation, byte[] encoding, int macKeySize, int cipherKeySize) : base(derivation, encoding, macKeySize)
        {
            this.cipherKeySize = cipherKeySize;
        }

        public int CipherKeySize =>
            this.cipherKeySize;
    }
}

