namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Utilities;
    using System;

    public class SicBlockCipher : IBlockCipher
    {
        private readonly IBlockCipher cipher;
        private readonly int blockSize;
        private readonly byte[] counter;
        private readonly byte[] counterOut;
        private byte[] IV;

        public SicBlockCipher(IBlockCipher cipher)
        {
            this.cipher = cipher;
            this.blockSize = cipher.GetBlockSize();
            this.counter = new byte[this.blockSize];
            this.counterOut = new byte[this.blockSize];
            this.IV = new byte[this.blockSize];
        }

        public virtual int GetBlockSize() => 
            this.cipher.GetBlockSize();

        public virtual IBlockCipher GetUnderlyingCipher() => 
            this.cipher;

        public virtual void Init(bool forEncryption, ICipherParameters parameters)
        {
            ParametersWithIV hiv = parameters as ParametersWithIV;
            if (hiv == null)
            {
                throw new ArgumentException("CTR/SIC mode requires ParametersWithIV", "parameters");
            }
            this.IV = Arrays.Clone(hiv.GetIV());
            if (this.blockSize < this.IV.Length)
            {
                throw new ArgumentException("CTR/SIC mode requires IV no greater than: " + this.blockSize + " bytes.");
            }
            int num = Math.Min(8, this.blockSize / 2);
            if ((this.blockSize - this.IV.Length) > num)
            {
                throw new ArgumentException("CTR/SIC mode requires IV of at least: " + (this.blockSize - num) + " bytes.");
            }
            if (hiv.Parameters != null)
            {
                this.cipher.Init(true, hiv.Parameters);
            }
            this.Reset();
        }

        public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
        {
            this.cipher.ProcessBlock(this.counter, 0, this.counterOut, 0);
            for (int i = 0; i < this.counterOut.Length; i++)
            {
                output[outOff + i] = (byte) (this.counterOut[i] ^ input[inOff + i]);
            }
            int length = this.counter.Length;
            while ((--length >= 0) && ((this.counter[length] = (byte) (this.counter[length] + 1)) == 0))
            {
            }
            return this.counter.Length;
        }

        public virtual void Reset()
        {
            Arrays.Fill(this.counter, 0);
            Array.Copy(this.IV, 0, this.counter, 0, this.IV.Length);
            this.cipher.Reset();
        }

        public virtual string AlgorithmName =>
            (this.cipher.AlgorithmName + "/SIC");

        public virtual bool IsPartialBlockOkay =>
            true;
    }
}

