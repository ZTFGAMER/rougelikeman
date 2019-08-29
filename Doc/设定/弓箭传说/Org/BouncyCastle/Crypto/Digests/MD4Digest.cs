namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class MD4Digest : GeneralDigest
    {
        private const int DigestLength = 0x10;
        private int H1;
        private int H2;
        private int H3;
        private int H4;
        private int[] X;
        private int xOff;
        private const int S11 = 3;
        private const int S12 = 7;
        private const int S13 = 11;
        private const int S14 = 0x13;
        private const int S21 = 3;
        private const int S22 = 5;
        private const int S23 = 9;
        private const int S24 = 13;
        private const int S31 = 3;
        private const int S32 = 9;
        private const int S33 = 11;
        private const int S34 = 15;

        public MD4Digest()
        {
            this.X = new int[0x10];
            this.Reset();
        }

        public MD4Digest(MD4Digest t) : base(t)
        {
            this.X = new int[0x10];
            this.CopyIn(t);
        }

        public override IMemoable Copy() => 
            new MD4Digest(this);

        private void CopyIn(MD4Digest t)
        {
            base.CopyIn(t);
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            Array.Copy(t.X, 0, this.X, 0, t.X.Length);
            this.xOff = t.xOff;
        }

        public override int DoFinal(byte[] output, int outOff)
        {
            base.Finish();
            this.UnpackWord(this.H1, output, outOff);
            this.UnpackWord(this.H2, output, outOff + 4);
            this.UnpackWord(this.H3, output, outOff + 8);
            this.UnpackWord(this.H4, output, outOff + 12);
            this.Reset();
            return 0x10;
        }

        private int F(int u, int v, int w) => 
            ((u & v) | (~u & w));

        private int G(int u, int v, int w) => 
            (((u & v) | (u & w)) | (v & w));

        public override int GetDigestSize() => 
            0x10;

        private int H(int u, int v, int w) => 
            ((u ^ v) ^ w);

        internal override void ProcessBlock()
        {
            int u = this.H1;
            int num2 = this.H2;
            int v = this.H3;
            int w = this.H4;
            u = this.RotateLeft((u + this.F(num2, v, w)) + this.X[0], 3);
            w = this.RotateLeft((w + this.F(u, num2, v)) + this.X[1], 7);
            v = this.RotateLeft((v + this.F(w, u, num2)) + this.X[2], 11);
            num2 = this.RotateLeft((num2 + this.F(v, w, u)) + this.X[3], 0x13);
            u = this.RotateLeft((u + this.F(num2, v, w)) + this.X[4], 3);
            w = this.RotateLeft((w + this.F(u, num2, v)) + this.X[5], 7);
            v = this.RotateLeft((v + this.F(w, u, num2)) + this.X[6], 11);
            num2 = this.RotateLeft((num2 + this.F(v, w, u)) + this.X[7], 0x13);
            u = this.RotateLeft((u + this.F(num2, v, w)) + this.X[8], 3);
            w = this.RotateLeft((w + this.F(u, num2, v)) + this.X[9], 7);
            v = this.RotateLeft((v + this.F(w, u, num2)) + this.X[10], 11);
            num2 = this.RotateLeft((num2 + this.F(v, w, u)) + this.X[11], 0x13);
            u = this.RotateLeft((u + this.F(num2, v, w)) + this.X[12], 3);
            w = this.RotateLeft((w + this.F(u, num2, v)) + this.X[13], 7);
            v = this.RotateLeft((v + this.F(w, u, num2)) + this.X[14], 11);
            num2 = this.RotateLeft((num2 + this.F(v, w, u)) + this.X[15], 0x13);
            u = this.RotateLeft(((u + this.G(num2, v, w)) + this.X[0]) + 0x5a827999, 3);
            w = this.RotateLeft(((w + this.G(u, num2, v)) + this.X[4]) + 0x5a827999, 5);
            v = this.RotateLeft(((v + this.G(w, u, num2)) + this.X[8]) + 0x5a827999, 9);
            num2 = this.RotateLeft(((num2 + this.G(v, w, u)) + this.X[12]) + 0x5a827999, 13);
            u = this.RotateLeft(((u + this.G(num2, v, w)) + this.X[1]) + 0x5a827999, 3);
            w = this.RotateLeft(((w + this.G(u, num2, v)) + this.X[5]) + 0x5a827999, 5);
            v = this.RotateLeft(((v + this.G(w, u, num2)) + this.X[9]) + 0x5a827999, 9);
            num2 = this.RotateLeft(((num2 + this.G(v, w, u)) + this.X[13]) + 0x5a827999, 13);
            u = this.RotateLeft(((u + this.G(num2, v, w)) + this.X[2]) + 0x5a827999, 3);
            w = this.RotateLeft(((w + this.G(u, num2, v)) + this.X[6]) + 0x5a827999, 5);
            v = this.RotateLeft(((v + this.G(w, u, num2)) + this.X[10]) + 0x5a827999, 9);
            num2 = this.RotateLeft(((num2 + this.G(v, w, u)) + this.X[14]) + 0x5a827999, 13);
            u = this.RotateLeft(((u + this.G(num2, v, w)) + this.X[3]) + 0x5a827999, 3);
            w = this.RotateLeft(((w + this.G(u, num2, v)) + this.X[7]) + 0x5a827999, 5);
            v = this.RotateLeft(((v + this.G(w, u, num2)) + this.X[11]) + 0x5a827999, 9);
            num2 = this.RotateLeft(((num2 + this.G(v, w, u)) + this.X[15]) + 0x5a827999, 13);
            u = this.RotateLeft(((u + this.H(num2, v, w)) + this.X[0]) + 0x6ed9eba1, 3);
            w = this.RotateLeft(((w + this.H(u, num2, v)) + this.X[8]) + 0x6ed9eba1, 9);
            v = this.RotateLeft(((v + this.H(w, u, num2)) + this.X[4]) + 0x6ed9eba1, 11);
            num2 = this.RotateLeft(((num2 + this.H(v, w, u)) + this.X[12]) + 0x6ed9eba1, 15);
            u = this.RotateLeft(((u + this.H(num2, v, w)) + this.X[2]) + 0x6ed9eba1, 3);
            w = this.RotateLeft(((w + this.H(u, num2, v)) + this.X[10]) + 0x6ed9eba1, 9);
            v = this.RotateLeft(((v + this.H(w, u, num2)) + this.X[6]) + 0x6ed9eba1, 11);
            num2 = this.RotateLeft(((num2 + this.H(v, w, u)) + this.X[14]) + 0x6ed9eba1, 15);
            u = this.RotateLeft(((u + this.H(num2, v, w)) + this.X[1]) + 0x6ed9eba1, 3);
            w = this.RotateLeft(((w + this.H(u, num2, v)) + this.X[9]) + 0x6ed9eba1, 9);
            v = this.RotateLeft(((v + this.H(w, u, num2)) + this.X[5]) + 0x6ed9eba1, 11);
            num2 = this.RotateLeft(((num2 + this.H(v, w, u)) + this.X[13]) + 0x6ed9eba1, 15);
            u = this.RotateLeft(((u + this.H(num2, v, w)) + this.X[3]) + 0x6ed9eba1, 3);
            w = this.RotateLeft(((w + this.H(u, num2, v)) + this.X[11]) + 0x6ed9eba1, 9);
            v = this.RotateLeft(((v + this.H(w, u, num2)) + this.X[7]) + 0x6ed9eba1, 11);
            num2 = this.RotateLeft(((num2 + this.H(v, w, u)) + this.X[15]) + 0x6ed9eba1, 15);
            this.H1 += u;
            this.H2 += num2;
            this.H3 += v;
            this.H4 += w;
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
            this.H1 = 0x67452301;
            this.H2 = -271733879;
            this.H3 = -1732584194;
            this.H4 = 0x10325476;
            this.xOff = 0;
            for (int i = 0; i != this.X.Length; i++)
            {
                this.X[i] = 0;
            }
        }

        public override void Reset(IMemoable other)
        {
            MD4Digest t = (MD4Digest) other;
            this.CopyIn(t);
        }

        private int RotateLeft(int x, int n) => 
            ((x << n) | (x >> (0x20 - n)));

        private void UnpackWord(int word, byte[] outBytes, int outOff)
        {
            outBytes[outOff] = (byte) word;
            outBytes[outOff + 1] = (byte) (word >> 8);
            outBytes[outOff + 2] = (byte) (word >> 0x10);
            outBytes[outOff + 3] = (byte) (word >> 0x18);
        }

        public override string AlgorithmName =>
            "MD4";
    }
}

