namespace Org.BouncyCastle.Crypto.Macs
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Paddings;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class CMac : IMac
    {
        private const byte CONSTANT_128 = 0x87;
        private const byte CONSTANT_64 = 0x1b;
        private byte[] ZEROES;
        private byte[] mac;
        private byte[] buf;
        private int bufOff;
        private IBlockCipher cipher;
        private int macSize;
        private byte[] L;
        private byte[] Lu;
        private byte[] Lu2;

        public CMac(IBlockCipher cipher) : this(cipher, cipher.GetBlockSize() * 8)
        {
        }

        public CMac(IBlockCipher cipher, int macSizeInBits)
        {
            if ((macSizeInBits % 8) != 0)
            {
                throw new ArgumentException("MAC size must be multiple of 8");
            }
            if (macSizeInBits > (cipher.GetBlockSize() * 8))
            {
                throw new ArgumentException("MAC size must be less or equal to " + (cipher.GetBlockSize() * 8));
            }
            if ((cipher.GetBlockSize() != 8) && (cipher.GetBlockSize() != 0x10))
            {
                throw new ArgumentException("Block size must be either 64 or 128 bits");
            }
            this.cipher = new CbcBlockCipher(cipher);
            this.macSize = macSizeInBits / 8;
            this.mac = new byte[cipher.GetBlockSize()];
            this.buf = new byte[cipher.GetBlockSize()];
            this.ZEROES = new byte[cipher.GetBlockSize()];
            this.bufOff = 0;
        }

        public void BlockUpdate(byte[] inBytes, int inOff, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("Can't have a negative input length!");
            }
            int blockSize = this.cipher.GetBlockSize();
            int length = blockSize - this.bufOff;
            if (len > length)
            {
                Array.Copy(inBytes, inOff, this.buf, this.bufOff, length);
                this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
                this.bufOff = 0;
                len -= length;
                inOff += length;
                while (len > blockSize)
                {
                    this.cipher.ProcessBlock(inBytes, inOff, this.mac, 0);
                    len -= blockSize;
                    inOff += blockSize;
                }
            }
            Array.Copy(inBytes, inOff, this.buf, this.bufOff, len);
            this.bufOff += len;
        }

        public int DoFinal(byte[] outBytes, int outOff)
        {
            byte[] lu;
            int blockSize = this.cipher.GetBlockSize();
            if (this.bufOff == blockSize)
            {
                lu = this.Lu;
            }
            else
            {
                new ISO7816d4Padding().AddPadding(this.buf, this.bufOff);
                lu = this.Lu2;
            }
            for (int i = 0; i < this.mac.Length; i++)
            {
                this.buf[i] = (byte) (this.buf[i] ^ lu[i]);
            }
            this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
            Array.Copy(this.mac, 0, outBytes, outOff, this.macSize);
            this.Reset();
            return this.macSize;
        }

        private static byte[] DoubleLu(byte[] input)
        {
            byte[] output = new byte[input.Length];
            int num = ShiftLeft(input, output);
            int num2 = (input.Length != 0x10) ? 0x1b : 0x87;
            output[input.Length - 1] = (byte) (output[input.Length - 1] ^ ((byte) (num2 >> ((1 - num) << 3))));
            return output;
        }

        public int GetMacSize() => 
            this.macSize;

        public void Init(ICipherParameters parameters)
        {
            if (parameters is KeyParameter)
            {
                this.cipher.Init(true, parameters);
                this.L = new byte[this.ZEROES.Length];
                this.cipher.ProcessBlock(this.ZEROES, 0, this.L, 0);
                this.Lu = DoubleLu(this.L);
                this.Lu2 = DoubleLu(this.Lu);
            }
            else if (parameters != null)
            {
                throw new ArgumentException("CMac mode only permits key to be set.", "parameters");
            }
            this.Reset();
        }

        public void Reset()
        {
            Array.Clear(this.buf, 0, this.buf.Length);
            this.bufOff = 0;
            this.cipher.Reset();
        }

        private static int ShiftLeft(byte[] block, byte[] output)
        {
            int length = block.Length;
            uint num2 = 0;
            while (--length >= 0)
            {
                uint num3 = block[length];
                output[length] = (byte) ((num3 << 1) | num2);
                num2 = (num3 >> 7) & 1;
            }
            return (int) num2;
        }

        public void Update(byte input)
        {
            if (this.bufOff == this.buf.Length)
            {
                this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
                this.bufOff = 0;
            }
            this.buf[this.bufOff++] = input;
        }

        public string AlgorithmName =>
            this.cipher.AlgorithmName;
    }
}

