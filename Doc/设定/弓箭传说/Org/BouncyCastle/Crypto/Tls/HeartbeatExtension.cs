namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.IO;

    public class HeartbeatExtension
    {
        protected readonly byte mMode;

        public HeartbeatExtension(byte mode)
        {
            if (!HeartbeatMode.IsValid(mode))
            {
                throw new ArgumentException("not a valid HeartbeatMode value", "mode");
            }
            this.mMode = mode;
        }

        public virtual void Encode(Stream output)
        {
            TlsUtilities.WriteUint8(this.mMode, output);
        }

        public static HeartbeatExtension Parse(Stream input)
        {
            byte heartbeatMode = TlsUtilities.ReadUint8(input);
            if (!HeartbeatMode.IsValid(heartbeatMode))
            {
                throw new TlsFatalAlert(0x2f);
            }
            return new HeartbeatExtension(heartbeatMode);
        }

        public virtual byte Mode =>
            this.mMode;
    }
}

