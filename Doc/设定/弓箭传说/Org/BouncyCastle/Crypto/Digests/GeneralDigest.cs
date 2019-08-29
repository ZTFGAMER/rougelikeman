namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Utilities;
    using System;

    public abstract class GeneralDigest : IDigest, IMemoable
    {
        private const int BYTE_LENGTH = 0x40;
        private byte[] xBuf;
        private int xBufOff;
        private long byteCount;

        internal GeneralDigest()
        {
            this.xBuf = new byte[4];
        }

        internal GeneralDigest(GeneralDigest t)
        {
            this.xBuf = new byte[t.xBuf.Length];
            this.CopyIn(t);
        }

        public void BlockUpdate(byte[] input, int inOff, int length)
        {
            length = Math.Max(0, length);
            int num = 0;
            if (this.xBufOff != 0)
            {
                while (num < length)
                {
                    this.xBuf[this.xBufOff++] = input[inOff + num++];
                    if (this.xBufOff == 4)
                    {
                        this.ProcessWord(this.xBuf, 0);
                        this.xBufOff = 0;
                        break;
                    }
                }
            }
            int num3 = ((length - num) & -4) + num;
            while (num < num3)
            {
                this.ProcessWord(input, inOff + num);
                num += 4;
            }
            while (num < length)
            {
                this.xBuf[this.xBufOff++] = input[inOff + num++];
            }
            this.byteCount += length;
        }

        public abstract IMemoable Copy();
        protected void CopyIn(GeneralDigest t)
        {
            Array.Copy(t.xBuf, 0, this.xBuf, 0, t.xBuf.Length);
            this.xBufOff = t.xBufOff;
            this.byteCount = t.byteCount;
        }

        public abstract int DoFinal(byte[] output, int outOff);
        public void Finish()
        {
            long bitLength = this.byteCount << 3;
            this.Update(0x80);
            while (this.xBufOff != 0)
            {
                this.Update(0);
            }
            this.ProcessLength(bitLength);
            this.ProcessBlock();
        }

        public int GetByteLength() => 
            0x40;

        public abstract int GetDigestSize();
        internal abstract void ProcessBlock();
        internal abstract void ProcessLength(long bitLength);
        internal abstract void ProcessWord(byte[] input, int inOff);
        public virtual void Reset()
        {
            this.byteCount = 0L;
            this.xBufOff = 0;
            Array.Clear(this.xBuf, 0, this.xBuf.Length);
        }

        public abstract void Reset(IMemoable t);
        public void Update(byte input)
        {
            this.xBuf[this.xBufOff++] = input;
            if (this.xBufOff == this.xBuf.Length)
            {
                this.ProcessWord(this.xBuf, 0);
                this.xBufOff = 0;
            }
            this.byteCount += 1L;
        }

        public abstract string AlgorithmName { get; }
    }
}

