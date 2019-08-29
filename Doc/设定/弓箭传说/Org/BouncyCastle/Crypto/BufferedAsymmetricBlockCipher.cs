namespace Org.BouncyCastle.Crypto
{
    using System;

    public class BufferedAsymmetricBlockCipher : BufferedCipherBase
    {
        private readonly IAsymmetricBlockCipher cipher;
        private byte[] buffer;
        private int bufOff;

        public BufferedAsymmetricBlockCipher(IAsymmetricBlockCipher cipher)
        {
            this.cipher = cipher;
        }

        public override byte[] DoFinal()
        {
            byte[] buffer = (this.bufOff <= 0) ? BufferedCipherBase.EmptyBuffer : this.cipher.ProcessBlock(this.buffer, 0, this.bufOff);
            this.Reset();
            return buffer;
        }

        public override byte[] DoFinal(byte[] input, int inOff, int length)
        {
            this.ProcessBytes(input, inOff, length);
            return this.DoFinal();
        }

        public override int GetBlockSize() => 
            this.cipher.GetInputBlockSize();

        internal int GetBufferPosition() => 
            this.bufOff;

        public override int GetOutputSize(int length) => 
            this.cipher.GetOutputBlockSize();

        public override int GetUpdateOutputSize(int length) => 
            0;

        public override void Init(bool forEncryption, ICipherParameters parameters)
        {
            this.Reset();
            this.cipher.Init(forEncryption, parameters);
            this.buffer = new byte[this.cipher.GetInputBlockSize() + (!forEncryption ? 0 : 1)];
            this.bufOff = 0;
        }

        public override byte[] ProcessByte(byte input)
        {
            if (this.bufOff >= this.buffer.Length)
            {
                throw new DataLengthException("attempt to process message to long for cipher");
            }
            this.buffer[this.bufOff++] = input;
            return null;
        }

        public override byte[] ProcessBytes(byte[] input, int inOff, int length)
        {
            if (length >= 1)
            {
                if (input == null)
                {
                    throw new ArgumentNullException("input");
                }
                if ((this.bufOff + length) > this.buffer.Length)
                {
                    throw new DataLengthException("attempt to process message to long for cipher");
                }
                Array.Copy(input, inOff, this.buffer, this.bufOff, length);
                this.bufOff += length;
            }
            return null;
        }

        public override void Reset()
        {
            if (this.buffer != null)
            {
                Array.Clear(this.buffer, 0, this.buffer.Length);
                this.bufOff = 0;
            }
        }

        public override string AlgorithmName =>
            this.cipher.AlgorithmName;
    }
}

