namespace Org.BouncyCastle.Crypto.Tls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public abstract class AbstractTlsClient : AbstractTlsPeer, TlsClient, TlsPeer
    {
        protected TlsCipherFactory mCipherFactory;
        protected TlsClientContext mContext;
        protected IList mSupportedSignatureAlgorithms;
        protected int[] mNamedCurves;
        protected byte[] mClientECPointFormats;
        protected byte[] mServerECPointFormats;
        protected int mSelectedCipherSuite;
        protected short mSelectedCompressionMethod;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> <HostNames>k__BackingField;

        public AbstractTlsClient() : this(new DefaultTlsCipherFactory())
        {
        }

        public AbstractTlsClient(TlsCipherFactory cipherFactory)
        {
            this.mCipherFactory = cipherFactory;
        }

        protected virtual bool AllowUnexpectedServerExtension(int extensionType, byte[] extensionData)
        {
            if (extensionType != 10)
            {
                return false;
            }
            TlsEccUtilities.ReadSupportedEllipticCurvesExtension(extensionData);
            return true;
        }

        protected virtual void CheckForUnexpectedServerExtension(IDictionary serverExtensions, int extensionType)
        {
            byte[] extensionData = TlsUtilities.GetExtensionData(serverExtensions, extensionType);
            if ((extensionData != null) && !this.AllowUnexpectedServerExtension(extensionType, extensionData))
            {
                throw new TlsFatalAlert(0x2f);
            }
        }

        public abstract TlsAuthentication GetAuthentication();
        public override TlsCipher GetCipher()
        {
            int encryptionAlgorithm = TlsUtilities.GetEncryptionAlgorithm(this.mSelectedCipherSuite);
            int macAlgorithm = TlsUtilities.GetMacAlgorithm(this.mSelectedCipherSuite);
            return this.mCipherFactory.CreateCipher(this.mContext, encryptionAlgorithm, macAlgorithm);
        }

        public abstract int[] GetCipherSuites();
        public virtual IDictionary GetClientExtensions()
        {
            IDictionary extensions = null;
            if (TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(this.mContext.ClientVersion))
            {
                this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultSupportedSignatureAlgorithms();
                extensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(extensions);
                TlsUtilities.AddSignatureAlgorithmsExtension(extensions, this.mSupportedSignatureAlgorithms);
            }
            if (TlsEccUtilities.ContainsEccCipherSuites(this.GetCipherSuites()))
            {
                this.mNamedCurves = new int[] { 0x17, 0x18 };
                byte[] buffer1 = new byte[3];
                buffer1[1] = 1;
                buffer1[2] = 2;
                this.mClientECPointFormats = buffer1;
                extensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(extensions);
                TlsEccUtilities.AddSupportedEllipticCurvesExtension(extensions, this.mNamedCurves);
                TlsEccUtilities.AddSupportedPointFormatsExtension(extensions, this.mClientECPointFormats);
            }
            if ((this.HostNames != null) && (this.HostNames.Count > 0))
            {
                List<ServerName> serverNameList = new List<ServerName>(this.HostNames.Count);
                for (int i = 0; i < this.HostNames.Count; i++)
                {
                    serverNameList.Add(new ServerName(0, this.HostNames[i]));
                }
                TlsExtensionsUtilities.AddServerNameExtension(extensions, new ServerNameList(serverNameList));
            }
            return extensions;
        }

        public virtual IList GetClientSupplementalData() => 
            null;

        public override TlsCompression GetCompression()
        {
            switch (this.mSelectedCompressionMethod)
            {
                case 0:
                    return new TlsNullCompression();

                case 1:
                    return new TlsDeflateCompression();
            }
            throw new TlsFatalAlert(80);
        }

        public virtual byte[] GetCompressionMethods() => 
            new byte[1];

        public abstract TlsKeyExchange GetKeyExchange();
        public virtual TlsSession GetSessionToResume() => 
            null;

        public virtual void Init(TlsClientContext context)
        {
            this.mContext = context;
        }

        public virtual void NotifyNewSessionTicket(NewSessionTicket newSessionTicket)
        {
        }

        public virtual void NotifySelectedCipherSuite(int selectedCipherSuite)
        {
            this.mSelectedCipherSuite = selectedCipherSuite;
        }

        public virtual void NotifySelectedCompressionMethod(byte selectedCompressionMethod)
        {
            this.mSelectedCompressionMethod = selectedCompressionMethod;
        }

        public virtual void NotifyServerVersion(ProtocolVersion serverVersion)
        {
            if (!this.MinimumVersion.IsEqualOrEarlierVersionOf(serverVersion))
            {
                throw new TlsFatalAlert(70);
            }
        }

        public virtual void NotifySessionID(byte[] sessionID)
        {
        }

        public virtual void ProcessServerExtensions(IDictionary serverExtensions)
        {
            if (serverExtensions != null)
            {
                this.CheckForUnexpectedServerExtension(serverExtensions, 13);
                this.CheckForUnexpectedServerExtension(serverExtensions, 10);
                if (TlsEccUtilities.IsEccCipherSuite(this.mSelectedCipherSuite))
                {
                    this.mServerECPointFormats = TlsEccUtilities.GetSupportedPointFormatsExtension(serverExtensions);
                }
                else
                {
                    this.CheckForUnexpectedServerExtension(serverExtensions, 11);
                }
                this.CheckForUnexpectedServerExtension(serverExtensions, 0x15);
            }
        }

        public virtual void ProcessServerSupplementalData(IList serverSupplementalData)
        {
            if (serverSupplementalData != null)
            {
                throw new TlsFatalAlert(10);
            }
        }

        public List<string> HostNames { get; set; }

        public virtual ProtocolVersion ClientHelloRecordLayerVersion =>
            this.ClientVersion;

        public virtual ProtocolVersion ClientVersion =>
            ProtocolVersion.TLSv12;

        public virtual bool IsFallback =>
            false;

        public virtual ProtocolVersion MinimumVersion =>
            ProtocolVersion.TLSv10;
    }
}

