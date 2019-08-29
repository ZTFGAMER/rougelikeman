namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities;
    using System;

    public class TlsNullCipher : TlsCipher
    {
        protected readonly TlsContext context;
        protected readonly TlsMac writeMac;
        protected readonly TlsMac readMac;

        public TlsNullCipher(TlsContext context)
        {
            this.context = context;
            this.writeMac = null;
            this.readMac = null;
        }

        public TlsNullCipher(TlsContext context, IDigest clientWriteDigest, IDigest serverWriteDigest)
        {
            if ((clientWriteDigest == null) != (serverWriteDigest == null))
            {
                throw new TlsFatalAlert(80);
            }
            this.context = context;
            TlsMac mac = null;
            TlsMac mac2 = null;
            if (clientWriteDigest != null)
            {
                int size = clientWriteDigest.GetDigestSize() + serverWriteDigest.GetDigestSize();
                byte[] key = TlsUtilities.CalculateKeyBlock(context, size);
                int keyOff = 0;
                mac = new TlsMac(context, clientWriteDigest, key, keyOff, clientWriteDigest.GetDigestSize());
                keyOff += clientWriteDigest.GetDigestSize();
                mac2 = new TlsMac(context, serverWriteDigest, key, keyOff, serverWriteDigest.GetDigestSize());
                keyOff += serverWriteDigest.GetDigestSize();
                if (keyOff != size)
                {
                    throw new TlsFatalAlert(80);
                }
            }
            if (context.IsServer)
            {
                this.writeMac = mac2;
                this.readMac = mac;
            }
            else
            {
                this.writeMac = mac;
                this.readMac = mac2;
            }
        }

        public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
        {
            if (this.readMac == null)
            {
                return Arrays.CopyOfRange(ciphertext, offset, offset + len);
            }
            int size = this.readMac.Size;
            if (len < size)
            {
                throw new TlsFatalAlert(50);
            }
            int length = len - size;
            byte[] a = Arrays.CopyOfRange(ciphertext, offset + length, offset + len);
            byte[] b = this.readMac.CalculateMac(seqNo, type, ciphertext, offset, length);
            if (!Arrays.ConstantTimeAreEqual(a, b))
            {
                throw new TlsFatalAlert(20);
            }
            return Arrays.CopyOfRange(ciphertext, offset, offset + length);
        }

        public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
        {
            if (this.writeMac == null)
            {
                return Arrays.CopyOfRange(plaintext, offset, offset + len);
            }
            byte[] sourceArray = this.writeMac.CalculateMac(seqNo, type, plaintext, offset, len);
            byte[] destinationArray = new byte[len + sourceArray.Length];
            Array.Copy(plaintext, offset, destinationArray, 0, len);
            Array.Copy(sourceArray, 0, destinationArray, len, sourceArray.Length);
            return destinationArray;
        }

        public virtual int GetPlaintextLimit(int ciphertextLimit)
        {
            int num = ciphertextLimit;
            if (this.writeMac != null)
            {
                num -= this.writeMac.Size;
            }
            return num;
        }
    }
}

