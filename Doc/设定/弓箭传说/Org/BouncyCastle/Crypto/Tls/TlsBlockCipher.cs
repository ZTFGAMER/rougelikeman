namespace Org.BouncyCastle.Crypto.Tls
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    public class TlsBlockCipher : TlsCipher
    {
        protected readonly TlsContext context;
        protected readonly byte[] randomData;
        protected readonly bool useExplicitIV;
        protected readonly bool encryptThenMac;
        protected readonly IBlockCipher encryptCipher;
        protected readonly IBlockCipher decryptCipher;
        protected readonly TlsMac mWriteMac;
        protected readonly TlsMac mReadMac;

        public TlsBlockCipher(TlsContext context, IBlockCipher clientWriteCipher, IBlockCipher serverWriteCipher, IDigest clientWriteDigest, IDigest serverWriteDigest, int cipherKeySize)
        {
            byte[] buffer2;
            byte[] buffer3;
            ICipherParameters parameters;
            ICipherParameters parameters2;
            this.context = context;
            this.randomData = new byte[0x100];
            context.NonceRandomGenerator.NextBytes(this.randomData);
            this.useExplicitIV = TlsUtilities.IsTlsV11(context);
            this.encryptThenMac = context.SecurityParameters.encryptThenMac;
            int size = ((2 * cipherKeySize) + clientWriteDigest.GetDigestSize()) + serverWriteDigest.GetDigestSize();
            if (!this.useExplicitIV)
            {
                size += clientWriteCipher.GetBlockSize() + serverWriteCipher.GetBlockSize();
            }
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
            if (this.useExplicitIV)
            {
                buffer2 = new byte[clientWriteCipher.GetBlockSize()];
                buffer3 = new byte[serverWriteCipher.GetBlockSize()];
            }
            else
            {
                buffer2 = Arrays.CopyOfRange(key, keyOff, keyOff + clientWriteCipher.GetBlockSize());
                keyOff += clientWriteCipher.GetBlockSize();
                buffer3 = Arrays.CopyOfRange(key, keyOff, keyOff + serverWriteCipher.GetBlockSize());
                keyOff += serverWriteCipher.GetBlockSize();
            }
            if (keyOff != size)
            {
                throw new TlsFatalAlert(80);
            }
            if (context.IsServer)
            {
                this.mWriteMac = mac2;
                this.mReadMac = mac;
                this.encryptCipher = serverWriteCipher;
                this.decryptCipher = clientWriteCipher;
                parameters = new ParametersWithIV(parameter2, buffer3);
                parameters2 = new ParametersWithIV(parameter, buffer2);
            }
            else
            {
                this.mWriteMac = mac;
                this.mReadMac = mac2;
                this.encryptCipher = clientWriteCipher;
                this.decryptCipher = serverWriteCipher;
                parameters = new ParametersWithIV(parameter, buffer2);
                parameters2 = new ParametersWithIV(parameter2, buffer3);
            }
            this.encryptCipher.Init(true, parameters);
            this.decryptCipher.Init(false, parameters2);
        }

        protected virtual int CheckPaddingConstantTime(byte[] buf, int off, int len, int blockSize, int macSize)
        {
            int num = off + len;
            byte num2 = buf[num - 1];
            int num3 = num2 & 0xff;
            int num4 = num3 + 1;
            int num5 = 0;
            byte num6 = 0;
            if ((TlsUtilities.IsSsl(this.context) && (num4 > blockSize)) || ((macSize + num4) > len))
            {
                num4 = 0;
            }
            else
            {
                int num7 = num - num4;
                do
                {
                    num6 = (byte) (num6 | ((byte) (buf[num7++] ^ num2)));
                }
                while (num7 < num);
                num5 = num4;
                if (num6 != 0)
                {
                    num4 = 0;
                }
            }
            byte[] randomData = this.randomData;
            while (num5 < 0x100)
            {
                num6 = (byte) (num6 | ((byte) (randomData[num5++] ^ num2)));
            }
            randomData[0] = (byte) (randomData[0] ^ num6);
            return num4;
        }

        protected virtual int ChooseExtraPadBlocks(SecureRandom r, int max)
        {
            int x = r.NextInt();
            return Math.Min(this.LowestBitSet(x), max);
        }

        public virtual byte[] DecodeCiphertext(long seqNo, byte type, byte[] ciphertext, int offset, int len)
        {
            int blockSize = this.decryptCipher.GetBlockSize();
            int size = this.mReadMac.Size;
            int num3 = blockSize;
            if (this.encryptThenMac)
            {
                num3 += size;
            }
            else
            {
                num3 = Math.Max(num3, size + 1);
            }
            if (this.useExplicitIV)
            {
                num3 += blockSize;
            }
            if (len < num3)
            {
                throw new TlsFatalAlert(50);
            }
            int num4 = len;
            if (this.encryptThenMac)
            {
                num4 -= size;
            }
            if ((num4 % blockSize) != 0)
            {
                throw new TlsFatalAlert(0x15);
            }
            if (this.encryptThenMac)
            {
                int to = offset + len;
                byte[] b = Arrays.CopyOfRange(ciphertext, to - size, to);
                if (!Arrays.ConstantTimeAreEqual(this.mReadMac.CalculateMac(seqNo, type, ciphertext, offset, len - size), b))
                {
                    throw new TlsFatalAlert(20);
                }
            }
            if (this.useExplicitIV)
            {
                this.decryptCipher.Init(false, new ParametersWithIV(null, ciphertext, offset, blockSize));
                offset += blockSize;
                num4 -= blockSize;
            }
            for (int i = 0; i < num4; i += blockSize)
            {
                this.decryptCipher.ProcessBlock(ciphertext, offset + i, ciphertext, offset + i);
            }
            int num7 = this.CheckPaddingConstantTime(ciphertext, offset, num4, blockSize, !this.encryptThenMac ? size : 0);
            bool flag2 = num7 == 0;
            int num8 = num4 - num7;
            if (!this.encryptThenMac)
            {
                num8 -= size;
                int length = num8;
                int from = offset + length;
                byte[] b = Arrays.CopyOfRange(ciphertext, from, from + size);
                byte[] a = this.mReadMac.CalculateMacConstantTime(seqNo, type, ciphertext, offset, length, num4 - size, this.randomData);
                flag2 |= !Arrays.ConstantTimeAreEqual(a, b);
            }
            if (flag2)
            {
                throw new TlsFatalAlert(20);
            }
            return Arrays.CopyOfRange(ciphertext, offset, offset + num8);
        }

        public virtual byte[] EncodePlaintext(long seqNo, byte type, byte[] plaintext, int offset, int len)
        {
            int blockSize = this.encryptCipher.GetBlockSize();
            int size = this.mWriteMac.Size;
            ProtocolVersion serverVersion = this.context.ServerVersion;
            int num3 = len;
            if (!this.encryptThenMac)
            {
                num3 += size;
            }
            int num4 = (blockSize - 1) - (num3 % blockSize);
            if (!serverVersion.IsDtls && !serverVersion.IsSsl)
            {
                int max = (0xff - num4) / blockSize;
                int num6 = this.ChooseExtraPadBlocks(this.context.SecureRandom, max);
                num4 += num6 * blockSize;
            }
            int num7 = ((len + size) + num4) + 1;
            if (this.useExplicitIV)
            {
                num7 += blockSize;
            }
            byte[] destinationArray = new byte[num7];
            int destinationIndex = 0;
            if (this.useExplicitIV)
            {
                byte[] bytes = new byte[blockSize];
                this.context.NonceRandomGenerator.NextBytes(bytes);
                this.encryptCipher.Init(true, new ParametersWithIV(null, bytes));
                Array.Copy(bytes, 0, destinationArray, destinationIndex, blockSize);
                destinationIndex += blockSize;
            }
            int num9 = destinationIndex;
            Array.Copy(plaintext, offset, destinationArray, destinationIndex, len);
            destinationIndex += len;
            if (!this.encryptThenMac)
            {
                byte[] sourceArray = this.mWriteMac.CalculateMac(seqNo, type, plaintext, offset, len);
                Array.Copy(sourceArray, 0, destinationArray, destinationIndex, sourceArray.Length);
                destinationIndex += sourceArray.Length;
            }
            for (int i = 0; i <= num4; i++)
            {
                destinationArray[destinationIndex++] = (byte) num4;
            }
            for (int j = num9; j < destinationIndex; j += blockSize)
            {
                this.encryptCipher.ProcessBlock(destinationArray, j, destinationArray, j);
            }
            if (this.encryptThenMac)
            {
                byte[] sourceArray = this.mWriteMac.CalculateMac(seqNo, type, destinationArray, 0, destinationIndex);
                Array.Copy(sourceArray, 0, destinationArray, destinationIndex, sourceArray.Length);
                destinationIndex += sourceArray.Length;
            }
            return destinationArray;
        }

        public virtual int GetPlaintextLimit(int ciphertextLimit)
        {
            int blockSize = this.encryptCipher.GetBlockSize();
            int size = this.mWriteMac.Size;
            int num3 = ciphertextLimit;
            if (this.useExplicitIV)
            {
                num3 -= blockSize;
            }
            if (this.encryptThenMac)
            {
                num3 -= size;
                num3 -= num3 % blockSize;
            }
            else
            {
                num3 -= num3 % blockSize;
                num3 -= size;
            }
            num3--;
            return num3;
        }

        protected virtual int LowestBitSet(int x)
        {
            if (x == 0)
            {
                return 0x20;
            }
            uint num = (uint) x;
            int num2 = 0;
            while ((num & 1) == 0)
            {
                num2++;
                num = num >> 1;
            }
            return num2;
        }

        public virtual TlsMac WriteMac =>
            this.mWriteMac;

        public virtual TlsMac ReadMac =>
            this.mReadMac;
    }
}

