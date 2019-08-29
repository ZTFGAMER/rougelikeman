namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecT239Field
    {
        private const ulong M47 = 0x7fffffffffffL;
        private const ulong M60 = 0xfffffffffffffffL;

        public static void Add(ulong[] x, ulong[] y, ulong[] z)
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
            z[2] = x[2] ^ y[2];
            z[3] = x[3] ^ y[3];
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
        }

        public static void AddOne(ulong[] x, ulong[] z)
        {
            z[0] = x[0] ^ ((ulong) 1L);
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
        }

        public static ulong[] FromBigInteger(BigInteger x)
        {
            ulong[] z = Nat256.FromBigInteger64(x);
            Reduce17(z, 0);
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
            zz[0] = num ^ (num2 << 60);
            zz[1] = (num2 >> 4) ^ (num3 << 0x38);
            zz[2] = (num3 >> 8) ^ (num4 << 0x34);
            zz[3] = (num4 >> 12) ^ (num5 << 0x30);
            zz[4] = (num5 >> 0x10) ^ (num6 << 0x2c);
            zz[5] = (num6 >> 20) ^ (num7 << 40);
            zz[6] = (num7 >> 0x18) ^ (num8 << 0x24);
            zz[7] = num8 >> 0x1c;
        }

        protected static void ImplExpand(ulong[] x, ulong[] z)
        {
            ulong num = x[0];
            ulong num2 = x[1];
            ulong num3 = x[2];
            ulong num4 = x[3];
            z[0] = num & ((ulong) 0xfffffffffffffffL);
            z[1] = ((num >> 60) ^ (num2 << 4)) & ((ulong) 0xfffffffffffffffL);
            z[2] = ((num2 >> 0x38) ^ (num3 << 8)) & ((ulong) 0xfffffffffffffffL);
            z[3] = (num3 >> 0x34) ^ (num4 << 12);
        }

        protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] z = new ulong[4];
            ulong[] numArray2 = new ulong[4];
            ImplExpand(x, z);
            ImplExpand(y, numArray2);
            ImplMulwAcc(z[0], numArray2[0], zz, 0);
            ImplMulwAcc(z[1], numArray2[1], zz, 1);
            ImplMulwAcc(z[2], numArray2[2], zz, 2);
            ImplMulwAcc(z[3], numArray2[3], zz, 3);
            for (int i = 5; i > 0; i--)
            {
                zz[i] ^= zz[i - 1];
            }
            ImplMulwAcc(z[0] ^ z[1], numArray2[0] ^ numArray2[1], zz, 1);
            ImplMulwAcc(z[2] ^ z[3], numArray2[2] ^ numArray2[3], zz, 3);
            for (int j = 7; j > 1; j--)
            {
                zz[j] ^= zz[j - 2];
            }
            ulong num3 = z[0] ^ z[2];
            ulong num4 = z[1] ^ z[3];
            ulong num5 = numArray2[0] ^ numArray2[2];
            ulong num6 = numArray2[1] ^ numArray2[3];
            ImplMulwAcc(num3 ^ num4, num5 ^ num6, zz, 3);
            ulong[] numArray3 = new ulong[3];
            ImplMulwAcc(num3, num5, numArray3, 0);
            ImplMulwAcc(num4, num6, numArray3, 1);
            ulong num7 = numArray3[0];
            ulong num8 = numArray3[1];
            ulong num9 = numArray3[2];
            zz[2] ^= num7;
            zz[3] ^= num7 ^ num8;
            zz[4] ^= num9 ^ num8;
            zz[5] ^= num9;
            ImplCompactExt(zz);
        }

        protected static void ImplMulwAcc(ulong x, ulong y, ulong[] z, int zOff)
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
            ulong num4 = numArray[(int) ((IntPtr) (num & 7))] ^ (numArray[(int) ((IntPtr) ((num >> 3) & 7))] << 3);
            int num5 = 0x36;
            do
            {
                num = (uint) (x >> num5);
                ulong num2 = numArray[(int) ((IntPtr) (num & 7))] ^ (numArray[(int) ((IntPtr) ((num >> 3) & 7))] << 3);
                num4 ^= num2 << num5;
                num3 ^= num2 >> -num5;
            }
            while ((num5 -= 6) > 0);
            num3 ^= ((x & 0x820820820820820L) & ((y << 4) >> 0x3f)) >> 5;
            z[zOff] ^= num4 & ((ulong) 0xfffffffffffffffL);
            z[zOff + 1] ^= (num4 >> 60) ^ (num3 << 4);
        }

        protected static void ImplSquare(ulong[] x, ulong[] zz)
        {
            Interleave.Expand64To128(x[0], zz, 0);
            Interleave.Expand64To128(x[1], zz, 2);
            Interleave.Expand64To128(x[2], zz, 4);
            ulong num = x[3];
            zz[6] = Interleave.Expand32to64((uint) num);
            zz[7] = Interleave.Expand16to32((uint) (num >> 0x20));
        }

        public static void Invert(ulong[] x, ulong[] z)
        {
            if (Nat256.IsZero64(x))
            {
                throw new InvalidOperationException();
            }
            ulong[] numArray = Nat256.Create64();
            ulong[] numArray2 = Nat256.Create64();
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
            Square(numArray2, numArray2);
            Multiply(numArray2, x, numArray2);
            SquareN(numArray2, 0x1d, numArray);
            Multiply(numArray, numArray2, numArray);
            Square(numArray, numArray);
            Multiply(numArray, x, numArray);
            SquareN(numArray, 0x3b, numArray2);
            Multiply(numArray2, numArray, numArray2);
            Square(numArray2, numArray2);
            Multiply(numArray2, x, numArray2);
            SquareN(numArray2, 0x77, numArray);
            Multiply(numArray, numArray2, numArray);
            Square(numArray, z);
        }

        public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
        {
            ulong[] zz = Nat256.CreateExt64();
            ImplMultiply(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] numArray = Nat256.CreateExt64();
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
            num4 ^= num8 << 0x11;
            num5 ^= num8 >> 0x2f;
            num6 ^= num8 << 0x2f;
            num7 ^= num8 >> 0x11;
            num3 ^= num7 << 0x11;
            num4 ^= num7 >> 0x2f;
            num5 ^= num7 << 0x2f;
            num6 ^= num7 >> 0x11;
            num2 ^= num6 << 0x11;
            num3 ^= num6 >> 0x2f;
            num4 ^= num6 << 0x2f;
            num5 ^= num6 >> 0x11;
            num ^= num5 << 0x11;
            num2 ^= num5 >> 0x2f;
            num3 ^= num5 << 0x2f;
            num4 ^= num5 >> 0x11;
            ulong num9 = num4 >> 0x2f;
            z[0] = num ^ num9;
            z[1] = num2;
            z[2] = num3 ^ (num9 << 30);
            z[3] = num4 & ((ulong) 0x7fffffffffffL);
        }

        public static void Reduce17(ulong[] z, int zOff)
        {
            ulong num = z[zOff + 3];
            ulong num2 = num >> 0x2f;
            z[zOff] ^= num2;
            z[zOff + 2] ^= num2 << 30;
            z[zOff + 3] = num & ((ulong) 0x7fffffffffffL);
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
            ulong num8 = num6 >> 0x31;
            ulong num7 = (num4 >> 0x31) | (num6 << 15);
            num6 ^= num4 << 15;
            ulong[] xx = Nat256.CreateExt64();
            int[] numArray2 = new int[] { 0x27, 120 };
            for (int i = 0; i < numArray2.Length; i++)
            {
                int index = numArray2[i] >> 6;
                int num11 = numArray2[i] & 0x3f;
                xx[index] ^= num4 << num11;
                xx[index + 1] ^= (num6 << num11) | (num4 >> -num11);
                xx[index + 2] ^= (num7 << num11) | (num6 >> -num11);
                xx[index + 3] ^= (num8 << num11) | (num7 >> -num11);
                xx[index + 4] ^= num8 >> -num11;
            }
            Reduce(xx, z);
            z[0] ^= num3;
            z[1] ^= num5;
        }

        public static void Square(ulong[] x, ulong[] z)
        {
            ulong[] zz = Nat256.CreateExt64();
            ImplSquare(x, zz);
            Reduce(zz, z);
        }

        public static void SquareAddToExt(ulong[] x, ulong[] zz)
        {
            ulong[] numArray = Nat256.CreateExt64();
            ImplSquare(x, numArray);
            AddExt(zz, numArray, zz);
        }

        public static void SquareN(ulong[] x, int n, ulong[] z)
        {
            ulong[] zz = Nat256.CreateExt64();
            ImplSquare(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                ImplSquare(z, zz);
                Reduce(zz, z);
            }
        }

        public static uint Trace(ulong[] x) => 
            (((uint) ((x[0] ^ (x[1] >> 0x11)) ^ (x[2] >> 0x22))) & 1);
    }
}

