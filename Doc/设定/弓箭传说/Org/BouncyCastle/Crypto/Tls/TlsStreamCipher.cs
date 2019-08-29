namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class TlsStreamCipher : TlsCipher
    {
        protected readonly TlsContext context;
        protected readonly IStreamCipher encryptCipher;
        protected readonly IStreamCipher decryptCipher;
        protected readonly TlsMac writeMac;
        protected readonly TlsMac readMac;
        protected readonly bool usesNonce;

        public TlsStreamCipher(TlsContext context, IStreamCipher clientWriteCipher, IStreamCipher serverWriteCipher, IDigest clientWriteDigest, IDigest serverWriteDigest, int cipherKeySize, bool usesNonce)
        {
            ICipherParameters parameters;
            ICipherParameters parameters2;
            bool isServer = context.IsServer;
            this.context = context;
            this.usesNonce = usesNonce;
            this.encryptCipher = clientWriteCipher;
            this.decryptCipher = serverWriteCipher;
            int size = ((2 * cipherKeySize) + clientWriteDigest.GetDigestSize()) + serverWriteDigest.GetDigestSize();
            byte[] key = TlsUtilities.CalculateKeyBlock(context, size);
            int keyOff = 0;
            TlsMac mac = new TlsMac(context, clientWriteDigest, key, keyOff, clientWriteDigest.GetDigestSize());
            keyOff += clientWriteDigest.GetDigestSize();
            TlsMac mac2 = new TlsMac(context, serverWriteDigest, key, keyOff, serverWriteDigest.GetDigestSize());
            keyOff += serverWriteDigest.GetDigestSize();
            KeyParameter parameter = new KeyParameter(key, keyOff, cipherKeySize);
            keyOff += cipherKeySize;
            KeyParameter parameter2 = new KeyParameter(key, keyOff, cipherKeySize);
            keyOff += cipherKeySize;
            if (keyOff != size)
            {
                throw new TlsFatalAlert(80);
            }
            if (isServer)
            {
                this.writeMac = mac2;
                this.readMac = mac;
                this.encryptCipher = serverWriteCipher;
                this.decryptCipher = clientWriteCipher;
                parameters = parameter2;
                parameters2 = parameter;
            }
            else
            {
                this.writeMac = mac;
                this.readMac = mac2;
                this.encryptCipher = clientWriteCipher;
                this.decryptCipher = serverWriteCipher;
                parameters = parameter;
                parameters2 = parameter2;
            }
            if (usesNonce)
            {
                byte[] iv = new byte[8];
                parameters = new ParametersWithIV(parameters, iv);
                parameters2 = new ParametersWithIV(parameters2, iv);
            }
            this.encryptCipher.Init(true, parameters);
            this.decryptCipher.Init(false, parameters2);
        }

        protected virtual void CheckMac(long seqNo, byte type, byte[] recBuf, int recStart, int recEnd, byte[] calcBuf, int calcOff, int calcLen)
        {
            byte[] a = Arrays.CopyOfRange(recBuf, recStart, recEnd);
            byte[] b = this.readMac.CalculateMac(seqNo, type, calcBuf, calcOff, calcLen);
            if (!Arrays.ConstantTimeAreEqual(a, b))
            {
                throw new TlsFatalAlert(20);
            }
        }

        public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
        {
            if (this.usesNonce)
            {
                this.UpdateIV(this.decryptCipher, false, seqNo);
            }
            int size = this.readMac.Size;
            if (len < size)
            {
                throw new TlsFatalAlert(50);
            }
            int recStart = len - size;
            byte[] output = new byte[len];
            this.decryptCipher.ProcessBytes(ciphertext, offset, len, output, 0);
            this.CheckMac(seqNo, type, output, recStart, len, output, 0, recStart);
            return Arrays.CopyOfRange(output, 0, recStart);
        }

        public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
        {
            if (this.usesNonce)
            {
                this.UpdateIV(this.encryptCipher, true, seqNo);
            }
            byte[] output = new byte[len + this.writeMac.Size];
            this.encryptCipher.ProcessBytes(plaintext, offset, len, output, 0);
            byte[] input = this.writeMac.CalculateMac(seqNo, type, plaintext, offset, len);
            this.encryptCipher.ProcessBytes(input, 0, input.Length, output, len);
            return output;
        }

        public virtual int GetPlaintextLimit(int ciphertextLimit) => 
            (ciphertextLimit - this.writeMac.Size);

        protected virtual void UpdateIV(IStreamCipher cipher, bool forEncryption, long seqNo)
        {
            byte[] buf = new byte[8];
            TlsUtilities.WriteUint64(seqNo, buf, 0);
            cipher.Init(forEncryption, new ParametersWithIV(null, buf));
        }
    }
}

