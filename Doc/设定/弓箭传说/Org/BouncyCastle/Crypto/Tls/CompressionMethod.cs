namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class CompressionMethod
    {
        public const byte cls_null = 0;
        public const byte DEFLATE = 1;

        protected CompressionMethod()
        {
        }
    }
}

