namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Macs;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Chacha20Poly1305 : TlsCipher
    {
        private static readonly byte[] Zeroes = new byte[15];
        protected readonly TlsContext context;
        protected readonly ChaCha7539Engine encryptCipher;
        protected readonly ChaCha7539Engine decryptCipher;
        protected readonly byte[] encryptIV;
        protected readonly byte[] decryptIV;

        public Chacha20Poly1305(TlsContext context)
        {
            KeyParameter parameter3;
            KeyParameter parameter4;
            if (!TlsUtilities.IsTlsV12(context))
            {
                throw new TlsFatalAlert(80);
            }
            this.context = context;
            int keyLen = 0x20;
            int num2 = 12;
            int size = (2 * keyLen) + (2 * num2);
            byte[] key = TlsUtilities.CalculateKeyBlock(context, size);
            int keyOff = 0;
            KeyParameter parameter = new KeyParameter(key, keyOff, keyLen);
            keyOff += keyLen;
            KeyParameter parameter2 = new KeyParameter(key, keyOff, keyLen);
            keyOff += keyLen;
            byte[] buffer2 = Arrays.CopyOfRange(key, keyOff, keyOff + num2);
            keyOff += num2;
            byte[] buffer3 = Arrays.CopyOfRange(key, keyOff, keyOff + num2);
            keyOff += num2;
            if (keyOff != size)
            {
                throw new TlsFatalAlert(80);
            }
            this.encryptCipher = new ChaCha7539Engine();
            this.decryptCipher = new ChaCha7539Engine();
            if (context.IsServer)
            {
                parameter3 = parameter2;
                parameter4 = parameter;
                this.encryptIV = buffer3;
                this.decryptIV = buffer2;
            }
            else
            {
                parameter3 = parameter;
                parameter4 = parameter2;
                this.encryptIV = buffer2;
                this.decryptIV = buffer3;
            }
            this.encryptCipher.Init(true, new ParametersWithIV(parameter3, this.encryptIV));
            this.decryptCipher.Init(false, new ParametersWithIV(parameter4, this.decryptIV));
        }

        protected virtual byte[] CalculateNonce(long seqNo, byte[] iv)
        {
            byte[] buf = new byte[12];
            TlsUtilities.WriteUint64(seqNo, buf, 4);
            for (int i = 0; i < 12; i++)
            {
                buf[i] = (byte) (buf[i] ^ iv[i]);
            }
            return buf;
        }

        protected virtual byte[] CalculateRecordMac(KeyParameter macKey, byte[] additionalData, byte[] buf, int off, int len)
        {
            IMac mac = new Poly1305();
            mac.Init(macKey);
            this.UpdateRecordMacText(mac, additionalData, 0, additionalData.Length);
            this.UpdateRecordMacText(mac, buf, off, len);
            this.UpdateRecordMacLength(mac, additionalData.Length);
            this.UpdateRecordMacLength(mac, len);
            return MacUtilities.DoFinal(mac);
        }

        public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
        {
            if (this.GetPlaintextLimit(len) < 0)
            {
                throw new TlsFatalAlert(50);
            }
            KeyParameter macKey = this.InitRecord(this.decryptCipher, false, seqNo, this.decryptIV);
            int num = len - 0x10;
            byte[] additionalData = this.GetAdditionalData(seqNo, type, num);
            byte[] a = this.CalculateRecordMac(macKey, additionalData, ciphertext, offset, num);
            byte[] b = Arrays.CopyOfRange(ciphertext, offset + num, offset + len);
            if (!Arrays.ConstantTimeAreEqual(a, b))
            {
                throw new TlsFatalAlert(20);
            }
            byte[] outBytes = new byte[num];
            this.decryptCipher.ProcessBytes(ciphertext, offset, num, outBytes, 0);
            return outBytes;
        }

        public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
        {
            KeyParameter macKey = this.InitRecord(this.encryptCipher, true, seqNo, this.encryptIV);
            byte[] outBytes = new byte[len + 0x10];
            this.encryptCipher.ProcessBytes(plaintext, offset, len, outBytes, 0);
            byte[] additionalData = this.GetAdditionalData(seqNo, type, len);
            byte[] sourceArray = this.CalculateRecordMac(macKey, additionalData, outBytes, 0, len);
            Array.Copy(sourceArray, 0, outBytes, len, sourceArray.Length);
            return outBytes;
        }

        protected virtual KeyParameter GenerateRecordMacKey(IStreamCipher cipher)
        {
            byte[] input = new byte[0x40];
            cipher.ProcessBytes(input, 0, input.Length, input, 0);
            KeyParameter parameter = new KeyParameter(input, 0, 0x20);
            Arrays.Fill(input, 0);
            return parameter;
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
            (ciphertextLimit - 0x10);

        protected virtual KeyParameter InitRecord(IStreamCipher cipher, bool forEncryption, long seqNo, byte[] iv)
        {
            byte[] buffer = this.CalculateNonce(seqNo, iv);
            cipher.Init(forEncryption, new ParametersWithIV(null, buffer));
            return this.GenerateRecordMacKey(cipher);
        }

        protected virtual void UpdateRecordMacLength(IMac mac, int len)
        {
            byte[] input = Pack.UInt64_To_LE((ulong) len);
            mac.BlockUpdate(input, 0, input.Length);
        }

        protected virtual void UpdateRecordMacText(IMac mac, byte[] buf, int off, int len)
        {
            mac.BlockUpdate(buf, off, len);
            int num = len % 0x10;
            if (num != 0)
            {
                mac.BlockUpdate(Zeroes, 0, 0x10 - num);
            }
        }
    }
}

