namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class ContentType
    {
        public const byte change_cipher_spec = 20;
        public const byte alert = 0x15;
        public const byte handshake = 0x16;
        public const byte application_data = 0x17;
        public const byte heartbeat = 0x18;

        protected ContentType()
        {
        }
    }
}

