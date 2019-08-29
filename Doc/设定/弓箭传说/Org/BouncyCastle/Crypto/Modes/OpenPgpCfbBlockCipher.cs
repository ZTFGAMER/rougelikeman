namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class OpenPgpCfbBlockCipher : IBlockCipher
    {
        private byte[] IV;
        private byte[] FR;
        private byte[] FRE;
        private readonly IBlockCipher cipher;
        private readonly int blockSize;
        private int count;
        private bool forEncryption;

        public OpenPgpCfbBlockCipher(IBlockCipher cipher)
        {
            this.cipher = cipher;
            this.blockSize = cipher.GetBlockSize();
            this.IV = new byte[this.blockSize];
            this.FR = new byte[this.blockSize];
            this.FRE = new byte[this.blockSize];
        }

        private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            if ((inOff + this.blockSize) > input.Length)
            {
                throw new DataLengthException("input buffer too short");
            }
            if ((outOff + this.blockSize) > outBytes.Length)
            {
                throw new DataLengthException("output buffer too short");
            }
            if (this.count > this.blockSize)
            {
                byte data = input[inOff];
                this.FR[this.blockSize - 2] = data;
                outBytes[outOff] = this.EncryptByte(data, this.blockSize - 2);
                data = input[inOff + 1];
                this.FR[this.blockSize - 1] = data;
                outBytes[outOff + 1] = this.EncryptByte(data, this.blockSize - 1);
                this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
                for (int i = 2; i < this.blockSize; i++)
                {
                    data = input[inOff + i];
                    this.FR[i - 2] = data;
                    outBytes[outOff + i] = this.EncryptByte(data, i - 2);
                }
            }
            else if (this.count == 0)
            {
                this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
                for (int i = 0; i < this.blockSize; i++)
                {
                    this.FR[i] = input[inOff + i];
                    outBytes[i] = this.EncryptByte(input[inOff + i], i);
                }
                this.count += this.blockSize;
            }
            else if (this.count == this.blockSize)
            {
                this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
                byte data = input[inOff];
                byte num5 = input[inOff + 1];
                outBytes[outOff] = this.EncryptByte(data, 0);
                outBytes[outOff + 1] = this.EncryptByte(num5, 1);
                Array.Copy(this.FR, 2, this.FR, 0, this.blockSize - 2);
                this.FR[this.blockSize - 2] = data;
                this.FR[this.blockSize - 1] = num5;
                this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
                for (int i = 2; i < this.blockSize; i++)
                {
                    byte num7 = input[inOff + i];
                    this.FR[i - 2] = num7;
                    outBytes[outOff + i] = this.EncryptByte(num7, i - 2);
                }
                this.count += this.blockSize;
            }
            return this.blockSize;
        }

        private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            if ((inOff + this.blockSize) > input.Length)
            {
                throw new DataLengthException("input buffer too short");
            }
            if ((outOff + this.blockSize) > outBytes.Length)
            {
                throw new DataLengthException("output buffer too short");
            }
            if (this.count > this.blockSize)
            {
                this.FR[this.blockSize - 2] = outBytes[outOff] = this.EncryptByte(input[inOff], this.blockSize - 2);
                this.FR[this.blockSize - 1] = outBytes[outOff + 1] = this.EncryptByte(input[inOff + 1], this.blockSize - 1);
                this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
                for (int i = 2; i < this.blockSize; i++)
                {
                    this.FR[i - 2] = outBytes[outOff + i] = this.EncryptByte(input[inOff + i], i - 2);
                }
            }
            else if (this.count == 0)
            {
                this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
                for (int i = 0; i < this.blockSize; i++)
                {
                    this.FR[i] = outBytes[outOff + i] = this.EncryptByte(input[inOff + i], i);
                }
                this.count += this.blockSize;
            }
            else if (this.count == this.blockSize)
            {
                this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
                outBytes[outOff] = this.EncryptByte(input[inOff], 0);
                outBytes[outOff + 1] = this.EncryptByte(input[inOff + 1], 1);
                Array.Copy(this.FR, 2, this.FR, 0, this.blockSize - 2);
                Array.Copy(outBytes, outOff, this.FR, this.blockSize - 2, 2);
                this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
                for (int i = 2; i < this.blockSize; i++)
                {
                    this.FR[i - 2] = outBytes[outOff + i] = this.EncryptByte(input[inOff + i], i - 2);
                }
                this.count += this.blockSize;
            }
            return this.blockSize;
        }

        private byte EncryptByte(byte data, int blockOff) => 
            ((byte) (this.FRE[blockOff] ^ data));

        public int GetBlockSize() => 
            this.cipher.GetBlockSize();

        public IBlockCipher GetUnderlyingCipher() => 
            this.cipher;

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            this.forEncryption = forEncryption;
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
            this.cipher.Init(true, parameters);
        }

        public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff) => 
            (!this.forEncryption ? this.DecryptBlock(input, inOff, output, outOff) : this.EncryptBlock(input, inOff, output, outOff));

        public void Reset()
        {
            this.count = 0;
            Array.Copy(this.IV, 0, this.FR, 0, this.FR.Length);
            this.cipher.Reset();
        }

        public string AlgorithmName =>
            (this.cipher.AlgorithmName + "/OpenPGPCFB");

        public bool IsPartialBlockOkay =>
            true;
    }
}

