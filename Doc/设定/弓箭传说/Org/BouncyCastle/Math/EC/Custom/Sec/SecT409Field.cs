namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecT409Field
    {
        private const ulong M25 = 0x1ffffffL;
        private const ulong M59 = 0x7ffffffffffffffL;

        public static void Add(ulong[] x, ulong[] y, ulong[] z)
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
            z[2] = x[2] ^ y[2];
            z[3] = x[3] ^ y[3];
            z[4] = x[4] ^ y[4];
            z[5] = x[5] ^ y[5];
            z[6] = x[6] ^ y[6];
        }

        public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
        {
            for (int i = 0; i < 13; i++)
            {
                zz[i] = xx[i] ^ yy[i];
            }
        }

        public static void AddOne(ulong[] x, ulong[] z)
        {
            z[0] = x[0] ^ ((ulong) 1L);
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
            z[4] = x[4];
            z[5] = x[5];
            z[6] = x[6];
        }

        public static ulong[] FromBigInteger(BigInteger x)
        {
            ulong[] z = Nat448.FromBigInteger64(x);
            Reduce39(z, 0);
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
            ulong num11 = zz[10];
            ulong num12 = zz[11];
            ulong num13 = zz[12];
            ulong num14 = zz[13];
            zz[0] = num ^ (num2 << 0x3b);
            zz[1] = (num2 >> 5) ^ (num3 << 0x36);
            zz[2] = (num3 >> 10) ^ (num4 << 0x31);
            zz[3] = (num4 >> 15) ^ (num5 << 0x2c);
            zz[4] = (num5 >> 20) ^ (num6 << 0x27);
            zz[5] = (num6 >> 0x19) ^ (num7 << 0x22);
            zz[6] = (num7 >> 30) ^ (num8 << 0x1d);
            zz[7] = (num8 >> 0x23) ^ (num9 << 0x18);
            zz[8] = (num9 >> 40) ^ (num10 << 0x13);
            zz[9] = (num10 >> 0x2d) ^ (num11 << 14);
            zz[10] = (num11 >> 50) ^ (num12 << 9);
            zz[11] = ((num12 >> 0x37) ^ (num13 << 4)) ^ (num14 << 0x3f);
            zz[12] = (num13 >> 60) ^ (num14 >> 1);
            zz[13] = 0L;
        }

        protected static void ImplExpand(ulong[] x, ulong[] z)
        {
            ulong num = x[0];
            ulong num2 = x[1];
            ulong num3 = x[2];
            ulong num4 = x[3];
            ulong num5 = x[4];
            ulong num6 = x[5];
            ulong num7 = x[6];
            z[0] = num & ((ulong) 0x7ffffffffffffffL);
            z[1] = ((num >> 0x3b) ^ (num2 << 5)) & ((ulong) 0x7ffffffffffffffL);
            z[2] = ((num2 >> 0x36) ^ (num3 << 10)) & ((ulong) 0x7ffffffffffffffL);
            z[3] = ((num3 >> 0x31) ^ (num4 << 15)) & ((ulong) 0x7ffffffffffffffL);
            z[4] = ((num4 >> 0x2c) ^ (num5 << 20)) & ((ulong) 0x7ffffffffffffffL);
            z[5] = ((num5 >> 0x27) ^ (num6 << 0x19)) & ((ulong) 0x7ffffffffffffffL);
            z[6] = (num6 >> 0x22) ^ (num7 << 30);
        }

        protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] z = new ulong[7];
            ulong[] numArray2 = new ulong[7];
            ImplExpand(x, z);
            ImplExpand(y, numArray2);
            for (int i = 0; i < 7; i++)
            {
                ImplMulwAcc(z, numArray2[i], zz, i);
            }
            ImplCompactExt(zz);
        }

        protected static void ImplMulwAcc(ulong[] xs, ulong y, ulong[] z, int zOff)
        {
            ulong[] numArray = new ulong[8];
            numArray[1] = y;
            numArray[2] = numArray[1] << 1;
            numArray[3] = numArray[2] ^ y;
            numArray[4] = numArray[2] << 1;
            numArray[5] = numArray[4] ^ y;
            numArray[6] = numArray[3] << 1;
            numArray[7] = numArray[6] ^ y;
            for (int i = 0; i < 7; i++)
            {
                ulong num2 = xs[i];
                uint num3 = (uint) num2;
                ulong num5 = 0L;
                ulong num6 = numArray[(int) ((IntPtr) (num3 & 7))] ^ (numArray[(int) ((IntPtr) ((num3 >> 3) & 7))] << 3);
                int num7 = 0x36;
                do
                {
                    num3 = (uint) (num2 >> num7);
                    ulong num4 = numArray[(int) ((IntPtr) (num3 & 7))] ^ (numArray[(int) ((IntPtr) ((num3 >> 3) & 7))] << 3);
                    num6 ^= num4 << num7;
                    num5 ^= num4 >> -num7;
                }
                while ((num7 -= 6) > 0);
                z[zOff + i] ^= num6 & ((ulong) 0x7ffffffffffffffL);
                z[(zOff + i) + 1] ^= (num6 >> 0x3b) ^ (num5 << 5);
            }
        }

        protected static void ImplSquare(ulong[] x, ulong[] zz)
        {
            for (int i = 0; i < 6; i++)
            {
                Interleave.Expand64To128(x[i], zz, i << 1);
            }
            zz[12] = Interleave.Expand32to64((uint) x[6]);
        }

        public static void Invert(ulong[] x, ulong[] z)
        {
            if (Nat448.IsZero64(x))
            {
                throw new InvalidOperationException();
            }
            ulong[] numArray = Nat448.Create64();
            ulong[] numArray2 = Nat448.Create64();
            ulong[] numArray3 = Nat448.Create64();
            Square(x, numArray);
            SquareN(numArray, 1, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray2, 1, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 3, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 6, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 12, numArray2);
            Multiply(numArray, numArray2, numArray3);
            SquareN(numArray3, 0x18, numArray);
            SquareN(numArray, 0x18, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 0x30, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 0x60, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 0xc0, numArray2);
            Multiply(numArray, numArray2, numArray);
            Multiply(numArray, numArray3, z);
        }

        public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
        {
            ulong[] zz = Nat448.CreateExt64();
            ImplMultiply(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] numArray = Nat448.CreateExt64();
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
            ulong num9 = xx[12];
            num6 ^= num9 << 0x27;
            num7 ^= (num9 >> 0x19) ^ (num9 << 0x3e);
            num8 ^= num9 >> 2;
            num9 = xx[11];
            num5 ^= num9 << 0x27;
            num6 ^= (num9 >> 0x19) ^ (num9 << 0x3e);
            num7 ^= num9 >> 2;
            num9 = xx[10];
            num4 ^= num9 << 0x27;
            num5 ^= (num9 >> 0x19) ^ (num9 << 0x3e);
            num6 ^= num9 >> 2;
            num9 = xx[9];
            num3 ^= num9 << 0x27;
            num4 ^= (num9 >> 0x19) ^ (num9 << 0x3e);
            num5 ^= num9 >> 2;
            num9 = xx[8];
            num2 ^= num9 << 0x27;
            num3 ^= (num9 >> 0x19) ^ (num9 << 0x3e);
            num4 ^= num9 >> 2;
            num9 = num8;
            num ^= num9 << 0x27;
            num2 ^= (num9 >> 0x19) ^ (num9 << 0x3e);
            num3 ^= num9 >> 2;
            ulong num10 = num7 >> 0x19;
            z[0] = num ^ num10;
            z[1] = num2 ^ (num10 << 0x17);
            z[2] = num3;
            z[3] = num4;
            z[4] = num5;
            z[5] = num6;
            z[6] = num7 & ((ulong) 0x1ffffffL);
        }

        public static void Reduce39(ulong[] z, int zOff)
        {
            ulong num = z[zOff + 6];
            ulong num2 = num >> 0x19;
            z[zOff] ^= num2;
            z[zOff + 1] ^= num2 << 0x17;
            z[zOff + 6] = num & ((ulong) 0x1ffffffL);
        }

        public static void Sqrt(ulong[] x, ulong[] z)
        {
            ulong num = Interleave.Unshuffle(x[0]);
            ulong num2 = Interleave.Unshuffle(x[1]);
            ulong num3 = (num & 0xffffffffL) | (num2 << 0x20);
            ulong num4 = (num >> 0x20) | (num2 & 18_446_744_069_414_584_320L);
            num = Interleave.Unshuffle(x[2]);
            num2 = Interleave.Unshuffle(x[3]);
            ulong num5 = (num & 0xffffffffL) | (num2 << 0x20);
            ulong num6 = (num >> 0x20) | (num2 & 18_446_744_069_414_584_320L);
            num = Interleave.Unshuffle(x[4]);
            num2 = Interleave.Unshuffle(x[5]);
            ulong num7 = (num & 0xffffffffL) | (num2 << 0x20);
            ulong num8 = (num >> 0x20) | (num2 & 18_446_744_069_414_584_320L);
            num = Interleave.Unshuffle(x[6]);
            ulong num9 = num & 0xffffffffL;
            ulong num10 = num >> 0x20;
            z[0] = num3 ^ (num4 << 0x2c);
            z[1] = (num5 ^ (num6 << 0x2c)) ^ (num4 >> 20);
            z[2] = (num7 ^ (num8 << 0x2c)) ^ (num6 >> 20);
            z[3] = ((num9 ^ (num10 << 0x2c)) ^ (num8 >> 20)) ^ (num4 << 13);
            z[4] = ((num10 >> 20) ^ (num6 << 13)) ^ (num4 >> 0x33);
            z[5] = (num8 << 13) ^ (num6 >> 0x33);
            z[6] = (num10 << 13) ^ (num8 >> 0x33);
        }

        public static void Square(ulong[] x, ulong[] z)
        {
            ulong[] zz = Nat.Create64(13);
            ImplSquare(x, zz);
            Reduce(zz, z);
        }

        public static void SquareAddToExt(ulong[] x, ulong[] zz)
        {
            ulong[] numArray = Nat.Create64(13);
            ImplSquare(x, numArray);
            AddExt(zz, numArray, zz);
        }

        public static void SquareN(ulong[] x, int n, ulong[] z)
        {
            ulong[] zz = Nat.Create64(13);
            ImplSquare(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                ImplSquare(z, zz);
                Reduce(zz, z);
            }
        }

        public static uint Trace(ulong[] x) => 
            (((uint) x[0]) & 1);
    }
}

