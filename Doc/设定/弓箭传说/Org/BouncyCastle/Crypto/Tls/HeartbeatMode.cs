namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class HeartbeatMode
    {
        public const byte peer_allowed_to_send = 1;
        public const byte peer_not_allowed_to_send = 2;

        protected HeartbeatMode()
        {
        }

        public static bool IsValid(byte heartbeatMode) => 
            ((heartbeatMode >= 1) && (heartbeatMode <= 2));
    }
}

