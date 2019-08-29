namespace Org.BouncyCastle.Crypto.Macs
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Paddings;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class ISO9797Alg3Mac : IMac
    {
        private byte[] mac;
        private byte[] buf;
        private int bufOff;
        private IBlockCipher cipher;
        private IBlockCipherPadding padding;
        private int macSize;
        private KeyParameter lastKey2;
        private KeyParameter lastKey3;

        public ISO9797Alg3Mac(IBlockCipher cipher) : this(cipher, cipher.GetBlockSize() * 8, null)
        {
        }

        public ISO9797Alg3Mac(IBlockCipher cipher, IBlockCipherPadding padding) : this(cipher, cipher.GetBlockSize() * 8, padding)
        {
        }

        public ISO9797Alg3Mac(IBlockCipher cipher, int macSizeInBits) : this(cipher, macSizeInBits, null)
        {
        }

        public ISO9797Alg3Mac(IBlockCipher cipher, int macSizeInBits, IBlockCipherPadding padding)
        {
            if ((macSizeInBits % 8) != 0)
            {
                throw new ArgumentException("MAC size must be multiple of 8");
            }
            if (!(cipher is DesEngine))
            {
                throw new ArgumentException("cipher must be instance of DesEngine");
            }
            this.cipher = new CbcBlockCipher(cipher);
            this.padding = padding;
            this.macSize = macSizeInBits / 8;
            this.mac = new byte[cipher.GetBlockSize()];
            this.buf = new byte[cipher.GetBlockSize()];
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
                Array.Copy(input, inOff, this.buf, this.bufOff, length);
                num2 += this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
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
            Array.Copy(input, inOff, this.buf, this.bufOff, len);
            this.bufOff += len;
        }

        public int DoFinal(byte[] output, int outOff)
        {
            int blockSize = this.cipher.GetBlockSize();
            if (this.padding == null)
            {
                while (this.bufOff < blockSize)
                {
                    this.buf[this.bufOff++] = 0;
                }
            }
            else
            {
                if (this.bufOff == blockSize)
                {
                    this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
                    this.bufOff = 0;
                }
                this.padding.AddPadding(this.buf, this.bufOff);
            }
            this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
            DesEngine engine = new DesEngine();
            engine.Init(false, this.lastKey2);
            engine.ProcessBlock(this.mac, 0, this.mac, 0);
            engine.Init(true, this.lastKey3);
            engine.ProcessBlock(this.mac, 0, this.mac, 0);
            Array.Copy(this.mac, 0, output, outOff, this.macSize);
            this.Reset();
            return this.macSize;
        }

        public int GetMacSize() => 
            this.macSize;

        public void Init(ICipherParameters parameters)
        {
            KeyParameter parameter;
            KeyParameter parameter2;
            this.Reset();
            if (!(parameters is KeyParameter) && !(parameters is ParametersWithIV))
            {
                throw new ArgumentException("parameters must be an instance of KeyParameter or ParametersWithIV");
            }
            if (parameters is KeyParameter)
            {
                parameter = (KeyParameter) parameters;
            }
            else
            {
                parameter = (KeyParameter) ((ParametersWithIV) parameters).Parameters;
            }
            byte[] key = parameter.GetKey();
            if (key.Length == 0x10)
            {
                parameter2 = new KeyParameter(key, 0, 8);
                this.lastKey2 = new KeyParameter(key, 8, 8);
                this.lastKey3 = parameter2;
            }
            else
            {
                if (key.Length != 0x18)
                {
                    throw new ArgumentException("Key must be either 112 or 168 bit long");
                }
                parameter2 = new KeyParameter(key, 0, 8);
                this.lastKey2 = new KeyParameter(key, 8, 8);
                this.lastKey3 = new KeyParameter(key, 0x10, 8);
            }
            if (parameters is ParametersWithIV)
            {
                this.cipher.Init(true, new ParametersWithIV(parameter2, ((ParametersWithIV) parameters).GetIV()));
            }
            else
            {
                this.cipher.Init(true, parameter2);
            }
        }

        public void Reset()
        {
            Array.Clear(this.buf, 0, this.buf.Length);
            this.bufOff = 0;
            this.cipher.Reset();
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
            "ISO9797Alg3";
    }
}

