namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Macs;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class EaxBlockCipher : IAeadBlockCipher
    {
        private SicBlockCipher cipher;
        private bool forEncryption;
        private int blockSize;
        private IMac mac;
        private byte[] nonceMac;
        private byte[] associatedTextMac;
        private byte[] macBlock;
        private int macSize;
        private byte[] bufBlock;
        private int bufOff;
        private bool cipherInitialized;
        private byte[] initialAssociatedText;

        public EaxBlockCipher(IBlockCipher cipher)
        {
            this.blockSize = cipher.GetBlockSize();
            this.mac = new CMac(cipher);
            this.macBlock = new byte[this.blockSize];
            this.associatedTextMac = new byte[this.mac.GetMacSize()];
            this.nonceMac = new byte[this.mac.GetMacSize()];
            this.cipher = new SicBlockCipher(cipher);
        }

        private void CalculateMac()
        {
            byte[] output = new byte[this.blockSize];
            this.mac.DoFinal(output, 0);
            for (int i = 0; i < this.macBlock.Length; i++)
            {
                this.macBlock[i] = (byte) ((this.nonceMac[i] ^ this.associatedTextMac[i]) ^ output[i]);
            }
        }

        public virtual int DoFinal(byte[] outBytes, int outOff)
        {
            this.InitCipher();
            int bufOff = this.bufOff;
            byte[] output = new byte[this.bufBlock.Length];
            this.bufOff = 0;
            if (this.forEncryption)
            {
                Check.OutputLength(outBytes, outOff, bufOff + this.macSize, "Output buffer too short");
                this.cipher.ProcessBlock(this.bufBlock, 0, output, 0);
                Array.Copy(output, 0, outBytes, outOff, bufOff);
                this.mac.BlockUpdate(output, 0, bufOff);
                this.CalculateMac();
                Array.Copy(this.macBlock, 0, outBytes, outOff + bufOff, this.macSize);
                this.Reset(false);
                return (bufOff + this.macSize);
            }
            if (bufOff < this.macSize)
            {
                throw new InvalidCipherTextException("data too short");
            }
            Check.OutputLength(outBytes, outOff, bufOff - this.macSize, "Output buffer too short");
            if (bufOff > this.macSize)
            {
                this.mac.BlockUpdate(this.bufBlock, 0, bufOff - this.macSize);
                this.cipher.ProcessBlock(this.bufBlock, 0, output, 0);
                Array.Copy(output, 0, outBytes, outOff, bufOff - this.macSize);
            }
            this.CalculateMac();
            if (!this.VerifyMac(this.bufBlock, bufOff - this.macSize))
            {
                throw new InvalidCipherTextException("mac check in EAX failed");
            }
            this.Reset(false);
            return (bufOff - this.macSize);
        }

        public virtual int GetBlockSize() => 
            this.cipher.GetBlockSize();

        public virtual byte[] GetMac()
        {
            byte[] destinationArray = new byte[this.macSize];
            Array.Copy(this.macBlock, 0, destinationArray, 0, this.macSize);
            return destinationArray;
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

        public virtual IBlockCipher GetUnderlyingCipher() => 
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
            return (num - (num % this.blockSize));
        }

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            byte[] nonce;
            ICipherParameters key;
            this.forEncryption = forEncryption;
            if (parameters is AeadParameters)
            {
                AeadParameters parameters3 = (AeadParameters) parameters;
                nonce = parameters3.GetNonce();
                this.initialAssociatedText = parameters3.GetAssociatedText();
                this.macSize = parameters3.MacSize / 8;
                key = parameters3.Key;
            }
            else
            {
                if (!(parameters is ParametersWithIV))
                {
                    throw new ArgumentException("invalid parameters passed to EAX");
                }
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                nonce = hiv.GetIV();
                this.initialAssociatedText = null;
                this.macSize = this.mac.GetMacSize() / 2;
                key = hiv.Parameters;
            }
            this.bufBlock = new byte[!forEncryption ? (this.blockSize + this.macSize) : this.blockSize];
            byte[] input = new byte[this.blockSize];
            this.mac.Init(key);
            input[this.blockSize - 1] = 0;
            this.mac.BlockUpdate(input, 0, this.blockSize);
            this.mac.BlockUpdate(nonce, 0, nonce.Length);
            this.mac.DoFinal(this.nonceMac, 0);
            this.cipher.Init(true, new ParametersWithIV(null, this.nonceMac));
            this.Reset();
        }

        private void InitCipher()
        {
            if (!this.cipherInitialized)
            {
                this.cipherInitialized = true;
                this.mac.DoFinal(this.associatedTextMac, 0);
                byte[] input = new byte[this.blockSize];
                input[this.blockSize - 1] = 2;
                this.mac.BlockUpdate(input, 0, this.blockSize);
            }
        }

        private int Process(byte b, byte[] outBytes, int outOff)
        {
            int num2;
            this.bufBlock[this.bufOff++] = b;
            if (this.bufOff != this.bufBlock.Length)
            {
                return 0;
            }
            Check.OutputLength(outBytes, outOff, this.blockSize, "Output buffer is too short");
            if (this.forEncryption)
            {
                num2 = this.cipher.ProcessBlock(this.bufBlock, 0, outBytes, outOff);
                this.mac.BlockUpdate(outBytes, outOff, this.blockSize);
            }
            else
            {
                this.mac.BlockUpdate(this.bufBlock, 0, this.blockSize);
                num2 = this.cipher.ProcessBlock(this.bufBlock, 0, outBytes, outOff);
            }
            this.bufOff = 0;
            if (!this.forEncryption)
            {
                Array.Copy(this.bufBlock, this.blockSize, this.bufBlock, 0, this.macSize);
                this.bufOff = this.macSize;
            }
            return num2;
        }

        public virtual void ProcessAadByte(byte input)
        {
            if (this.cipherInitialized)
            {
                throw new InvalidOperationException("AAD data cannot be added after encryption/decryption processing has begun.");
            }
            this.mac.Update(input);
        }

        public virtual void ProcessAadBytes(byte[] inBytes, int inOff, int len)
        {
            if (this.cipherInitialized)
            {
                throw new InvalidOperationException("AAD data cannot be added after encryption/decryption processing has begun.");
            }
            this.mac.BlockUpdate(inBytes, inOff, len);
        }

        public virtual int ProcessByte(byte input, byte[] outBytes, int outOff)
        {
            this.InitCipher();
            return this.Process(input, outBytes, outOff);
        }

        public virtual int ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff)
        {
            this.InitCipher();
            int num = 0;
            for (int i = 0; i != len; i++)
            {
                num += this.Process(inBytes[inOff + i], outBytes, outOff + num);
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
            this.mac.Reset();
            this.bufOff = 0;
            Array.Clear(this.bufBlock, 0, this.bufBlock.Length);
            if (clearMac)
            {
                Array.Clear(this.macBlock, 0, this.macBlock.Length);
            }
            byte[] input = new byte[this.blockSize];
            input[this.blockSize - 1] = 1;
            this.mac.BlockUpdate(input, 0, this.blockSize);
            this.cipherInitialized = false;
            if (this.initialAssociatedText != null)
            {
                this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
            }
        }

        private bool VerifyMac(byte[] mac, int off)
        {
            int num = 0;
            for (int i = 0; i < this.macSize; i++)
            {
                num |= this.macBlock[i] ^ mac[off + i];
            }
            return (num == 0);
        }

        public virtual string AlgorithmName =>
            (this.cipher.GetUnderlyingCipher().AlgorithmName + "/EAX");

        private enum Tag : byte
        {
            N = 0,
            H = 1,
            C = 2
        }
    }
}

