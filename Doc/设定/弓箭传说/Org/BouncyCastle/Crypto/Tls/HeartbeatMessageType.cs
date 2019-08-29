namespace Org.BouncyCastle.Crypto.Tls
{
    using System;

    public abstract class HeartbeatMessageType
    {
        public const byte heartbeat_request = 1;
        public const byte heartbeat_response = 2;

        protected HeartbeatMessageType()
        {
        }

        public static bool IsValid(byte heartbeatMessageType) => 
            ((heartbeatMessageType >= 1) && (heartbeatMessageType <= 2));
    }
}

