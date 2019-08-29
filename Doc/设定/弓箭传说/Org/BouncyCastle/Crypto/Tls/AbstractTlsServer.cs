namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public abstract class AbstractTlsServer : AbstractTlsPeer, TlsServer, TlsPeer
    {
        protected TlsCipherFactory mCipherFactory;
        protected TlsServerContext mContext;
        protected ProtocolVersion mClientVersion;
        protected int[] mOfferedCipherSuites;
        protected byte[] mOfferedCompressionMethods;
        protected IDictionary mClientExtensions;
        protected bool mEncryptThenMacOffered;
        protected short mMaxFragmentLengthOffered;
        protected bool mTruncatedHMacOffered;
        protected IList mSupportedSignatureAlgorithms;
        protected bool mEccCipherSuitesOffered;
        protected int[] mNamedCurves;
        protected byte[] mClientECPointFormats;
        protected byte[] mServerECPointFormats;
        protected ProtocolVersion mServerVersion;
        protected int mSelectedCipherSuite;
        protected byte mSelectedCompressionMethod;
        protected IDictionary mServerExtensions;

        public AbstractTlsServer() : this(new DefaultTlsCipherFactory())
        {
        }

        public AbstractTlsServer(TlsCipherFactory cipherFactory)
        {
            this.mCipherFactory = cipherFactory;
        }

        protected virtual IDictionary CheckServerExtensions() => 
            (this.mServerExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(this.mServerExtensions));

        public virtual CertificateRequest GetCertificateRequest() => 
            null;

        public virtual CertificateStatus GetCertificateStatus() => 
            null;

        public override TlsCipher GetCipher()
        {
            int encryptionAlgorithm = TlsUtilities.GetEncryptionAlgorithm(this.mSelectedCipherSuite);
            int macAlgorithm = TlsUtilities.GetMacAlgorithm(this.mSelectedCipherSuite);
            return this.mCipherFactory.CreateCipher(this.mContext, encryptionAlgorithm, macAlgorithm);
        }

        protected abstract int[] GetCipherSuites();
        public override TlsCompression GetCompression()
        {
            if (this.mSelectedCompressionMethod != 0)
            {
                throw new TlsFatalAlert(80);
            }
            return new TlsNullCompression();
        }

        protected byte[] GetCompressionMethods() => 
            new byte[1];

        public abstract TlsCredentials GetCredentials();
        public abstract TlsKeyExchange GetKeyExchange();
        public virtual NewSessionTicket GetNewSessionTicket() => 
            new NewSessionTicket(0L, TlsUtilities.EmptyBytes);

        public virtual int GetSelectedCipherSuite()
        {
            bool flag = this.SupportsClientEccCapabilities(this.mNamedCurves, this.mClientECPointFormats);
            foreach (int num2 in this.GetCipherSuites())
            {
                if ((Arrays.Contains(this.mOfferedCipherSuites, num2) && (flag || !TlsEccUtilities.IsEccCipherSuite(num2))) && TlsUtilities.IsValidCipherSuiteForVersion(num2, this.mServerVersion))
                {
                    return (this.mSelectedCipherSuite = num2);
                }
            }
            throw new TlsFatalAlert(40);
        }

        public virtual byte GetSelectedCompressionMethod()
        {
            byte[] compressionMethods = this.GetCompressionMethods();
            for (int i = 0; i < compressionMethods.Length; i++)
            {
                if (Arrays.Contains(this.mOfferedCompressionMethods, compressionMethods[i]))
                {
                    return (this.mSelectedCompressionMethod = compressionMethods[i]);
                }
            }
            throw new TlsFatalAlert(40);
        }

        public virtual IDictionary GetServerExtensions()
        {
            if ((this.mEncryptThenMacOffered && this.AllowEncryptThenMac) && TlsUtilities.IsBlockCipherSuite(this.mSelectedCipherSuite))
            {
                TlsExtensionsUtilities.AddEncryptThenMacExtension(this.CheckServerExtensions());
            }
            if (((this.mMaxFragmentLengthOffered >= 0) && TlsUtilities.IsValidUint8((int) this.mMaxFragmentLengthOffered)) && MaxFragmentLength.IsValid((byte) this.mMaxFragmentLengthOffered))
            {
                TlsExtensionsUtilities.AddMaxFragmentLengthExtension(this.CheckServerExtensions(), (byte) this.mMaxFragmentLengthOffered);
            }
            if (this.mTruncatedHMacOffered && this.AllowTruncatedHMac)
            {
                TlsExtensionsUtilities.AddTruncatedHMacExtension(this.CheckServerExtensions());
            }
            if ((this.mClientECPointFormats != null) && TlsEccUtilities.IsEccCipherSuite(this.mSelectedCipherSuite))
            {
                byte[] buffer1 = new byte[3];
                buffer1[1] = 1;
                buffer1[2] = 2;
                this.mServerECPointFormats = buffer1;
                TlsEccUtilities.AddSupportedPointFormatsExtension(this.CheckServerExtensions(), this.mServerECPointFormats);
            }
            return this.mServerExtensions;
        }

        public virtual IList GetServerSupplementalData() => 
            null;

        public virtual ProtocolVersion GetServerVersion()
        {
            if (this.MinimumVersion.IsEqualOrEarlierVersionOf(this.mClientVersion))
            {
                ProtocolVersion maximumVersion = this.MaximumVersion;
                if (this.mClientVersion.IsEqualOrEarlierVersionOf(maximumVersion))
                {
                    return (this.mServerVersion = this.mClientVersion);
                }
                if (this.mClientVersion.IsLaterVersionOf(maximumVersion))
                {
                    return (this.mServerVersion = maximumVersion);
                }
            }
            throw new TlsFatalAlert(70);
        }

        public virtual void Init(TlsServerContext context)
        {
            this.mContext = context;
        }

        public virtual void NotifyClientCertificate(Certificate clientCertificate)
        {
            throw new TlsFatalAlert(80);
        }

        public virtual void NotifyClientVersion(ProtocolVersion clientVersion)
        {
            this.mClientVersion = clientVersion;
        }

        public virtual void NotifyFallback(bool isFallback)
        {
            if (isFallback && this.MaximumVersion.IsLaterVersionOf(this.mClientVersion))
            {
                throw new TlsFatalAlert(0x56);
            }
        }

        public virtual void NotifyOfferedCipherSuites(int[] offeredCipherSuites)
        {
            this.mOfferedCipherSuites = offeredCipherSuites;
            this.mEccCipherSuitesOffered = TlsEccUtilities.ContainsEccCipherSuites(this.mOfferedCipherSuites);
        }

        public virtual void NotifyOfferedCompressionMethods(byte[] offeredCompressionMethods)
        {
            this.mOfferedCompressionMethods = offeredCompressionMethods;
        }

        public virtual void ProcessClientExtensions(IDictionary clientExtensions)
        {
            this.mClientExtensions = clientExtensions;
            if (clientExtensions != null)
            {
                this.mEncryptThenMacOffered = TlsExtensionsUtilities.HasEncryptThenMacExtension(clientExtensions);
                this.mMaxFragmentLengthOffered = TlsExtensionsUtilities.GetMaxFragmentLengthExtension(clientExtensions);
                if ((this.mMaxFragmentLengthOffered >= 0) && !MaxFragmentLength.IsValid((byte) this.mMaxFragmentLengthOffered))
                {
                    throw new TlsFatalAlert(0x2f);
                }
                this.mTruncatedHMacOffered = TlsExtensionsUtilities.HasTruncatedHMacExtension(clientExtensions);
                this.mSupportedSignatureAlgorithms = TlsUtilities.GetSignatureAlgorithmsExtension(clientExtensions);
                if ((this.mSupportedSignatureAlgorithms != null) && !TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(this.mClientVersion))
                {
                    throw new TlsFatalAlert(0x2f);
                }
                this.mNamedCurves = TlsEccUtilities.GetSupportedEllipticCurvesExtension(clientExtensions);
                this.mClientECPointFormats = TlsEccUtilities.GetSupportedPointFormatsExtension(clientExtensions);
            }
        }

        public virtual void ProcessClientSupplementalData(IList clientSupplementalData)
        {
            if (clientSupplementalData != null)
            {
                throw new TlsFatalAlert(10);
            }
        }

        protected virtual bool SupportsClientEccCapabilities(int[] namedCurves, byte[] ecPointFormats)
        {
            if (namedCurves == null)
            {
                return TlsEccUtilities.HasAnySupportedNamedCurves();
            }
            for (int i = 0; i < namedCurves.Length; i++)
            {
                int namedCurve = namedCurves[i];
                if (NamedCurve.IsValid(namedCurve) && (!NamedCurve.RefersToASpecificNamedCurve(namedCurve) || TlsEccUtilities.IsSupportedNamedCurve(namedCurve)))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual bool AllowEncryptThenMac =>
            true;

        protected virtual bool AllowTruncatedHMac =>
            false;

        protected virtual ProtocolVersion MaximumVersion =>
            ProtocolVersion.TLSv11;

        protected virtual ProtocolVersion MinimumVersion =>
            ProtocolVersion.TLSv10;
    }
}

