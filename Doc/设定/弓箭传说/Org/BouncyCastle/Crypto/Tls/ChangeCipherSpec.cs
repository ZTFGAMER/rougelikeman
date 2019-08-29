namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class ChangeCipherSpec
    {
        public const byte change_cipher_spec = 1;

        protected ChangeCipherSpec()
        {
        }
    }
}

