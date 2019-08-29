namespace Org.BouncyCastle.Crypto
{
    using System;

    public abstract class BufferedCipherBase : IBufferedCipher
    {
        protected static readonly byte[] EmptyBuffer = new byte[0];

        protected BufferedCipherBase()
        {
        }

        public abstract byte[] DoFinal();
        public virtual byte[] DoFinal(byte[] input) => 
            this.DoFinal(input, 0, input.Length);

        public virtual int DoFinal(byte[] output, int outOff)
        {
            byte[] buffer = this.DoFinal();
            if ((outOff + buffer.Length) > output.Length)
            {
                throw new DataLengthException("output buffer too short");
            }
            buffer.CopyTo(output, outOff);
            return buffer.Length;
        }

        public abstract byte[] DoFinal(byte[] input, int inOff, int length);
        public virtual int DoFinal(byte[] input, byte[] output, int outOff) => 
            this.DoFinal(input, 0, input.Length, output, outOff);

        public virtual int DoFinal(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            int num = this.ProcessBytes(input, inOff, length, output, outOff);
            return (num + this.DoFinal(output, outOff + num));
        }

        public abstract int GetBlockSize();
        public abstract int GetOutputSize(int inputLen);
        public abstract int GetUpdateOutputSize(int inputLen);
        public abstract void Init(bool forEncryption, ICipherParameters parameters);
        public abstract byte[] ProcessByte(byte input);
        public virtual int ProcessByte(byte input, byte[] output, int outOff)
        {
            byte[] buffer = this.ProcessByte(input);
            if (buffer == null)
            {
                return 0;
            }
            if ((outOff + buffer.Length) > output.Length)
            {
                throw new DataLengthException("output buffer too short");
            }
            buffer.CopyTo(output, outOff);
            return buffer.Length;
        }

        public virtual byte[] ProcessBytes(byte[] input) => 
            this.ProcessBytes(input, 0, input.Length);

        public abstract byte[] ProcessBytes(byte[] input, int inOff, int length);
        public virtual int ProcessBytes(byte[] input, byte[] output, int outOff) => 
            this.ProcessBytes(input, 0, input.Length, output, outOff);

        public virtual int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            byte[] buffer = this.ProcessBytes(input, inOff, length);
            if (buffer == null)
            {
                return 0;
            }
            if ((outOff + buffer.Length) > output.Length)
            {
                throw new DataLengthException("output buffer too short");
            }
            buffer.CopyTo(output, outOff);
            return buffer.Length;
        }

        public abstract void Reset();

        public abstract string AlgorithmName { get; }
    }
}

