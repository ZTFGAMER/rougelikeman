namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Sha256Digest : GeneralDigest
    {
        private const int DigestLength = 0x20;
        private uint H1;
        private uint H2;
        private uint H3;
        private uint H4;
        private uint H5;
        private uint H6;
        private uint H7;
        private uint H8;
        private uint[] X;
        private int xOff;
        private static readonly uint[] K = new uint[] { 
            0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5, 0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
            0xe49b69c1, 0xefbe4786, 0xfc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da, 0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x6ca6351, 0x14292967,
            0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85, 0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3, 0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
        };

        public Sha256Digest()
        {
            this.X = new uint[0x40];
            this.initHs();
        }

        public Sha256Digest(Sha256Digest t) : base(t)
        {
            this.X = new uint[0x40];
            this.CopyIn(t);
        }

        public override IMemoable Copy() => 
            new Sha256Digest(this);

        private void CopyIn(Sha256Digest t)
        {
            base.CopyIn(t);
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            this.H5 = t.H5;
            this.H6 = t.H6;
            this.H7 = t.H7;
            this.H8 = t.H8;
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
            Pack.UInt32_To_BE(this.H6, output, outOff + 20);
            Pack.UInt32_To_BE(this.H7, output, outOff + 0x18);
            Pack.UInt32_To_BE(this.H8, output, outOff + 0x1c);
            this.Reset();
            return 0x20;
        }

        public override int GetDigestSize() => 
            0x20;

        private void initHs()
        {
            this.H1 = 0x6a09e667;
            this.H2 = 0xbb67ae85;
            this.H3 = 0x3c6ef372;
            this.H4 = 0xa54ff53a;
            this.H5 = 0x510e527f;
            this.H6 = 0x9b05688c;
            this.H7 = 0x1f83d9ab;
            this.H8 = 0x5be0cd19;
        }

        internal override void ProcessBlock()
        {
            for (int i = 0x10; i <= 0x3f; i++)
            {
                this.X[i] = ((Theta1(this.X[i - 2]) + this.X[i - 7]) + Theta0(this.X[i - 15])) + this.X[i - 0x10];
            }
            uint x = this.H1;
            uint y = this.H2;
            uint z = this.H3;
            uint num5 = this.H4;
            uint num6 = this.H5;
            uint num7 = this.H6;
            uint num8 = this.H7;
            uint num9 = this.H8;
            int index = 0;
            for (int j = 0; j < 8; j++)
            {
                num9 += (Sum1Ch(num6, num7, num8) + K[index]) + this.X[index];
                num5 += num9;
                num9 += Sum0Maj(x, y, z);
                index++;
                num8 += (Sum1Ch(num5, num6, num7) + K[index]) + this.X[index];
                z += num8;
                num8 += Sum0Maj(num9, x, y);
                index++;
                num7 += (Sum1Ch(z, num5, num6) + K[index]) + this.X[index];
                y += num7;
                num7 += Sum0Maj(num8, num9, x);
                index++;
                num6 += (Sum1Ch(y, z, num5) + K[index]) + this.X[index];
                x += num6;
                num6 += Sum0Maj(num7, num8, num9);
                index++;
                num5 += (Sum1Ch(x, y, z) + K[index]) + this.X[index];
                num9 += num5;
                num5 += Sum0Maj(num6, num7, num8);
                index++;
                z += (Sum1Ch(num9, x, y) + K[index]) + this.X[index];
                num8 += z;
                z += Sum0Maj(num5, num6, num7);
                index++;
                y += (Sum1Ch(num8, num9, x) + K[index]) + this.X[index];
                num7 += y;
                y += Sum0Maj(z, num5, num6);
                index++;
                x += (Sum1Ch(num7, num8, num9) + K[index]) + this.X[index];
                num6 += x;
                x += Sum0Maj(y, z, num5);
                index++;
            }
            this.H1 += x;
            this.H2 += y;
            this.H3 += z;
            this.H4 += num5;
            this.H5 += num6;
            this.H6 += num7;
            this.H7 += num8;
            this.H8 += num9;
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
            this.initHs();
            this.xOff = 0;
            Array.Clear(this.X, 0, this.X.Length);
        }

        public override void Reset(IMemoable other)
        {
            Sha256Digest t = (Sha256Digest) other;
            this.CopyIn(t);
        }

        private static uint Sum0Maj(uint x, uint y, uint z) => 
            (((((x >> 2) | (x << 30)) ^ ((x >> 13) | (x << 0x13))) ^ ((x >> 0x16) | (x << 10))) + (((x & y) ^ (x & z)) ^ (y & z)));

        private static uint Sum1Ch(uint x, uint y, uint z) => 
            (((((x >> 6) | (x << 0x1a)) ^ ((x >> 11) | (x << 0x15))) ^ ((x >> 0x19) | (x << 7))) + ((x & y) ^ (~x & z)));

        private static uint Theta0(uint x) => 
            ((((x >> 7) | (x << 0x19)) ^ ((x >> 0x12) | (x << 14))) ^ (x >> 3));

        private static uint Theta1(uint x) => 
            ((((x >> 0x11) | (x << 15)) ^ ((x >> 0x13) | (x << 13))) ^ (x >> 10));

        public override string AlgorithmName =>
            "SHA-256";
    }
}

