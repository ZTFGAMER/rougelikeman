namespace Org.BouncyCastle.Crypto.Macs
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Paddings;
    using System;

    public class CfbBlockCipherMac : IMac
    {
        private byte[] mac;
        private byte[] Buffer;
        private int bufOff;
        private MacCFBBlockCipher cipher;
        private IBlockCipherPadding padding;
        private int macSize;

        public CfbBlockCipherMac(IBlockCipher cipher) : this(cipher, 8, (cipher.GetBlockSize() * 8) / 2, null)
        {
        }

        public CfbBlockCipherMac(IBlockCipher cipher, IBlockCipherPadding padding) : this(cipher, 8, (cipher.GetBlockSize() * 8) / 2, padding)
        {
        }

        public CfbBlockCipherMac(IBlockCipher cipher, int cfbBitSize, int macSizeInBits) : this(cipher, cfbBitSize, macSizeInBits, null)
        {
        }

        public CfbBlockCipherMac(IBlockCipher cipher, int cfbBitSize, int macSizeInBits, IBlockCipherPadding padding)
        {
            if ((macSizeInBits % 8) != 0)
            {
                throw new ArgumentException("MAC size must be multiple of 8");
            }
            this.mac = new byte[cipher.GetBlockSize()];
            this.cipher = new MacCFBBlockCipher(cipher, cfbBitSize);
            this.padding = padding;
            this.macSize = macSizeInBits / 8;
            this.Buffer = new byte[this.cipher.GetBlockSize()];
            this.bufOff = 0;
        }

        public void BlockUpdate(byte[] input, int inOff, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("Can't have a negative input length!");
            }
            int blockSize = this.cipher.GetBlockSize();
            int num2 = 0;
            int length = blockSize - this.bufOff;
            if (len > length)
            {
                Array.Copy(input, inOff, this.Buffer, this.bufOff, length);
                num2 += this.cipher.ProcessBlock(this.Buffer, 0, this.mac, 0);
                this.bufOff = 0;
                len -= length;
                inOff += length;
                while (len > blockSize)
                {
                    num2 += this.cipher.ProcessBlock(input, inOff, this.mac, 0);
                    len -= blockSize;
                    inOff += blockSize;
                }
            }
            Array.Copy(input, inOff, this.Buffer, this.bufOff, len);
            this.bufOff += len;
        }

        public int DoFinal(byte[] output, int outOff)
        {
            int blockSize = this.cipher.GetBlockSize();
            if (this.padding == null)
            {
                while (this.bufOff < blockSize)
                {
                    this.Buffer[this.bufOff++] = 0;
                }
            }
            else
            {
                this.padding.AddPadding(this.Buffer, this.bufOff);
            }
            this.cipher.ProcessBlock(this.Buffer, 0, this.mac, 0);
            this.cipher.GetMacBlock(this.mac);
            Array.Copy(this.mac, 0, output, outOff, this.macSize);
            this.Reset();
            return this.macSize;
        }

        public int GetMacSize() => 
            this.macSize;

        public void Init(ICipherParameters parameters)
        {
            this.Reset();
            this.cipher.Init(true, parameters);
        }

        public void Reset()
        {
            Array.Clear(this.Buffer, 0, this.Buffer.Length);
            this.bufOff = 0;
            this.cipher.Reset();
        }

        public void Update(byte input)
        {
            if (this.bufOff == this.Buffer.Length)
            {
                this.cipher.ProcessBlock(this.Buffer, 0, this.mac, 0);
                this.bufOff = 0;
            }
            this.Buffer[this.bufOff++] = input;
        }

        public string AlgorithmName =>
            this.cipher.AlgorithmName;
    }
}

