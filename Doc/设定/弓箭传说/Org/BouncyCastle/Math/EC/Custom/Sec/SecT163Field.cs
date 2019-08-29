namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecT163Field
    {
        private const ulong M35 = 0x7ffffffffL;
        private const ulong M55 = 0x7fffffffffffffL;
        private static readonly ulong[] ROOT_Z = new ulong[] { 13_176_245_766_935_393_968L, 0x492492492492db6dL, 0x492492492L };

        public static void Add(ulong[] x, ulong[] y, ulong[] z)
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
            z[2] = x[2] ^ y[2];
        }

        public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
        {
            zz[0] = xx[0] ^ yy[0];
            zz[1] = xx[1] ^ yy[1];
            zz[2] = xx[2] ^ yy[2];
            zz[3] = xx[3] ^ yy[3];
            zz[4] = xx[4] ^ yy[4];
            zz[5] = xx[5] ^ yy[5];
        }

        public static void AddOne(ulong[] x, ulong[] z)
        {
            z[0] = x[0] ^ ((ulong) 1L);
            z[1] = x[1];
            z[2] = x[2];
        }

        public static ulong[] FromBigInteger(BigInteger x)
        {
            ulong[] z = Nat192.FromBigInteger64(x);
            Reduce29(z, 0);
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
            zz[0] = num ^ (num2 << 0x37);
            zz[1] = (num2 >> 9) ^ (num3 << 0x2e);
            zz[2] = (num3 >> 0x12) ^ (num4 << 0x25);
            zz[3] = (num4 >> 0x1b) ^ (num5 << 0x1c);
            zz[4] = (num5 >> 0x24) ^ (num6 << 0x13);
            zz[5] = num6 >> 0x2d;
        }

        protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong num = x[0];
            ulong num2 = x[1];
            ulong num3 = x[2];
            num3 = (num2 >> 0x2e) ^ (num3 << 0x12);
            num2 = ((num >> 0x37) ^ (num2 << 9)) & ((ulong) 0x7fffffffffffffL);
            num &= (ulong) 0x7fffffffffffffL;
            ulong num4 = y[0];
            ulong num5 = y[1];
            ulong num6 = y[2];
            num6 = (num5 >> 0x2e) ^ (num6 << 0x12);
            num5 = ((num4 >> 0x37) ^ (num5 << 9)) & ((ulong) 0x7fffffffffffffL);
            num4 &= (ulong) 0x7fffffffffffffL;
            ulong[] z = new ulong[10];
            ImplMulw(num, num4, z, 0);
            ImplMulw(num3, num6, z, 2);
            ulong num7 = (num ^ num2) ^ num3;
            ulong num8 = (num4 ^ num5) ^ num6;
            ImplMulw(num7, num8, z, 4);
            ulong num9 = (num2 << 1) ^ (num3 << 2);
            ulong num10 = (num5 << 1) ^ (num6 << 2);
            ImplMulw(num ^ num9, num4 ^ num10, z, 6);
            ImplMulw(num7 ^ num9, num8 ^ num10, z, 8);
            ulong num11 = z[6] ^ z[8];
            ulong num12 = z[7] ^ z[9];
            ulong num13 = (num11 << 1) ^ z[6];
            ulong num14 = (num11 ^ (num12 << 1)) ^ z[7];
            ulong num15 = num12;
            ulong num16 = z[0];
            ulong num17 = (z[1] ^ z[0]) ^ z[4];
            ulong num18 = z[1] ^ z[5];
            ulong num19 = ((num16 ^ num13) ^ (z[2] << 4)) ^ (z[2] << 1);
            ulong num20 = ((num17 ^ num14) ^ (z[3] << 4)) ^ (z[3] << 1);
            ulong num21 = num18 ^ num15;
            num20 ^= num19 >> 0x37;
            num19 &= (ulong) 0x7fffffffffffffL;
            num21 ^= num20 >> 0x37;
            num20 &= (ulong) 0x7fffffffffffffL;
            num19 = (num19 >> 1) ^ ((num20 & 1L) << 0x36);
            num20 = (num20 >> 1) ^ ((num21 & 1L) << 0x36);
            num21 = num21 >> 1;
            num19 ^= num19 << 1;
            num19 ^= num19 << 2;
            num19 ^= num19 << 4;
            num19 ^= num19 << 8;
            num19 ^= num19 << 0x10;
            num19 ^= num19 << 0x20;
            num19 &= (ulong) 0x7fffffffffffffL;
            num20 ^= num19 >> 0x36;
            num20 ^= num20 << 1;
            num20 ^= num20 << 2;
            num20 ^= num20 << 4;
            num20 ^= num20 << 8;
            num20 ^= num20 << 0x10;
            num20 ^= num20 << 0x20;
            num20 &= (ulong) 0x7fffffffffffffL;
            num21 ^= num20 >> 0x36;
            num21 ^= num21 << 1;
            num21 ^= num21 << 2;
            num21 ^= num21 << 4;
            num21 ^= num21 << 8;
            num21 ^= num21 << 0x10;
            num21 ^= num21 << 0x20;
            zz[0] = num16;
            zz[1] = (num17 ^ num19) ^ z[2];
            zz[2] = ((num18 ^ num20) ^ num19) ^ z[3];
            zz[3] = num21 ^ num20;
            zz[4] = num21 ^ z[2];
            zz[5] = z[3];
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
            ulong num4 = numArray[(int) ((IntPtr) (num & 3))];
            int num5 = 0x2f;
            do
            {
                num = (uint) (x >> num5);
                ulong num2 = (numArray[(int) ((IntPtr) (num & 7))] ^ (numArray[(int) ((IntPtr) ((num >> 3) & 7))] << 3)) ^ (numArray[(int) ((IntPtr) ((num >> 6) & 7))] << 6);
                num4 ^= num2 << num5;
                num3 ^= num2 >> -num5;
            }
            while ((num5 -= 9) > 0);
            z[zOff] = num4 & ((ulong) 0x7fffffffffffffL);
            z[zOff + 1] = (num4 >> 0x37) ^ (num3 << 9);
        }

        protected static void ImplSquare(ulong[] x, ulong[] zz)
        {
            Interleave.Expand64To128(x[0], zz, 0);
            Interleave.Expand64To128(x[1], zz, 2);
            ulong num = x[2];
            zz[4] = Interleave.Expand32to64((uint) num);
            zz[5] = Interleave.Expand8to16((uint) (num >> 0x20));
        }

        public static void Invert(ulong[] x, ulong[] z)
        {
            if (Nat192.IsZero64(x))
            {
                throw new InvalidOperationException();
            }
            ulong[] numArray = Nat192.Create64();
            ulong[] numArray2 = Nat192.Create64();
            Square(x, numArray);
            SquareN(numArray, 1, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray2, 1, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 3, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray2, 3, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 9, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray2, 9, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 0x1b, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray2, 0x1b, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 0x51, numArray2);
            Multiply(numArray, numArray2, z);
        }

        public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
        {
            ulong[] zz = Nat192.CreateExt64();
            ImplMultiply(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] numArray = Nat192.CreateExt64();
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
            num3 ^= (((num6 << 0x1d) ^ (num6 << 0x20)) ^ (num6 << 0x23)) ^ (num6 << 0x24);
            num4 ^= (((num6 >> 0x23) ^ (num6 >> 0x20)) ^ (num6 >> 0x1d)) ^ (num6 >> 0x1c);
            num2 ^= (((num5 << 0x1d) ^ (num5 << 0x20)) ^ (num5 << 0x23)) ^ (num5 << 0x24);
            num3 ^= (((num5 >> 0x23) ^ (num5 >> 0x20)) ^ (num5 >> 0x1d)) ^ (num5 >> 0x1c);
            num ^= (((num4 << 0x1d) ^ (num4 << 0x20)) ^ (num4 << 0x23)) ^ (num4 << 0x24);
            num2 ^= (((num4 >> 0x23) ^ (num4 >> 0x20)) ^ (num4 >> 0x1d)) ^ (num4 >> 0x1c);
            ulong num7 = num3 >> 0x23;
            z[0] = (((num ^ num7) ^ (num7 << 3)) ^ (num7 << 6)) ^ (num7 << 7);
            z[1] = num2;
            z[2] = num3 & ((ulong) 0x7ffffffffL);
        }

        public static void Reduce29(ulong[] z, int zOff)
        {
            ulong num = z[zOff + 2];
            ulong num2 = num >> 0x23;
            z[zOff] ^= ((num2 ^ (num2 << 3)) ^ (num2 << 6)) ^ (num2 << 7);
            z[zOff + 2] = num & ((ulong) 0x7ffffffffL);
        }

        public static void Sqrt(ulong[] x, ulong[] z)
        {
            ulong[] numArray = Nat192.Create64();
            ulong num = Interleave.Unshuffle(x[0]);
            ulong num2 = Interleave.Unshuffle(x[1]);
            ulong num3 = (num & 0xffffffffL) | (num2 << 0x20);
            numArray[0] = (num >> 0x20) | (num2 & 18_446_744_069_414_584_320L);
            num = Interleave.Unshuffle(x[2]);
            ulong num4 = num & 0xffffffffL;
            numArray[1] = num >> 0x20;
            Multiply(numArray, ROOT_Z, z);
            z[0] ^= num3;
            z[1] ^= num4;
        }

        public static void Square(ulong[] x, ulong[] z)
        {
            ulong[] zz = Nat192.CreateExt64();
            ImplSquare(x, zz);
            Reduce(zz, z);
        }

        public static void SquareAddToExt(ulong[] x, ulong[] zz)
        {
            ulong[] numArray = Nat192.CreateExt64();
            ImplSquare(x, numArray);
            AddExt(zz, numArray, zz);
        }

        public static void SquareN(ulong[] x, int n, ulong[] z)
        {
            ulong[] zz = Nat192.CreateExt64();
            ImplSquare(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                ImplSquare(z, zz);
                Reduce(zz, z);
            }
        }

        public static uint Trace(ulong[] x) => 
            (((uint) (x[0] ^ (x[2] >> 0x1d))) & 1);
    }
}

