namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class CbcBlockCipher : IBlockCipher
    {
        private byte[] IV;
        private byte[] cbcV;
        private byte[] cbcNextV;
        private int blockSize;
        private IBlockCipher cipher;
        private bool encrypting;

        public CbcBlockCipher(IBlockCipher cipher)
        {
            this.cipher = cipher;
            this.blockSize = cipher.GetBlockSize();
            this.IV = new byte[this.blockSize];
            this.cbcV = new byte[this.blockSize];
            this.cbcNextV = new byte[this.blockSize];
        }

        private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            if ((inOff + this.blockSize) > input.Length)
            {
                throw new DataLengthException("input buffer too short");
            }
            Array.Copy(input, inOff, this.cbcNextV, 0, this.blockSize);
            int num = this.cipher.ProcessBlock(input, inOff, outBytes, outOff);
            for (int i = 0; i < this.blockSize; i++)
            {
                outBytes[outOff + i] = (byte) (outBytes[outOff + i] ^ this.cbcV[i]);
            }
            byte[] cbcV = this.cbcV;
            this.cbcV = this.cbcNextV;
            this.cbcNextV = cbcV;
            return num;
        }

        private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
        {
            if ((inOff + this.blockSize) > input.Length)
            {
                throw new DataLengthException("input buffer too short");
            }
            for (int i = 0; i < this.blockSize; i++)
            {
                this.cbcV[i] = (byte) (this.cbcV[i] ^ input[inOff + i]);
            }
            int num2 = this.cipher.ProcessBlock(this.cbcV, 0, outBytes, outOff);
            Array.Copy(outBytes, outOff, this.cbcV, 0, this.cbcV.Length);
            return num2;
        }

        public int GetBlockSize() => 
            this.cipher.GetBlockSize();

        public IBlockCipher GetUnderlyingCipher() => 
            this.cipher;

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            bool encrypting = this.encrypting;
            this.encrypting = forEncryption;
            if (parameters is ParametersWithIV)
            {
                ParametersWithIV hiv = (ParametersWithIV) parameters;
                byte[] iV = hiv.GetIV();
                if (iV.Length != this.blockSize)
                {
                    throw new ArgumentException("initialisation vector must be the same length as block size");
                }
                Array.Copy(iV, 0, this.IV, 0, iV.Length);
                parameters = hiv.Parameters;
            }
            this.Reset();
            if (parameters != null)
            {
                this.cipher.Init(this.encrypting, parameters);
            }
            else if (encrypting != this.encrypting)
            {
                throw new ArgumentException("cannot change encrypting state without providing key.");
            }
        }

        public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff) => 
            (!this.encrypting ? this.DecryptBlock(input, inOff, output, outOff) : this.EncryptBlock(input, inOff, output, outOff));

        public void Reset()
        {
            Array.Copy(this.IV, 0, this.cbcV, 0, this.IV.Length);
            Array.Clear(this.cbcNextV, 0, this.cbcNextV.Length);
            this.cipher.Reset();
        }

        public string AlgorithmName =>
            (this.cipher.AlgorithmName + "/CBC");

        public bool IsPartialBlockOkay =>
            false;
    }
}

