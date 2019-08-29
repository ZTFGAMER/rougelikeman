namespace Org.BouncyCastle.Crypto
{
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Parameters;
    using System;

    public class BufferedAeadBlockCipher : BufferedCipherBase
    {
        private readonly IAeadBlockCipher cipher;

        public BufferedAeadBlockCipher(IAeadBlockCipher cipher)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException("cipher");
            }
            this.cipher = cipher;
        }

        public override byte[] DoFinal()
        {
            byte[] output = new byte[this.GetOutputSize(0)];
            int length = this.DoFinal(output, 0);
            if (length < output.Length)
            {
                byte[] destinationArray = new byte[length];
                Array.Copy(output, 0, destinationArray, 0, length);
                output = destinationArray;
            }
            return output;
        }

        public override int DoFinal(byte[] output, int outOff) => 
            this.cipher.DoFinal(output, outOff);

        public override byte[] DoFinal(byte[] input, int inOff, int inLen)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            byte[] output = new byte[this.GetOutputSize(inLen)];
            int outOff = (inLen <= 0) ? 0 : this.ProcessBytes(input, inOff, inLen, output, 0);
            outOff += this.DoFinal(output, outOff);
            if (outOff < output.Length)
            {
                byte[] destinationArray = new byte[outOff];
                Array.Copy(output, 0, destinationArray, 0, outOff);
                output = destinationArray;
            }
            return output;
        }

        public override int GetBlockSize() => 
            this.cipher.GetBlockSize();

        public override int GetOutputSize(int length) => 
            this.cipher.GetOutputSize(length);

        public override int GetUpdateOutputSize(int length) => 
            this.cipher.GetUpdateOutputSize(length);

        public override void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (parameters is ParametersWithRandom)
            {
                parameters = ((ParametersWithRandom) parameters).Parameters;
            }
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

        public override int ProcessByte(byte input, byte[] output, int outOff) => 
            this.cipher.ProcessByte(input, output, outOff);

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

        public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff) => 
            this.cipher.ProcessBytes(input, inOff, length, output, outOff);

        public override void Reset()
        {
            this.cipher.Reset();
        }

        public override string AlgorithmName =>
            this.cipher.AlgorithmName;
    }
}

