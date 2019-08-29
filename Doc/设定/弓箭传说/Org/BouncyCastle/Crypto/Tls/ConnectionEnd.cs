namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class ConnectionEnd
    {
        public const int server = 0;
        public const int client = 1;

        protected ConnectionEnd()
        {
        }
    }
}

