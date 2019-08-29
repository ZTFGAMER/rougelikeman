namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;

    public class TlsClientProtocol : TlsProtocol
    {
        protected TlsClient mTlsClient;
        internal TlsClientContextImpl mTlsClientContext;
        protected byte[] mSelectedSessionID;
        protected TlsKeyExchange mKeyExchange;
        protected TlsAuthentication mAuthentication;
        protected CertificateStatus mCertificateStatus;
        protected CertificateRequest mCertificateRequest;

        public TlsClientProtocol(SecureRandom secureRandom) : base(secureRandom)
        {
        }

        public TlsClientProtocol(Stream stream, SecureRandom secureRandom) : base(stream, secureRandom)
        {
        }

        public TlsClientProtocol(Stream input, Stream output, SecureRandom secureRandom) : base(input, output, secureRandom)
        {
        }

        protected override void CleanupHandshake()
        {
            base.CleanupHandshake();
            this.mSelectedSessionID = null;
            this.mKeyExchange = null;
            this.mAuthentication = null;
            this.mCertificateStatus = null;
            this.mCertificateRequest = null;
        }

        public virtual void Connect(TlsClient tlsClient)
        {
            if (tlsClient == null)
            {
                throw new ArgumentNullException("tlsClient");
            }
            if (this.mTlsClient != null)
            {
                throw new InvalidOperationException("'Connect' can only be called once");
            }
            this.mTlsClient = tlsClient;
            base.mSecurityParameters = new SecurityParameters();
            base.mSecurityParameters.entity = 1;
            this.mTlsClientContext = new TlsClientContextImpl(base.mSecureRandom, base.mSecurityParameters);
            base.mSecurityParameters.clientRandom = TlsProtocol.CreateRandomBlock(tlsClient.ShouldUseGmtUnixTime(), this.mTlsClientContext.NonceRandomGenerator);
            this.mTlsClient.Init(this.mTlsClientContext);
            base.mRecordStream.Init(this.mTlsClientContext);
            TlsSession sessionToResume = tlsClient.GetSessionToResume();
            if ((sessionToResume != null) && sessionToResume.IsResumable)
            {
                SessionParameters parameters = sessionToResume.ExportSessionParameters();
                if (parameters != null)
                {
                    base.mTlsSession = sessionToResume;
                    base.mSessionParameters = parameters;
                }
            }
            this.SendClientHelloMessage();
            base.mConnectionState = 1;
            this.BlockForHandshake();
        }

        protected override void HandleHandshakeMessage(byte type, byte[] data)
        {
            MemoryStream buf = new MemoryStream(data, false);
            if (base.mResumedSession)
            {
                if ((type != 20) || (base.mConnectionState != 2))
                {
                    throw new TlsFatalAlert(10);
                }
                this.ProcessFinishedMessage(buf);
                base.mConnectionState = 15;
                this.SendFinishedMessage();
                base.mConnectionState = 13;
                base.mConnectionState = 0x10;
                this.CompleteHandshake();
            }
            else
            {
                switch (type)
                {
                    case 0:
                        TlsProtocol.AssertEmpty(buf);
                        if (base.mConnectionState == 0x10)
                        {
                            this.RefuseRenegotiation();
                        }
                        return;

                    case 2:
                        if (base.mConnectionState != 1)
                        {
                            throw new TlsFatalAlert(10);
                        }
                        this.ReceiveServerHelloMessage(buf);
                        base.mConnectionState = 2;
                        base.mRecordStream.NotifyHelloComplete();
                        this.ApplyMaxFragmentLengthExtension();
                        if (base.mResumedSession)
                        {
                            base.mSecurityParameters.masterSecret = Arrays.Clone(base.mSessionParameters.MasterSecret);
                            base.mRecordStream.SetPendingConnectionState(this.Peer.GetCompression(), this.Peer.GetCipher());
                            this.SendChangeCipherSpecMessage();
                        }
                        else
                        {
                            this.InvalidateSession();
                            if (this.mSelectedSessionID.Length > 0)
                            {
                                base.mTlsSession = new TlsSessionImpl(this.mSelectedSessionID, null);
                            }
                        }
                        return;

                    case 4:
                        if (base.mConnectionState != 13)
                        {
                            throw new TlsFatalAlert(10);
                        }
                        if (!base.mExpectSessionTicket)
                        {
                            throw new TlsFatalAlert(10);
                        }
                        this.InvalidateSession();
                        this.ReceiveNewSessionTicketMessage(buf);
                        base.mConnectionState = 14;
                        return;

                    case 11:
                        switch (base.mConnectionState)
                        {
                            case 2:
                            case 3:
                                if (base.mConnectionState == 2)
                                {
                                    this.HandleSupplementalData(null);
                                }
                                base.mPeerCertificate = Certificate.Parse(buf);
                                TlsProtocol.AssertEmpty(buf);
                                if ((base.mPeerCertificate == null) || base.mPeerCertificate.IsEmpty)
                                {
                                    base.mAllowCertificateStatus = false;
                                }
                                this.mKeyExchange.ProcessServerCertificate(base.mPeerCertificate);
                                this.mAuthentication = this.mTlsClient.GetAuthentication();
                                this.mAuthentication.NotifyServerCertificate(base.mPeerCertificate);
                                base.mConnectionState = 4;
                                return;
                        }
                        throw new TlsFatalAlert(10);

                    case 12:
                        switch (base.mConnectionState)
                        {
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                                if (base.mConnectionState < 3)
                                {
                                    this.HandleSupplementalData(null);
                                }
                                if (base.mConnectionState < 4)
                                {
                                    this.mKeyExchange.SkipServerCredentials();
                                    this.mAuthentication = null;
                                }
                                this.mKeyExchange.ProcessServerKeyExchange(buf);
                                TlsProtocol.AssertEmpty(buf);
                                base.mConnectionState = 6;
                                return;
                        }
                        throw new TlsFatalAlert(10);

                    case 13:
                        switch (base.mConnectionState)
                        {
                            case 4:
                            case 5:
                            case 6:
                                if (base.mConnectionState != 6)
                                {
                                    this.mKeyExchange.SkipServerKeyExchange();
                                }
                                if (this.mAuthentication == null)
                                {
                                    throw new TlsFatalAlert(40);
                                }
                                this.mCertificateRequest = CertificateRequest.Parse(this.Context, buf);
                                TlsProtocol.AssertEmpty(buf);
                                this.mKeyExchange.ValidateCertificateRequest(this.mCertificateRequest);
                                TlsUtilities.TrackHashAlgorithms(base.mRecordStream.HandshakeHash, this.mCertificateRequest.SupportedSignatureAlgorithms);
                                base.mConnectionState = 7;
                                return;
                        }
                        throw new TlsFatalAlert(10);

                    case 14:
                        switch (base.mConnectionState)
                        {
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            {
                                if (base.mConnectionState < 3)
                                {
                                    this.HandleSupplementalData(null);
                                }
                                if (base.mConnectionState < 4)
                                {
                                    this.mKeyExchange.SkipServerCredentials();
                                    this.mAuthentication = null;
                                }
                                if (base.mConnectionState < 6)
                                {
                                    this.mKeyExchange.SkipServerKeyExchange();
                                }
                                TlsProtocol.AssertEmpty(buf);
                                base.mConnectionState = 8;
                                base.mRecordStream.HandshakeHash.SealHashAlgorithms();
                                IList clientSupplementalData = this.mTlsClient.GetClientSupplementalData();
                                if (clientSupplementalData != null)
                                {
                                    this.SendSupplementalDataMessage(clientSupplementalData);
                                }
                                base.mConnectionState = 9;
                                TlsCredentials clientCredentials = null;
                                if (this.mCertificateRequest == null)
                                {
                                    this.mKeyExchange.SkipClientCredentials();
                                }
                                else
                                {
                                    clientCredentials = this.mAuthentication.GetClientCredentials(this.Context, this.mCertificateRequest);
                                    if (clientCredentials == null)
                                    {
                                        this.mKeyExchange.SkipClientCredentials();
                                        this.SendCertificateMessage(Certificate.EmptyChain);
                                    }
                                    else
                                    {
                                        this.mKeyExchange.ProcessClientCredentials(clientCredentials);
                                        this.SendCertificateMessage(clientCredentials.Certificate);
                                    }
                                }
                                base.mConnectionState = 10;
                                this.SendClientKeyExchangeMessage();
                                base.mConnectionState = 11;
                                TlsHandshakeHash handshakeHash = base.mRecordStream.PrepareToFinish();
                                base.mSecurityParameters.sessionHash = TlsProtocol.GetCurrentPrfHash(this.Context, handshakeHash, null);
                                TlsProtocol.EstablishMasterSecret(this.Context, this.mKeyExchange);
                                base.mRecordStream.SetPendingConnectionState(this.Peer.GetCompression(), this.Peer.GetCipher());
                                if ((clientCredentials != null) && (clientCredentials is TlsSignerCredentials))
                                {
                                    byte[] sessionHash;
                                    TlsSignerCredentials signerCredentials = (TlsSignerCredentials) clientCredentials;
                                    SignatureAndHashAlgorithm signatureAndHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm(this.Context, signerCredentials);
                                    if (signatureAndHashAlgorithm == null)
                                    {
                                        sessionHash = base.mSecurityParameters.SessionHash;
                                    }
                                    else
                                    {
                                        sessionHash = handshakeHash.GetFinalHash(signatureAndHashAlgorithm.Hash);
                                    }
                                    byte[] signature = signerCredentials.GenerateCertificateSignature(sessionHash);
                                    DigitallySigned certificateVerify = new DigitallySigned(signatureAndHashAlgorithm, signature);
                                    this.SendCertificateVerifyMessage(certificateVerify);
                                    base.mConnectionState = 12;
                                }
                                this.SendChangeCipherSpecMessage();
                                this.SendFinishedMessage();
                                base.mConnectionState = 13;
                                return;
                            }
                        }
                        throw new TlsFatalAlert(40);

                    case 20:
                        switch (base.mConnectionState)
                        {
                            case 13:
                            case 14:
                                if ((base.mConnectionState == 13) && base.mExpectSessionTicket)
                                {
                                    throw new TlsFatalAlert(10);
                                }
                                this.ProcessFinishedMessage(buf);
                                base.mConnectionState = 15;
                                base.mConnectionState = 0x10;
                                this.CompleteHandshake();
                                return;
                        }
                        throw new TlsFatalAlert(10);

                    case 0x16:
                        if (base.mConnectionState != 4)
                        {
                            throw new TlsFatalAlert(10);
                        }
                        if (!base.mAllowCertificateStatus)
                        {
                            throw new TlsFatalAlert(10);
                        }
                        this.mCertificateStatus = CertificateStatus.Parse(buf);
                        TlsProtocol.AssertEmpty(buf);
                        base.mConnectionState = 5;
                        return;

                    case 0x17:
                        if (base.mConnectionState != 2)
                        {
                            throw new TlsFatalAlert(10);
                        }
                        this.HandleSupplementalData(TlsProtocol.ReadSupplementalDataMessage(buf));
                        return;
                }
                throw new TlsFatalAlert(10);
            }
        }

        protected virtual void HandleSupplementalData(IList serverSupplementalData)
        {
            this.mTlsClient.ProcessServerSupplementalData(serverSupplementalData);
            base.mConnectionState = 3;
            this.mKeyExchange = this.mTlsClient.GetKeyExchange();
            this.mKeyExchange.Init(this.Context);
        }

        protected virtual void ReceiveNewSessionTicketMessage(MemoryStream buf)
        {
            NewSessionTicket newSessionTicket = NewSessionTicket.Parse(buf);
            TlsProtocol.AssertEmpty(buf);
            this.mTlsClient.NotifyNewSessionTicket(newSessionTicket);
        }

        protected virtual void ReceiveServerHelloMessage(MemoryStream buf)
        {
            ProtocolVersion writeVersion = TlsUtilities.ReadVersion(buf);
            if (writeVersion.IsDtls)
            {
                throw new TlsFatalAlert(0x2f);
            }
            if (!writeVersion.Equals(base.mRecordStream.ReadVersion))
            {
                throw new TlsFatalAlert(0x2f);
            }
            ProtocolVersion clientVersion = this.Context.ClientVersion;
            if (!writeVersion.IsEqualOrEarlierVersionOf(clientVersion))
            {
                throw new TlsFatalAlert(0x2f);
            }
            base.mRecordStream.SetWriteVersion(writeVersion);
            this.ContextAdmin.SetServerVersion(writeVersion);
            this.mTlsClient.NotifyServerVersion(writeVersion);
            base.mSecurityParameters.serverRandom = TlsUtilities.ReadFully(0x20, buf);
            this.mSelectedSessionID = TlsUtilities.ReadOpaque8(buf);
            if (this.mSelectedSessionID.Length > 0x20)
            {
                throw new TlsFatalAlert(0x2f);
            }
            this.mTlsClient.NotifySessionID(this.mSelectedSessionID);
            base.mResumedSession = ((this.mSelectedSessionID.Length > 0) && (base.mTlsSession != null)) && Arrays.AreEqual(this.mSelectedSessionID, base.mTlsSession.SessionID);
            int n = TlsUtilities.ReadUint16(buf);
            if ((!Arrays.Contains(base.mOfferedCipherSuites, n) || (n == 0)) || (CipherSuite.IsScsv(n) || !TlsUtilities.IsValidCipherSuiteForVersion(n, this.Context.ServerVersion)))
            {
                throw new TlsFatalAlert(0x2f);
            }
            this.mTlsClient.NotifySelectedCipherSuite(n);
            byte num2 = TlsUtilities.ReadUint8(buf);
            if (!Arrays.Contains(base.mOfferedCompressionMethods, num2))
            {
                throw new TlsFatalAlert(0x2f);
            }
            this.mTlsClient.NotifySelectedCompressionMethod(num2);
            base.mServerExtensions = TlsProtocol.ReadExtensions(buf);
            if (base.mServerExtensions != null)
            {
                IEnumerator enumerator = base.mServerExtensions.Keys.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        int current = (int) enumerator.Current;
                        if (current != 0xff01)
                        {
                            if (TlsUtilities.GetExtensionData(base.mClientExtensions, current) == null)
                            {
                                throw new TlsFatalAlert(110);
                            }
                            if (base.mResumedSession)
                            {
                            }
                        }
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
            byte[] extensionData = TlsUtilities.GetExtensionData(base.mServerExtensions, 0xff01);
            if (extensionData != null)
            {
                base.mSecureRenegotiation = true;
                if (!Arrays.ConstantTimeAreEqual(extensionData, TlsProtocol.CreateRenegotiationInfo(TlsUtilities.EmptyBytes)))
                {
                    throw new TlsFatalAlert(40);
                }
            }
            this.mTlsClient.NotifySecureRenegotiation(base.mSecureRenegotiation);
            IDictionary mClientExtensions = base.mClientExtensions;
            IDictionary mServerExtensions = base.mServerExtensions;
            if (base.mResumedSession)
            {
                if ((n != base.mSessionParameters.CipherSuite) || (num2 != base.mSessionParameters.CompressionAlgorithm))
                {
                    throw new TlsFatalAlert(0x2f);
                }
                mClientExtensions = null;
                mServerExtensions = base.mSessionParameters.ReadServerExtensions();
            }
            base.mSecurityParameters.cipherSuite = n;
            base.mSecurityParameters.compressionAlgorithm = num2;
            if (mServerExtensions != null)
            {
                bool flag = TlsExtensionsUtilities.HasEncryptThenMacExtension(mServerExtensions);
                if (flag && !TlsUtilities.IsBlockCipherSuite(n))
                {
                    throw new TlsFatalAlert(0x2f);
                }
                base.mSecurityParameters.encryptThenMac = flag;
                base.mSecurityParameters.extendedMasterSecret = TlsExtensionsUtilities.HasExtendedMasterSecretExtension(mServerExtensions);
                base.mSecurityParameters.maxFragmentLength = this.ProcessMaxFragmentLengthExtension(mClientExtensions, mServerExtensions, 0x2f);
                base.mSecurityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension(mServerExtensions);
                base.mAllowCertificateStatus = !base.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(mServerExtensions, 5, 0x2f);
                base.mExpectSessionTicket = !base.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(mServerExtensions, 0x23, 0x2f);
            }
            if (mClientExtensions != null)
            {
                this.mTlsClient.ProcessServerExtensions(mServerExtensions);
            }
            base.mSecurityParameters.prfAlgorithm = TlsProtocol.GetPrfAlgorithm(this.Context, base.mSecurityParameters.CipherSuite);
            base.mSecurityParameters.verifyDataLength = 12;
        }

        protected virtual void SendCertificateVerifyMessage(DigitallySigned certificateVerify)
        {
            TlsProtocol.HandshakeMessage output = new TlsProtocol.HandshakeMessage(15);
            certificateVerify.Encode(output);
            output.WriteToRecordStream(this);
        }

        protected virtual void SendClientHelloMessage()
        {
            base.mRecordStream.SetWriteVersion(this.mTlsClient.ClientHelloRecordLayerVersion);
            ProtocolVersion clientVersion = this.mTlsClient.ClientVersion;
            if (clientVersion.IsDtls)
            {
                throw new TlsFatalAlert(80);
            }
            this.ContextAdmin.SetClientVersion(clientVersion);
            byte[] emptyBytes = TlsUtilities.EmptyBytes;
            if (base.mTlsSession != null)
            {
                emptyBytes = base.mTlsSession.SessionID;
                if ((emptyBytes == null) || (emptyBytes.Length > 0x20))
                {
                    emptyBytes = TlsUtilities.EmptyBytes;
                }
            }
            bool isFallback = this.mTlsClient.IsFallback;
            base.mOfferedCipherSuites = this.mTlsClient.GetCipherSuites();
            base.mOfferedCompressionMethods = this.mTlsClient.GetCompressionMethods();
            if (((emptyBytes.Length > 0) && (base.mSessionParameters != null)) && (!Arrays.Contains(base.mOfferedCipherSuites, base.mSessionParameters.CipherSuite) || !Arrays.Contains(base.mOfferedCompressionMethods, base.mSessionParameters.CompressionAlgorithm)))
            {
                emptyBytes = TlsUtilities.EmptyBytes;
            }
            base.mClientExtensions = this.mTlsClient.GetClientExtensions();
            TlsProtocol.HandshakeMessage output = new TlsProtocol.HandshakeMessage(1);
            TlsUtilities.WriteVersion(clientVersion, output);
            output.Write(base.mSecurityParameters.ClientRandom);
            TlsUtilities.WriteOpaque8(emptyBytes, output);
            byte[] extensionData = TlsUtilities.GetExtensionData(base.mClientExtensions, 0xff01);
            bool flag2 = null == extensionData;
            bool flag3 = !Arrays.Contains(base.mOfferedCipherSuites, 0xff);
            if (flag2 && flag3)
            {
                base.mOfferedCipherSuites = Arrays.Append(base.mOfferedCipherSuites, 0xff);
            }
            if (isFallback && !Arrays.Contains(base.mOfferedCipherSuites, 0x5600))
            {
                base.mOfferedCipherSuites = Arrays.Append(base.mOfferedCipherSuites, 0x5600);
            }
            TlsUtilities.WriteUint16ArrayWithUint16Length(base.mOfferedCipherSuites, output);
            TlsUtilities.WriteUint8ArrayWithUint8Length(base.mOfferedCompressionMethods, output);
            if (base.mClientExtensions != null)
            {
                TlsProtocol.WriteExtensions(output, base.mClientExtensions);
            }
            output.WriteToRecordStream(this);
        }

        protected virtual void SendClientKeyExchangeMessage()
        {
            TlsProtocol.HandshakeMessage output = new TlsProtocol.HandshakeMessage(0x10);
            this.mKeyExchange.GenerateClientKeyExchange(output);
            output.WriteToRecordStream(this);
        }

        protected override TlsContext Context =>
            this.mTlsClientContext;

        internal override AbstractTlsContext ContextAdmin =>
            this.mTlsClientContext;

        protected override TlsPeer Peer =>
            this.mTlsClient;
    }
}

