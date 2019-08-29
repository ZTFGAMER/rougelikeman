namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using System;
    using System.IO;

    public class ServerDHParams
    {
        protected readonly DHPublicKeyParameters mPublicKey;

        public ServerDHParams(DHPublicKeyParameters publicKey)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException("publicKey");
            }
            this.mPublicKey = publicKey;
        }

        public virtual void Encode(Stream output)
        {
            DHParameters parameters = this.mPublicKey.Parameters;
            BigInteger y = this.mPublicKey.Y;
            TlsDHUtilities.WriteDHParameter(parameters.P, output);
            TlsDHUtilities.WriteDHParameter(parameters.G, output);
            TlsDHUtilities.WriteDHParameter(y, output);
        }

        public static ServerDHParams Parse(Stream input)
        {
            BigInteger p = TlsDHUtilities.ReadDHParameter(input);
            BigInteger g = TlsDHUtilities.ReadDHParameter(input);
            return new ServerDHParams(TlsDHUtilities.ValidateDHPublicKey(new DHPublicKeyParameters(TlsDHUtilities.ReadDHParameter(input), new DHParameters(p, g))));
        }

        public virtual DHPublicKeyParameters PublicKey =>
            this.mPublicKey;
    }
}

