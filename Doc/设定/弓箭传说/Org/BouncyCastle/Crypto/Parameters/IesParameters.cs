namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using System;

    public class IesParameters : ICipherParameters
    {
        private byte[] derivation;
        private byte[] encoding;
        private int macKeySize;

        public IesParameters(byte[] derivation, byte[] encoding, int macKeySize)
        {
            this.derivation = derivation;
            this.encoding = encoding;
            this.macKeySize = macKeySize;
        }

        public byte[] GetDerivationV() => 
            this.derivation;

        public byte[] GetEncodingV() => 
            this.encoding;

        public int MacKeySize =>
            this.macKeySize;
    }
}

