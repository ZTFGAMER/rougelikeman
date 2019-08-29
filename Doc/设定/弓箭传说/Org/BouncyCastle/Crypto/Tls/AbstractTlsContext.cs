namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Prng;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Threading;

    internal abstract class AbstractTlsContext : TlsContext
    {
        private static long counter = Times.NanoTime();
        private readonly IRandomGenerator mNonceRandom;
        private readonly Org.BouncyCastle.Security.SecureRandom mSecureRandom;
        private readonly Org.BouncyCastle.Crypto.Tls.SecurityParameters mSecurityParameters;
        private ProtocolVersion mClientVersion;
        private ProtocolVersion mServerVersion;
        private TlsSession mSession;
        private object mUserObject;

        internal AbstractTlsContext(Org.BouncyCastle.Security.SecureRandom secureRandom, Org.BouncyCastle.Crypto.Tls.SecurityParameters securityParameters)
        {
            IDigest digest = TlsUtilities.CreateHash((byte) 4);
            byte[] buffer = new byte[digest.GetDigestSize()];
            secureRandom.NextBytes(buffer);
            this.mNonceRandom = new DigestRandomGenerator(digest);
            this.mNonceRandom.AddSeedMaterial(NextCounterValue());
            this.mNonceRandom.AddSeedMaterial(Times.NanoTime());
            this.mNonceRandom.AddSeedMaterial(buffer);
            this.mSecureRandom = secureRandom;
            this.mSecurityParameters = securityParameters;
        }

        public virtual byte[] ExportKeyingMaterial(string asciiLabel, byte[] context_value, int length)
        {
            if ((context_value != null) && !TlsUtilities.IsValidUint16(context_value.Length))
            {
                throw new ArgumentException("must have length less than 2^16 (or be null)", "context_value");
            }
            Org.BouncyCastle.Crypto.Tls.SecurityParameters securityParameters = this.SecurityParameters;
            byte[] clientRandom = securityParameters.ClientRandom;
            byte[] serverRandom = securityParameters.ServerRandom;
            int num = clientRandom.Length + serverRandom.Length;
            if (context_value != null)
            {
                num += 2 + context_value.Length;
            }
            byte[] destinationArray = new byte[num];
            int destinationIndex = 0;
            Array.Copy(clientRandom, 0, destinationArray, destinationIndex, clientRandom.Length);
            destinationIndex += clientRandom.Length;
            Array.Copy(serverRandom, 0, destinationArray, destinationIndex, serverRandom.Length);
            destinationIndex += serverRandom.Length;
            if (context_value != null)
            {
                TlsUtilities.WriteUint16(context_value.Length, destinationArray, destinationIndex);
                destinationIndex += 2;
                Array.Copy(context_value, 0, destinationArray, destinationIndex, context_value.Length);
                destinationIndex += context_value.Length;
            }
            if (destinationIndex != num)
            {
                throw new InvalidOperationException("error in calculation of seed for export");
            }
            return TlsUtilities.PRF(this, securityParameters.MasterSecret, asciiLabel, destinationArray, length);
        }

        private static long NextCounterValue() => 
            Interlocked.Increment(ref counter);

        internal virtual void SetClientVersion(ProtocolVersion clientVersion)
        {
            this.mClientVersion = clientVersion;
        }

        internal virtual void SetResumableSession(TlsSession session)
        {
            this.mSession = session;
        }

        internal virtual void SetServerVersion(ProtocolVersion serverVersion)
        {
            this.mServerVersion = serverVersion;
        }

        public virtual IRandomGenerator NonceRandomGenerator =>
            this.mNonceRandom;

        public virtual Org.BouncyCastle.Security.SecureRandom SecureRandom =>
            this.mSecureRandom;

        public virtual Org.BouncyCastle.Crypto.Tls.SecurityParameters SecurityParameters =>
            this.mSecurityParameters;

        public abstract bool IsServer { get; }

        public virtual ProtocolVersion ClientVersion =>
            this.mClientVersion;

        public virtual ProtocolVersion ServerVersion =>
            this.mServerVersion;

        public virtual TlsSession ResumableSession =>
            this.mSession;

        public virtual object UserObject
        {
            get => 
                this.mUserObject;
            set => 
                (this.mUserObject = value);
        }
    }
}

