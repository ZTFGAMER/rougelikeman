namespace Org.BouncyCastle.Crypto
{
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class BufferedBlockCipher : BufferedCipherBase
    {
        internal byte[] buf;
        internal int bufOff;
        internal bool forEncryption;
        internal IBlockCipher cipher;

        protected BufferedBlockCipher()
        {
        }

        public BufferedBlockCipher(IBlockCipher cipher)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException("cipher");
            }
            this.cipher = cipher;
            this.buf = new byte[cipher.GetBlockSize()];
            this.bufOff = 0;
        }

        public override byte[] DoFinal()
        {
            byte[] emptyBuffer = BufferedCipherBase.EmptyBuffer;
            int outputSize = this.GetOutputSize(0);
            if (outputSize > 0)
            {
                emptyBuffer = new byte[outputSize];
                int length = this.DoFinal(emptyBuffer, 0);
                if (length < emptyBuffer.Length)
                {
                    byte[] destinationArray = new byte[length];
                    Array.Copy(emptyBuffer, 0, destinationArray, 0, length);
                    emptyBuffer = destinationArray;
                }
                return emptyBuffer;
            }
            this.Reset();
            return emptyBuffer;
        }

        public override int DoFinal(byte[] output, int outOff)
        {
            int bufOff;
            try
            {
                if (this.bufOff != 0)
                {
                    Check.DataLength(!this.cipher.IsPartialBlockOkay, "data not block size aligned");
                    Check.OutputLength(output, outOff, this.bufOff, "output buffer too short for DoFinal()");
                    this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
                    Array.Copy(this.buf, 0, output, outOff, this.bufOff);
                }
                bufOff = this.bufOff;
            }
            finally
            {
                this.Reset();
            }
            return bufOff;
        }

        public override byte[] DoFinal(byte[] input, int inOff, int inLen)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            int outputSize = this.GetOutputSize(inLen);
            byte[] emptyBuffer = BufferedCipherBase.EmptyBuffer;
            if (outputSize > 0)
            {
                emptyBuffer = new byte[outputSize];
                int outOff = (inLen <= 0) ? 0 : this.ProcessBytes(input, inOff, inLen, emptyBuffer, 0);
                outOff += this.DoFinal(emptyBuffer, outOff);
                if (outOff < emptyBuffer.Length)
                {
                    byte[] destinationArray = new byte[outOff];
                    Array.Copy(emptyBuffer, 0, destinationArray, 0, outOff);
                    emptyBuffer = destinationArray;
                }
                return emptyBuffer;
            }
            this.Reset();
            return emptyBuffer;
        }

        public override int GetBlockSize() => 
            this.cipher.GetBlockSize();

        public override int GetOutputSize(int length) => 
            (length + this.bufOff);

        public override int GetUpdateOutputSize(int length)
        {
            int num = length + this.bufOff;
            int num2 = num % this.buf.Length;
            return (num - num2);
        }

        public override void Init(bool forEncryption, ICipherParameters parameters)
        {
            this.forEncryption = forEncryption;
            ParametersWithRandom random = parameters as ParametersWithRandom;
            if (random != null)
            {
                parameters = random.Parameters;
            }
            this.Reset();
            this.cipher.Init(forEncryption, parameters);
        }

        public override byte[] ProcessByte(byte input)
        {
            int updateOutputSize = this.GetUpdateOutputSize(1);
            byte[] output = (updateOutputSize <= 0) ? null : new byte[updateOutputSize];
            int length = this.ProcessByte(input, output, 0);
            if ((updateOutputSize > 0) && (length < updateOutputSize))
            {
                byte[] destinationArray = new byte[length];
                Array.Copy(output, 0, destinationArray, 0, length);
                output = destinationArray;
            }
            return output;
        }

        public override int ProcessByte(byte input, byte[] output, int outOff)
        {
            this.buf[this.bufOff++] = input;
            if (this.bufOff != this.buf.Length)
            {
                return 0;
            }
            if ((outOff + this.buf.Length) > output.Length)
            {
                throw new DataLengthException("output buffer too short");
            }
            this.bufOff = 0;
            return this.cipher.ProcessBlock(this.buf, 0, output, outOff);
        }

        public override byte[] ProcessBytes(byte[] input, int inOff, int length)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (length < 1)
            {
                return null;
            }
            int updateOutputSize = this.GetUpdateOutputSize(length);
            byte[] output = (updateOutputSize <= 0) ? null : new byte[updateOutputSize];
            int num2 = this.ProcessBytes(input, inOff, length, output, 0);
            if ((updateOutputSize > 0) && (num2 < updateOutputSize))
            {
                byte[] destinationArray = new byte[num2];
                Array.Copy(output, 0, destinationArray, 0, num2);
                output = destinationArray;
            }
            return output;
        }

        public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            if (length < 1)
            {
                if (length < 0)
                {
                    throw new ArgumentException("Can't have a negative input length!");
                }
                return 0;
            }
            int blockSize = this.GetBlockSize();
            int updateOutputSize = this.GetUpdateOutputSize(length);
            if (updateOutputSize > 0)
            {
                Check.OutputLength(output, outOff, updateOutputSize, "output buffer too short");
            }
            int num3 = 0;
            int num4 = this.buf.Length - this.bufOff;
            if (length > num4)
            {
                Array.Copy(input, inOff, this.buf, this.bufOff, num4);
                num3 += this.cipher.ProcessBlock(this.buf, 0, output, outOff);
                this.bufOff = 0;
                length -= num4;
                inOff += num4;
                while (length > this.buf.Length)
                {
                    num3 += this.cipher.ProcessBlock(input, inOff, output, outOff + num3);
                    length -= blockSize;
                    inOff += blockSize;
                }
            }
            Array.Copy(input, inOff, this.buf, this.bufOff, length);
            this.bufOff += length;
            if (this.bufOff == this.buf.Length)
            {
                num3 += this.cipher.ProcessBlock(this.buf, 0, output, outOff + num3);
                this.bufOff = 0;
            }
            return num3;
        }

        public override void Reset()
        {
            Array.Clear(this.buf, 0, this.buf.Length);
            this.bufOff = 0;
            this.cipher.Reset();
        }

        public override string AlgorithmName =>
            this.cipher.AlgorithmName;
    }
}

