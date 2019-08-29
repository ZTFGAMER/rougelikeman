namespace Org.BouncyCastle.Crypto.Modes
{
    using Org.BouncyCastle.Crypto;
    using System;

    public class CtsBlockCipher : BufferedBlockCipher
    {
        private readonly int blockSize;

        public CtsBlockCipher(IBlockCipher cipher)
        {
            if ((cipher is OfbBlockCipher) || (cipher is CfbBlockCipher))
            {
                throw new ArgumentException("CtsBlockCipher can only accept ECB, or CBC ciphers");
            }
            base.cipher = cipher;
            this.blockSize = cipher.GetBlockSize();
            base.buf = new byte[this.blockSize * 2];
            base.bufOff = 0;
        }

        public override int DoFinal(byte[] output, int outOff)
        {
            if ((base.bufOff + outOff) > output.Length)
            {
                throw new DataLengthException("output buffer too small in doFinal");
            }
            int blockSize = base.cipher.GetBlockSize();
            int length = base.bufOff - blockSize;
            byte[] outBuf = new byte[blockSize];
            if (base.forEncryption)
            {
                base.cipher.ProcessBlock(base.buf, 0, outBuf, 0);
                if (base.bufOff < blockSize)
                {
                    throw new DataLengthException("need at least one block of input for CTS");
                }
                for (int i = base.bufOff; i != base.buf.Length; i++)
                {
                    base.buf[i] = outBuf[i - blockSize];
                }
                for (int j = blockSize; j != base.bufOff; j++)
                {
                    base.buf[j] = (byte) (base.buf[j] ^ outBuf[j - blockSize]);
                }
                (!(base.cipher is CbcBlockCipher) ? base.cipher : ((CbcBlockCipher) base.cipher).GetUnderlyingCipher()).ProcessBlock(base.buf, blockSize, output, outOff);
                Array.Copy(outBuf, 0, output, outOff + blockSize, length);
            }
            else
            {
                byte[] sourceArray = new byte[blockSize];
                (!(base.cipher is CbcBlockCipher) ? base.cipher : ((CbcBlockCipher) base.cipher).GetUnderlyingCipher()).ProcessBlock(base.buf, 0, outBuf, 0);
                for (int i = blockSize; i != base.bufOff; i++)
                {
                    sourceArray[i - blockSize] = (byte) (outBuf[i - blockSize] ^ base.buf[i]);
                }
                Array.Copy(base.buf, blockSize, outBuf, 0, length);
                base.cipher.ProcessBlock(outBuf, 0, output, outOff);
                Array.Copy(sourceArray, 0, output, outOff + blockSize, length);
            }
            int bufOff = base.bufOff;
            this.Reset();
            return bufOff;
        }

        public override int GetOutputSize(int length) => 
            (length + base.bufOff);

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

        public override int ProcessByte(byte input, byte[] output, int outOff)
        {
            int num = 0;
            if (base.bufOff == base.buf.Length)
            {
                num = base.cipher.ProcessBlock(base.buf, 0, output, outOff);
                Array.Copy(base.buf, this.blockSize, base.buf, 0, this.blockSize);
                base.bufOff = this.blockSize;
            }
            base.buf[base.bufOff++] = input;
            return num;
        }

        public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            if (length < 0)
            {
                throw new ArgumentException("Can't have a negative input outLength!");
            }
            int blockSize = this.GetBlockSize();
            int updateOutputSize = this.GetUpdateOutputSize(length);
            if ((updateOutputSize > 0) && ((outOff + updateOutputSize) > output.Length))
            {
                throw new DataLengthException("output buffer too short");
            }
            int num3 = 0;
            int num4 = base.buf.Length - base.bufOff;
            if (length > num4)
            {
                Array.Copy(input, inOff, base.buf, base.bufOff, num4);
                num3 += base.cipher.ProcessBlock(base.buf, 0, output, outOff);
                Array.Copy(base.buf, blockSize, base.buf, 0, blockSize);
                base.bufOff = blockSize;
                length -= num4;
                inOff += num4;
                while (length > blockSize)
                {
                    Array.Copy(input, inOff, base.buf, base.bufOff, blockSize);
                    num3 += base.cipher.ProcessBlock(base.buf, 0, output, outOff + num3);
                    Array.Copy(base.buf, blockSize, base.buf, 0, blockSize);
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

