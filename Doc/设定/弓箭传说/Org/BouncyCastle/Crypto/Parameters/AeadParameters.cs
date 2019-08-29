namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using System;

    public class AeadParameters : ICipherParameters
    {
        private readonly byte[] associatedText;
        private readonly byte[] nonce;
        private readonly KeyParameter key;
        private readonly int macSize;

        public AeadParameters(KeyParameter key, int macSize, byte[] nonce) : this(key, macSize, nonce, null)
        {
        }

        public AeadParameters(KeyParameter key, int macSize, byte[] nonce, byte[] associatedText)
        {
            this.key = key;
            this.nonce = nonce;
            this.macSize = macSize;
            this.associatedText = associatedText;
        }

        public virtual byte[] GetAssociatedText() => 
            this.associatedText;

        public virtual byte[] GetNonce() => 
            this.nonce;

        public virtual KeyParameter Key =>
            this.key;

        public virtual int MacSize =>
            this.macSize;
    }
}

