namespace Org.BouncyCastle.Crypto.Digests
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Utilities;
    using System;

    public abstract class LongDigest : IDigest, IMemoable
    {
        private int MyByteLength;
        private byte[] xBuf;
        private int xBufOff;
        private long byteCount1;
        private long byteCount2;
        internal ulong H1;
        internal ulong H2;
        internal ulong H3;
        internal ulong H4;
        internal ulong H5;
        internal ulong H6;
        internal ulong H7;
        internal ulong H8;
        private ulong[] W;
        private int wOff;
        internal static readonly ulong[] K = new ulong[] { 
            0x428a2f98d728ae22L, 0x7137449123ef65cdL, 13_096_744_586_834_688_815L, 16_840_607_885_511_220_156L, 0x3956c25bf348b538L, 0x59f111f1b605d019L, 10_538_285_296_894_168_987L, 12_329_834_152_419_229_976L, 15_566_598_209_576_043_074L, 0x12835b0145706fbeL, 0x243185be4ee4b28cL, 0x550c7dc3d5ffb4e2L, 0x72be5d74f27b896fL, 9_286_055_187_155_687_089L, 11_230_858_885_718_282_805L, 13_951_009_754_708_518_548L,
            16_472_876_342_353_939_154L, 17_275_323_862_435_702_243L, 0xfc19dc68b8cd5b5L, 0x240ca1cc77ac9c65L, 0x2de92c6f592b0275L, 0x4a7484aa6ea6e483L, 0x5cb0a9dcbd41fbd4L, 0x76f988da831153b5L, 10_970_295_158_949_994_411L, 12_119_686_244_451_234_320L, 12_683_024_718_118_986_047L, 13_788_192_230_050_041_572L, 14_330_467_153_632_333_762L, 15_395_433_587_784_984_357L, 0x6ca6351e003826fL, 0x142929670a0e6e70L,
            0x27b70a8546d22ffcL, 0x2e1b21385c26c926L, 0x4d2c6dfc5ac42aedL, 0x53380d139d95b3dfL, 0x650a73548baf63deL, 0x766a0abb3c77b2a8L, 9_350_256_976_987_008_742L, 10_552_545_826_968_843_579L, 11_727_347_734_174_303_076L, 12_113_106_623_233_404_929L, 14_000_437_183_269_869_457L, 14_369_950_271_660_146_224L, 15_101_387_698_204_529_176L, 15_463_397_548_674_623_760L, 17_586_052_441_742_319_658L, 0x106aa07032bbd1b8L,
            0x19a4c116b8d2d0c8L, 0x1e376c085141ab53L, 0x2748774cdf8eeb99L, 0x34b0bcb5e19b48a8L, 0x391c0cb3c5c95a63L, 0x4ed8aa4ae3418acbL, 0x5b9cca4f7763e373L, 0x682e6ff3d6b2b8a3L, 0x748f82ee5defb2fcL, 0x78a5636f43172f60L, 9_568_029_438_360_202_098L, 10_144_078_919_501_101_548L, 10_430_055_236_837_252_648L, 11_840_083_180_663_258_601L, 13_761_210_420_658_862_357L, 14_299_343_276_471_374_635L,
            14_566_680_578_165_727_644L, 15_097_957_966_210_449_927L, 16_922_976_911_328_602_910L, 17_689_382_322_260_857_208L, 0x6f067aa72176fbaL, 0xa637dc5a2c898a6L, 0x113f9804bef90daeL, 0x1b710b35131c471bL, 0x28db77f523047d84L, 0x32caab7b40c72493L, 0x3c9ebe0a15c9bebcL, 0x431d67c49c100d4cL, 0x4cc5d4becb3e42b6L, 0x597f299cfc657e2aL, 0x5fcb6fab3ad6faecL, 0x6c44198c4a475817L
        };

        internal LongDigest()
        {
            this.MyByteLength = 0x80;
            this.W = new ulong[80];
            this.xBuf = new byte[8];
            this.Reset();
        }

        internal LongDigest(LongDigest t)
        {
            this.MyByteLength = 0x80;
            this.W = new ulong[80];
            this.xBuf = new byte[t.xBuf.Length];
            this.CopyIn(t);
        }

        private void AdjustByteCounts()
        {
            if (this.byteCount1 > 0x1fffffffffffffffL)
            {
                this.byteCount2 += this.byteCount1 >> 0x3d;
                this.byteCount1 &= 0x1fffffffffffffffL;
            }
        }

        public void BlockUpdate(byte[] input, int inOff, int length)
        {
            while ((this.xBufOff != 0) && (length > 0))
            {
                this.Update(input[inOff]);
                inOff++;
                length--;
            }
            while (length > this.xBuf.Length)
            {
                this.ProcessWord(input, inOff);
                inOff += this.xBuf.Length;
                length -= this.xBuf.Length;
                this.byteCount1 += this.xBuf.Length;
            }
            while (length > 0)
            {
                this.Update(input[inOff]);
                inOff++;
                length--;
            }
        }

        private static ulong Ch(ulong x, ulong y, ulong z) => 
            ((x & y) ^ (~x & z));

        public abstract IMemoable Copy();
        protected void CopyIn(LongDigest t)
        {
            Array.Copy(t.xBuf, 0, this.xBuf, 0, t.xBuf.Length);
            this.xBufOff = t.xBufOff;
            this.byteCount1 = t.byteCount1;
            this.byteCount2 = t.byteCount2;
            this.H1 = t.H1;
            this.H2 = t.H2;
            this.H3 = t.H3;
            this.H4 = t.H4;
            this.H5 = t.H5;
            this.H6 = t.H6;
            this.H7 = t.H7;
            this.H8 = t.H8;
            Array.Copy(t.W, 0, this.W, 0, t.W.Length);
            this.wOff = t.wOff;
        }

        public abstract int DoFinal(byte[] output, int outOff);
        public void Finish()
        {
            this.AdjustByteCounts();
            long lowW = this.byteCount1 << 3;
            long hiW = this.byteCount2;
            this.Update(0x80);
            while (this.xBufOff != 0)
            {
                this.Update(0);
            }
            this.ProcessLength(lowW, hiW);
            this.ProcessBlock();
        }

        public int GetByteLength() => 
            this.MyByteLength;

        public abstract int GetDigestSize();
        private static ulong Maj(ulong x, ulong y, ulong z) => 
            (((x & y) ^ (x & z)) ^ (y & z));

        internal void ProcessBlock()
        {
            this.AdjustByteCounts();
            for (int i = 0x10; i <= 0x4f; i++)
            {
                this.W[i] = ((Sigma1(this.W[i - 2]) + this.W[i - 7]) + Sigma0(this.W[i - 15])) + this.W[i - 0x10];
            }
            ulong x = this.H1;
            ulong y = this.H2;
            ulong z = this.H3;
            ulong num5 = this.H4;
            ulong num6 = this.H5;
            ulong num7 = this.H6;
            ulong num8 = this.H7;
            ulong num9 = this.H8;
            int index = 0;
            for (int j = 0; j < 10; j++)
            {
                num9 += ((Sum1(num6) + Ch(num6, num7, num8)) + K[index]) + this.W[index++];
                num5 += num9;
                num9 += Sum0(x) + Maj(x, y, z);
                num8 += ((Sum1(num5) + Ch(num5, num6, num7)) + K[index]) + this.W[index++];
                z += num8;
                num8 += Sum0(num9) + Maj(num9, x, y);
                num7 += ((Sum1(z) + Ch(z, num5, num6)) + K[index]) + this.W[index++];
                y += num7;
                num7 += Sum0(num8) + Maj(num8, num9, x);
                num6 += ((Sum1(y) + Ch(y, z, num5)) + K[index]) + this.W[index++];
                x += num6;
                num6 += Sum0(num7) + Maj(num7, num8, num9);
                num5 += ((Sum1(x) + Ch(x, y, z)) + K[index]) + this.W[index++];
                num9 += num5;
                num5 += Sum0(num6) + Maj(num6, num7, num8);
                z += ((Sum1(num9) + Ch(num9, x, y)) + K[index]) + this.W[index++];
                num8 += z;
                z += Sum0(num5) + Maj(num5, num6, num7);
                y += ((Sum1(num8) + Ch(num8, num9, x)) + K[index]) + this.W[index++];
                num7 += y;
                y += Sum0(z) + Maj(z, num5, num6);
                x += ((Sum1(num7) + Ch(num7, num8, num9)) + K[index]) + this.W[index++];
                num6 += x;
                x += Sum0(y) + Maj(y, z, num5);
            }
            this.H1 += x;
            this.H2 += y;
            this.H3 += z;
            this.H4 += num5;
            this.H5 += num6;
            this.H6 += num7;
            this.H7 += num8;
            this.H8 += num9;
            this.wOff = 0;
            Array.Clear(this.W, 0, 0x10);
        }

        internal void ProcessLength(long lowW, long hiW)
        {
            if (this.wOff > 14)
            {
                this.ProcessBlock();
            }
            this.W[14] = (ulong) hiW;
            this.W[15] = (ulong) lowW;
        }

        internal void ProcessWord(byte[] input, int inOff)
        {
            this.W[this.wOff] = Pack.BE_To_UInt64(input, inOff);
            if (++this.wOff == 0x10)
            {
                this.ProcessBlock();
            }
        }

        public virtual void Reset()
        {
            this.byteCount1 = 0L;
            this.byteCount2 = 0L;
            this.xBufOff = 0;
            for (int i = 0; i < this.xBuf.Length; i++)
            {
                this.xBuf[i] = 0;
            }
            this.wOff = 0;
            Array.Clear(this.W, 0, this.W.Length);
        }

        public abstract void Reset(IMemoable t);
        private static ulong Sigma0(ulong x) => 
            ((((x << 0x3f) | (x >> 1)) ^ ((x << 0x38) | (x >> 8))) ^ (x >> 7));

        private static ulong Sigma1(ulong x) => 
            ((((x << 0x2d) | (x >> 0x13)) ^ ((x << 3) | (x >> 0x3d))) ^ (x >> 6));

        private static ulong Sum0(ulong x) => 
            ((((x << 0x24) | (x >> 0x1c)) ^ ((x << 30) | (x >> 0x22))) ^ ((x << 0x19) | (x >> 0x27)));

        private static ulong Sum1(ulong x) => 
            ((((x << 50) | (x >> 14)) ^ ((x << 0x2e) | (x >> 0x12))) ^ ((x << 0x17) | (x >> 0x29)));

        public void Update(byte input)
        {
            this.xBuf[this.xBufOff++] = input;
            if (this.xBufOff == this.xBuf.Length)
            {
                this.ProcessWord(this.xBuf, 0);
                this.xBufOff = 0;
            }
            this.byteCount1 += 1L;
        }

        public abstract string AlgorithmName { get; }
    }
}

