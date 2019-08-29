namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class RipeMD320Digest : GeneralDigest
    {
        private const int DigestLength = 40;
        private int H0;
        private int H1;
        private int H2;
        private int H3;
        private int H4;
        private int H5;
        private int H6;
        private int H7;
        private int H8;
        private int H9;
        private int[] X;
        private int xOff;

        public RipeMD320Digest()
        {
            this.X = new int[0x10];
            this.Reset();
        }

        public RipeMD320Digest(RipeMD320Digest t) : base(t)
        {
            this.X = new int[0x10];
            this.CopyIn(t);
        }

        public override IMemoable Copy() => 
            new RipeMD320Digest(this);

        private void CopyIn(RipeMD320Digest t)
        {
            base.CopyIn(t);
            this.H0 = t.H0;
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            this.H5 = t.H5;
            this.H6 = t.H6;
            this.H7 = t.H7;
            this.H8 = t.H8;
            this.H9 = t.H9;
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
            this.UnpackWord(this.H4, output, outOff + 0x10);
            this.UnpackWord(this.H5, output, outOff + 20);
            this.UnpackWord(this.H6, output, outOff + 0x18);
            this.UnpackWord(this.H7, output, outOff + 0x1c);
            this.UnpackWord(this.H8, output, outOff + 0x20);
            this.UnpackWord(this.H9, output, outOff + 0x24);
            this.Reset();
            return 40;
        }

        private int F1(int x, int y, int z) => 
            ((x ^ y) ^ z);

        private int F2(int x, int y, int z) => 
            ((x & y) | (~x & z));

        private int F3(int x, int y, int z) => 
            ((x | ~y) ^ z);

        private int F4(int x, int y, int z) => 
            ((x & z) | (y & ~z));

        private int F5(int x, int y, int z) => 
            (x ^ (y | ~z));

        public override int GetDigestSize() => 
            40;

        internal override void ProcessBlock()
        {
            int x = this.H0;
            int num3 = this.H1;
            int y = this.H2;
            int z = this.H3;
            int num9 = this.H4;
            int num2 = this.H5;
            int num4 = this.H6;
            int num6 = this.H7;
            int num8 = this.H8;
            int num10 = this.H9;
            x = this.RL((x + this.F1(num3, y, z)) + this.X[0], 11) + num9;
            y = this.RL(y, 10);
            num9 = this.RL((num9 + this.F1(x, num3, y)) + this.X[1], 14) + z;
            num3 = this.RL(num3, 10);
            z = this.RL((z + this.F1(num9, x, num3)) + this.X[2], 15) + y;
            x = this.RL(x, 10);
            y = this.RL((y + this.F1(z, num9, x)) + this.X[3], 12) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL((num3 + this.F1(y, z, num9)) + this.X[4], 5) + x;
            z = this.RL(z, 10);
            x = this.RL((x + this.F1(num3, y, z)) + this.X[5], 8) + num9;
            y = this.RL(y, 10);
            num9 = this.RL((num9 + this.F1(x, num3, y)) + this.X[6], 7) + z;
            num3 = this.RL(num3, 10);
            z = this.RL((z + this.F1(num9, x, num3)) + this.X[7], 9) + y;
            x = this.RL(x, 10);
            y = this.RL((y + this.F1(z, num9, x)) + this.X[8], 11) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL((num3 + this.F1(y, z, num9)) + this.X[9], 13) + x;
            z = this.RL(z, 10);
            x = this.RL((x + this.F1(num3, y, z)) + this.X[10], 14) + num9;
            y = this.RL(y, 10);
            num9 = this.RL((num9 + this.F1(x, num3, y)) + this.X[11], 15) + z;
            num3 = this.RL(num3, 10);
            z = this.RL((z + this.F1(num9, x, num3)) + this.X[12], 6) + y;
            x = this.RL(x, 10);
            y = this.RL((y + this.F1(z, num9, x)) + this.X[13], 7) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL((num3 + this.F1(y, z, num9)) + this.X[14], 9) + x;
            z = this.RL(z, 10);
            x = this.RL((x + this.F1(num3, y, z)) + this.X[15], 8) + num9;
            y = this.RL(y, 10);
            num2 = this.RL(((num2 + this.F5(num4, num6, num8)) + this.X[5]) + 0x50a28be6, 8) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F5(num2, num4, num6)) + this.X[14]) + 0x50a28be6, 9) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F5(num10, num2, num4)) + this.X[7]) + 0x50a28be6, 9) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F5(num8, num10, num2)) + this.X[0]) + 0x50a28be6, 11) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F5(num6, num8, num10)) + this.X[9]) + 0x50a28be6, 13) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F5(num4, num6, num8)) + this.X[2]) + 0x50a28be6, 15) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F5(num2, num4, num6)) + this.X[11]) + 0x50a28be6, 15) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F5(num10, num2, num4)) + this.X[4]) + 0x50a28be6, 5) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F5(num8, num10, num2)) + this.X[13]) + 0x50a28be6, 7) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F5(num6, num8, num10)) + this.X[6]) + 0x50a28be6, 7) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F5(num4, num6, num8)) + this.X[15]) + 0x50a28be6, 8) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F5(num2, num4, num6)) + this.X[8]) + 0x50a28be6, 11) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F5(num10, num2, num4)) + this.X[1]) + 0x50a28be6, 14) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F5(num8, num10, num2)) + this.X[10]) + 0x50a28be6, 14) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F5(num6, num8, num10)) + this.X[3]) + 0x50a28be6, 12) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F5(num4, num6, num8)) + this.X[12]) + 0x50a28be6, 6) + num10;
            num6 = this.RL(num6, 10);
            int num11 = x;
            x = num2;
            num2 = num11;
            num9 = this.RL(((num9 + this.F2(x, num3, y)) + this.X[7]) + 0x5a827999, 7) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F2(num9, x, num3)) + this.X[4]) + 0x5a827999, 6) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F2(z, num9, x)) + this.X[13]) + 0x5a827999, 8) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F2(y, z, num9)) + this.X[1]) + 0x5a827999, 13) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F2(num3, y, z)) + this.X[10]) + 0x5a827999, 11) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F2(x, num3, y)) + this.X[6]) + 0x5a827999, 9) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F2(num9, x, num3)) + this.X[15]) + 0x5a827999, 7) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F2(z, num9, x)) + this.X[3]) + 0x5a827999, 15) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F2(y, z, num9)) + this.X[12]) + 0x5a827999, 7) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F2(num3, y, z)) + this.X[0]) + 0x5a827999, 12) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F2(x, num3, y)) + this.X[9]) + 0x5a827999, 15) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F2(num9, x, num3)) + this.X[5]) + 0x5a827999, 9) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F2(z, num9, x)) + this.X[2]) + 0x5a827999, 11) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F2(y, z, num9)) + this.X[14]) + 0x5a827999, 7) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F2(num3, y, z)) + this.X[11]) + 0x5a827999, 13) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F2(x, num3, y)) + this.X[8]) + 0x5a827999, 12) + z;
            num3 = this.RL(num3, 10);
            num10 = this.RL(((num10 + this.F4(num2, num4, num6)) + this.X[6]) + 0x5c4dd124, 9) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F4(num10, num2, num4)) + this.X[11]) + 0x5c4dd124, 13) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F4(num8, num10, num2)) + this.X[3]) + 0x5c4dd124, 15) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F4(num6, num8, num10)) + this.X[7]) + 0x5c4dd124, 7) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F4(num4, num6, num8)) + this.X[0]) + 0x5c4dd124, 12) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F4(num2, num4, num6)) + this.X[13]) + 0x5c4dd124, 8) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F4(num10, num2, num4)) + this.X[5]) + 0x5c4dd124, 9) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F4(num8, num10, num2)) + this.X[10]) + 0x5c4dd124, 11) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F4(num6, num8, num10)) + this.X[14]) + 0x5c4dd124, 7) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F4(num4, num6, num8)) + this.X[15]) + 0x5c4dd124, 7) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F4(num2, num4, num6)) + this.X[8]) + 0x5c4dd124, 12) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F4(num10, num2, num4)) + this.X[12]) + 0x5c4dd124, 7) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F4(num8, num10, num2)) + this.X[4]) + 0x5c4dd124, 6) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F4(num6, num8, num10)) + this.X[9]) + 0x5c4dd124, 15) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F4(num4, num6, num8)) + this.X[1]) + 0x5c4dd124, 13) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F4(num2, num4, num6)) + this.X[2]) + 0x5c4dd124, 11) + num8;
            num4 = this.RL(num4, 10);
            num11 = num3;
            num3 = num4;
            num4 = num11;
            z = this.RL(((z + this.F3(num9, x, num3)) + this.X[3]) + 0x6ed9eba1, 11) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F3(z, num9, x)) + this.X[10]) + 0x6ed9eba1, 13) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F3(y, z, num9)) + this.X[14]) + 0x6ed9eba1, 6) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F3(num3, y, z)) + this.X[4]) + 0x6ed9eba1, 7) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F3(x, num3, y)) + this.X[9]) + 0x6ed9eba1, 14) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F3(num9, x, num3)) + this.X[15]) + 0x6ed9eba1, 9) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F3(z, num9, x)) + this.X[8]) + 0x6ed9eba1, 13) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F3(y, z, num9)) + this.X[1]) + 0x6ed9eba1, 15) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F3(num3, y, z)) + this.X[2]) + 0x6ed9eba1, 14) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F3(x, num3, y)) + this.X[7]) + 0x6ed9eba1, 8) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F3(num9, x, num3)) + this.X[0]) + 0x6ed9eba1, 13) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F3(z, num9, x)) + this.X[6]) + 0x6ed9eba1, 6) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F3(y, z, num9)) + this.X[13]) + 0x6ed9eba1, 5) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F3(num3, y, z)) + this.X[11]) + 0x6ed9eba1, 12) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F3(x, num3, y)) + this.X[5]) + 0x6ed9eba1, 7) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F3(num9, x, num3)) + this.X[12]) + 0x6ed9eba1, 5) + y;
            x = this.RL(x, 10);
            num8 = this.RL(((num8 + this.F3(num10, num2, num4)) + this.X[15]) + 0x6d703ef3, 9) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F3(num8, num10, num2)) + this.X[5]) + 0x6d703ef3, 7) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F3(num6, num8, num10)) + this.X[1]) + 0x6d703ef3, 15) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F3(num4, num6, num8)) + this.X[3]) + 0x6d703ef3, 11) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F3(num2, num4, num6)) + this.X[7]) + 0x6d703ef3, 8) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F3(num10, num2, num4)) + this.X[14]) + 0x6d703ef3, 6) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F3(num8, num10, num2)) + this.X[6]) + 0x6d703ef3, 6) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F3(num6, num8, num10)) + this.X[9]) + 0x6d703ef3, 14) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F3(num4, num6, num8)) + this.X[11]) + 0x6d703ef3, 12) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F3(num2, num4, num6)) + this.X[8]) + 0x6d703ef3, 13) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F3(num10, num2, num4)) + this.X[12]) + 0x6d703ef3, 5) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F3(num8, num10, num2)) + this.X[2]) + 0x6d703ef3, 14) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F3(num6, num8, num10)) + this.X[10]) + 0x6d703ef3, 13) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F3(num4, num6, num8)) + this.X[0]) + 0x6d703ef3, 13) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F3(num2, num4, num6)) + this.X[4]) + 0x6d703ef3, 7) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F3(num10, num2, num4)) + this.X[13]) + 0x6d703ef3, 5) + num6;
            num2 = this.RL(num2, 10);
            num11 = y;
            y = num6;
            num6 = num11;
            y = this.RL(((y + this.F4(z, num9, x)) + this.X[1]) + -1894007588, 11) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F4(y, z, num9)) + this.X[9]) + -1894007588, 12) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F4(num3, y, z)) + this.X[11]) + -1894007588, 14) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F4(x, num3, y)) + this.X[10]) + -1894007588, 15) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F4(num9, x, num3)) + this.X[0]) + -1894007588, 14) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F4(z, num9, x)) + this.X[8]) + -1894007588, 15) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F4(y, z, num9)) + this.X[12]) + -1894007588, 9) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F4(num3, y, z)) + this.X[4]) + -1894007588, 8) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F4(x, num3, y)) + this.X[13]) + -1894007588, 9) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F4(num9, x, num3)) + this.X[3]) + -1894007588, 14) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F4(z, num9, x)) + this.X[7]) + -1894007588, 5) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F4(y, z, num9)) + this.X[15]) + -1894007588, 6) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F4(num3, y, z)) + this.X[14]) + -1894007588, 8) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F4(x, num3, y)) + this.X[5]) + -1894007588, 6) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F4(num9, x, num3)) + this.X[6]) + -1894007588, 5) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F4(z, num9, x)) + this.X[2]) + -1894007588, 12) + num3;
            num9 = this.RL(num9, 10);
            num6 = this.RL(((num6 + this.F2(num8, num10, num2)) + this.X[8]) + 0x7a6d76e9, 15) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F2(num6, num8, num10)) + this.X[6]) + 0x7a6d76e9, 5) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F2(num4, num6, num8)) + this.X[4]) + 0x7a6d76e9, 8) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F2(num2, num4, num6)) + this.X[1]) + 0x7a6d76e9, 11) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F2(num10, num2, num4)) + this.X[3]) + 0x7a6d76e9, 14) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F2(num8, num10, num2)) + this.X[11]) + 0x7a6d76e9, 14) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F2(num6, num8, num10)) + this.X[15]) + 0x7a6d76e9, 6) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F2(num4, num6, num8)) + this.X[0]) + 0x7a6d76e9, 14) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F2(num2, num4, num6)) + this.X[5]) + 0x7a6d76e9, 6) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F2(num10, num2, num4)) + this.X[12]) + 0x7a6d76e9, 9) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F2(num8, num10, num2)) + this.X[2]) + 0x7a6d76e9, 12) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL(((num4 + this.F2(num6, num8, num10)) + this.X[13]) + 0x7a6d76e9, 9) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL(((num2 + this.F2(num4, num6, num8)) + this.X[9]) + 0x7a6d76e9, 12) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL(((num10 + this.F2(num2, num4, num6)) + this.X[7]) + 0x7a6d76e9, 5) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL(((num8 + this.F2(num10, num2, num4)) + this.X[10]) + 0x7a6d76e9, 15) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL(((num6 + this.F2(num8, num10, num2)) + this.X[14]) + 0x7a6d76e9, 8) + num4;
            num10 = this.RL(num10, 10);
            num11 = z;
            z = num8;
            num8 = num11;
            num3 = this.RL(((num3 + this.F5(y, z, num9)) + this.X[4]) + -1454113458, 9) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F5(num3, y, z)) + this.X[0]) + -1454113458, 15) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F5(x, num3, y)) + this.X[5]) + -1454113458, 5) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F5(num9, x, num3)) + this.X[9]) + -1454113458, 11) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F5(z, num9, x)) + this.X[7]) + -1454113458, 6) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F5(y, z, num9)) + this.X[12]) + -1454113458, 8) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F5(num3, y, z)) + this.X[2]) + -1454113458, 13) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F5(x, num3, y)) + this.X[10]) + -1454113458, 12) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F5(num9, x, num3)) + this.X[14]) + -1454113458, 5) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F5(z, num9, x)) + this.X[1]) + -1454113458, 12) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F5(y, z, num9)) + this.X[3]) + -1454113458, 13) + x;
            z = this.RL(z, 10);
            x = this.RL(((x + this.F5(num3, y, z)) + this.X[8]) + -1454113458, 14) + num9;
            y = this.RL(y, 10);
            num9 = this.RL(((num9 + this.F5(x, num3, y)) + this.X[11]) + -1454113458, 11) + z;
            num3 = this.RL(num3, 10);
            z = this.RL(((z + this.F5(num9, x, num3)) + this.X[6]) + -1454113458, 8) + y;
            x = this.RL(x, 10);
            y = this.RL(((y + this.F5(z, num9, x)) + this.X[15]) + -1454113458, 5) + num3;
            num9 = this.RL(num9, 10);
            num3 = this.RL(((num3 + this.F5(y, z, num9)) + this.X[13]) + -1454113458, 6) + x;
            z = this.RL(z, 10);
            num4 = this.RL((num4 + this.F1(num6, num8, num10)) + this.X[12], 8) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL((num2 + this.F1(num4, num6, num8)) + this.X[15], 5) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL((num10 + this.F1(num2, num4, num6)) + this.X[10], 12) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL((num8 + this.F1(num10, num2, num4)) + this.X[4], 9) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL((num6 + this.F1(num8, num10, num2)) + this.X[1], 12) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL((num4 + this.F1(num6, num8, num10)) + this.X[5], 5) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL((num2 + this.F1(num4, num6, num8)) + this.X[8], 14) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL((num10 + this.F1(num2, num4, num6)) + this.X[7], 6) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL((num8 + this.F1(num10, num2, num4)) + this.X[6], 8) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL((num6 + this.F1(num8, num10, num2)) + this.X[2], 13) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL((num4 + this.F1(num6, num8, num10)) + this.X[13], 6) + num2;
            num8 = this.RL(num8, 10);
            num2 = this.RL((num2 + this.F1(num4, num6, num8)) + this.X[14], 5) + num10;
            num6 = this.RL(num6, 10);
            num10 = this.RL((num10 + this.F1(num2, num4, num6)) + this.X[0], 15) + num8;
            num4 = this.RL(num4, 10);
            num8 = this.RL((num8 + this.F1(num10, num2, num4)) + this.X[3], 13) + num6;
            num2 = this.RL(num2, 10);
            num6 = this.RL((num6 + this.F1(num8, num10, num2)) + this.X[9], 11) + num4;
            num10 = this.RL(num10, 10);
            num4 = this.RL((num4 + this.F1(num6, num8, num10)) + this.X[11], 11) + num2;
            num8 = this.RL(num8, 10);
            this.H0 += x;
            this.H1 += num3;
            this.H2 += y;
            this.H3 += z;
            this.H4 += num10;
            this.H5 += num2;
            this.H6 += num4;
            this.H7 += num6;
            this.H8 += num8;
            this.H9 += num9;
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
            this.H4 = -1009589776;
            this.H5 = 0x76543210;
            this.H6 = -19088744;
            this.H7 = -1985229329;
            this.H8 = 0x1234567;
            this.H9 = 0x3c2d1e0f;
            this.xOff = 0;
            for (int i = 0; i != this.X.Length; i++)
            {
                this.X[i] = 0;
            }
        }

        public override void Reset(IMemoable other)
        {
            RipeMD320Digest t = (RipeMD320Digest) other;
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
            "RIPEMD320";
    }
}

