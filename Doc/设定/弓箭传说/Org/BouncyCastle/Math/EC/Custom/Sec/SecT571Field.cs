namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecT571Field
    {
        private const ulong M59 = 0x7ffffffffffffffL;
        private const ulong RM = 17_256_631_552_825_064_414L;
        private static readonly ulong[] ROOT_Z = new ulong[] { 0x2be1195f08cafb99L, 10_804_290_191_530_228_771L, 14_625_517_132_619_890_193L, 0x657c232be1195f08L, 17_890_083_061_325_672_324L, 0x7c232be1195f08caL, 13_695_892_802_195_391_589L, 0x5f08caf84657c232L, 0x784657c232be119L };

        public static void Add(ulong[] x, ulong[] y, ulong[] z)
        {
            for (int i = 0; i < 9; i++)
            {
                z[i] = x[i] ^ y[i];
            }
        }

        private static void Add(ulong[] x, int xOff, ulong[] y, int yOff, ulong[] z, int zOff)
        {
            for (int i = 0; i < 9; i++)
            {
                z[zOff + i] = x[xOff + i] ^ y[yOff + i];
            }
        }

        private static void AddBothTo(ulong[] x, int xOff, ulong[] y, int yOff, ulong[] z, int zOff)
        {
            for (int i = 0; i < 9; i++)
            {
                z[zOff + i] ^= x[xOff + i] ^ y[yOff + i];
            }
        }

        public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
        {
            for (int i = 0; i < 0x12; i++)
            {
                zz[i] = xx[i] ^ yy[i];
            }
        }

        public static void AddOne(ulong[] x, ulong[] z)
        {
            z[0] = x[0] ^ ((ulong) 1L);
            for (int i = 1; i < 9; i++)
            {
                z[i] = x[i];
            }
        }

        public static ulong[] FromBigInteger(BigInteger x)
        {
            ulong[] z = Nat576.FromBigInteger64(x);
            Reduce5(z, 0);
            return z;
        }

        protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] destinationArray = new ulong[0x90];
            Array.Copy(y, 0, destinationArray, 9, 9);
            int zOff = 0;
            for (int i = 7; i > 0; i--)
            {
                zOff += 0x12;
                Nat.ShiftUpBit64(9, destinationArray, zOff >> 1, 0L, destinationArray, zOff);
                Reduce5(destinationArray, zOff);
                Add(destinationArray, 9, destinationArray, zOff, destinationArray, zOff + 9);
            }
            ulong[] z = new ulong[destinationArray.Length];
            Nat.ShiftUpBits64(destinationArray.Length, destinationArray, 0, 4, 0L, z, 0);
            uint num3 = 15;
            for (int j = 0x38; j >= 0; j -= 8)
            {
                for (int m = 1; m < 9; m += 2)
                {
                    uint num6 = (uint) (x[m] >> j);
                    uint num7 = num6 & num3;
                    uint num8 = (num6 >> 4) & num3;
                    AddBothTo(destinationArray, (int) (9 * num7), z, (int) (9 * num8), zz, m - 1);
                }
                Nat.ShiftUpBits64(0x10, zz, 0, 8, 0L);
            }
            for (int k = 0x38; k >= 0; k -= 8)
            {
                for (int m = 0; m < 9; m += 2)
                {
                    uint num11 = (uint) (x[m] >> k);
                    uint num12 = num11 & num3;
                    uint num13 = (num11 >> 4) & num3;
                    AddBothTo(destinationArray, (int) (9 * num12), z, (int) (9 * num13), zz, m);
                }
                if (k > 0)
                {
                    Nat.ShiftUpBits64(0x12, zz, 0, 8, 0L);
                }
            }
        }

        protected static void ImplMulwAcc(ulong[] xs, ulong y, ulong[] z, int zOff)
        {
            ulong[] numArray = new ulong[0x20];
            numArray[1] = y;
            for (int i = 2; i < 0x20; i += 2)
            {
                numArray[i] = numArray[i >> 1] << 1;
                numArray[i + 1] = numArray[i] ^ y;
            }
            ulong num2 = 0L;
            for (int j = 0; j < 9; j++)
            {
                ulong num4 = xs[j];
                uint num5 = (uint) num4;
                num2 ^= numArray[(int) ((IntPtr) (num5 & 0x1f))];
                ulong num7 = 0L;
                int num8 = 60;
                do
                {
                    num5 = (uint) (num4 >> num8);
                    ulong num6 = numArray[(int) ((IntPtr) (num5 & 0x1f))];
                    num2 ^= num6 << num8;
                    num7 ^= num6 >> -num8;
                }
                while ((num8 -= 5) > 0);
                for (int k = 0; k < 4; k++)
                {
                    num4 = (num4 & -1190112520884487202L) >> 1;
                    num7 ^= num4 & ((y << (k & 0x3f)) >> 0x3f);
                }
                z[zOff + j] ^= num2;
                num2 = num7;
            }
            z[zOff + 9] ^= num2;
        }

        protected static void ImplSquare(ulong[] x, ulong[] zz)
        {
            for (int i = 0; i < 9; i++)
            {
                Interleave.Expand64To128(x[i], zz, i << 1);
            }
        }

        public static void Invert(ulong[] x, ulong[] z)
        {
            if (Nat576.IsZero64(x))
            {
                throw new InvalidOperationException();
            }
            ulong[] numArray = Nat576.Create64();
            ulong[] numArray2 = Nat576.Create64();
            ulong[] numArray3 = Nat576.Create64();
            Square(x, numArray3);
            Square(numArray3, numArray);
            Square(numArray, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 2, numArray2);
            Multiply(numArray, numArray2, numArray);
            Multiply(numArray, numArray3, numArray);
            SquareN(numArray, 5, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray2, 5, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 15, numArray2);
            Multiply(numArray, numArray2, numArray3);
            SquareN(numArray3, 30, numArray);
            SquareN(numArray, 30, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 60, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray2, 60, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray, 180, numArray2);
            Multiply(numArray, numArray2, numArray);
            SquareN(numArray2, 180, numArray2);
            Multiply(numArray, numArray2, numArray);
            Multiply(numArray, numArray3, z);
        }

        public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
        {
            ulong[] zz = Nat576.CreateExt64();
            ImplMultiply(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
        {
            ulong[] numArray = Nat576.CreateExt64();
            ImplMultiply(x, y, numArray);
            AddExt(zz, numArray, zz);
        }

        public static void Reduce(ulong[] xx, ulong[] z)
        {
            ulong num = xx[9];
            ulong num2 = xx[0x11];
            ulong num3 = num;
            num = (((num3 ^ (num2 >> 0x3b)) ^ (num2 >> 0x39)) ^ (num2 >> 0x36)) ^ (num2 >> 0x31);
            num3 = (((xx[8] ^ (num2 << 5)) ^ (num2 << 7)) ^ (num2 << 10)) ^ (num2 << 15);
            for (int i = 0x10; i >= 10; i--)
            {
                num2 = xx[i];
                z[i - 8] = (((num3 ^ (num2 >> 0x3b)) ^ (num2 >> 0x39)) ^ (num2 >> 0x36)) ^ (num2 >> 0x31);
                num3 = (((xx[i - 9] ^ (num2 << 5)) ^ (num2 << 7)) ^ (num2 << 10)) ^ (num2 << 15);
            }
            num2 = num;
            z[1] = (((num3 ^ (num2 >> 0x3b)) ^ (num2 >> 0x39)) ^ (num2 >> 0x36)) ^ (num2 >> 0x31);
            num3 = (((xx[0] ^ (num2 << 5)) ^ (num2 << 7)) ^ (num2 << 10)) ^ (num2 << 15);
            ulong num5 = z[8];
            ulong num6 = num5 >> 0x3b;
            z[0] = (((num3 ^ num6) ^ (num6 << 2)) ^ (num6 << 5)) ^ (num6 << 10);
            z[8] = num5 & ((ulong) 0x7ffffffffffffffL);
        }

        public static void Reduce5(ulong[] z, int zOff)
        {
            ulong num = z[zOff + 8];
            ulong num2 = num >> 0x3b;
            z[zOff] ^= ((num2 ^ (num2 << 2)) ^ (num2 << 5)) ^ (num2 << 10);
            z[zOff + 8] = num & ((ulong) 0x7ffffffffffffffL);
        }

        public static void Sqrt(ulong[] x, ulong[] z)
        {
            ulong[] y = Nat576.Create64();
            ulong[] numArray2 = Nat576.Create64();
            int index = 0;
            for (int i = 0; i < 4; i++)
            {
                ulong num3 = Interleave.Unshuffle(x[index++]);
                ulong num4 = Interleave.Unshuffle(x[index++]);
                y[i] = (num3 & 0xffffffffL) | (num4 << 0x20);
                numArray2[i] = (num3 >> 0x20) | (num4 & 18_446_744_069_414_584_320L);
            }
            ulong num5 = Interleave.Unshuffle(x[index]);
            y[4] = num5 & 0xffffffffL;
            numArray2[4] = num5 >> 0x20;
            Multiply(numArray2, ROOT_Z, z);
            Add(z, y, z);
        }

        public static void Square(ulong[] x, ulong[] z)
        {
            ulong[] zz = Nat576.CreateExt64();
            ImplSquare(x, zz);
            Reduce(zz, z);
        }

        public static void SquareAddToExt(ulong[] x, ulong[] zz)
        {
            ulong[] numArray = Nat576.CreateExt64();
            ImplSquare(x, numArray);
            AddExt(zz, numArray, zz);
        }

        public static void SquareN(ulong[] x, int n, ulong[] z)
        {
            ulong[] zz = Nat576.CreateExt64();
            ImplSquare(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                ImplSquare(z, zz);
                Reduce(zz, z);
            }
        }

        public static uint Trace(ulong[] x) => 
            (((uint) ((x[0] ^ (x[8] >> 0x31)) ^ (x[8] >> 0x39))) & 1);
    }
}

