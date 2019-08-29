namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    internal class DeferredHash : TlsHandshakeHash, IDigest
    {
        protected const int BUFFERING_HASH_LIMIT = 4;
        protected TlsContext mContext;
        private DigestInputBuffer mBuf;
        private IDictionary mHashes;
        private int mPrfHashAlgorithm;

        internal DeferredHash()
        {
            this.mBuf = new DigestInputBuffer();
            this.mHashes = Platform.CreateHashtable();
            this.mPrfHashAlgorithm = -1;
        }

        private DeferredHash(byte prfHashAlgorithm, IDigest prfHash)
        {
            this.mBuf = null;
            this.mHashes = Platform.CreateHashtable();
            this.mPrfHashAlgorithm = prfHashAlgorithm;
            this.mHashes[prfHashAlgorithm] = prfHash;
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int len)
        {
            if (this.mBuf != null)
            {
                this.mBuf.Write(input, inOff, len);
            }
            else
            {
                IEnumerator enumerator = this.mHashes.Values.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ((IDigest) enumerator.Current).BlockUpdate(input, inOff, len);
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        IDisposable disposable;
                        disposable.Dispose();
                    }
                }
            }
        }

        protected virtual void CheckStopBuffering()
        {
            if ((this.mBuf != null) && (this.mHashes.Count <= 4))
            {
                IEnumerator enumerator = this.mHashes.Values.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        IDigest current = (IDigest) enumerator.Current;
                        this.mBuf.UpdateDigest(current);
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        IDisposable disposable;
                        disposable.Dispose();
                    }
                }
                this.mBuf = null;
            }
        }

        protected virtual void CheckTrackingHash(byte hashAlgorithm)
        {
            if (!this.mHashes.Contains(hashAlgorithm))
            {
                IDigest digest = TlsUtilities.CreateHash(hashAlgorithm);
                this.mHashes[hashAlgorithm] = digest;
            }
        }

        public virtual int DoFinal(byte[] output, int outOff)
        {
            throw new InvalidOperationException("Use Fork() to get a definite IDigest");
        }

        public virtual IDigest ForkPrfHash()
        {
            this.CheckStopBuffering();
            byte mPrfHashAlgorithm = (byte) this.mPrfHashAlgorithm;
            if (this.mBuf != null)
            {
                IDigest d = TlsUtilities.CreateHash(mPrfHashAlgorithm);
                this.mBuf.UpdateDigest(d);
                return d;
            }
            return TlsUtilities.CloneHash(mPrfHashAlgorithm, (IDigest) this.mHashes[mPrfHashAlgorithm]);
        }

        public virtual int GetByteLength()
        {
            throw new InvalidOperationException("Use Fork() to get a definite IDigest");
        }

        public virtual int GetDigestSize()
        {
            throw new InvalidOperationException("Use Fork() to get a definite IDigest");
        }

        public virtual byte[] GetFinalHash(byte hashAlgorithm)
        {
            IDigest hash = (IDigest) this.mHashes[hashAlgorithm];
            if (hash == null)
            {
                throw new InvalidOperationException("HashAlgorithm." + HashAlgorithm.GetText(hashAlgorithm) + " is not being tracked");
            }
            hash = TlsUtilities.CloneHash(hashAlgorithm, hash);
            if (this.mBuf != null)
            {
                this.mBuf.UpdateDigest(hash);
            }
            return DigestUtilities.DoFinal(hash);
        }

        public virtual void Init(TlsContext context)
        {
            this.mContext = context;
        }

        public virtual TlsHandshakeHash NotifyPrfDetermined()
        {
            int prfAlgorithm = this.mContext.SecurityParameters.PrfAlgorithm;
            if (prfAlgorithm == 0)
            {
                CombinedHash d = new CombinedHash();
                d.Init(this.mContext);
                this.mBuf.UpdateDigest(d);
                return d.NotifyPrfDetermined();
            }
            this.mPrfHashAlgorithm = TlsUtilities.GetHashAlgorithmForPrfAlgorithm(prfAlgorithm);
            this.CheckTrackingHash((byte) this.mPrfHashAlgorithm);
            return this;
        }

        public virtual void Reset()
        {
            if (this.mBuf != null)
            {
                this.mBuf.SetLength(0L);
            }
            else
            {
                IEnumerator enumerator = this.mHashes.Values.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ((IDigest) enumerator.Current).Reset();
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        IDisposable disposable;
                        disposable.Dispose();
                    }
                }
            }
        }

        public virtual void SealHashAlgorithms()
        {
            this.CheckStopBuffering();
        }

        public virtual TlsHandshakeHash StopTracking()
        {
            byte mPrfHashAlgorithm = (byte) this.mPrfHashAlgorithm;
            IDigest d = TlsUtilities.CloneHash(mPrfHashAlgorithm, (IDigest) this.mHashes[mPrfHashAlgorithm]);
            if (this.mBuf != null)
            {
                this.mBuf.UpdateDigest(d);
            }
            DeferredHash hash = new DeferredHash(mPrfHashAlgorithm, d);
            hash.Init(this.mContext);
            return hash;
        }

        public virtual void TrackHashAlgorithm(byte hashAlgorithm)
        {
            if (this.mBuf == null)
            {
                throw new InvalidOperationException("Too late to track more hash algorithms");
            }
            this.CheckTrackingHash(hashAlgorithm);
        }

        public virtual void Update(byte input)
        {
            if (this.mBuf != null)
            {
                this.mBuf.WriteByte(input);
            }
            else
            {
                IEnumerator enumerator = this.mHashes.Values.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        ((IDigest) enumerator.Current).Update(input);
                    }
                }
                finally
                {
                    if (enumerator is IDisposable disposable)
                    {
                        IDisposable disposable;
                        disposable.Dispose();
                    }
                }
            }
        }

        public virtual string AlgorithmName
        {
            get
            {
                throw new InvalidOperationException("Use Fork() to get a definite IDigest");
            }
        }
    }
}

