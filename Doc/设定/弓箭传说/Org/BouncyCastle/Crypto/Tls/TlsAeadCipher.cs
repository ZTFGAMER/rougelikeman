namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class TlsAeadCipher : TlsCipher
    {
        public const int NONCE_RFC5288 = 1;
        internal const int NONCE_DRAFT_CHACHA20_POLY1305 = 2;
        protected readonly TlsContext context;
        protected readonly int macSize;
        protected readonly int record_iv_length;
        protected readonly IAeadBlockCipher encryptCipher;
        protected readonly IAeadBlockCipher decryptCipher;
        protected readonly byte[] encryptImplicitNonce;
        protected readonly byte[] decryptImplicitNonce;
        protected readonly int nonceMode;

        public TlsAeadCipher(TlsContext context, IAeadBlockCipher clientWriteCipher, IAeadBlockCipher serverWriteCipher, int cipherKeySize, int macSize) : this(context, clientWriteCipher, serverWriteCipher, cipherKeySize, macSize, 1)
        {
        }

        internal TlsAeadCipher(TlsContext context, IAeadBlockCipher clientWriteCipher, IAeadBlockCipher serverWriteCipher, int cipherKeySize, int macSize, int nonceMode)
        {
            int num;
            KeyParameter parameter3;
            KeyParameter parameter4;
            if (!TlsUtilities.IsTlsV12(context))
            {
                throw new TlsFatalAlert(80);
            }
            this.nonceMode = nonceMode;
            if (nonceMode != 1)
            {
                if (nonceMode != 2)
                {
                    throw new TlsFatalAlert(80);
                }
            }
            else
            {
                num = 4;
                this.record_iv_length = 8;
                goto Label_005B;
            }
            num = 12;
            this.record_iv_length = 0;
        Label_005B:
            this.context = context;
            this.macSize = macSize;
            int size = (2 * cipherKeySize) + (2 * num);
            byte[] key = TlsUtilities.CalculateKeyBlock(context, size);
            int keyOff = 0;
            KeyParameter parameter = new KeyParameter(key, keyOff, cipherKeySize);
            keyOff += cipherKeySize;
            KeyParameter parameter2 = new KeyParameter(key, keyOff, cipherKeySize);
            keyOff += cipherKeySize;
            byte[] buffer2 = Arrays.CopyOfRange(key, keyOff, keyOff + num);
            keyOff += num;
            byte[] buffer3 = Arrays.CopyOfRange(key, keyOff, keyOff + num);
            keyOff += num;
            if (keyOff != size)
            {
                throw new TlsFatalAlert(80);
            }
            if (context.IsServer)
            {
                this.encryptCipher = serverWriteCipher;
                this.decryptCipher = clientWriteCipher;
                this.encryptImplicitNonce = buffer3;
                this.decryptImplicitNonce = buffer2;
                parameter3 = parameter2;
                parameter4 = parameter;
            }
            else
            {
                this.encryptCipher = clientWriteCipher;
                this.decryptCipher = serverWriteCipher;
                this.encryptImplicitNonce = buffer2;
                this.decryptImplicitNonce = buffer3;
                parameter3 = parameter;
                parameter4 = parameter2;
            }
            byte[] nonce = new byte[num + this.record_iv_length];
            this.encryptCipher.Init(true, new AeadParameters(parameter3, 8 * macSize, nonce));
            this.decryptCipher.Init(false, new AeadParameters(parameter4, 8 * macSize, nonce));
        }

        public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
        {
            if (this.GetPlaintextLimit(len) < 0)
            {
                throw new TlsFatalAlert(50);
            }
            byte[] destinationArray = new byte[this.decryptImplicitNonce.Length + this.record_iv_length];
            switch (this.nonceMode)
            {
                case 1:
                    Array.Copy(this.decryptImplicitNonce, 0, destinationArray, 0, this.decryptImplicitNonce.Length);
                    Array.Copy(ciphertext, offset, destinationArray, destinationArray.Length - this.record_iv_length, this.record_iv_length);
                    break;

                case 2:
                    TlsUtilities.WriteUint64(seqNo, destinationArray, destinationArray.Length - 8);
                    for (int i = 0; i < this.decryptImplicitNonce.Length; i++)
                    {
                        destinationArray[i] = (byte) (destinationArray[i] ^ this.decryptImplicitNonce[i]);
                    }
                    break;

                default:
                    throw new TlsFatalAlert(80);
            }
            int inOff = offset + this.record_iv_length;
            int num4 = len - this.record_iv_length;
            int outputSize = this.decryptCipher.GetOutputSize(num4);
            byte[] outBytes = new byte[outputSize];
            int outOff = 0;
            byte[] associatedText = this.GetAdditionalData(seqNo, type, outputSize);
            AeadParameters parameters = new AeadParameters(null, 8 * this.macSize, destinationArray, associatedText);
            try
            {
                this.decryptCipher.Init(false, parameters);
                outOff += this.decryptCipher.ProcessBytes(ciphertext, inOff, num4, outBytes, outOff);
                outOff += this.decryptCipher.DoFinal(outBytes, outOff);
            }
            catch (Exception exception)
            {
                throw new TlsFatalAlert(20, exception);
            }
            if (outOff != outBytes.Length)
            {
                throw new TlsFatalAlert(80);
            }
            return outBytes;
        }

        public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
        {
            byte[] destinationArray = new byte[this.encryptImplicitNonce.Length + this.record_iv_length];
            switch (this.nonceMode)
            {
                case 1:
                    Array.Copy(this.encryptImplicitNonce, 0, destinationArray, 0, this.encryptImplicitNonce.Length);
                    TlsUtilities.WriteUint64(seqNo, destinationArray, this.encryptImplicitNonce.Length);
                    break;

                case 2:
                    TlsUtilities.WriteUint64(seqNo, destinationArray, destinationArray.Length - 8);
                    for (int i = 0; i < this.encryptImplicitNonce.Length; i++)
                    {
                        destinationArray[i] = (byte) (destinationArray[i] ^ this.encryptImplicitNonce[i]);
                    }
                    break;

                default:
                    throw new TlsFatalAlert(80);
            }
            int inOff = offset;
            int num4 = len;
            int outputSize = this.encryptCipher.GetOutputSize(num4);
            byte[] buffer2 = new byte[this.record_iv_length + outputSize];
            if (this.record_iv_length != 0)
            {
                Array.Copy(destinationArray, destinationArray.Length - this.record_iv_length, buffer2, 0, this.record_iv_length);
            }
            int outOff = this.record_iv_length;
            byte[] associatedText = this.GetAdditionalData(seqNo, type, num4);
            AeadParameters parameters = new AeadParameters(null, 8 * this.macSize, destinationArray, associatedText);
            try
            {
                this.encryptCipher.Init(true, parameters);
                outOff += this.encryptCipher.ProcessBytes(plaintext, inOff, num4, buffer2, outOff);
                outOff += this.encryptCipher.DoFinal(buffer2, outOff);
            }
            catch (Exception exception)
            {
                throw new TlsFatalAlert(80, exception);
            }
            if (outOff != buffer2.Length)
            {
                throw new TlsFatalAlert(80);
            }
            return buffer2;
        }

        protected virtual byte[] GetAdditionalData(long seqNo, byte type, int len)
        {
            byte[] buf = new byte[13];
            TlsUtilities.WriteUint64(seqNo, buf, 0);
            TlsUtilities.WriteUint8(type, buf, 8);
            TlsUtilities.WriteVersion(this.context.ServerVersion, buf, 9);
            TlsUtilities.WriteUint16(len, buf, 11);
            return buf;
        }

        public virtual int GetPlaintextLimit(int ciphertextLimit) => 
            ((ciphertextLimit - this.macSize) - this.record_iv_length);
    }
}

