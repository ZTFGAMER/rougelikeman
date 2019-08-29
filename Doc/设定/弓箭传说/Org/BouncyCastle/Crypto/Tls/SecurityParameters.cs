namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class SecurityParameters
    {
        internal int entity = -1;
        internal int cipherSuite = -1;
        internal byte compressionAlgorithm;
        internal int prfAlgorithm = -1;
        internal int verifyDataLength = -1;
        internal byte[] masterSecret;
        internal byte[] clientRandom;
        internal byte[] serverRandom;
        internal byte[] sessionHash;
        internal byte[] pskIdentity;
        internal byte[] srpIdentity;
        internal short maxFragmentLength = -1;
        internal bool truncatedHMac;
        internal bool encryptThenMac;
        internal bool extendedMasterSecret;

        internal virtual void Clear()
        {
            if (this.masterSecret != null)
            {
                Arrays.Fill(this.masterSecret, 0);
                this.masterSecret = null;
            }
        }

        public virtual int Entity =>
            this.entity;

        public virtual int CipherSuite =>
            this.cipherSuite;

        public byte CompressionAlgorithm =>
            this.compressionAlgorithm;

        public virtual int PrfAlgorithm =>
            this.prfAlgorithm;

        public virtual int VerifyDataLength =>
            this.verifyDataLength;

        public virtual byte[] MasterSecret =>
            this.masterSecret;

        public virtual byte[] ClientRandom =>
            this.clientRandom;

        public virtual byte[] ServerRandom =>
            this.serverRandom;

        public virtual byte[] SessionHash =>
            this.sessionHash;

        public virtual byte[] PskIdentity =>
            this.pskIdentity;

        public virtual byte[] SrpIdentity =>
            this.srpIdentity;
    }
}

