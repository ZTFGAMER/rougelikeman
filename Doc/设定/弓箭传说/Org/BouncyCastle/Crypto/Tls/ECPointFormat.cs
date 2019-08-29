namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class ECPointFormat
    {
        public const byte uncompressed = 0;
        public const byte ansiX962_compressed_prime = 1;
        public const byte ansiX962_compressed_char2 = 2;

        protected ECPointFormat()
        {
        }
    }
}

