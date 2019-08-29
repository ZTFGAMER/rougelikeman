namespace Org.BouncyCastle.Crypto.Paddings
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using System;

    public class PaddedBufferedBlockCipher : BufferedBlockCipher
    {
        private readonly IBlockCipherPadding padding;

        public PaddedBufferedBlockCipher(IBlockCipher cipher) : this(cipher, new Pkcs7Padding())
        {
        }

        public PaddedBufferedBlockCipher(IBlockCipher cipher, IBlockCipherPadding padding)
        {
            base.cipher = cipher;
            this.padding = padding;
            base.buf = new byte[cipher.GetBlockSize()];
            base.bufOff = 0;
        }

        public override int DoFinal(byte[] output, int outOff)
        {
            int blockSize = base.cipher.GetBlockSize();
            int length = 0;
            if (base.forEncryption)
            {
                if (base.bufOff == blockSize)
                {
                    if ((outOff + (2 * blockSize)) > output.Length)
                    {
                        this.Reset();
                        throw new OutputLengthException("output buffer too short");
                    }
                    length = base.cipher.ProcessBlock(base.buf, 0, output, outOff);
                    base.bufOff = 0;
                }
                this.padding.AddPadding(base.buf, base.bufOff);
                length += base.cipher.ProcessBlock(base.buf, 0, output, outOff + length);
                this.Reset();
                return length;
            }
            if (base.bufOff == blockSize)
            {
                length = base.cipher.ProcessBlock(base.buf, 0, base.buf, 0);
                base.bufOff = 0;
            }
            else
            {
                this.Reset();
                throw new DataLengthException("last block incomplete in decryption");
            }
            try
            {
                length -= this.padding.PadCount(base.buf);
                Array.Copy(base.buf, 0, output, outOff, length);
            }
            finally
            {
                this.Reset();
            }
            return length;
        }

        public override int GetOutputSize(int length)
        {
            int num = length + base.bufOff;
            int num2 = num % base.buf.Length;
            if (num2 != 0)
            {
                return ((num - num2) + base.buf.Length);
            }
            if (base.forEncryption)
            {
                return (num + base.buf.Length);
            }
            return num;
        }

        public override int GetUpdateOutputSize(int length)
        {
            int num = length + base.bufOff;
            int num2 = num % base.buf.Length;
            if (num2 == 0)
            {
                return (num - base.buf.Length);
            }
            return (num - num2);
        }

        public override void Init(bool forEncryption, ICipherParameters parameters)
        {
            base.forEncryption = forEncryption;
            SecureRandom random = null;
            if (parameters is ParametersWithRandom)
            {
                ParametersWithRandom random2 = (ParametersWithRandom) parameters;
                random = random2.Random;
                parameters = random2.Parameters;
            }
            this.Reset();
            this.padding.Init(random);
            base.cipher.Init(forEncryption, parameters);
        }

        public override int ProcessByte(byte input, byte[] output, int outOff)
        {
            int num = 0;
            if (base.bufOff == base.buf.Length)
            {
                num = base.cipher.ProcessBlock(base.buf, 0, output, outOff);
                base.bufOff = 0;
            }
            base.buf[base.bufOff++] = input;
            return num;
        }

        public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            if (length < 0)
            {
                throw new ArgumentException("Can't have a negative input length!");
            }
            int blockSize = this.GetBlockSize();
            int updateOutputSize = this.GetUpdateOutputSize(length);
            if (updateOutputSize > 0)
            {
                Check.OutputLength(output, outOff, updateOutputSize, "output buffer too short");
            }
            int num3 = 0;
            int num4 = base.buf.Length - base.bufOff;
            if (length > num4)
            {
                Array.Copy(input, inOff, base.buf, base.bufOff, num4);
                num3 += base.cipher.ProcessBlock(base.buf, 0, output, outOff);
                base.bufOff = 0;
                length -= num4;
                inOff += num4;
                while (length > base.buf.Length)
                {
                    num3 += base.cipher.ProcessBlock(input, inOff, output, outOff + num3);
                    length -= blockSize;
                    inOff += blockSize;
                }
            }
            Array.Copy(input, inOff, base.buf, base.bufOff, length);
            base.bufOff += length;
            return num3;
        }
    }
}

