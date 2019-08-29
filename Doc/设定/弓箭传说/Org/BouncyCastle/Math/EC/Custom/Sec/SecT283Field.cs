namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecT283Field
    {
        private const ulong M27 = 0x7ffffffL;
        private const ulong M57 = 0x1ffffffffffffffL;
        private static readonly ulong[] ROOT_Z = new ulong[] { 0xc30c30c30c30808L, 0x30c30c30c30c30c3L, 9_369_774_767_598_502_668L, 0x820820820820820L, 0x2082082L };

        public static void Add(ulong[] x, ulong[] y, ulong[] z)
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
            z[2] = x[2] ^ y[2];
            z[3] = x[3] ^ y[3];
            z[4] = x[4] ^ y[4];
        }

        public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
        {
            zz[0] = xx[0] ^ yy[0];
            zz[1] = xx[1] ^ yy[1];
            zz[2] = xx[2] ^ yy[2];
            zz[3] = xx[3] ^ yy[3];
            zz[4] = xx[4] ^ yy[4];
            zz[5] = xx[5] ^ yy[5];
            zz[6] = xx[6] ^ yy[6];
            zz[7] = xx[7] ^ yy[7];
            zz[8] = xx[8] ^ yy[8];
        }

        public static void AddOne(ulong[] x, ulong[] z)
        {
            z[0] = x[0] ^ ((ulong) 1L);
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
            z[4] = x[4];
        }

        public static ulong[] FromBigInteger(BigInteger x)
        {
            ulong[] z = Nat320.FromBigInteger64(x);
            Reduce37(z, 0);
            return z;
        }

        protected static void ImplCompactExt(ulong[] zz)
        {
            ulong num = zz[0];
            ulong num2 = zz[1];
            ulong num3 = zz[2];
            ulong num4 = zz[3];
            ulong num5 = zz[4];
            ulong num6 = zz[5];
            ulong num7 = zz[6];
            ulong num8 = zz[7];
            ulong num9 = zz[8];
            ulong num10 = zz[9];
            zz[0] = num ^ (num2 << 0x39);
            zz[1] = (num2 >> 7) ^ (num3 << 50);
            zz[2] = (num3 >> 14) ^ (num4 << 0x2b);
            zz[3] = (num4 >> 0x15) ^ (num5 << 0x24);
            zz[4] = (num5 >> 0x1c) ^ (num6 << 0x1d);
            zz[5] = (num6 >> 0x23) ^ (num7 << 0x16);
            zz[6] = (num7 >> 0x2a) ^ (num8 << 15);
            zz[7] = (num8 >> 0x31) ^ (num9 << 8);
            zz[8] = (num9 >> 0x38) ^ (num10 << 1);
            zz[9] = num10 >> 0x3f;
        }

        protected static void ImplExpand(ulong[] x, ulong[] z)
        {
            ulong num = x[0];
            ulong num2 = x[1];
            ulong num3 = x[2];
            ulong num4 = x[3];
            ulong num5 = x[4];
            z[0] = num & ((ulong) 0x1ffffffffffffffL);
            z[1] = ((num >> 0x39) ^ (num2 << 7)) & ((ulong) 0x1ffffffffffffffL);
            z[2] = ((num2 >> 50) ^ (num3 << 14)) & ((ulong) 0x1ffffffffffffffL);
            z[3] = ((num3 >> 0x2b) ^ (num4 << 0x15)) & ((ulong) 0x1ffffffffffffffL);
            z[4] = (num4 >> 0x24) ^ (num5 << 0x1c);
        }

        protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] z = new ulong[5];
            ulong[] numArray2 = new ulong[5];
            ImplExpand(x, z);
            ImplExpand(y, numArray2);
            ulong[] numArray3 = new ulong[0x1a];
            ImplMulw(z[0], numArray2[0], numArray3, 0);
            ImplMulw(z[1], numArray2[1], numArray3, 2);
            ImplMulw(z[2], numArray2[2], numArray3, 4);
            ImplMulw(z[3], numArray2[3], numArray3, 6);
            ImplMulw(z[4], numArray2[4], numArray3, 8);
            ulong num = z[0] ^ z[1];
            ulong num2 = numArray2[0] ^ numArray2[1];
            ulong num3 = z[0] ^ z[2];
            ulong num4 = numArray2[0] ^ numArray2[2];
            ulong num5 = z[2] ^ z[4];
            ulong num6 = numArray2[2] ^ numArray2[4];
            ulong num7 = z[3] ^ z[4];
            ulong num8 = numArray2[3] ^ numArray2[4];
            ImplMulw(num3 ^ z[3], num4 ^ numArray2[3], numArray3, 0x12);
            ImplMulw(num5 ^ z[1], num6 ^ numArray2[1], numArray3, 20);
            ulong num9 = num ^ num7;
            ulong num10 = num2 ^ num8;
            ulong num11 = num9 ^ z[2];
            ulong num12 = num10 ^ numArray2[2];
            ImplMulw(num9, num10, numArray3, 0x16);
            ImplMulw(num11, num12, numArray3, 0x18);
            ImplMulw(num, num2, numArray3, 10);
            ImplMulw(num3, num4, numArray3, 12);
            ImplMulw(num5, num6, numArray3, 14);
            ImplMulw(num7, num8, numArray3, 0x10);
            zz[0] = numArray3[0];
            zz[9] = numArray3[9];
            ulong num13 = numArray3[0] ^ numArray3[1];
            ulong num14 = num13 ^ numArray3[2];
            ulong num15 = num14 ^ numArray3[10];
            zz[1] = num15;
            ulong num16 = numArray3[3] ^ numArray3[4];
            ulong num17 = numArray3[11] ^ numArray3[12];
            ulong num18 = num16 ^ num17;
            ulong num19 = num14 ^ num18;
            zz[2] = num19;
            ulong num20 = num13 ^ num16;
            ulong num21 = numArray3[5] ^ numArray3[6];
            ulong num22 = num20 ^ num21;
            ulong num23 = num22 ^ numArray3[8];
            ulong num24 = numArray3[13] ^ numArray3[14];
            ulong num25 = num23 ^ num24;
            ulong num26 = numArray3[0x12] ^ numArray3[0x16];
            ulong num27 = num26 ^ numArray3[0x18];
            ulong num28 = num25 ^ num27;
            zz[3] = num28;
            ulong num29 = numArray3[7] ^ numArray3[8];
            ulong num30 = num29 ^ numArray3[9];
            ulong num31 = num30 ^ numArray3[0x11];
            zz[8] = num31;
            ulong num32 = num30 ^ num21;
            ulong num33 = numArray3[15] ^ numArray3[0x10];
            ulong num34 = num32 ^ num33;
            zz[7] = num34;
            ulong num35 = num34 ^ num15;
            ulong num36 = numArray3[0x13] ^ numArray3[20];
            ulong num37 = numArray3[0x19] ^ numArray3[0x18];
            ulong num38 = numArray3[0x12] ^ numArray3[0x17];
            ulong num39 = num36 ^ num37;
            ulong num40 = num39 ^ num38;
            ulong num41 = num40 ^ num35;
            zz[4] = num41;
            ulong num42 = num19 ^ num31;
            ulong num43 = num39 ^ num42;
            ulong num44 = numArray3[0x15] ^ numArray3[0x16];
            ulong num45 = num43 ^ num44;
            zz[5] = num45;
            ulong num46 = num23 ^ numArray3[0];
            ulong num47 = num46 ^ numArray3[9];
            ulong num48 = num47 ^ num24;
            ulong num49 = num48 ^ numArray3[0x15];
            ulong num50 = num49 ^ numArray3[0x17];
            ulong num51 = num50 ^ numArray3[0x19];
            zz[6] = num51;
            ImplCompactExt(zz);
        }

        protected static void ImplMulw(ulong x, ulong y, ulong[] z, int zOff)
        {
            ulong[] numArray = new ulong[8];
            numArray[1] = y;
            numArray[2] = numArray[1] << 1;
            numArray[3] = numArray[2] ^ y;
            numArray[4] = numArray[2] << 1;
            numArray[5] = numArray[4] ^ y;
            numArray[6] = numArray[3] << 1;
            numArray[7] = numArray[6] ^ y;
            uint num = (uint) x;
            ulong num3 = 0L;
            ulong num4 = numArray[(int) ((IntPtr) (num & 7))];
            int num5 = 0x30;
            do
            {
                num = (uint) (x >> num5);
                ulong num2 = (numArray[(int) ((IntPtr) (num & 7))] ^ (numArray[(int) ((IntPtr) ((num >> 3) & 7))] << 3)) ^ (numArray[(int) ((IntPtr) ((num >> 6) & 7))] << 6);
                num4 ^= num2 << num5;
                num3 ^= num2 >> -num5;
            }
            while ((num5 -= 9) > 0);
            num3 ^= ((x & 0x100804020100800L) & ((y << 7) >> 0x3f)) >> 8;
            z[zOff] = num4 & ((ulong) 0x1ffffffffffffffL);
            z[zOff + 1] = (num4 >> 0x39) ^ (num3 << 7);
        }

        protected static void ImplSquare(ulong[] x, ulong[] zz)
        {
            for (int i = 0; i < 4; i++)
            {
                Interleave.Expand64To128(x[i], zz, i << 1);
            }
            zz[8] = Interleave.Expand32to64((uint) x[4]);
        }

        public static void Invert(ulong[] x, ulong[] z)
        {
            if (Nat320.IsZero64(x))
            {
                throw new InvalidOperationException();
            }
            ulong[] numArray = Nat320.Create64();
            ulong[] numArray2 = Nat320.Create64();
            Square(x, numArray);
            Multiply(numArray, x, numArray);
            SquareN(numArray, 2, numArray2);
            Multiply(numArray2, numArray, numArray2);
            SquareN(numArray2, 4, numArray);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 8, numArray2);
            Multiply(numArray2, numArray, numArray2);
            Square(numArray2, numArray2);
            Multiply(numArray2, x, numArray2);
            SquareN(numArray2, 0x11, numArray);
            Multiply(numArray, numArray2, numArray);
            Square(numArray, numArray);
            Multiply(numArray, x, numArray);
            SquareN(numArray, 0x23, numArray2);
            Multiply(numArray2, numArray, numArray2);
            SquareN(numArray2, 70, numArray);
            Multiply(numArray, numArray2, numArray);
            Square(numArray, numArray);
            Multiply(numArray, x, numArray);
            SquareN(numArray, 0x8d, numArray2);
            Multiply(numArray2, numArray, numArray2);
            Square(numArray2, z);
        }

        public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
        {
            ulong[] zz = Nat320.CreateExt64();
            ImplMultiply(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] numArray = Nat320.CreateExt64();
            ImplMultiply(x, y, numArray);
            AddExt(zz, numArray, zz);
        }

        public static void Reduce(ulong[] xx, ulong[] z)
        {
            ulong num = xx[0];
            ulong num2 = xx[1];
            ulong num3 = xx[2];
            ulong num4 = xx[3];
            ulong num5 = xx[4];
            ulong num6 = xx[5];
            ulong num7 = xx[6];
            ulong num8 = xx[7];
            ulong num9 = xx[8];
            num4 ^= (((num9 << 0x25) ^ (num9 << 0x2a)) ^ (num9 << 0x2c)) ^ (num9 << 0x31);
            num5 ^= (((num9 >> 0x1b) ^ (num9 >> 0x16)) ^ (num9 >> 20)) ^ (num9 >> 15);
            num3 ^= (((num8 << 0x25) ^ (num8 << 0x2a)) ^ (num8 << 0x2c)) ^ (num8 << 0x31);
            num4 ^= (((num8 >> 0x1b) ^ (num8 >> 0x16)) ^ (num8 >> 20)) ^ (num8 >> 15);
            num2 ^= (((num7 << 0x25) ^ (num7 << 0x2a)) ^ (num7 << 0x2c)) ^ (num7 << 0x31);
            num3 ^= (((num7 >> 0x1b) ^ (num7 >> 0x16)) ^ (num7 >> 20)) ^ (num7 >> 15);
            num ^= (((num6 << 0x25) ^ (num6 << 0x2a)) ^ (num6 << 0x2c)) ^ (num6 << 0x31);
            num2 ^= (((num6 >> 0x1b) ^ (num6 >> 0x16)) ^ (num6 >> 20)) ^ (num6 >> 15);
            ulong num10 = num5 >> 0x1b;
            z[0] = (((num ^ num10) ^ (num10 << 5)) ^ (num10 << 7)) ^ (num10 << 12);
            z[1] = num2;
            z[2] = num3;
            z[3] = num4;
            z[4] = num5 & ((ulong) 0x7ffffffL);
        }

        public static void Reduce37(ulong[] z, int zOff)
        {
            ulong num = z[zOff + 4];
            ulong num2 = num >> 0x1b;
            z[zOff] ^= ((num2 ^ (num2 << 5)) ^ (num2 << 7)) ^ (num2 << 12);
            z[zOff + 4] = num & ((ulong) 0x7ffffffL);
        }

        public static void Sqrt(ulong[] x, ulong[] z)
        {
            ulong[] numArray = Nat320.Create64();
            ulong num = Interleave.Unshuffle(x[0]);
            ulong num2 = Interleave.Unshuffle(x[1]);
            ulong num3 = (num & 0xffffffffL) | (num2 << 0x20);
            numArray[0] = (num >> 0x20) | (num2 & 18_446_744_069_414_584_320L);
            num = Interleave.Unshuffle(x[2]);
            num2 = Interleave.Unshuffle(x[3]);
            ulong num4 = (num & 0xffffffffL) | (num2 << 0x20);
            numArray[1] = (num >> 0x20) | (num2 & 18_446_744_069_414_584_320L);
            num = Interleave.Unshuffle(x[4]);
            ulong num5 = num & 0xffffffffL;
            numArray[2] = num >> 0x20;
            Multiply(numArray, ROOT_Z, z);
            z[0] ^= num3;
            z[1] ^= num4;
            z[2] ^= num5;
        }

        public static void Square(ulong[] x, ulong[] z)
        {
            ulong[] zz = Nat.Create64(9);
            ImplSquare(x, zz);
            Reduce(zz, z);
        }

        public static void SquareAddToExt(ulong[] x, ulong[] zz)
        {
            ulong[] numArray = Nat.Create64(9);
            ImplSquare(x, numArray);
            AddExt(zz, numArray, zz);
        }

        public static void SquareN(ulong[] x, int n, ulong[] z)
        {
            ulong[] zz = Nat.Create64(9);
            ImplSquare(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                ImplSquare(z, zz);
                Reduce(zz, z);
            }
        }

        public static uint Trace(ulong[] x) => 
            (((uint) (x[0] ^ (x[4] >> 15))) & 1);
    }
}

