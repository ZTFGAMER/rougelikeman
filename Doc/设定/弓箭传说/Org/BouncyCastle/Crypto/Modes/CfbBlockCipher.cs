namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class CfbBlockCipher : IBlockCipher
    {
        private byte[] IV;
        private byte[] cfbV;
        private byte[] cfbOutV;
        private bool encrypting;
        private readonly int blockSize;
        private readonly IBlockCipher cipher;

        public CfbBlockCipher(IBlockCipher cipher, int bitBlockSize)
        {
            this.cipher = cipher;
            this.blockSize = bitBlockSize / 8;
            this.IV = new byte[cipher.GetBlockSize()];
            this.cfbV = new byte[cipher.GetBlockSize()];
            this.cfbOutV = new byte[cipher.GetBlockSize()];
        }

        public int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
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
            Array.Copy(this.cfbV, this.blockSize, this.cfbV, 0, this.cfbV.Length - this.blockSize);
            Array.Copy(input, inOff, this.cfbV, this.cfbV.Length - this.blockSize, this.blockSize);
            for (int i = 0; i < this.blockSize; i++)
            {
                outBytes[outOff + i] = (byte) (this.cfbOutV[i] ^ input[inOff + i]);
            }
            return this.blockSize;
        }

        public int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
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

        public int GetBlockSize() => 
            this.blockSize;

        public IBlockCipher GetUnderlyingCipher() => 
            this.cipher;

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            this.encrypting = forEncryption;
            if (parameters is ParametersWithIV)
            {
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                byte[] iV = hiv.GetIV();
                int destinationIndex = this.IV.Length - iV.Length;
                Array.Copy(iV, 0, this.IV, destinationIndex, iV.Length);
                Array.Clear(this.IV, 0, destinationIndex);
                parameters = hiv.Parameters;
            }
            this.Reset();
            if (parameters != null)
            {
                this.cipher.Init(true, parameters);
            }
        }

        public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff) => 
            (!this.encrypting ? this.DecryptBlock(input, inOff, output, outOff) : this.EncryptBlock(input, inOff, output, outOff));

        public void Reset()
        {
            Array.Copy(this.IV, 0, this.cfbV, 0, this.IV.Length);
            this.cipher.Reset();
        }

        public string AlgorithmName =>
            (this.cipher.AlgorithmName + "/CFB" + (this.blockSize * 8));

        public bool IsPartialBlockOkay =>
            true;
    }
}

