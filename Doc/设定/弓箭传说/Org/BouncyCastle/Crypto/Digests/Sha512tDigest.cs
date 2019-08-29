namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class Sha512tDigest : LongDigest
    {
        private const ulong A5 = 11_936_128_518_282_651_045L;
        private readonly int digestLength;
        private ulong H1t;
        private ulong H2t;
        private ulong H3t;
        private ulong H4t;
        private ulong H5t;
        private ulong H6t;
        private ulong H7t;
        private ulong H8t;

        public Sha512tDigest(Sha512tDigest t) : base(t)
        {
            this.digestLength = t.digestLength;
            this.Reset(t);
        }

        public Sha512tDigest(int bitLength)
        {
            if (bitLength >= 0x200)
            {
                throw new ArgumentException("cannot be >= 512", "bitLength");
            }
            if ((bitLength % 8) != 0)
            {
                throw new ArgumentException("needs to be a multiple of 8", "bitLength");
            }
            if (bitLength == 0x180)
            {
                throw new ArgumentException("cannot be 384 use SHA384 instead", "bitLength");
            }
            this.digestLength = bitLength / 8;
            this.tIvGenerate(this.digestLength * 8);
            this.Reset();
        }

        public override IMemoable Copy() => 
            new Sha512tDigest(this);

        public override int DoFinal(byte[] output, int outOff)
        {
            base.Finish();
            UInt64_To_BE(base.H1, output, outOff, this.digestLength);
            UInt64_To_BE(base.H2, output, outOff + 8, this.digestLength - 8);
            UInt64_To_BE(base.H3, output, outOff + 0x10, this.digestLength - 0x10);
            UInt64_To_BE(base.H4, output, outOff + 0x18, this.digestLength - 0x18);
            UInt64_To_BE(base.H5, output, outOff + 0x20, this.digestLength - 0x20);
            UInt64_To_BE(base.H6, output, outOff + 40, this.digestLength - 40);
            UInt64_To_BE(base.H7, output, outOff + 0x30, this.digestLength - 0x30);
            UInt64_To_BE(base.H8, output, outOff + 0x38, this.digestLength - 0x38);
            this.Reset();
            return this.digestLength;
        }

        public override int GetDigestSize() => 
            this.digestLength;

        public override void Reset()
        {
            base.Reset();
            base.H1 = this.H1t;
            base.H2 = this.H2t;
            base.H3 = this.H3t;
            base.H4 = this.H4t;
            base.H5 = this.H5t;
            base.H6 = this.H6t;
            base.H7 = this.H7t;
            base.H8 = this.H8t;
        }

        public override void Reset(IMemoable other)
        {
            Sha512tDigest t = (Sha512tDigest) other;
            if (this.digestLength != t.digestLength)
            {
                throw new MemoableResetException("digestLength inappropriate in other");
            }
            base.CopyIn(t);
            this.H1t = t.H1t;
            this.H2t = t.H2t;
            this.H3t = t.H3t;
            this.H4t = t.H4t;
            this.H5t = t.H5t;
            this.H6t = t.H6t;
            this.H7t = t.H7t;
            this.H8t = t.H8t;
        }

        private void tIvGenerate(int bitLength)
        {
            base.H1 = 14_964_410_163_792_538_797L;
            base.H2 = 0x1ec20b20216f029eL;
            base.H3 = 11_082_046_791_023_156_622L;
            base.H4 = 0xea509ffab89354L;
            base.H5 = 17_630_457_682_085_488_500L;
            base.H6 = 0x3ea0cd298e9bc9baL;
            base.H7 = 13_413_544_941_332_994_254L;
            base.H8 = 18_322_165_818_757_711_068L;
            base.Update(0x53);
            base.Update(0x48);
            base.Update(0x41);
            base.Update(0x2d);
            base.Update(0x35);
            base.Update(0x31);
            base.Update(50);
            base.Update(0x2f);
            if (bitLength > 100)
            {
                base.Update((byte) ((bitLength / 100) + 0x30));
                bitLength = bitLength % 100;
                base.Update((byte) ((bitLength / 10) + 0x30));
                bitLength = bitLength % 10;
                base.Update((byte) (bitLength + 0x30));
            }
            else if (bitLength > 10)
            {
                base.Update((byte) ((bitLength / 10) + 0x30));
                bitLength = bitLength % 10;
                base.Update((byte) (bitLength + 0x30));
            }
            else
            {
                base.Update((byte) (bitLength + 0x30));
            }
            base.Finish();
            this.H1t = base.H1;
            this.H2t = base.H2;
            this.H3t = base.H3;
            this.H4t = base.H4;
            this.H5t = base.H5;
            this.H6t = base.H6;
            this.H7t = base.H7;
            this.H8t = base.H8;
        }

        private static void UInt32_To_BE(uint n, byte[] bs, int off, int max)
        {
            int num = Math.Min(4, max);
            while (--num >= 0)
            {
                int num2 = 8 * (3 - num);
                bs[off + num] = (byte) (n >> num2);
            }
        }

        private static void UInt64_To_BE(ulong n, byte[] bs, int off, int max)
        {
            if (max > 0)
            {
                UInt32_To_BE((uint) (n >> 0x20), bs, off, max);
                if (max > 4)
                {
                    UInt32_To_BE((uint) n, bs, off + 4, max - 4);
                }
            }
        }

        public override string AlgorithmName =>
            ("SHA-512/" + (this.digestLength * 8));
    }
}

