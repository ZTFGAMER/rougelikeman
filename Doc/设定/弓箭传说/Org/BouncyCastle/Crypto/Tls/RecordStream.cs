namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    internal class RecordStream
    {
        private const int DEFAULT_PLAINTEXT_LIMIT = 0x4000;
        internal const int TLS_HEADER_SIZE = 5;
        internal const int TLS_HEADER_TYPE_OFFSET = 0;
        internal const int TLS_HEADER_VERSION_OFFSET = 1;
        internal const int TLS_HEADER_LENGTH_OFFSET = 3;
        private TlsProtocol mHandler;
        private Stream mInput;
        private Stream mOutput;
        private TlsCompression mPendingCompression;
        private TlsCompression mReadCompression;
        private TlsCompression mWriteCompression;
        private TlsCipher mPendingCipher;
        private TlsCipher mReadCipher;
        private TlsCipher mWriteCipher;
        private long mReadSeqNo;
        private long mWriteSeqNo;
        private MemoryStream mBuffer = new MemoryStream();
        private TlsHandshakeHash mHandshakeHash;
        private ProtocolVersion mReadVersion;
        private ProtocolVersion mWriteVersion;
        private bool mRestrictReadVersion = true;
        private int mPlaintextLimit;
        private int mCompressedLimit;
        private int mCiphertextLimit;

        internal RecordStream(TlsProtocol handler, Stream input, Stream output)
        {
            this.mHandler = handler;
            this.mInput = input;
            this.mOutput = output;
            this.mReadCompression = new TlsNullCompression();
            this.mWriteCompression = this.mReadCompression;
        }

        private static void CheckLength(int length, int limit, byte alertDescription)
        {
            if (length > limit)
            {
                throw new TlsFatalAlert(alertDescription);
            }
        }

        private static void CheckType(byte type, byte alertDescription)
        {
            switch (type)
            {
                case 20:
                case 0x15:
                case 0x16:
                case 0x17:
                case 0x18:
                    return;
            }
            throw new TlsFatalAlert(alertDescription);
        }

        internal virtual byte[] DecodeAndVerify(byte type, Stream input, int len)
        {
            long num;
            CheckLength(len, this.mCiphertextLimit, 0x16);
            byte[] ciphertext = TlsUtilities.ReadFully(len, input);
            this.mReadSeqNo = (num = this.mReadSeqNo) + 1L;
            byte[] buffer = this.mReadCipher.DecodeCiphertext(num, type, ciphertext, 0, ciphertext.Length);
            CheckLength(buffer.Length, this.mCompressedLimit, 0x16);
            Stream stream = this.mReadCompression.Decompress(this.mBuffer);
            if (stream != this.mBuffer)
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                buffer = this.GetBufferContents();
            }
            CheckLength(buffer.Length, this.mPlaintextLimit, 30);
            if ((buffer.Length < 1) && (type != 0x17))
            {
                throw new TlsFatalAlert(0x2f);
            }
            return buffer;
        }

        internal virtual void FinaliseHandshake()
        {
            if (((this.mReadCompression != this.mPendingCompression) || (this.mWriteCompression != this.mPendingCompression)) || ((this.mReadCipher != this.mPendingCipher) || (this.mWriteCipher != this.mPendingCipher)))
            {
                throw new TlsFatalAlert(40);
            }
            this.mPendingCompression = null;
            this.mPendingCipher = null;
        }

        internal virtual void Flush()
        {
            this.mOutput.Flush();
        }

        private byte[] GetBufferContents()
        {
            byte[] buffer = this.mBuffer.ToArray();
            this.mBuffer.SetLength(0L);
            return buffer;
        }

        internal virtual int GetPlaintextLimit() => 
            this.mPlaintextLimit;

        internal virtual void Init(TlsContext context)
        {
            this.mReadCipher = new TlsNullCipher(context);
            this.mWriteCipher = this.mReadCipher;
            this.mHandshakeHash = new DeferredHash();
            this.mHandshakeHash.Init(context);
            this.SetPlaintextLimit(0x4000);
        }

        internal virtual void NotifyHelloComplete()
        {
            this.mHandshakeHash = this.mHandshakeHash.NotifyPrfDetermined();
        }

        internal virtual TlsHandshakeHash PrepareToFinish()
        {
            TlsHandshakeHash mHandshakeHash = this.mHandshakeHash;
            this.mHandshakeHash = this.mHandshakeHash.StopTracking();
            return mHandshakeHash;
        }

        internal virtual bool ReadRecord()
        {
            byte[] buf = TlsUtilities.ReadAllOrNothing(5, this.mInput);
            if (buf == null)
            {
                return false;
            }
            byte type = TlsUtilities.ReadUint8(buf, 0);
            CheckType(type, 10);
            if (!this.mRestrictReadVersion)
            {
                if ((TlsUtilities.ReadVersionRaw(buf, 1) & 0xffffff00L) != 0x300L)
                {
                    throw new TlsFatalAlert(0x2f);
                }
            }
            else
            {
                ProtocolVersion version = TlsUtilities.ReadVersion(buf, 1);
                if (this.mReadVersion == null)
                {
                    this.mReadVersion = version;
                }
                else if (!version.Equals(this.mReadVersion))
                {
                    throw new TlsFatalAlert(0x2f);
                }
            }
            int len = TlsUtilities.ReadUint16(buf, 3);
            byte[] buffer2 = this.DecodeAndVerify(type, this.mInput, len);
            this.mHandler.ProcessRecord(type, buffer2, 0, buffer2.Length);
            return true;
        }

        internal virtual void ReceivedReadCipherSpec()
        {
            if ((this.mPendingCompression == null) || (this.mPendingCipher == null))
            {
                throw new TlsFatalAlert(40);
            }
            this.mReadCompression = this.mPendingCompression;
            this.mReadCipher = this.mPendingCipher;
            this.mReadSeqNo = 0L;
        }

        internal virtual void SafeClose()
        {
            try
            {
                Platform.Dispose(this.mInput);
            }
            catch (IOException)
            {
            }
            try
            {
                Platform.Dispose(this.mOutput);
            }
            catch (IOException)
            {
            }
        }

        internal virtual void SentWriteCipherSpec()
        {
            if ((this.mPendingCompression == null) || (this.mPendingCipher == null))
            {
                throw new TlsFatalAlert(40);
            }
            this.mWriteCompression = this.mPendingCompression;
            this.mWriteCipher = this.mPendingCipher;
            this.mWriteSeqNo = 0L;
        }

        internal virtual void SetPendingConnectionState(TlsCompression tlsCompression, TlsCipher tlsCipher)
        {
            this.mPendingCompression = tlsCompression;
            this.mPendingCipher = tlsCipher;
        }

        internal virtual void SetPlaintextLimit(int plaintextLimit)
        {
            this.mPlaintextLimit = plaintextLimit;
            this.mCompressedLimit = this.mPlaintextLimit + 0x400;
            this.mCiphertextLimit = this.mCompressedLimit + 0x400;
        }

        internal virtual void SetRestrictReadVersion(bool enabled)
        {
            this.mRestrictReadVersion = enabled;
        }

        internal virtual void SetWriteVersion(ProtocolVersion writeVersion)
        {
            this.mWriteVersion = writeVersion;
        }

        internal virtual void UpdateHandshakeData(byte[] message, int offset, int len)
        {
            this.mHandshakeHash.BlockUpdate(message, offset, len);
        }

        internal virtual void WriteRecord(byte type, byte[] plaintext, int plaintextOffset, int plaintextLength)
        {
            if (this.mWriteVersion != null)
            {
                byte[] buffer;
                long num;
                CheckType(type, 80);
                CheckLength(plaintextLength, this.mPlaintextLimit, 80);
                if ((plaintextLength < 1) && (type != 0x17))
                {
                    throw new TlsFatalAlert(80);
                }
                if (type == 0x16)
                {
                    this.UpdateHandshakeData(plaintext, plaintextOffset, plaintextLength);
                }
                Stream stream = this.mWriteCompression.Compress(this.mBuffer);
                if (stream == this.mBuffer)
                {
                    this.mWriteSeqNo = (num = this.mWriteSeqNo) + 1L;
                    buffer = this.mWriteCipher.EncodePlaintext(num, type, plaintext, plaintextOffset, plaintextLength);
                }
                else
                {
                    stream.Write(plaintext, plaintextOffset, plaintextLength);
                    stream.Flush();
                    byte[] bufferContents = this.GetBufferContents();
                    CheckLength(bufferContents.Length, plaintextLength + 0x400, 80);
                    this.mWriteSeqNo = (num = this.mWriteSeqNo) + 1L;
                    buffer = this.mWriteCipher.EncodePlaintext(num, type, bufferContents, 0, bufferContents.Length);
                }
                CheckLength(buffer.Length, this.mCiphertextLimit, 80);
                byte[] buf = new byte[buffer.Length + 5];
                TlsUtilities.WriteUint8(type, buf, 0);
                TlsUtilities.WriteVersion(this.mWriteVersion, buf, 1);
                TlsUtilities.WriteUint16(buffer.Length, buf, 3);
                Array.Copy(buffer, 0, buf, 5, buffer.Length);
                this.mOutput.Write(buf, 0, buf.Length);
                this.mOutput.Flush();
            }
        }

        internal virtual ProtocolVersion ReadVersion
        {
            get => 
                this.mReadVersion;
            set => 
                (this.mReadVersion = value);
        }

        internal virtual TlsHandshakeHash HandshakeHash =>
            this.mHandshakeHash;
    }
}

