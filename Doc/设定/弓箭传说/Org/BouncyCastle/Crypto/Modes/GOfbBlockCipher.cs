namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class GOfbBlockCipher : IBlockCipher
    {
        private byte[] IV;
        private byte[] ofbV;
        private byte[] ofbOutV;
        private readonly int blockSize;
        private readonly IBlockCipher cipher;
        private bool firstStep = true;
        private int N3;
        private int N4;
        private const int C1 = 0x1010104;
        private const int C2 = 0x1010101;

        public GOfbBlockCipher(IBlockCipher cipher)
        {
            this.cipher = cipher;
            this.blockSize = cipher.GetBlockSize();
            if (this.blockSize != 8)
            {
                throw new ArgumentException("GCTR only for 64 bit block ciphers");
            }
            this.IV = new byte[cipher.GetBlockSize()];
            this.ofbV = new byte[cipher.GetBlockSize()];
            this.ofbOutV = new byte[cipher.GetBlockSize()];
        }

        private int bytesToint(byte[] inBytes, int inOff) => 
            (((((inBytes[inOff + 3] << 0x18) & 0xff000000L) + ((inBytes[inOff + 2] << 0x10) & 0xff0000)) + ((inBytes[inOff + 1] << 8) & 0xff00)) + (inBytes[inOff] & 0xff));

        public int GetBlockSize() => 
            this.blockSize;

        public IBlockCipher GetUnderlyingCipher() => 
            this.cipher;

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            this.firstStep = true;
            this.N3 = 0;
            this.N4 = 0;
            if (parameters is ParametersWithIV)
            {
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                byte[] iV = hiv.GetIV();
                if (iV.Length < this.IV.Length)
                {
                    Array.Copy(iV, 0, this.IV, this.IV.Length - iV.Length, iV.Length);
                    for (int i = 0; i < (this.IV.Length - iV.Length); i++)
                    {
                        this.IV[i] = 0;
                    }
                }
                else
                {
                    Array.Copy(iV, 0, this.IV, 0, this.IV.Length);
                }
                parameters = hiv.Parameters;
            }
            this.Reset();
            if (parameters != null)
            {
                this.cipher.Init(true, parameters);
            }
        }

        private void intTobytes(int num, byte[] outBytes, int outOff)
        {
            outBytes[outOff + 3] = (byte) (num >> 0x18);
            outBytes[outOff + 2] = (byte) (num >> 0x10);
            outBytes[outOff + 1] = (byte) (num >> 8);
            outBytes[outOff] = (byte) num;
        }

        public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            if ((inOff + this.blockSize) > input.Length)
            {
                throw new DataLengthException("input buffer too short");
            }
            if ((outOff + this.blockSize) > output.Length)
            {
                throw new DataLengthException("output buffer too short");
            }
            if (this.firstStep)
            {
                this.firstStep = false;
                this.cipher.ProcessBlock(this.ofbV, 0, this.ofbOutV, 0);
                this.N3 = this.bytesToint(this.ofbOutV, 0);
                this.N4 = this.bytesToint(this.ofbOutV, 4);
            }
            this.N3 += 0x1010101;
            this.N4 += 0x1010104;
            this.intTobytes(this.N3, this.ofbV, 0);
            this.intTobytes(this.N4, this.ofbV, 4);
            this.cipher.ProcessBlock(this.ofbV, 0, this.ofbOutV, 0);
            for (int i = 0; i < this.blockSize; i++)
            {
                output[outOff + i] = (byte) (this.ofbOutV[i] ^ input[inOff + i]);
            }
            Array.Copy(this.ofbV, this.blockSize, this.ofbV, 0, this.ofbV.Length - this.blockSize);
            Array.Copy(this.ofbOutV, 0, this.ofbV, this.ofbV.Length - this.blockSize, this.blockSize);
            return this.blockSize;
        }

        public void Reset()
        {
            Array.Copy(this.IV, 0, this.ofbV, 0, this.IV.Length);
            this.cipher.Reset();
        }

        public string AlgorithmName =>
            (this.cipher.AlgorithmName + "/GCTR");

        public bool IsPartialBlockOkay =>
            true;
    }
}

