namespace Org.BouncyCastle.Crypto.Macs
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    internal class MacCFBBlockCipher : IBlockCipher
    {
        private byte[] IV;
        private byte[] cfbV;
        private byte[] cfbOutV;
        private readonly int blockSize;
        private readonly IBlockCipher cipher;

        public MacCFBBlockCipher(IBlockCipher cipher, int bitBlockSize)
        {
            this.cipher = cipher;
            this.blockSize = bitBlockSize / 8;
            this.IV = new byte[cipher.GetBlockSize()];
            this.cfbV = new byte[cipher.GetBlockSize()];
            this.cfbOutV = new byte[cipher.GetBlockSize()];
        }

        public int GetBlockSize() => 
            this.blockSize;

        public void GetMacBlock(byte[] mac)
        {
            this.cipher.ProcessBlock(this.cfbV, 0, mac, 0);
        }

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (parameters is ParametersWithIV)
            {
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                byte[] iV = hiv.GetIV();
                if (iV.Length < this.IV.Length)
                {
                    Array.Copy(iV, 0, this.IV, this.IV.Length - iV.Length, iV.Length);
                }
                else
                {
                    Array.Copy(iV, 0, this.IV, 0, this.IV.Length);
                }
                parameters = hiv.Parameters;
            }
            this.Reset();
            this.cipher.Init(true, parameters);
        }

        public int ProcessBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            if ((inOff + this.blockSize) > input.Length)
            {
                throw new DataLengthException("input buffer too short");
            }
            if ((outOff + this.blockSize) > outBytes.Length)
            {
                throw new DataLengthException("output buffer too short");
            }
            this.cipher.ProcessBlock(this.cfbV, 0, this.cfbOutV, 0);
            for (int i = 0; i < this.blockSize; i++)
            {
                outBytes[outOff + i] = (byte) (this.cfbOutV[i] ^ input[inOff + i]);
            }
            Array.Copy(this.cfbV, this.blockSize, this.cfbV, 0, this.cfbV.Length - this.blockSize);
            Array.Copy(outBytes, outOff, this.cfbV, this.cfbV.Length - this.blockSize, this.blockSize);
            return this.blockSize;
        }

        public void Reset()
        {
            this.IV.CopyTo(this.cfbV, 0);
            this.cipher.Reset();
        }

        public string AlgorithmName =>
            (this.cipher.AlgorithmName + "/CFB" + (this.blockSize * 8));

        public bool IsPartialBlockOkay =>
            true;
    }
}

