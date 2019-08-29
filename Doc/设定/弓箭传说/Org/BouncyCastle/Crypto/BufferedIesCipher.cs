namespace Org.BouncyCastle.Crypto
{
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.IO;

    public class BufferedIesCipher : BufferedCipherBase
    {
        private readonly IesEngine engine;
        private bool forEncryption;
        private MemoryStream buffer = new MemoryStream();

        public BufferedIesCipher(IesEngine engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            this.engine = engine;
        }

        public override byte[] DoFinal()
        {
            byte[] input = this.buffer.ToArray();
            this.Reset();
            return this.engine.ProcessBlock(input, 0, input.Length);
        }

        public override byte[] DoFinal(byte[] input, int inOff, int length)
        {
            this.ProcessBytes(input, inOff, length);
            return this.DoFinal();
        }

        public override int GetBlockSize() => 
            0;

        public override int GetOutputSize(int inputLen)
        {
            if (this.engine == null)
            {
                throw new InvalidOperationException("cipher not initialised");
            }
            int num = inputLen + ((int) this.buffer.Length);
            return (!this.forEncryption ? (num - 20) : (num + 20));
        }

        public override int GetUpdateOutputSize(int inputLen) => 
            0;

        public override void Init(bool forEncryption, ICipherParameters parameters)
        {
            this.forEncryption = forEncryption;
            throw Platform.CreateNotImplementedException("IES");
        }

        public override byte[] ProcessByte(byte input)
        {
            this.buffer.WriteByte(input);
            return null;
        }

        public override byte[] ProcessBytes(byte[] input, int inOff, int length)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (inOff < 0)
            {
                throw new ArgumentException("inOff");
            }
            if (length < 0)
            {
                throw new ArgumentException("length");
            }
            if ((inOff + length) > input.Length)
            {
                throw new ArgumentException("invalid offset/length specified for input array");
            }
            this.buffer.Write(input, inOff, length);
            return null;
        }

        public override void Reset()
        {
            this.buffer.SetLength(0L);
        }

        public override string AlgorithmName =>
            "IES";
    }
}

