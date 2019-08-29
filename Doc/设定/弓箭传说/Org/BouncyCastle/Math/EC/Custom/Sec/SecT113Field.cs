namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecT113Field
    {
        private const ulong M49 = 0x1ffffffffffffL;
        private const ulong M57 = 0x1ffffffffffffffL;

        public static void Add(ulong[] x, ulong[] y, ulong[] z)
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
        }

        public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
        {
            zz[0] = xx[0] ^ yy[0];
            zz[1] = xx[1] ^ yy[1];
            zz[2] = xx[2] ^ yy[2];
            zz[3] = xx[3] ^ yy[3];
        }

        public static void AddOne(ulong[] x, ulong[] z)
        {
            z[0] = x[0] ^ ((ulong) 1L);
            z[1] = x[1];
        }

        public static ulong[] FromBigInteger(BigInteger x)
        {
            ulong[] z = Nat128.FromBigInteger64(x);
            Reduce15(z, 0);
            return z;
        }

        protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong num = x[0];
            ulong num2 = x[1];
            num2 = ((num >> 0x39) ^ (num2 << 7)) & ((ulong) 0x1ffffffffffffffL);
            num &= (ulong) 0x1ffffffffffffffL;
            ulong num3 = y[0];
            ulong num4 = y[1];
            num4 = ((num3 >> 0x39) ^ (num4 << 7)) & ((ulong) 0x1ffffffffffffffL);
            num3 &= (ulong) 0x1ffffffffffffffL;
            ulong[] z = new ulong[6];
            ImplMulw(num, num3, z, 0);
            ImplMulw(num2, num4, z, 2);
            ImplMulw(num ^ num2, num3 ^ num4, z, 4);
            ulong num5 = z[1] ^ z[2];
            ulong num6 = z[0];
            ulong num7 = z[3];
            ulong num8 = (z[4] ^ num6) ^ num5;
            ulong num9 = (z[5] ^ num7) ^ num5;
            zz[0] = num6 ^ (num8 << 0x39);
            zz[1] = (num8 >> 7) ^ (num9 << 50);
            zz[2] = (num9 >> 14) ^ (num7 << 0x2b);
            zz[3] = num7 >> 0x15;
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
            Interleave.Expand64To128(x[0], zz, 0);
            Interleave.Expand64To128(x[1], zz, 2);
        }

        public static void Invert(ulong[] x, ulong[] z)
        {
            if (Nat128.IsZero64(x))
            {
                throw new InvalidOperationException();
            }
            ulong[] numArray = Nat128.Create64();
            ulong[] numArray2 = Nat128.Create64();
            Square(x, numArray);
            Multiply(numArray, x, numArray);
            Square(numArray, numArray);
            Multiply(numArray, x, numArray);
            SquareN(numArray, 3, numArray2);
            Multiply(numArray2, numArray, numArray2);
            Square(numArray2, numArray2);
            Multiply(numArray2, x, numArray2);
            SquareN(numArray2, 7, numArray);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 14, numArray2);
            Multiply(numArray2, numArray, numArray2);
            SquareN(numArray2, 0x1c, numArray);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 0x38, numArray2);
            Multiply(numArray2, numArray, numArray2);
            Square(numArray2, z);
        }

        public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
        {
            ulong[] zz = Nat128.CreateExt64();
            ImplMultiply(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] numArray = Nat128.CreateExt64();
            ImplMultiply(x, y, numArray);
            AddExt(zz, numArray, zz);
        }

        public static void Reduce(ulong[] xx, ulong[] z)
        {
            ulong num = xx[0];
            ulong num2 = xx[1];
            ulong num3 = xx[2];
            ulong num4 = xx[3];
            num2 ^= (num4 << 15) ^ (num4 << 0x18);
            num3 ^= (num4 >> 0x31) ^ (num4 >> 40);
            num ^= (num3 << 15) ^ (num3 << 0x18);
            num2 ^= (num3 >> 0x31) ^ (num3 >> 40);
            ulong num5 = num2 >> 0x31;
            z[0] = (num ^ num5) ^ (num5 << 9);
            z[1] = num2 & ((ulong) 0x1ffffffffffffL);
        }

        public static void Reduce15(ulong[] z, int zOff)
        {
            ulong num = z[zOff + 1];
            ulong num2 = num >> 0x31;
            z[zOff] ^= num2 ^ (num2 << 9);
            z[zOff + 1] = num & ((ulong) 0x1ffffffffffffL);
        }

        public static void Sqrt(ulong[] x, ulong[] z)
        {
            ulong num = Interleave.Unshuffle(x[0]);
            ulong num2 = Interleave.Unshuffle(x[1]);
            ulong num3 = (num & 0xffffffffL) | (num2 << 0x20);
            ulong num4 = (num >> 0x20) | (num2 & 18_446_744_069_414_584_320L);
            z[0] = (num3 ^ (num4 << 0x39)) ^ (num4 << 5);
            z[1] = (num4 >> 7) ^ (num4 >> 0x3b);
        }

        public static void Square(ulong[] x, ulong[] z)
        {
            ulong[] zz = Nat128.CreateExt64();
            ImplSquare(x, zz);
            Reduce(zz, z);
        }

        public static void SquareAddToExt(ulong[] x, ulong[] zz)
        {
            ulong[] numArray = Nat128.CreateExt64();
            ImplSquare(x, numArray);
            AddExt(zz, numArray, zz);
        }

        public static void SquareN(ulong[] x, int n, ulong[] z)
        {
            ulong[] zz = Nat128.CreateExt64();
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

