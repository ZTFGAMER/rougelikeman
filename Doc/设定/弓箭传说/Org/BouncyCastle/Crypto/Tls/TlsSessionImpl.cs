namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Utilities;
    using System;

    internal class TlsSessionImpl : TlsSession
    {
        internal readonly byte[] mSessionID;
        internal SessionParameters mSessionParameters;

        internal TlsSessionImpl(byte[] sessionID, SessionParameters sessionParameters)
        {
            if (sessionID == null)
            {
                throw new ArgumentNullException("sessionID");
            }
            if ((sessionID.Length < 1) || (sessionID.Length > 0x20))
            {
                throw new ArgumentException("must have length between 1 and 32 bytes, inclusive", "sessionID");
            }
            this.mSessionID = Arrays.Clone(sessionID);
            this.mSessionParameters = sessionParameters;
        }

        public virtual SessionParameters ExportSessionParameters()
        {
            object obj2 = this;
            lock (obj2)
            {
                return this.mSessionParameters?.Copy();
            }
        }

        public virtual void Invalidate()
        {
            object obj2 = this;
            lock (obj2)
            {
                if (this.mSessionParameters != null)
                {
                    this.mSessionParameters.Clear();
                    this.mSessionParameters = null;
                }
            }
        }

        public virtual byte[] SessionID
        {
            get
            {
                object obj2 = this;
                lock (obj2)
                {
                    return this.mSessionID;
                }
            }
        }

        public virtual bool IsResumable
        {
            get
            {
                object obj2 = this;
                lock (obj2)
                {
                    return (this.mSessionParameters != null);
                }
            }
        }
    }
}

