namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public class OcbBlockCipher : IAeadBlockCipher
    {
        private const int BLOCK_SIZE = 0x10;
        private readonly IBlockCipher hashCipher;
        private readonly IBlockCipher mainCipher;
        private bool forEncryption;
        private int macSize;
        private byte[] initialAssociatedText;
        private IList L;
        private byte[] L_Asterisk;
        private byte[] L_Dollar;
        private byte[] KtopInput;
        private byte[] Stretch = new byte[0x18];
        private byte[] OffsetMAIN_0 = new byte[0x10];
        private byte[] hashBlock;
        private byte[] mainBlock;
        private int hashBlockPos;
        private int mainBlockPos;
        private long hashBlockCount;
        private long mainBlockCount;
        private byte[] OffsetHASH;
        private byte[] Sum;
        private byte[] OffsetMAIN = new byte[0x10];
        private byte[] Checksum;
        private byte[] macBlock;

        public OcbBlockCipher(IBlockCipher hashCipher, IBlockCipher mainCipher)
        {
            if (hashCipher == null)
            {
                throw new ArgumentNullException("hashCipher");
            }
            if (hashCipher.GetBlockSize() != 0x10)
            {
                throw new ArgumentException("must have a block size of " + 0x10, "hashCipher");
            }
            if (mainCipher == null)
            {
                throw new ArgumentNullException("mainCipher");
            }
            if (mainCipher.GetBlockSize() != 0x10)
            {
                throw new ArgumentException("must have a block size of " + 0x10, "mainCipher");
            }
            if (!hashCipher.AlgorithmName.Equals(mainCipher.AlgorithmName))
            {
                throw new ArgumentException("'hashCipher' and 'mainCipher' must be the same algorithm");
            }
            this.hashCipher = hashCipher;
            this.mainCipher = mainCipher;
        }

        protected virtual void Clear(byte[] bs)
        {
            if (bs != null)
            {
                Array.Clear(bs, 0, bs.Length);
            }
        }

        public virtual int DoFinal(byte[] output, int outOff)
        {
            byte[] destinationArray = null;
            if (!this.forEncryption)
            {
                if (this.mainBlockPos < this.macSize)
                {
                    throw new InvalidCipherTextException("data too short");
                }
                this.mainBlockPos -= this.macSize;
                destinationArray = new byte[this.macSize];
                Array.Copy(this.mainBlock, this.mainBlockPos, destinationArray, 0, this.macSize);
            }
            if (this.hashBlockPos > 0)
            {
                OCB_extend(this.hashBlock, this.hashBlockPos);
                this.UpdateHASH(this.L_Asterisk);
            }
            if (this.mainBlockPos > 0)
            {
                if (this.forEncryption)
                {
                    OCB_extend(this.mainBlock, this.mainBlockPos);
                    Xor(this.Checksum, this.mainBlock);
                }
                Xor(this.OffsetMAIN, this.L_Asterisk);
                byte[] outBuf = new byte[0x10];
                this.hashCipher.ProcessBlock(this.OffsetMAIN, 0, outBuf, 0);
                Xor(this.mainBlock, outBuf);
                Check.OutputLength(output, outOff, this.mainBlockPos, "Output buffer too short");
                Array.Copy(this.mainBlock, 0, output, outOff, this.mainBlockPos);
                if (!this.forEncryption)
                {
                    OCB_extend(this.mainBlock, this.mainBlockPos);
                    Xor(this.Checksum, this.mainBlock);
                }
            }
            Xor(this.Checksum, this.OffsetMAIN);
            Xor(this.Checksum, this.L_Dollar);
            this.hashCipher.ProcessBlock(this.Checksum, 0, this.Checksum, 0);
            Xor(this.Checksum, this.Sum);
            this.macBlock = new byte[this.macSize];
            Array.Copy(this.Checksum, 0, this.macBlock, 0, this.macSize);
            int mainBlockPos = this.mainBlockPos;
            if (this.forEncryption)
            {
                Check.OutputLength(output, outOff, mainBlockPos + this.macSize, "Output buffer too short");
                Array.Copy(this.macBlock, 0, output, outOff + mainBlockPos, this.macSize);
                mainBlockPos += this.macSize;
            }
            else if (!Arrays.ConstantTimeAreEqual(this.macBlock, destinationArray))
            {
                throw new InvalidCipherTextException("mac check in OCB failed");
            }
            this.Reset(false);
            return mainBlockPos;
        }

        public virtual int GetBlockSize() => 
            0x10;

        protected virtual byte[] GetLSub(int n)
        {
            while (n >= this.L.Count)
            {
                this.L.Add(OCB_double((byte[]) this.L[this.L.Count - 1]));
            }
            return (byte[]) this.L[n];
        }

        public virtual byte[] GetMac() => 
            Arrays.Clone(this.macBlock);

        public virtual int GetOutputSize(int len)
        {
            int num = len + this.mainBlockPos;
            if (this.forEncryption)
            {
                return (num + this.macSize);
            }
            return ((num >= this.macSize) ? (num - this.macSize) : 0);
        }

        public virtual IBlockCipher GetUnderlyingCipher() => 
            this.mainCipher;

        public virtual int GetUpdateOutputSize(int len)
        {
            int num = len + this.mainBlockPos;
            if (!this.forEncryption)
            {
                if (num < this.macSize)
                {
                    return 0;
                }
                num -= this.macSize;
            }
            return (num - (num % 0x10));
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            KeyParameter key;
            byte[] nonce;
            bool flag = this.forEncryption;
            this.forEncryption = forEncryption;
            this.macBlock = null;
            if (parameters is AeadParameters)
            {
                AeadParameters parameters2 = (AeadParameters) parameters;
                nonce = parameters2.GetNonce();
                this.initialAssociatedText = parameters2.GetAssociatedText();
                int macSize = parameters2.MacSize;
                if (((macSize < 0x40) || (macSize > 0x80)) || ((macSize % 8) != 0))
                {
                    throw new ArgumentException("Invalid value for MAC size: " + macSize);
                }
                this.macSize = macSize / 8;
                key = parameters2.Key;
            }
            else
            {
                if (!(parameters is ParametersWithIV))
                {
                    throw new ArgumentException("invalid parameters passed to OCB");
                }
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                nonce = hiv.GetIV();
                this.initialAssociatedText = null;
                this.macSize = 0x10;
                key = (KeyParameter) hiv.Parameters;
            }
            this.hashBlock = new byte[0x10];
            this.mainBlock = new byte[!forEncryption ? (0x10 + this.macSize) : 0x10];
            if (nonce == null)
            {
                nonce = new byte[0];
            }
            if (nonce.Length > 15)
            {
                throw new ArgumentException("IV must be no more than 15 bytes");
            }
            if (key != null)
            {
                this.hashCipher.Init(true, key);
                this.mainCipher.Init(forEncryption, key);
                this.KtopInput = null;
            }
            else if (flag != forEncryption)
            {
                throw new ArgumentException("cannot change encrypting state without providing key.");
            }
            this.L_Asterisk = new byte[0x10];
            this.hashCipher.ProcessBlock(this.L_Asterisk, 0, this.L_Asterisk, 0);
            this.L_Dollar = OCB_double(this.L_Asterisk);
            this.L = Platform.CreateArrayList();
            this.L.Add(OCB_double(this.L_Dollar));
            int num2 = this.ProcessNonce(nonce);
            int num3 = num2 % 8;
            int sourceIndex = num2 / 8;
            if (num3 == 0)
            {
                Array.Copy(this.Stretch, sourceIndex, this.OffsetMAIN_0, 0, 0x10);
            }
            else
            {
                for (int i = 0; i < 0x10; i++)
                {
                    uint num6 = this.Stretch[sourceIndex];
                    uint num7 = this.Stretch[++sourceIndex];
                    this.OffsetMAIN_0[i] = (byte) ((num6 << num3) | (num7 >> (8 - num3)));
                }
            }
            this.hashBlockPos = 0;
            this.mainBlockPos = 0;
            this.hashBlockCount = 0L;
            this.mainBlockCount = 0L;
            this.OffsetHASH = new byte[0x10];
            this.Sum = new byte[0x10];
            Array.Copy(this.OffsetMAIN_0, 0, this.OffsetMAIN, 0, 0x10);
            this.Checksum = new byte[0x10];
            if (this.initialAssociatedText != null)
            {
                this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
            }
        }

        protected static byte[] OCB_double(byte[] block)
        {
            byte[] output = new byte[0x10];
            int num = ShiftLeft(block, output);
            output[15] = (byte) (output[15] ^ ((byte) (((int) 0x87) >> ((1 - num) << 3))));
            return output;
        }

        protected static void OCB_extend(byte[] block, int pos)
        {
            block[pos] = 0x80;
            while (++pos < 0x10)
            {
                block[pos] = 0;
            }
        }

        protected static int OCB_ntz(long x)
        {
            if (x == 0L)
            {
                return 0x40;
            }
            int num = 0;
            for (ulong i = (ulong) x; (i & ((ulong) 1L)) == 0L; i = i >> 1)
            {
                num++;
            }
            return num;
        }

        public virtual void ProcessAadByte(byte input)
        {
            this.hashBlock[this.hashBlockPos] = input;
            if (++this.hashBlockPos == this.hashBlock.Length)
            {
                this.ProcessHashBlock();
            }
        }

        public virtual void ProcessAadBytes(byte[] input, int off, int len)
        {
            for (int i = 0; i < len; i++)
            {
                this.hashBlock[this.hashBlockPos] = input[off + i];
                if (++this.hashBlockPos == this.hashBlock.Length)
                {
                    this.ProcessHashBlock();
                }
            }
        }

        public virtual int ProcessByte(byte input, byte[] output, int outOff)
        {
            this.mainBlock[this.mainBlockPos] = input;
            if (++this.mainBlockPos == this.mainBlock.Length)
            {
                this.ProcessMainBlock(output, outOff);
                return 0x10;
            }
            return 0;
        }

        public virtual int ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
        {
            int num = 0;
            for (int i = 0; i < len; i++)
            {
                this.mainBlock[this.mainBlockPos] = input[inOff + i];
                if (++this.mainBlockPos == this.mainBlock.Length)
                {
                    this.ProcessMainBlock(output, outOff + num);
                    num += 0x10;
                }
            }
            return num;
        }

        protected virtual void ProcessHashBlock()
        {
            this.UpdateHASH(this.GetLSub(OCB_ntz(this.hashBlockCount += 1L)));
            this.hashBlockPos = 0;
        }

        protected virtual void ProcessMainBlock(byte[] output, int outOff)
        {
            Check.DataLength(output, outOff, 0x10, "Output buffer too short");
            if (this.forEncryption)
            {
                Xor(this.Checksum, this.mainBlock);
                this.mainBlockPos = 0;
            }
            Xor(this.OffsetMAIN, this.GetLSub(OCB_ntz(this.mainBlockCount += 1L)));
            Xor(this.mainBlock, this.OffsetMAIN);
            this.mainCipher.ProcessBlock(this.mainBlock, 0, this.mainBlock, 0);
            Xor(this.mainBlock, this.OffsetMAIN);
            Array.Copy(this.mainBlock, 0, output, outOff, 0x10);
            if (!this.forEncryption)
            {
                Xor(this.Checksum, this.mainBlock);
                Array.Copy(this.mainBlock, 0x10, this.mainBlock, 0, this.macSize);
                this.mainBlockPos = this.macSize;
            }
        }

        protected virtual int ProcessNonce(byte[] N)
        {
            byte[] destinationArray = new byte[0x10];
            Array.Copy(N, 0, destinationArray, destinationArray.Length - N.Length, N.Length);
            destinationArray[0] = (byte) (this.macSize << 4);
            destinationArray[15 - N.Length] = (byte) (destinationArray[15 - N.Length] | 1);
            int num = destinationArray[15] & 0x3f;
            destinationArray[15] = (byte) (destinationArray[15] & 0xc0);
            if ((this.KtopInput == null) || !Arrays.AreEqual(destinationArray, this.KtopInput))
            {
                byte[] outBuf = new byte[0x10];
                this.KtopInput = destinationArray;
                this.hashCipher.ProcessBlock(this.KtopInput, 0, outBuf, 0);
                Array.Copy(outBuf, 0, this.Stretch, 0, 0x10);
                for (int i = 0; i < 8; i++)
                {
                    this.Stretch[0x10 + i] = (byte) (outBuf[i] ^ outBuf[i + 1]);
                }
            }
            return num;
        }

        public virtual void Reset()
        {
            this.Reset(true);
        }

        protected virtual void Reset(bool clearMac)
        {
            this.hashCipher.Reset();
            this.mainCipher.Reset();
            this.Clear(this.hashBlock);
            this.Clear(this.mainBlock);
            this.hashBlockPos = 0;
            this.mainBlockPos = 0;
            this.hashBlockCount = 0L;
            this.mainBlockCount = 0L;
            this.Clear(this.OffsetHASH);
            this.Clear(this.Sum);
            Array.Copy(this.OffsetMAIN_0, 0, this.OffsetMAIN, 0, 0x10);
            this.Clear(this.Checksum);
            if (clearMac)
            {
                this.macBlock = null;
            }
            if (this.initialAssociatedText != null)
            {
                this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
            }
        }

        protected static int ShiftLeft(byte[] block, byte[] output)
        {
            int index = 0x10;
            uint num2 = 0;
            while (--index >= 0)
            {
                uint num3 = block[index];
                output[index] = (byte) ((num3 << 1) | num2);
                num2 = (num3 >> 7) & 1;
            }
            return (int) num2;
        }

        protected virtual void UpdateHASH(byte[] LSub)
        {
            Xor(this.OffsetHASH, LSub);
            Xor(this.hashBlock, this.OffsetHASH);
            this.hashCipher.ProcessBlock(this.hashBlock, 0, this.hashBlock, 0);
            Xor(this.Sum, this.hashBlock);
        }

        protected static void Xor(byte[] block, byte[] val)
        {
            for (int i = 15; i >= 0; i--)
            {
                block[i] = (byte) (block[i] ^ val[i]);
            }
        }

        public virtual string AlgorithmName =>
            (this.mainCipher.AlgorithmName + "/OCB");
    }
}

