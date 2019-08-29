namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Modes.Gcm;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class GcmBlockCipher : IAeadBlockCipher
    {
        private const int BlockSize = 0x10;
        private readonly IBlockCipher cipher;
        private readonly IGcmMultiplier multiplier;
        private IGcmExponentiator exp;
        private bool forEncryption;
        private int macSize;
        private byte[] nonce;
        private byte[] initialAssociatedText;
        private byte[] H;
        private byte[] J0;
        private byte[] bufBlock;
        private byte[] macBlock;
        private byte[] S;
        private byte[] S_at;
        private byte[] S_atPre;
        private byte[] counter;
        private uint blocksRemaining;
        private int bufOff;
        private ulong totalLength;
        private byte[] atBlock;
        private int atBlockPos;
        private ulong atLength;
        private ulong atLengthPre;

        public GcmBlockCipher(IBlockCipher c) : this(c, null)
        {
        }

        public GcmBlockCipher(IBlockCipher c, IGcmMultiplier m)
        {
            if (c.GetBlockSize() != 0x10)
            {
                throw new ArgumentException("cipher required with a block size of " + 0x10 + ".");
            }
            if (m == null)
            {
                m = new Tables8kGcmMultiplier();
            }
            this.cipher = c;
            this.multiplier = m;
        }

        public int DoFinal(byte[] output, int outOff)
        {
            if (this.totalLength == 0L)
            {
                this.InitCipher();
            }
            int bufOff = this.bufOff;
            if (this.forEncryption)
            {
                Check.OutputLength(output, outOff, bufOff + this.macSize, "Output buffer too short");
            }
            else
            {
                if (bufOff < this.macSize)
                {
                    throw new InvalidCipherTextException("data too short");
                }
                bufOff -= this.macSize;
                Check.OutputLength(output, outOff, bufOff, "Output buffer too short");
            }
            if (bufOff > 0)
            {
                this.gCTRPartial(this.bufBlock, 0, bufOff, output, outOff);
            }
            this.atLength += (ulong) this.atBlockPos;
            if (this.atLength > this.atLengthPre)
            {
                if (this.atBlockPos > 0)
                {
                    this.gHASHPartial(this.S_at, this.atBlock, 0, this.atBlockPos);
                }
                if (this.atLengthPre > 0L)
                {
                    GcmUtilities.Xor(this.S_at, this.S_atPre);
                }
                long pow = (long) (((this.totalLength * 8L) + 0x7fL) >> 7);
                byte[] buffer = new byte[0x10];
                if (this.exp == null)
                {
                    this.exp = new Tables1kGcmExponentiator();
                    this.exp.Init(this.H);
                }
                this.exp.ExponentiateX(pow, buffer);
                GcmUtilities.Multiply(this.S_at, buffer);
                GcmUtilities.Xor(this.S, this.S_at);
            }
            byte[] bs = new byte[0x10];
            Pack.UInt64_To_BE((ulong) (this.atLength * ((ulong) 8L)), bs, 0);
            Pack.UInt64_To_BE((ulong) (this.totalLength * ((ulong) 8L)), bs, 8);
            this.gHASHBlock(this.S, bs);
            byte[] outBuf = new byte[0x10];
            this.cipher.ProcessBlock(this.J0, 0, outBuf, 0);
            GcmUtilities.Xor(outBuf, this.S);
            int num3 = bufOff;
            this.macBlock = new byte[this.macSize];
            Array.Copy(outBuf, 0, this.macBlock, 0, this.macSize);
            if (this.forEncryption)
            {
                Array.Copy(this.macBlock, 0, output, outOff + this.bufOff, this.macSize);
                num3 += this.macSize;
            }
            else
            {
                byte[] destinationArray = new byte[this.macSize];
                Array.Copy(this.bufBlock, bufOff, destinationArray, 0, this.macSize);
                if (!Arrays.ConstantTimeAreEqual(this.macBlock, destinationArray))
                {
                    throw new InvalidCipherTextException("mac check in GCM failed");
                }
            }
            this.Reset(false);
            return num3;
        }

        private void gCTRBlock(byte[] block, byte[] output, int outOff)
        {
            byte[] nextCounterBlock = this.GetNextCounterBlock();
            GcmUtilities.Xor(nextCounterBlock, block);
            Array.Copy(nextCounterBlock, 0, output, outOff, 0x10);
            this.gHASHBlock(this.S, !this.forEncryption ? block : nextCounterBlock);
            this.totalLength += (ulong) 0x10L;
        }

        private void gCTRPartial(byte[] buf, int off, int len, byte[] output, int outOff)
        {
            byte[] nextCounterBlock = this.GetNextCounterBlock();
            GcmUtilities.Xor(nextCounterBlock, buf, off, len);
            Array.Copy(nextCounterBlock, 0, output, outOff, len);
            this.gHASHPartial(this.S, !this.forEncryption ? buf : nextCounterBlock, 0, len);
            this.totalLength += (ulong) len;
        }

        public virtual int GetBlockSize() => 
            0x10;

        public virtual byte[] GetMac() => 
            Arrays.Clone(this.macBlock);

        private byte[] GetNextCounterBlock()
        {
            if (this.blocksRemaining == 0)
            {
                throw new InvalidOperationException("Attempt to process too many blocks");
            }
            this.blocksRemaining--;
            uint num = 1;
            num += this.counter[15];
            this.counter[15] = (byte) num;
            num = num >> 8;
            num += this.counter[14];
            this.counter[14] = (byte) num;
            num = num >> 8;
            num += this.counter[13];
            this.counter[13] = (byte) num;
            num = num >> 8;
            num += this.counter[12];
            this.counter[12] = (byte) num;
            byte[] outBuf = new byte[0x10];
            this.cipher.ProcessBlock(this.counter, 0, outBuf, 0);
            return outBuf;
        }

        public virtual int GetOutputSize(int len)
        {
            int num = len + this.bufOff;
            if (this.forEncryption)
            {
                return (num + this.macSize);
            }
            return ((num >= this.macSize) ? (num - this.macSize) : 0);
        }

        public IBlockCipher GetUnderlyingCipher() => 
            this.cipher;

        public virtual int GetUpdateOutputSize(int len)
        {
            int num = len + this.bufOff;
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

        private void gHASH(byte[] Y, byte[] b, int len)
        {
            for (int i = 0; i < len; i += 0x10)
            {
                int num2 = Math.Min(len - i, 0x10);
                this.gHASHPartial(Y, b, i, num2);
            }
        }

        private void gHASHBlock(byte[] Y, byte[] b)
        {
            GcmUtilities.Xor(Y, b);
            this.multiplier.MultiplyH(Y);
        }

        private void gHASHPartial(byte[] Y, byte[] b, int off, int len)
        {
            GcmUtilities.Xor(Y, b, off, len);
            this.multiplier.MultiplyH(Y);
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            KeyParameter key;
            this.forEncryption = forEncryption;
            this.macBlock = null;
            if (parameters is AeadParameters)
            {
                AeadParameters parameters2 = (AeadParameters) parameters;
                this.nonce = parameters2.GetNonce();
                this.initialAssociatedText = parameters2.GetAssociatedText();
                int macSize = parameters2.MacSize;
                if (((macSize < 0x20) || (macSize > 0x80)) || ((macSize % 8) != 0))
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
                    throw new ArgumentException("invalid parameters passed to GCM");
                }
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                this.nonce = hiv.GetIV();
                this.initialAssociatedText = null;
                this.macSize = 0x10;
                key = (KeyParameter) hiv.Parameters;
            }
            int num2 = !forEncryption ? (0x10 + this.macSize) : 0x10;
            this.bufBlock = new byte[num2];
            if ((this.nonce == null) || (this.nonce.Length < 1))
            {
                throw new ArgumentException("IV must be at least 1 byte");
            }
            if (key != null)
            {
                this.cipher.Init(true, key);
                this.H = new byte[0x10];
                this.cipher.ProcessBlock(this.H, 0, this.H, 0);
                this.multiplier.Init(this.H);
                this.exp = null;
            }
            else if (this.H == null)
            {
                throw new ArgumentException("Key must be specified in initial init");
            }
            this.J0 = new byte[0x10];
            if (this.nonce.Length == 12)
            {
                Array.Copy(this.nonce, 0, this.J0, 0, this.nonce.Length);
                this.J0[15] = 1;
            }
            else
            {
                this.gHASH(this.J0, this.nonce, this.nonce.Length);
                byte[] bs = new byte[0x10];
                Pack.UInt64_To_BE((ulong) (this.nonce.Length * 8L), bs, 8);
                this.gHASHBlock(this.J0, bs);
            }
            this.S = new byte[0x10];
            this.S_at = new byte[0x10];
            this.S_atPre = new byte[0x10];
            this.atBlock = new byte[0x10];
            this.atBlockPos = 0;
            this.atLength = 0L;
            this.atLengthPre = 0L;
            this.counter = Arrays.Clone(this.J0);
            this.blocksRemaining = 0xfffffffe;
            this.bufOff = 0;
            this.totalLength = 0L;
            if (this.initialAssociatedText != null)
            {
                this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
            }
        }

        private void InitCipher()
        {
            if (this.atLength > 0L)
            {
                Array.Copy(this.S_at, 0, this.S_atPre, 0, 0x10);
                this.atLengthPre = this.atLength;
            }
            if (this.atBlockPos > 0)
            {
                this.gHASHPartial(this.S_atPre, this.atBlock, 0, this.atBlockPos);
                this.atLengthPre += (ulong) this.atBlockPos;
            }
            if (this.atLengthPre > 0L)
            {
                Array.Copy(this.S_atPre, 0, this.S, 0, 0x10);
            }
        }

        private void OutputBlock(byte[] output, int offset)
        {
            Check.OutputLength(output, offset, 0x10, "Output buffer too short");
            if (this.totalLength == 0L)
            {
                this.InitCipher();
            }
            this.gCTRBlock(this.bufBlock, output, offset);
            if (this.forEncryption)
            {
                this.bufOff = 0;
            }
            else
            {
                Array.Copy(this.bufBlock, 0x10, this.bufBlock, 0, this.macSize);
                this.bufOff = this.macSize;
            }
        }

        public virtual void ProcessAadByte(byte input)
        {
            this.atBlock[this.atBlockPos] = input;
            if (++this.atBlockPos == 0x10)
            {
                this.gHASHBlock(this.S_at, this.atBlock);
                this.atBlockPos = 0;
                this.atLength += (ulong) 0x10L;
            }
        }

        public virtual void ProcessAadBytes(byte[] inBytes, int inOff, int len)
        {
            for (int i = 0; i < len; i++)
            {
                this.atBlock[this.atBlockPos] = inBytes[inOff + i];
                if (++this.atBlockPos == 0x10)
                {
                    this.gHASHBlock(this.S_at, this.atBlock);
                    this.atBlockPos = 0;
                    this.atLength += (ulong) 0x10L;
                }
            }
        }

        public virtual int ProcessByte(byte input, byte[] output, int outOff)
        {
            this.bufBlock[this.bufOff] = input;
            if (++this.bufOff == this.bufBlock.Length)
            {
                this.OutputBlock(output, outOff);
                return 0x10;
            }
            return 0;
        }

        public virtual int ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
        {
            if (input.Length < (inOff + len))
            {
                throw new DataLengthException("Input buffer too short");
            }
            int num = 0;
            for (int i = 0; i < len; i++)
            {
                this.bufBlock[this.bufOff] = input[inOff + i];
                if (++this.bufOff == this.bufBlock.Length)
                {
                    this.OutputBlock(output, outOff + num);
                    num += 0x10;
                }
            }
            return num;
        }

        public virtual void Reset()
        {
            this.Reset(true);
        }

        private void Reset(bool clearMac)
        {
            this.cipher.Reset();
            this.S = new byte[0x10];
            this.S_at = new byte[0x10];
            this.S_atPre = new byte[0x10];
            this.atBlock = new byte[0x10];
            this.atBlockPos = 0;
            this.atLength = 0L;
            this.atLengthPre = 0L;
            this.counter = Arrays.Clone(this.J0);
            this.blocksRemaining = 0xfffffffe;
            this.bufOff = 0;
            this.totalLength = 0L;
            if (this.bufBlock != null)
            {
                Arrays.Fill(this.bufBlock, 0);
            }
            if (clearMac)
            {
                this.macBlock = null;
            }
            if (this.initialAssociatedText != null)
            {
                this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
            }
        }

        public virtual string AlgorithmName =>
            (this.cipher.AlgorithmName + "/GCM");
    }
}

