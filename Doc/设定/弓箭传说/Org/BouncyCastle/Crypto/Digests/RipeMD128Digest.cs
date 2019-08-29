namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class RipeMD128Digest : GeneralDigest
    {
        private const int DigestLength = 0x10;
        private int H0;
        private int H1;
        private int H2;
        private int H3;
        private int[] X;
        private int xOff;

        public RipeMD128Digest()
        {
            this.X = new int[0x10];
            this.Reset();
        }

        public RipeMD128Digest(RipeMD128Digest t) : base(t)
        {
            this.X = new int[0x10];
            this.CopyIn(t);
        }

        public override IMemoable Copy() => 
            new RipeMD128Digest(this);

        private void CopyIn(RipeMD128Digest t)
        {
            base.CopyIn(t);
            this.H0 = t.H0;
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            Array.Copy(t.X, 0, this.X, 0, t.X.Length);
            this.xOff = t.xOff;
        }

        public override int DoFinal(byte[] output, int outOff)
        {
            base.Finish();
            this.UnpackWord(this.H0, output, outOff);
            this.UnpackWord(this.H1, output, outOff + 4);
            this.UnpackWord(this.H2, output, outOff + 8);
            this.UnpackWord(this.H3, output, outOff + 12);
            this.Reset();
            return 0x10;
        }

        private int F1(int x, int y, int z) => 
            ((x ^ y) ^ z);

        private int F1(int a, int b, int c, int d, int x, int s) => 
            this.RL((a + this.F1(b, c, d)) + x, s);

        private int F2(int x, int y, int z) => 
            ((x & y) | (~x & z));

        private int F2(int a, int b, int c, int d, int x, int s) => 
            this.RL(((a + this.F2(b, c, d)) + x) + 0x5a827999, s);

        private int F3(int x, int y, int z) => 
            ((x | ~y) ^ z);

        private int F3(int a, int b, int c, int d, int x, int s) => 
            this.RL(((a + this.F3(b, c, d)) + x) + 0x6ed9eba1, s);

        private int F4(int x, int y, int z) => 
            ((x & z) | (y & ~z));

        private int F4(int a, int b, int c, int d, int x, int s) => 
            this.RL(((a + this.F4(b, c, d)) + x) + -1894007588, s);

        private int FF1(int a, int b, int c, int d, int x, int s) => 
            this.RL((a + this.F1(b, c, d)) + x, s);

        private int FF2(int a, int b, int c, int d, int x, int s) => 
            this.RL(((a + this.F2(b, c, d)) + x) + 0x6d703ef3, s);

        private int FF3(int a, int b, int c, int d, int x, int s) => 
            this.RL(((a + this.F3(b, c, d)) + x) + 0x5c4dd124, s);

        private int FF4(int a, int b, int c, int d, int x, int s) => 
            this.RL(((a + this.F4(b, c, d)) + x) + 0x50a28be6, s);

        public override int GetDigestSize() => 
            0x10;

        internal override void ProcessBlock()
        {
            int num2;
            int num4;
            int num6;
            int num8;
            int a = num2 = this.H0;
            int b = num4 = this.H1;
            int c = num6 = this.H2;
            int d = num8 = this.H3;
            a = this.F1(a, b, c, d, this.X[0], 11);
            d = this.F1(d, a, b, c, this.X[1], 14);
            c = this.F1(c, d, a, b, this.X[2], 15);
            b = this.F1(b, c, d, a, this.X[3], 12);
            a = this.F1(a, b, c, d, this.X[4], 5);
            d = this.F1(d, a, b, c, this.X[5], 8);
            c = this.F1(c, d, a, b, this.X[6], 7);
            b = this.F1(b, c, d, a, this.X[7], 9);
            a = this.F1(a, b, c, d, this.X[8], 11);
            d = this.F1(d, a, b, c, this.X[9], 13);
            c = this.F1(c, d, a, b, this.X[10], 14);
            b = this.F1(b, c, d, a, this.X[11], 15);
            a = this.F1(a, b, c, d, this.X[12], 6);
            d = this.F1(d, a, b, c, this.X[13], 7);
            c = this.F1(c, d, a, b, this.X[14], 9);
            b = this.F1(b, c, d, a, this.X[15], 8);
            a = this.F2(a, b, c, d, this.X[7], 7);
            d = this.F2(d, a, b, c, this.X[4], 6);
            c = this.F2(c, d, a, b, this.X[13], 8);
            b = this.F2(b, c, d, a, this.X[1], 13);
            a = this.F2(a, b, c, d, this.X[10], 11);
            d = this.F2(d, a, b, c, this.X[6], 9);
            c = this.F2(c, d, a, b, this.X[15], 7);
            b = this.F2(b, c, d, a, this.X[3], 15);
            a = this.F2(a, b, c, d, this.X[12], 7);
            d = this.F2(d, a, b, c, this.X[0], 12);
            c = this.F2(c, d, a, b, this.X[9], 15);
            b = this.F2(b, c, d, a, this.X[5], 9);
            a = this.F2(a, b, c, d, this.X[2], 11);
            d = this.F2(d, a, b, c, this.X[14], 7);
            c = this.F2(c, d, a, b, this.X[11], 13);
            b = this.F2(b, c, d, a, this.X[8], 12);
            a = this.F3(a, b, c, d, this.X[3], 11);
            d = this.F3(d, a, b, c, this.X[10], 13);
            c = this.F3(c, d, a, b, this.X[14], 6);
            b = this.F3(b, c, d, a, this.X[4], 7);
            a = this.F3(a, b, c, d, this.X[9], 14);
            d = this.F3(d, a, b, c, this.X[15], 9);
            c = this.F3(c, d, a, b, this.X[8], 13);
            b = this.F3(b, c, d, a, this.X[1], 15);
            a = this.F3(a, b, c, d, this.X[2], 14);
            d = this.F3(d, a, b, c, this.X[7], 8);
            c = this.F3(c, d, a, b, this.X[0], 13);
            b = this.F3(b, c, d, a, this.X[6], 6);
            a = this.F3(a, b, c, d, this.X[13], 5);
            d = this.F3(d, a, b, c, this.X[11], 12);
            c = this.F3(c, d, a, b, this.X[5], 7);
            b = this.F3(b, c, d, a, this.X[12], 5);
            a = this.F4(a, b, c, d, this.X[1], 11);
            d = this.F4(d, a, b, c, this.X[9], 12);
            c = this.F4(c, d, a, b, this.X[11], 14);
            b = this.F4(b, c, d, a, this.X[10], 15);
            a = this.F4(a, b, c, d, this.X[0], 14);
            d = this.F4(d, a, b, c, this.X[8], 15);
            c = this.F4(c, d, a, b, this.X[12], 9);
            b = this.F4(b, c, d, a, this.X[4], 8);
            a = this.F4(a, b, c, d, this.X[13], 9);
            d = this.F4(d, a, b, c, this.X[3], 14);
            c = this.F4(c, d, a, b, this.X[7], 5);
            b = this.F4(b, c, d, a, this.X[15], 6);
            a = this.F4(a, b, c, d, this.X[14], 8);
            d = this.F4(d, a, b, c, this.X[5], 6);
            c = this.F4(c, d, a, b, this.X[6], 5);
            b = this.F4(b, c, d, a, this.X[2], 12);
            num2 = this.FF4(num2, num4, num6, num8, this.X[5], 8);
            num8 = this.FF4(num8, num2, num4, num6, this.X[14], 9);
            num6 = this.FF4(num6, num8, num2, num4, this.X[7], 9);
            num4 = this.FF4(num4, num6, num8, num2, this.X[0], 11);
            num2 = this.FF4(num2, num4, num6, num8, this.X[9], 13);
            num8 = this.FF4(num8, num2, num4, num6, this.X[2], 15);
            num6 = this.FF4(num6, num8, num2, num4, this.X[11], 15);
            num4 = this.FF4(num4, num6, num8, num2, this.X[4], 5);
            num2 = this.FF4(num2, num4, num6, num8, this.X[13], 7);
            num8 = this.FF4(num8, num2, num4, num6, this.X[6], 7);
            num6 = this.FF4(num6, num8, num2, num4, this.X[15], 8);
            num4 = this.FF4(num4, num6, num8, num2, this.X[8], 11);
            num2 = this.FF4(num2, num4, num6, num8, this.X[1], 14);
            num8 = this.FF4(num8, num2, num4, num6, this.X[10], 14);
            num6 = this.FF4(num6, num8, num2, num4, this.X[3], 12);
            num4 = this.FF4(num4, num6, num8, num2, this.X[12], 6);
            num2 = this.FF3(num2, num4, num6, num8, this.X[6], 9);
            num8 = this.FF3(num8, num2, num4, num6, this.X[11], 13);
            num6 = this.FF3(num6, num8, num2, num4, this.X[3], 15);
            num4 = this.FF3(num4, num6, num8, num2, this.X[7], 7);
            num2 = this.FF3(num2, num4, num6, num8, this.X[0], 12);
            num8 = this.FF3(num8, num2, num4, num6, this.X[13], 8);
            num6 = this.FF3(num6, num8, num2, num4, this.X[5], 9);
            num4 = this.FF3(num4, num6, num8, num2, this.X[10], 11);
            num2 = this.FF3(num2, num4, num6, num8, this.X[14], 7);
            num8 = this.FF3(num8, num2, num4, num6, this.X[15], 7);
            num6 = this.FF3(num6, num8, num2, num4, this.X[8], 12);
            num4 = this.FF3(num4, num6, num8, num2, this.X[12], 7);
            num2 = this.FF3(num2, num4, num6, num8, this.X[4], 6);
            num8 = this.FF3(num8, num2, num4, num6, this.X[9], 15);
            num6 = this.FF3(num6, num8, num2, num4, this.X[1], 13);
            num4 = this.FF3(num4, num6, num8, num2, this.X[2], 11);
            num2 = this.FF2(num2, num4, num6, num8, this.X[15], 9);
            num8 = this.FF2(num8, num2, num4, num6, this.X[5], 7);
            num6 = this.FF2(num6, num8, num2, num4, this.X[1], 15);
            num4 = this.FF2(num4, num6, num8, num2, this.X[3], 11);
            num2 = this.FF2(num2, num4, num6, num8, this.X[7], 8);
            num8 = this.FF2(num8, num2, num4, num6, this.X[14], 6);
            num6 = this.FF2(num6, num8, num2, num4, this.X[6], 6);
            num4 = this.FF2(num4, num6, num8, num2, this.X[9], 14);
            num2 = this.FF2(num2, num4, num6, num8, this.X[11], 12);
            num8 = this.FF2(num8, num2, num4, num6, this.X[8], 13);
            num6 = this.FF2(num6, num8, num2, num4, this.X[12], 5);
            num4 = this.FF2(num4, num6, num8, num2, this.X[2], 14);
            num2 = this.FF2(num2, num4, num6, num8, this.X[10], 13);
            num8 = this.FF2(num8, num2, num4, num6, this.X[0], 13);
            num6 = this.FF2(num6, num8, num2, num4, this.X[4], 7);
            num4 = this.FF2(num4, num6, num8, num2, this.X[13], 5);
            num2 = this.FF1(num2, num4, num6, num8, this.X[8], 15);
            num8 = this.FF1(num8, num2, num4, num6, this.X[6], 5);
            num6 = this.FF1(num6, num8, num2, num4, this.X[4], 8);
            num4 = this.FF1(num4, num6, num8, num2, this.X[1], 11);
            num2 = this.FF1(num2, num4, num6, num8, this.X[3], 14);
            num8 = this.FF1(num8, num2, num4, num6, this.X[11], 14);
            num6 = this.FF1(num6, num8, num2, num4, this.X[15], 6);
            num4 = this.FF1(num4, num6, num8, num2, this.X[0], 14);
            num2 = this.FF1(num2, num4, num6, num8, this.X[5], 6);
            num8 = this.FF1(num8, num2, num4, num6, this.X[12], 9);
            num6 = this.FF1(num6, num8, num2, num4, this.X[2], 12);
            num4 = this.FF1(num4, num6, num8, num2, this.X[13], 9);
            num2 = this.FF1(num2, num4, num6, num8, this.X[9], 12);
            num8 = this.FF1(num8, num2, num4, num6, this.X[7], 5);
            num6 = this.FF1(num6, num8, num2, num4, this.X[10], 15);
            num4 = this.FF1(num4, num6, num8, num2, this.X[14], 8);
            num8 += c + this.H1;
            this.H1 = (this.H2 + d) + num2;
            this.H2 = (this.H3 + a) + num4;
            this.H3 = (this.H0 + b) + num6;
            this.H0 = num8;
            this.xOff = 0;
            for (int i = 0; i != this.X.Length; i++)
            {
                this.X[i] = 0;
            }
        }

        internal override void ProcessLength(long bitLength)
        {
            if (this.xOff > 14)
            {
                this.ProcessBlock();
            }
            this.X[14] = (int) (((ulong) bitLength) & 0xffffffffL);
            this.X[15] = (int) (bitLength >> 0x20);
        }

        internal override void ProcessWord(byte[] input, int inOff)
        {
            this.X[this.xOff++] = (((input[inOff] & 0xff) | ((input[inOff + 1] & 0xff) << 8)) | ((input[inOff + 2] & 0xff) << 0x10)) | ((input[inOff + 3] & 0xff) << 0x18);
            if (this.xOff == 0x10)
            {
                this.ProcessBlock();
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.H0 = 0x67452301;
            this.H1 = -271733879;
            this.H2 = -1732584194;
            this.H3 = 0x10325476;
            this.xOff = 0;
            for (int i = 0; i != this.X.Length; i++)
            {
                this.X[i] = 0;
            }
        }

        public override void Reset(IMemoable other)
        {
            RipeMD128Digest t = (RipeMD128Digest) other;
            this.CopyIn(t);
        }

        private int RL(int x, int n) => 
            ((x << n) | (x >> (0x20 - n)));

        private void UnpackWord(int word, byte[] outBytes, int outOff)
        {
            outBytes[outOff] = (byte) word;
            outBytes[outOff + 1] = (byte) (word >> 8);
            outBytes[outOff + 2] = (byte) (word >> 0x10);
            outBytes[outOff + 3] = (byte) (word >> 0x18);
        }

        public override string AlgorithmName =>
            "RIPEMD128";
    }
}

