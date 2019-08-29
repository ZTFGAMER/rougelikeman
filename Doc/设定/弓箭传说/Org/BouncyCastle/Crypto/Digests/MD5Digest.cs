namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class MD5Digest : GeneralDigest
    {
        private const int DigestLength = 0x10;
        private uint H1;
        private uint H2;
        private uint H3;
        private uint H4;
        private uint[] X;
        private int xOff;
        private static readonly int S11 = 7;
        private static readonly int S12 = 12;
        private static readonly int S13 = 0x11;
        private static readonly int S14 = 0x16;
        private static readonly int S21 = 5;
        private static readonly int S22 = 9;
        private static readonly int S23 = 14;
        private static readonly int S24 = 20;
        private static readonly int S31 = 4;
        private static readonly int S32 = 11;
        private static readonly int S33 = 0x10;
        private static readonly int S34 = 0x17;
        private static readonly int S41 = 6;
        private static readonly int S42 = 10;
        private static readonly int S43 = 15;
        private static readonly int S44 = 0x15;

        public MD5Digest()
        {
            this.X = new uint[0x10];
            this.Reset();
        }

        public MD5Digest(MD5Digest t) : base(t)
        {
            this.X = new uint[0x10];
            this.CopyIn(t);
        }

        public override IMemoable Copy() => 
            new MD5Digest(this);

        private void CopyIn(MD5Digest t)
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
            Pack.UInt32_To_LE(this.H1, output, outOff);
            Pack.UInt32_To_LE(this.H2, output, outOff + 4);
            Pack.UInt32_To_LE(this.H3, output, outOff + 8);
            Pack.UInt32_To_LE(this.H4, output, outOff + 12);
            this.Reset();
            return 0x10;
        }

        private static uint F(uint u, uint v, uint w) => 
            ((u & v) | (~u & w));

        private static uint G(uint u, uint v, uint w) => 
            ((u & w) | (v & ~w));

        public override int GetDigestSize() => 
            0x10;

        private static uint H(uint u, uint v, uint w) => 
            ((u ^ v) ^ w);

        private static uint K(uint u, uint v, uint w) => 
            (v ^ (u | ~w));

        internal override void ProcessBlock()
        {
            uint u = this.H1;
            uint num2 = this.H2;
            uint v = this.H3;
            uint w = this.H4;
            u = RotateLeft(((u + F(num2, v, w)) + this.X[0]) + 0xd76aa478, S11) + num2;
            w = RotateLeft(((w + F(u, num2, v)) + this.X[1]) + 0xe8c7b756, S12) + u;
            v = RotateLeft(((v + F(w, u, num2)) + this.X[2]) + 0x242070db, S13) + w;
            num2 = RotateLeft(((num2 + F(v, w, u)) + this.X[3]) + 0xc1bdceee, S14) + v;
            u = RotateLeft(((u + F(num2, v, w)) + this.X[4]) + 0xf57c0faf, S11) + num2;
            w = RotateLeft(((w + F(u, num2, v)) + this.X[5]) + 0x4787c62a, S12) + u;
            v = RotateLeft(((v + F(w, u, num2)) + this.X[6]) + 0xa8304613, S13) + w;
            num2 = RotateLeft(((num2 + F(v, w, u)) + this.X[7]) + 0xfd469501, S14) + v;
            u = RotateLeft(((u + F(num2, v, w)) + this.X[8]) + 0x698098d8, S11) + num2;
            w = RotateLeft(((w + F(u, num2, v)) + this.X[9]) + 0x8b44f7af, S12) + u;
            v = RotateLeft(((v + F(w, u, num2)) + this.X[10]) + 0xffff5bb1, S13) + w;
            num2 = RotateLeft(((num2 + F(v, w, u)) + this.X[11]) + 0x895cd7be, S14) + v;
            u = RotateLeft(((u + F(num2, v, w)) + this.X[12]) + 0x6b901122, S11) + num2;
            w = RotateLeft(((w + F(u, num2, v)) + this.X[13]) + 0xfd987193, S12) + u;
            v = RotateLeft(((v + F(w, u, num2)) + this.X[14]) + 0xa679438e, S13) + w;
            num2 = RotateLeft(((num2 + F(v, w, u)) + this.X[15]) + 0x49b40821, S14) + v;
            u = RotateLeft(((u + G(num2, v, w)) + this.X[1]) + 0xf61e2562, S21) + num2;
            w = RotateLeft(((w + G(u, num2, v)) + this.X[6]) + 0xc040b340, S22) + u;
            v = RotateLeft(((v + G(w, u, num2)) + this.X[11]) + 0x265e5a51, S23) + w;
            num2 = RotateLeft(((num2 + G(v, w, u)) + this.X[0]) + 0xe9b6c7aa, S24) + v;
            u = RotateLeft(((u + G(num2, v, w)) + this.X[5]) + 0xd62f105d, S21) + num2;
            w = RotateLeft(((w + G(u, num2, v)) + this.X[10]) + 0x2441453, S22) + u;
            v = RotateLeft(((v + G(w, u, num2)) + this.X[15]) + 0xd8a1e681, S23) + w;
            num2 = RotateLeft(((num2 + G(v, w, u)) + this.X[4]) + 0xe7d3fbc8, S24) + v;
            u = RotateLeft(((u + G(num2, v, w)) + this.X[9]) + 0x21e1cde6, S21) + num2;
            w = RotateLeft(((w + G(u, num2, v)) + this.X[14]) + 0xc33707d6, S22) + u;
            v = RotateLeft(((v + G(w, u, num2)) + this.X[3]) + 0xf4d50d87, S23) + w;
            num2 = RotateLeft(((num2 + G(v, w, u)) + this.X[8]) + 0x455a14ed, S24) + v;
            u = RotateLeft(((u + G(num2, v, w)) + this.X[13]) + 0xa9e3e905, S21) + num2;
            w = RotateLeft(((w + G(u, num2, v)) + this.X[2]) + 0xfcefa3f8, S22) + u;
            v = RotateLeft(((v + G(w, u, num2)) + this.X[7]) + 0x676f02d9, S23) + w;
            num2 = RotateLeft(((num2 + G(v, w, u)) + this.X[12]) + 0x8d2a4c8a, S24) + v;
            u = RotateLeft(((u + H(num2, v, w)) + this.X[5]) + 0xfffa3942, S31) + num2;
            w = RotateLeft(((w + H(u, num2, v)) + this.X[8]) + 0x8771f681, S32) + u;
            v = RotateLeft(((v + H(w, u, num2)) + this.X[11]) + 0x6d9d6122, S33) + w;
            num2 = RotateLeft(((num2 + H(v, w, u)) + this.X[14]) + 0xfde5380c, S34) + v;
            u = RotateLeft(((u + H(num2, v, w)) + this.X[1]) + 0xa4beea44, S31) + num2;
            w = RotateLeft(((w + H(u, num2, v)) + this.X[4]) + 0x4bdecfa9, S32) + u;
            v = RotateLeft(((v + H(w, u, num2)) + this.X[7]) + 0xf6bb4b60, S33) + w;
            num2 = RotateLeft(((num2 + H(v, w, u)) + this.X[10]) + 0xbebfbc70, S34) + v;
            u = RotateLeft(((u + H(num2, v, w)) + this.X[13]) + 0x289b7ec6, S31) + num2;
            w = RotateLeft(((w + H(u, num2, v)) + this.X[0]) + 0xeaa127fa, S32) + u;
            v = RotateLeft(((v + H(w, u, num2)) + this.X[3]) + 0xd4ef3085, S33) + w;
            num2 = RotateLeft(((num2 + H(v, w, u)) + this.X[6]) + 0x4881d05, S34) + v;
            u = RotateLeft(((u + H(num2, v, w)) + this.X[9]) + 0xd9d4d039, S31) + num2;
            w = RotateLeft(((w + H(u, num2, v)) + this.X[12]) + 0xe6db99e5, S32) + u;
            v = RotateLeft(((v + H(w, u, num2)) + this.X[15]) + 0x1fa27cf8, S33) + w;
            num2 = RotateLeft(((num2 + H(v, w, u)) + this.X[2]) + 0xc4ac5665, S34) + v;
            u = RotateLeft(((u + K(num2, v, w)) + this.X[0]) + 0xf4292244, S41) + num2;
            w = RotateLeft(((w + K(u, num2, v)) + this.X[7]) + 0x432aff97, S42) + u;
            v = RotateLeft(((v + K(w, u, num2)) + this.X[14]) + 0xab9423a7, S43) + w;
            num2 = RotateLeft(((num2 + K(v, w, u)) + this.X[5]) + 0xfc93a039, S44) + v;
            u = RotateLeft(((u + K(num2, v, w)) + this.X[12]) + 0x655b59c3, S41) + num2;
            w = RotateLeft(((w + K(u, num2, v)) + this.X[3]) + 0x8f0ccc92, S42) + u;
            v = RotateLeft(((v + K(w, u, num2)) + this.X[10]) + 0xffeff47d, S43) + w;
            num2 = RotateLeft(((num2 + K(v, w, u)) + this.X[1]) + 0x85845dd1, S44) + v;
            u = RotateLeft(((u + K(num2, v, w)) + this.X[8]) + 0x6fa87e4f, S41) + num2;
            w = RotateLeft(((w + K(u, num2, v)) + this.X[15]) + 0xfe2ce6e0, S42) + u;
            v = RotateLeft(((v + K(w, u, num2)) + this.X[6]) + 0xa3014314, S43) + w;
            num2 = RotateLeft(((num2 + K(v, w, u)) + this.X[13]) + 0x4e0811a1, S44) + v;
            u = RotateLeft(((u + K(num2, v, w)) + this.X[4]) + 0xf7537e82, S41) + num2;
            w = RotateLeft(((w + K(u, num2, v)) + this.X[11]) + 0xbd3af235, S42) + u;
            v = RotateLeft(((v + K(w, u, num2)) + this.X[2]) + 0x2ad7d2bb, S43) + w;
            num2 = RotateLeft(((num2 + K(v, w, u)) + this.X[9]) + 0xeb86d391, S44) + v;
            this.H1 += u;
            this.H2 += num2;
            this.H3 += v;
            this.H4 += w;
            this.xOff = 0;
        }

        internal override void ProcessLength(long bitLength)
        {
            if (this.xOff > 14)
            {
                if (this.xOff == 15)
                {
                    this.X[15] = 0;
                }
                this.ProcessBlock();
            }
            for (int i = this.xOff; i < 14; i++)
            {
                this.X[i] = 0;
            }
            this.X[14] = (uint) bitLength;
            this.X[15] = (uint) (bitLength >> 0x20);
        }

        internal override void ProcessWord(byte[] input, int inOff)
        {
            this.X[this.xOff] = Pack.LE_To_UInt32(input, inOff);
            if (++this.xOff == 0x10)
            {
                this.ProcessBlock();
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.H1 = 0x67452301;
            this.H2 = 0xefcdab89;
            this.H3 = 0x98badcfe;
            this.H4 = 0x10325476;
            this.xOff = 0;
            for (int i = 0; i != this.X.Length; i++)
            {
                this.X[i] = 0;
            }
        }

        public override void Reset(IMemoable other)
        {
            MD5Digest t = (MD5Digest) other;
            this.CopyIn(t);
        }

        private static uint RotateLeft(uint x, int n) => 
            ((x << n) | (x >> (0x20 - n)));

        public override string AlgorithmName =>
            "MD5";
    }
}

