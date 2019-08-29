namespace Org.BouncyCastle.Crypto
{
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class BufferedStreamCipher : BufferedCipherBase
    {
        private readonly IStreamCipher cipher;

        public BufferedStreamCipher(IStreamCipher cipher)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException("cipher");
            }
            this.cipher = cipher;
        }

        public override byte[] DoFinal()
        {
            this.Reset();
            return BufferedCipherBase.EmptyBuffer;
        }

        public override byte[] DoFinal(byte[] input, int inOff, int length)
        {
            if (length < 1)
            {
                return BufferedCipherBase.EmptyBuffer;
            }
            byte[] buffer = this.ProcessBytes(input, inOff, length);
            this.Reset();
            return buffer;
        }

        public override int GetBlockSize() => 
            0;

        public override int GetOutputSize(int inputLen) => 
            inputLen;

        public override int GetUpdateOutputSize(int inputLen) => 
            inputLen;

        public override void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (parameters is ParametersWithRandom)
            {
                parameters = ((ParametersWithRandom) parameters).Parameters;
            }
            this.cipher.Init(forEncryption, parameters);
        }

        public override byte[] ProcessByte(byte input) => 
            new byte[] { this.cipher.ReturnByte(input) };

        public override int ProcessByte(byte input, byte[] output, int outOff)
        {
            if (outOff >= output.Length)
            {
                throw new DataLengthException("output buffer too short");
            }
            output[outOff] = this.cipher.ReturnByte(input);
            return 1;
        }

        public override byte[] ProcessBytes(byte[] input, int inOff, int length)
        {
            if (length < 1)
            {
                return null;
            }
            byte[] output = new byte[length];
            this.cipher.ProcessBytes(input, inOff, length, output, 0);
            return output;
        }

        public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            if (length < 1)
            {
                return 0;
            }
            if (length > 0)
            {
                this.cipher.ProcessBytes(input, inOff, length, output, outOff);
            }
            return length;
        }

        public override void Reset()
        {
            this.cipher.Reset();
        }

        public override string AlgorithmName =>
            this.cipher.AlgorithmName;
    }
}

