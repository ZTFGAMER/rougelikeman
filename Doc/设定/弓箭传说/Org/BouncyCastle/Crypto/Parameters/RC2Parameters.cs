namespace Org.BouncyCastle.Crypto.Parameters
{
    using System;

    public class RC2Parameters : KeyParameter
    {
        private readonly int bits;

        public RC2Parameters(byte[] key) : this(key, (key.Length <= 0x80) ? (key.Length * 8) : 0x400)
        {
        }

        public RC2Parameters(byte[] key, int bits) : base(key)
        {
            this.bits = bits;
        }

        public RC2Parameters(byte[] key, int keyOff, int keyLen) : this(key, keyOff, keyLen, (keyLen <= 0x80) ? (keyLen * 8) : 0x400)
        {
        }

        public RC2Parameters(byte[] key, int keyOff, int keyLen, int bits) : base(key, keyOff, keyLen)
        {
            this.bits = bits;
        }

        public int EffectiveKeyBits =>
            this.bits;
    }
}

