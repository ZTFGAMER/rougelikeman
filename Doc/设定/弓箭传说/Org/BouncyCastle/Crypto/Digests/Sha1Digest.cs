namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Sha1Digest : GeneralDigest
    {
        private const int DigestLength = 20;
        private uint H1;
        private uint H2;
        private uint H3;
        private uint H4;
        private uint H5;
        private uint[] X;
        private int xOff;
        private const uint Y1 = 0x5a827999;
        private const uint Y2 = 0x6ed9eba1;
        private const uint Y3 = 0x8f1bbcdc;
        private const uint Y4 = 0xca62c1d6;

        public Sha1Digest()
        {
            this.X = new uint[80];
            this.Reset();
        }

        public Sha1Digest(Sha1Digest t) : base(t)
        {
            this.X = new uint[80];
            this.CopyIn(t);
        }

        public override IMemoable Copy() => 
            new Sha1Digest(this);

        private void CopyIn(Sha1Digest t)
        {
            base.CopyIn(t);
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            this.H5 = t.H5;
            Array.Copy(t.X, 0, this.X, 0, t.X.Length);
            this.xOff = t.xOff;
        }

        public override int DoFinal(byte[] output, int outOff)
        {
            base.Finish();
            Pack.UInt32_To_BE(this.H1, output, outOff);
            Pack.UInt32_To_BE(this.H2, output, outOff + 4);
            Pack.UInt32_To_BE(this.H3, output, outOff + 8);
            Pack.UInt32_To_BE(this.H4, output, outOff + 12);
            Pack.UInt32_To_BE(this.H5, output, outOff + 0x10);
            this.Reset();
            return 20;
        }

        private static uint F(uint u, uint v, uint w) => 
            ((u & v) | (~u & w));

        private static uint G(uint u, uint v, uint w) => 
            (((u & v) | (u & w)) | (v & w));

        public override int GetDigestSize() => 
            20;

        private static uint H(uint u, uint v, uint w) => 
            ((u ^ v) ^ w);

        internal override void ProcessBlock()
        {
            for (int i = 0x10; i < 80; i++)
            {
                uint num2 = ((this.X[i - 3] ^ this.X[i - 8]) ^ this.X[i - 14]) ^ this.X[i - 0x10];
                this.X[i] = (num2 << 1) | (num2 >> 0x1f);
            }
            uint u = this.H1;
            uint num4 = this.H2;
            uint v = this.H3;
            uint w = this.H4;
            uint num7 = this.H5;
            int num8 = 0;
            for (int j = 0; j < 4; j++)
            {
                num7 += ((((u << 5) | (u >> 0x1b)) + F(num4, v, w)) + this.X[num8++]) + 0x5a827999;
                num4 = (num4 << 30) | (num4 >> 2);
                w += ((((num7 << 5) | (num7 >> 0x1b)) + F(u, num4, v)) + this.X[num8++]) + 0x5a827999;
                u = (u << 30) | (u >> 2);
                v += ((((w << 5) | (w >> 0x1b)) + F(num7, u, num4)) + this.X[num8++]) + 0x5a827999;
                num7 = (num7 << 30) | (num7 >> 2);
                num4 += ((((v << 5) | (v >> 0x1b)) + F(w, num7, u)) + this.X[num8++]) + 0x5a827999;
                w = (w << 30) | (w >> 2);
                u += ((((num4 << 5) | (num4 >> 0x1b)) + F(v, w, num7)) + this.X[num8++]) + 0x5a827999;
                v = (v << 30) | (v >> 2);
            }
            for (int k = 0; k < 4; k++)
            {
                num7 += ((((u << 5) | (u >> 0x1b)) + H(num4, v, w)) + this.X[num8++]) + 0x6ed9eba1;
                num4 = (num4 << 30) | (num4 >> 2);
                w += ((((num7 << 5) | (num7 >> 0x1b)) + H(u, num4, v)) + this.X[num8++]) + 0x6ed9eba1;
                u = (u << 30) | (u >> 2);
                v += ((((w << 5) | (w >> 0x1b)) + H(num7, u, num4)) + this.X[num8++]) + 0x6ed9eba1;
                num7 = (num7 << 30) | (num7 >> 2);
                num4 += ((((v << 5) | (v >> 0x1b)) + H(w, num7, u)) + this.X[num8++]) + 0x6ed9eba1;
                w = (w << 30) | (w >> 2);
                u += ((((num4 << 5) | (num4 >> 0x1b)) + H(v, w, num7)) + this.X[num8++]) + 0x6ed9eba1;
                v = (v << 30) | (v >> 2);
            }
            for (int m = 0; m < 4; m++)
            {
                num7 += ((((u << 5) | (u >> 0x1b)) + G(num4, v, w)) + this.X[num8++]) + 0x8f1bbcdc;
                num4 = (num4 << 30) | (num4 >> 2);
                w += ((((num7 << 5) | (num7 >> 0x1b)) + G(u, num4, v)) + this.X[num8++]) + 0x8f1bbcdc;
                u = (u << 30) | (u >> 2);
                v += ((((w << 5) | (w >> 0x1b)) + G(num7, u, num4)) + this.X[num8++]) + 0x8f1bbcdc;
                num7 = (num7 << 30) | (num7 >> 2);
                num4 += ((((v << 5) | (v >> 0x1b)) + G(w, num7, u)) + this.X[num8++]) + 0x8f1bbcdc;
                w = (w << 30) | (w >> 2);
                u += ((((num4 << 5) | (num4 >> 0x1b)) + G(v, w, num7)) + this.X[num8++]) + 0x8f1bbcdc;
                v = (v << 30) | (v >> 2);
            }
            for (int n = 0; n < 4; n++)
            {
                num7 += ((((u << 5) | (u >> 0x1b)) + H(num4, v, w)) + this.X[num8++]) + 0xca62c1d6;
                num4 = (num4 << 30) | (num4 >> 2);
                w += ((((num7 << 5) | (num7 >> 0x1b)) + H(u, num4, v)) + this.X[num8++]) + 0xca62c1d6;
                u = (u << 30) | (u >> 2);
                v += ((((w << 5) | (w >> 0x1b)) + H(num7, u, num4)) + this.X[num8++]) + 0xca62c1d6;
                num7 = (num7 << 30) | (num7 >> 2);
                num4 += ((((v << 5) | (v >> 0x1b)) + H(w, num7, u)) + this.X[num8++]) + 0xca62c1d6;
                w = (w << 30) | (w >> 2);
                u += ((((num4 << 5) | (num4 >> 0x1b)) + H(v, w, num7)) + this.X[num8++]) + 0xca62c1d6;
                v = (v << 30) | (v >> 2);
            }
            this.H1 += u;
            this.H2 += num4;
            this.H3 += v;
            this.H4 += w;
            this.H5 += num7;
            this.xOff = 0;
            Array.Clear(this.X, 0, 0x10);
        }

        internal override void ProcessLength(long bitLength)
        {
            if (this.xOff > 14)
            {
                this.ProcessBlock();
            }
            this.X[14] = (uint) (bitLength >> 0x20);
            this.X[15] = (uint) bitLength;
        }

        internal override void ProcessWord(byte[] input, int inOff)
        {
            this.X[this.xOff] = Pack.BE_To_UInt32(input, inOff);
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
            this.H5 = 0xc3d2e1f0;
            this.xOff = 0;
            Array.Clear(this.X, 0, this.X.Length);
        }

        public override void Reset(IMemoable other)
        {
            Sha1Digest t = (Sha1Digest) other;
            this.CopyIn(t);
        }

        public override string AlgorithmName =>
            "SHA-1";
    }
}

