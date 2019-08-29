namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Prng;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class TlsProtocol
    {
        private static readonly string TLS_ERROR_MESSAGE = "Internal TLS error, this could be an attack";
        protected const short CS_START = 0;
        protected const short CS_CLIENT_HELLO = 1;
        protected const short CS_SERVER_HELLO = 2;
        protected const short CS_SERVER_SUPPLEMENTAL_DATA = 3;
        protected const short CS_SERVER_CERTIFICATE = 4;
        protected const short CS_CERTIFICATE_STATUS = 5;
        protected const short CS_SERVER_KEY_EXCHANGE = 6;
        protected const short CS_CERTIFICATE_REQUEST = 7;
        protected const short CS_SERVER_HELLO_DONE = 8;
        protected const short CS_CLIENT_SUPPLEMENTAL_DATA = 9;
        protected const short CS_CLIENT_CERTIFICATE = 10;
        protected const short CS_CLIENT_KEY_EXCHANGE = 11;
        protected const short CS_CERTIFICATE_VERIFY = 12;
        protected const short CS_CLIENT_FINISHED = 13;
        protected const short CS_SERVER_SESSION_TICKET = 14;
        protected const short CS_SERVER_FINISHED = 15;
        protected const short CS_END = 0x10;
        protected const short ADS_MODE_1_Nsub1 = 0;
        protected const short ADS_MODE_0_N = 1;
        protected const short ADS_MODE_0_N_FIRSTONLY = 2;
        private ByteQueue mApplicationDataQueue;
        private ByteQueue mAlertQueue;
        private ByteQueue mHandshakeQueue;
        internal RecordStream mRecordStream;
        protected SecureRandom mSecureRandom;
        private TlsStream mTlsStream;
        private volatile bool mClosed;
        private volatile bool mFailedWithError;
        private volatile bool mAppDataReady;
        private volatile bool mAppDataSplitEnabled;
        private volatile int mAppDataSplitMode;
        private byte[] mExpectedVerifyData;
        protected TlsSession mTlsSession;
        protected SessionParameters mSessionParameters;
        protected SecurityParameters mSecurityParameters;
        protected Certificate mPeerCertificate;
        protected int[] mOfferedCipherSuites;
        protected byte[] mOfferedCompressionMethods;
        protected IDictionary mClientExtensions;
        protected IDictionary mServerExtensions;
        protected short mConnectionState;
        protected bool mResumedSession;
        protected bool mReceivedChangeCipherSpec;
        protected bool mSecureRenegotiation;
        protected bool mAllowCertificateStatus;
        protected bool mExpectSessionTicket;
        protected bool mBlocking;
        protected ByteQueueStream mInputBuffers;
        protected ByteQueueStream mOutputBuffer;

        public TlsProtocol(SecureRandom secureRandom)
        {
            this.mApplicationDataQueue = new ByteQueue();
            this.mAlertQueue = new ByteQueue(2);
            this.mHandshakeQueue = new ByteQueue();
            this.mAppDataSplitEnabled = true;
            this.mBlocking = true;
            this.mBlocking = false;
            this.mInputBuffers = new ByteQueueStream();
            this.mOutputBuffer = new ByteQueueStream();
            this.mRecordStream = new RecordStream(this, this.mInputBuffers, this.mOutputBuffer);
            this.mSecureRandom = secureRandom;
        }

        public TlsProtocol(System.IO.Stream stream, SecureRandom secureRandom) : this(stream, stream, secureRandom)
        {
        }

        public TlsProtocol(System.IO.Stream input, System.IO.Stream output, SecureRandom secureRandom)
        {
            this.mApplicationDataQueue = new ByteQueue();
            this.mAlertQueue = new ByteQueue(2);
            this.mHandshakeQueue = new ByteQueue();
            this.mAppDataSplitEnabled = true;
            this.mBlocking = true;
            this.mRecordStream = new RecordStream(this, input, output);
            this.mSecureRandom = secureRandom;
        }

        protected internal virtual int ApplicationDataAvailable() => 
            this.mApplicationDataQueue.Available;

        protected virtual void ApplyMaxFragmentLengthExtension()
        {
            if (this.mSecurityParameters.maxFragmentLength >= 0)
            {
                if (!MaxFragmentLength.IsValid((byte) this.mSecurityParameters.maxFragmentLength))
                {
                    throw new TlsFatalAlert(80);
                }
                int plaintextLimit = ((int) 1) << (8 + this.mSecurityParameters.maxFragmentLength);
                this.mRecordStream.SetPlaintextLimit(plaintextLimit);
            }
        }

        protected internal static void AssertEmpty(MemoryStream buf)
        {
            if (buf.Position < buf.Length)
            {
                throw new TlsFatalAlert(50);
            }
        }

        protected virtual void BlockForHandshake()
        {
            if (this.mBlocking)
            {
                while (this.mConnectionState != 0x10)
                {
                    if (this.mClosed)
                    {
                    }
                    this.SafeReadRecord();
                }
            }
        }

        protected virtual void CheckReceivedChangeCipherSpec(bool expected)
        {
            if (expected != this.mReceivedChangeCipherSpec)
            {
                throw new TlsFatalAlert(10);
            }
        }

        protected virtual void CleanupHandshake()
        {
            if (this.mExpectedVerifyData != null)
            {
                Arrays.Fill(this.mExpectedVerifyData, 0);
                this.mExpectedVerifyData = null;
            }
            this.mSecurityParameters.Clear();
            this.mPeerCertificate = null;
            this.mOfferedCipherSuites = null;
            this.mOfferedCompressionMethods = null;
            this.mClientExtensions = null;
            this.mServerExtensions = null;
            this.mResumedSession = false;
            this.mReceivedChangeCipherSpec = false;
            this.mSecureRenegotiation = false;
            this.mAllowCertificateStatus = false;
            this.mExpectSessionTicket = false;
        }

        public virtual void Close()
        {
            this.HandleClose(true);
        }

        protected virtual void CompleteHandshake()
        {
            try
            {
                this.mRecordStream.FinaliseHandshake();
                this.mAppDataSplitEnabled = !TlsUtilities.IsTlsV11(this.Context);
                if (!this.mAppDataReady)
                {
                    this.mAppDataReady = true;
                    if (this.mBlocking)
                    {
                        this.mTlsStream = new TlsStream(this);
                    }
                }
                if (this.mTlsSession != null)
                {
                    if (this.mSessionParameters == null)
                    {
                        this.mSessionParameters = new SessionParameters.Builder().SetCipherSuite(this.mSecurityParameters.CipherSuite).SetCompressionAlgorithm(this.mSecurityParameters.CompressionAlgorithm).SetMasterSecret(this.mSecurityParameters.MasterSecret).SetPeerCertificate(this.mPeerCertificate).SetPskIdentity(this.mSecurityParameters.PskIdentity).SetSrpIdentity(this.mSecurityParameters.SrpIdentity).SetServerExtensions(this.mServerExtensions).Build();
                        this.mTlsSession = new TlsSessionImpl(this.mTlsSession.SessionID, this.mSessionParameters);
                    }
                    this.ContextAdmin.SetResumableSession(this.mTlsSession);
                }
                this.Peer.NotifyHandshakeComplete();
            }
            finally
            {
                this.CleanupHandshake();
            }
        }

        protected internal static byte[] CreateRandomBlock(bool useGmtUnixTime, IRandomGenerator randomGenerator)
        {
            byte[] bytes = new byte[0x20];
            randomGenerator.NextBytes(bytes);
            if (useGmtUnixTime)
            {
                TlsUtilities.WriteGmtUnixTime(bytes, 0);
            }
            return bytes;
        }

        protected internal static byte[] CreateRenegotiationInfo(byte[] renegotiated_connection) => 
            TlsUtilities.EncodeOpaque8(renegotiated_connection);

        protected virtual byte[] CreateVerifyData(bool isServer)
        {
            TlsContext context = this.Context;
            string asciiLabel = !isServer ? "client finished" : "server finished";
            byte[] sslSender = !isServer ? TlsUtilities.SSL_CLIENT : TlsUtilities.SSL_SERVER;
            byte[] handshakeHash = GetCurrentPrfHash(context, this.mRecordStream.HandshakeHash, sslSender);
            return TlsUtilities.CalculateVerifyData(context, asciiLabel, handshakeHash);
        }

        protected internal static void EstablishMasterSecret(TlsContext context, TlsKeyExchange keyExchange)
        {
            byte[] buffer = keyExchange.GeneratePremasterSecret();
            try
            {
                context.SecurityParameters.masterSecret = TlsUtilities.CalculateMasterSecret(context, buffer);
            }
            finally
            {
                if (buffer != null)
                {
                    Arrays.Fill(buffer, 0);
                }
            }
        }

        protected virtual void FailWithError(byte alertLevel, byte alertDescription, string message, Exception cause)
        {
            if (!this.mClosed)
            {
                this.mClosed = true;
                if (alertLevel == 2)
                {
                    this.InvalidateSession();
                    this.mFailedWithError = true;
                }
                this.RaiseAlert(alertLevel, alertDescription, message, cause);
                this.mRecordStream.SafeClose();
                if (alertLevel != 2)
                {
                    return;
                }
            }
            throw new IOException(TLS_ERROR_MESSAGE);
        }

        protected internal virtual void Flush()
        {
            this.mRecordStream.Flush();
        }

        public virtual int GetAvailableInputBytes()
        {
            if (this.mBlocking)
            {
                throw new InvalidOperationException("Cannot use GetAvailableInputBytes() in blocking mode! Use ApplicationDataAvailable() instead.");
            }
            return this.ApplicationDataAvailable();
        }

        public virtual int GetAvailableOutputBytes()
        {
            if (this.mBlocking)
            {
                throw new InvalidOperationException("Cannot use GetAvailableOutputBytes() in blocking mode! Use Stream instead.");
            }
            return this.mOutputBuffer.Available;
        }

        protected internal static byte[] GetCurrentPrfHash(TlsContext context, TlsHandshakeHash handshakeHash, byte[] sslSender)
        {
            IDigest digest = handshakeHash.ForkPrfHash();
            if ((sslSender != null) && TlsUtilities.IsSsl(context))
            {
                digest.BlockUpdate(sslSender, 0, sslSender.Length);
            }
            return DigestUtilities.DoFinal(digest);
        }

        protected internal static int GetPrfAlgorithm(TlsContext context, int ciphersuite)
        {
            bool flag = TlsUtilities.IsTlsV12(context);
            switch (ciphersuite)
            {
                case 0xc072:
                case 0xc074:
                case 0xc076:
                case 0xc078:
                case 0xc07a:
                case 0xc07c:
                case 0xc07e:
                case 0xc080:
                case 0xc082:
                case 0xc084:
                case 0xc086:
                case 0xc088:
                case 0xc08a:
                case 0xc08c:
                case 0xc08e:
                case 0xc090:
                case 0xc092:
                case 0xc09c:
                case 0xc09d:
                case 0xc09e:
                case 0xc09f:
                case 0xc0a0:
                case 0xc0a1:
                case 0xc0a2:
                case 0xc0a3:
                case 0xc0a4:
                case 0xc0a5:
                case 0xc0a6:
                case 0xc0a7:
                case 0xc0a8:
                case 0xc0a9:
                case 0xc0aa:
                case 0xc0ab:
                case 0xc0ac:
                case 0xc0ad:
                case 0xc0ae:
                case 0xc0af:
                case 0x9c:
                case 0x9e:
                case 160:
                case 0xa2:
                case 0xa4:
                case 0xa8:
                case 170:
                case 0xac:
                case 0xba:
                case 0xbb:
                case 0xbc:
                case 0xbd:
                case 190:
                case 0xbf:
                case 0xc0:
                case 0xc1:
                case 0xc2:
                case 0xc3:
                case 0xc4:
                case 0xc5:
                case 0xc023:
                case 0xc025:
                case 0xc027:
                case 0xc029:
                case 0xc02b:
                case 0xc02d:
                case 0xc02f:
                case 0xc031:
                    break;

                case 0xc073:
                case 0xc075:
                case 0xc077:
                case 0xc079:
                case 0xc07b:
                case 0xc07d:
                case 0xc07f:
                case 0xc081:
                case 0xc083:
                case 0xc085:
                case 0xc087:
                case 0xc089:
                case 0xc08b:
                case 0xc08d:
                case 0xc08f:
                case 0xc091:
                case 0xc093:
                case 0x9d:
                case 0x9f:
                case 0xa1:
                case 0xa3:
                case 0xa5:
                case 0xa9:
                case 0xab:
                case 0xad:
                case 0xc024:
                case 0xc026:
                case 0xc028:
                case 0xc02a:
                case 0xc02c:
                case 0xc02e:
                case 0xc030:
                case 0xc032:
                    if (!flag)
                    {
                        throw new TlsFatalAlert(0x2f);
                    }
                    return 2;

                case 0xc095:
                case 0xc097:
                case 0xc099:
                case 0xc09b:
                case 0xaf:
                case 0xb1:
                case 0xb3:
                case 0xb5:
                case 0xb7:
                case 0xb9:
                case 0xc038:
                case 0xc03b:
                    if (flag)
                    {
                        return 2;
                    }
                    return 0;

                default:
                    switch (ciphersuite)
                    {
                        case 0xcca8:
                        case 0xcca9:
                        case 0xccaa:
                        case 0xccab:
                        case 0xccac:
                        case 0xccad:
                        case 0xccae:
                            break;

                        default:
                            if (flag)
                            {
                                return 1;
                            }
                            return 0;
                    }
                    break;
            }
            if (!flag)
            {
                throw new TlsFatalAlert(0x2f);
            }
            return 1;
        }

        protected virtual void HandleChangeCipherSpecMessage()
        {
        }

        protected virtual void HandleClose(bool user_canceled)
        {
            if (!this.mClosed)
            {
                if (user_canceled && !this.mAppDataReady)
                {
                    this.RaiseWarning(90, "User canceled handshake");
                }
                this.FailWithError(1, 0, "Connection closed", null);
            }
        }

        protected abstract void HandleHandshakeMessage(byte type, byte[] buf);
        protected virtual void HandleWarningMessage(byte description)
        {
        }

        protected virtual void InvalidateSession()
        {
            if (this.mSessionParameters != null)
            {
                this.mSessionParameters.Clear();
                this.mSessionParameters = null;
            }
            if (this.mTlsSession != null)
            {
                this.mTlsSession.Invalidate();
                this.mTlsSession = null;
            }
        }

        public virtual void OfferInput(byte[] input)
        {
            if (this.mBlocking)
            {
                throw new InvalidOperationException("Cannot use OfferInput() in blocking mode! Use Stream instead.");
            }
            if (this.mClosed)
            {
                throw new IOException("Connection is closed, cannot accept any more input");
            }
            this.mInputBuffers.Write(input);
            while (this.mInputBuffers.Available >= 5)
            {
                byte[] buf = new byte[5];
                this.mInputBuffers.Peek(buf);
                int num = TlsUtilities.ReadUint16(buf, 3) + 5;
                if (this.mInputBuffers.Available < num)
                {
                    break;
                }
                this.SafeReadRecord();
            }
        }

        public virtual void OfferOutput(byte[] buffer, int offset, int length)
        {
            if (this.mBlocking)
            {
                throw new InvalidOperationException("Cannot use OfferOutput() in blocking mode! Use Stream instead.");
            }
            if (!this.mAppDataReady)
            {
                throw new IOException("Application data cannot be sent until the handshake is complete!");
            }
            this.WriteData(buffer, offset, length);
        }

        private void ProcessAlert()
        {
            while (this.mAlertQueue.Available >= 2)
            {
                byte[] buffer = this.mAlertQueue.RemoveData(2, 0);
                byte alertLevel = buffer[0];
                byte alertDescription = buffer[1];
                this.Peer.NotifyAlertReceived(alertLevel, alertDescription);
                if (alertLevel == 2)
                {
                    this.InvalidateSession();
                    this.mFailedWithError = true;
                    this.mClosed = true;
                    this.mRecordStream.SafeClose();
                    throw new IOException(TLS_ERROR_MESSAGE);
                }
                if (alertDescription == 0)
                {
                    this.HandleClose(false);
                }
                this.HandleWarningMessage(alertDescription);
            }
        }

        private void ProcessApplicationData()
        {
        }

        private void ProcessChangeCipherSpec(byte[] buf, int off, int len)
        {
            for (int i = 0; i < len; i++)
            {
                if (TlsUtilities.ReadUint8(buf, off + i) != 1)
                {
                    throw new TlsFatalAlert(50);
                }
                if ((this.mReceivedChangeCipherSpec || (this.mAlertQueue.Available > 0)) || (this.mHandshakeQueue.Available > 0))
                {
                    throw new TlsFatalAlert(10);
                }
                this.mRecordStream.ReceivedReadCipherSpec();
                this.mReceivedChangeCipherSpec = true;
                this.HandleChangeCipherSpecMessage();
            }
        }

        protected virtual void ProcessFinishedMessage(MemoryStream buf)
        {
            if (this.mExpectedVerifyData == null)
            {
                throw new TlsFatalAlert(80);
            }
            byte[] b = TlsUtilities.ReadFully(this.mExpectedVerifyData.Length, buf);
            AssertEmpty(buf);
            if (!Arrays.ConstantTimeAreEqual(this.mExpectedVerifyData, b))
            {
                throw new TlsFatalAlert(0x33);
            }
        }

        private void ProcessHandshake()
        {
            bool flag;
            do
            {
                flag = false;
                if (this.mHandshakeQueue.Available >= 4)
                {
                    byte[] buf = new byte[4];
                    this.mHandshakeQueue.Read(buf, 0, 4, 0);
                    byte type = TlsUtilities.ReadUint8(buf, 0);
                    int len = TlsUtilities.ReadUint24(buf, 1);
                    if (this.mHandshakeQueue.Available >= (len + 4))
                    {
                        byte[] message = this.mHandshakeQueue.RemoveData(len, 4);
                        this.CheckReceivedChangeCipherSpec((this.mConnectionState == 0x10) || (type == 20));
                        switch (type)
                        {
                            case 0:
                                break;

                            default:
                            {
                                TlsContext context = this.Context;
                                if (((type == 20) && (this.mExpectedVerifyData == null)) && (context.SecurityParameters.MasterSecret != null))
                                {
                                    this.mExpectedVerifyData = this.CreateVerifyData(!context.IsServer);
                                }
                                this.mRecordStream.UpdateHandshakeData(buf, 0, 4);
                                this.mRecordStream.UpdateHandshakeData(message, 0, len);
                                break;
                            }
                        }
                        this.HandleHandshakeMessage(type, message);
                        flag = true;
                    }
                }
            }
            while (flag);
        }

        protected virtual short ProcessMaxFragmentLengthExtension(IDictionary clientExtensions, IDictionary serverExtensions, byte alertDescription)
        {
            short maxFragmentLengthExtension = TlsExtensionsUtilities.GetMaxFragmentLengthExtension(serverExtensions);
            if ((maxFragmentLengthExtension >= 0) && (!MaxFragmentLength.IsValid((byte) maxFragmentLengthExtension) || (!this.mResumedSession && (maxFragmentLengthExtension != TlsExtensionsUtilities.GetMaxFragmentLengthExtension(clientExtensions)))))
            {
                throw new TlsFatalAlert(alertDescription);
            }
            return maxFragmentLengthExtension;
        }

        protected internal void ProcessRecord(byte protocol, byte[] buf, int offset, int len)
        {
            switch (protocol)
            {
                case 20:
                    this.ProcessChangeCipherSpec(buf, offset, len);
                    break;

                case 0x15:
                    this.mAlertQueue.AddData(buf, offset, len);
                    this.ProcessAlert();
                    break;

                case 0x16:
                    this.mHandshakeQueue.AddData(buf, offset, len);
                    this.ProcessHandshake();
                    break;

                case 0x17:
                    if (!this.mAppDataReady)
                    {
                        throw new TlsFatalAlert(10);
                    }
                    this.mApplicationDataQueue.AddData(buf, offset, len);
                    this.ProcessApplicationData();
                    break;

                case 0x18:
                    if (!this.mAppDataReady)
                    {
                        throw new TlsFatalAlert(10);
                    }
                    break;
            }
        }

        protected virtual void RaiseAlert(byte alertLevel, byte alertDescription, string message, Exception cause)
        {
            this.Peer.NotifyAlertRaised(alertLevel, alertDescription, message, cause);
            byte[] buf = new byte[] { alertLevel, alertDescription };
            this.SafeWriteRecord(0x15, buf, 0, 2);
        }

        protected virtual void RaiseWarning(byte alertDescription, string message)
        {
            this.RaiseAlert(1, alertDescription, message, null);
        }

        protected internal virtual int ReadApplicationData(byte[] buf, int offset, int len)
        {
            if (len < 1)
            {
                return 0;
            }
            while (this.mApplicationDataQueue.Available == 0)
            {
                if (this.mClosed)
                {
                    if (this.mFailedWithError)
                    {
                        throw new IOException(TLS_ERROR_MESSAGE);
                    }
                    return 0;
                }
                this.SafeReadRecord();
            }
            len = Math.Min(len, this.mApplicationDataQueue.Available);
            this.mApplicationDataQueue.RemoveData(buf, offset, len, 0);
            return len;
        }

        protected internal static IDictionary ReadExtensions(MemoryStream input)
        {
            if (input.Position >= input.Length)
            {
                return null;
            }
            byte[] buffer = TlsUtilities.ReadOpaque16(input);
            AssertEmpty(input);
            MemoryStream stream = new MemoryStream(buffer, false);
            IDictionary dictionary = Platform.CreateHashtable();
            while (stream.Position < stream.Length)
            {
                int key = TlsUtilities.ReadUint16(stream);
                byte[] buffer2 = TlsUtilities.ReadOpaque16(stream);
                if (dictionary.Contains(key))
                {
                    throw new TlsFatalAlert(0x2f);
                }
                dictionary.Add(key, buffer2);
            }
            return dictionary;
        }

        public virtual int ReadInput(byte[] buffer, int offset, int length)
        {
            if (this.mBlocking)
            {
                throw new InvalidOperationException("Cannot use ReadInput() in blocking mode! Use Stream instead.");
            }
            return this.ReadApplicationData(buffer, offset, Math.Min(length, this.ApplicationDataAvailable()));
        }

        public virtual int ReadOutput(byte[] buffer, int offset, int length)
        {
            if (this.mBlocking)
            {
                throw new InvalidOperationException("Cannot use ReadOutput() in blocking mode! Use Stream instead.");
            }
            return this.mOutputBuffer.Read(buffer, offset, length);
        }

        protected internal static IList ReadSupplementalDataMessage(MemoryStream input)
        {
            byte[] buffer = TlsUtilities.ReadOpaque24(input);
            AssertEmpty(input);
            MemoryStream stream = new MemoryStream(buffer, false);
            IList list = Platform.CreateArrayList();
            while (stream.Position < stream.Length)
            {
                int dataType = TlsUtilities.ReadUint16(stream);
                byte[] data = TlsUtilities.ReadOpaque16(stream);
                list.Add(new SupplementalDataEntry(dataType, data));
            }
            return list;
        }

        protected virtual void RefuseRenegotiation()
        {
            if (TlsUtilities.IsSsl(this.Context))
            {
                throw new TlsFatalAlert(40);
            }
            this.RaiseWarning(100, "Renegotiation not supported");
        }

        protected virtual void SafeReadRecord()
        {
            try
            {
                if (!this.mRecordStream.ReadRecord())
                {
                    throw new EndOfStreamException();
                }
            }
            catch (TlsFatalAlert alert)
            {
                if (!this.mClosed)
                {
                    this.FailWithError(2, alert.AlertDescription, "Failed to read record", alert);
                }
                throw alert;
            }
            catch (Exception exception)
            {
                if (!this.mClosed)
                {
                    this.FailWithError(2, 80, "Failed to read record", exception);
                }
                throw exception;
            }
        }

        protected virtual void SafeWriteRecord(byte type, byte[] buf, int offset, int len)
        {
            try
            {
                this.mRecordStream.WriteRecord(type, buf, offset, len);
            }
            catch (TlsFatalAlert alert)
            {
                if (!this.mClosed)
                {
                    this.FailWithError(2, alert.AlertDescription, "Failed to write record", alert);
                }
                throw alert;
            }
            catch (Exception exception)
            {
                if (!this.mClosed)
                {
                    this.FailWithError(2, 80, "Failed to write record", exception);
                }
                throw exception;
            }
        }

        protected virtual void SendCertificateMessage(Certificate certificate)
        {
            if (certificate == null)
            {
                certificate = Certificate.EmptyChain;
            }
            if (certificate.IsEmpty && !this.Context.IsServer)
            {
                ProtocolVersion serverVersion = this.Context.ServerVersion;
                if (serverVersion.IsSsl)
                {
                    string message = serverVersion.ToString() + " client didn't provide credentials";
                    this.RaiseWarning(0x29, message);
                    return;
                }
            }
            HandshakeMessage output = new HandshakeMessage(11);
            certificate.Encode(output);
            output.WriteToRecordStream(this);
        }

        protected virtual void SendChangeCipherSpecMessage()
        {
            byte[] buf = new byte[] { 1 };
            this.SafeWriteRecord(20, buf, 0, buf.Length);
            this.mRecordStream.SentWriteCipherSpec();
        }

        protected virtual void SendFinishedMessage()
        {
            byte[] buffer = this.CreateVerifyData(this.Context.IsServer);
            HandshakeMessage message = new HandshakeMessage(20, buffer.Length);
            message.Write(buffer, 0, buffer.Length);
            message.WriteToRecordStream(this);
        }

        protected virtual void SendSupplementalDataMessage(IList supplementalData)
        {
            HandshakeMessage output = new HandshakeMessage(0x17);
            WriteSupplementalData(output, supplementalData);
            output.WriteToRecordStream(this);
        }

        protected virtual void SetAppDataSplitMode(int appDataSplitMode)
        {
            if ((appDataSplitMode < 0) || (appDataSplitMode > 2))
            {
                throw new ArgumentException("Illegal appDataSplitMode mode: " + appDataSplitMode, "appDataSplitMode");
            }
            this.mAppDataSplitMode = appDataSplitMode;
        }

        protected internal virtual void WriteData(byte[] buf, int offset, int len)
        {
            if (this.mClosed)
            {
                if (this.mFailedWithError)
                {
                    throw new IOException(TLS_ERROR_MESSAGE);
                }
                throw new IOException("Sorry, connection has been closed, you cannot write more data");
            }
            while (len > 0)
            {
                if (this.mAppDataSplitEnabled)
                {
                    switch (this.mAppDataSplitMode)
                    {
                        case 1:
                            this.SafeWriteRecord(0x17, TlsUtilities.EmptyBytes, 0, 0);
                            goto Label_00AD;

                        case 2:
                            this.mAppDataSplitEnabled = false;
                            this.SafeWriteRecord(0x17, TlsUtilities.EmptyBytes, 0, 0);
                            goto Label_00AD;
                    }
                    this.SafeWriteRecord(0x17, buf, offset, 1);
                    offset++;
                    len--;
                }
            Label_00AD:
                if (len > 0)
                {
                    int num2 = Math.Min(len, this.mRecordStream.GetPlaintextLimit());
                    this.SafeWriteRecord(0x17, buf, offset, num2);
                    offset += num2;
                    len -= num2;
                }
            }
        }

        protected internal static void WriteExtensions(System.IO.Stream output, IDictionary extensions)
        {
            MemoryStream stream = new MemoryStream();
            WriteSelectedExtensions(stream, extensions, true);
            WriteSelectedExtensions(stream, extensions, false);
            TlsUtilities.WriteOpaque16(stream.ToArray(), output);
        }

        protected virtual void WriteHandshakeMessage(byte[] buf, int off, int len)
        {
            while (len > 0)
            {
                int num = Math.Min(len, this.mRecordStream.GetPlaintextLimit());
                this.SafeWriteRecord(0x16, buf, off, num);
                off += num;
                len -= num;
            }
        }

        protected internal static void WriteSelectedExtensions(System.IO.Stream output, IDictionary extensions, bool selectEmpty)
        {
            IEnumerator enumerator = extensions.Keys.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    int current = (int) enumerator.Current;
                    byte[] buf = (byte[]) extensions[current];
                    if (selectEmpty == (buf.Length == 0))
                    {
                        TlsUtilities.CheckUint16(current);
                        TlsUtilities.WriteUint16(current, output);
                        TlsUtilities.WriteOpaque16(buf, output);
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

        protected internal static void WriteSupplementalData(System.IO.Stream output, IList supplementalData)
        {
            MemoryStream stream = new MemoryStream();
            IEnumerator enumerator = supplementalData.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    SupplementalDataEntry current = (SupplementalDataEntry) enumerator.Current;
                    int dataType = current.DataType;
                    TlsUtilities.CheckUint16(dataType);
                    TlsUtilities.WriteUint16(dataType, stream);
                    TlsUtilities.WriteOpaque16(current.Data, stream);
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
            TlsUtilities.WriteOpaque24(stream.ToArray(), output);
        }

        protected abstract TlsContext Context { get; }

        internal abstract AbstractTlsContext ContextAdmin { get; }

        protected abstract TlsPeer Peer { get; }

        public virtual System.IO.Stream Stream
        {
            get
            {
                if (!this.mBlocking)
                {
                    throw new InvalidOperationException("Cannot use Stream in non-blocking mode! Use OfferInput()/OfferOutput() instead.");
                }
                return this.mTlsStream;
            }
        }

        public virtual bool IsClosed =>
            this.mClosed;

        internal class HandshakeMessage : MemoryStream
        {
            internal HandshakeMessage(byte handshakeType) : this(handshakeType, 60)
            {
            }

            internal HandshakeMessage(byte handshakeType, int length) : base((int) (length + 4))
            {
                TlsUtilities.WriteUint8(handshakeType, this);
                TlsUtilities.WriteUint24(0, this);
            }

            internal void Write(byte[] data)
            {
                this.Write(data, 0, data.Length);
            }

            internal void WriteToRecordStream(TlsProtocol protocol)
            {
                long i = this.Length - 4L;
                TlsUtilities.CheckUint24(i);
                this.Position = 1L;
                TlsUtilities.WriteUint24((int) i, this);
                byte[] buf = this.GetBuffer();
                int length = (int) this.Length;
                protocol.WriteHandshakeMessage(buf, 0, length);
                Platform.Dispose(this);
            }
        }
    }
}

