namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP256R1Field
    {
        internal static readonly uint[] P = new uint[] { uint.MaxValue, uint.MaxValue, uint.MaxValue, 0, 0, 0, 1, uint.MaxValue };
        internal static readonly uint[] PExt = new uint[] { 1, 0, 0, 0xfffffffe, uint.MaxValue, uint.MaxValue, 0xfffffffe, 1, 0xfffffffe, 1, 0xfffffffe, 1, 1, 0xfffffffe, 2, 0xfffffffe };
        internal const uint P7 = uint.MaxValue;
        internal const uint PExt15 = 0xfffffffe;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            if ((Nat256.Add(x, y, z) != 0) || ((z[7] == uint.MaxValue) && Nat256.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if ((Nat.Add(0x10, xx, yy, zz) != 0) || ((zz[15] >= 0xfffffffe) && Nat.Gte(0x10, zz, PExt)))
            {
                Nat.SubFrom(0x10, PExt, zz);
            }
        }

        public static void AddOne(uint[] x, uint[] z)
        {
            if ((Nat.Inc(8, x, z) != 0) || ((z[7] == uint.MaxValue) && Nat256.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        private static void AddPInvTo(uint[] z)
        {
            long num = z[0] + 1L;
            z[0] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num += z[1];
                z[1] = (uint) num;
                num = num >> 0x20;
                num += z[2];
                z[2] = (uint) num;
                num = num >> 0x20;
            }
            num = ((long) ((ulong) num)) + (z[3] - 1L);
            z[3] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num += z[4];
                z[4] = (uint) num;
                num = num >> 0x20;
                num += z[5];
                z[5] = (uint) num;
                num = num >> 0x20;
            }
            num = ((long) ((ulong) num)) + (z[6] - 1L);
            z[6] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[7] + 1L);
            z[7] = (uint) num;
        }

        public static uint[] FromBigInteger(BigInteger x)
        {
            uint[] numArray = Nat256.FromBigInteger(x);
            if ((numArray[7] == uint.MaxValue) && Nat256.Gte(numArray, P))
            {
                Nat256.SubFrom(P, numArray);
            }
            return numArray;
        }

        public static void Half(uint[] x, uint[] z)
        {
            if ((x[0] & 1) == 0)
            {
                Nat.ShiftDownBit(8, x, 0, z);
            }
            else
            {
                uint c = Nat256.Add(x, P, z);
                Nat.ShiftDownBit(8, z, c);
            }
        }

        public static void Multiply(uint[] x, uint[] y, uint[] z)
        {
            uint[] zz = Nat256.CreateExt();
            Nat256.Mul(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
        {
            if ((Nat256.MulAddTo(x, y, zz) != 0) || ((zz[15] >= 0xfffffffe) && Nat.Gte(0x10, zz, PExt)))
            {
                Nat.SubFrom(0x10, PExt, zz);
            }
        }

        public static void Negate(uint[] x, uint[] z)
        {
            if (Nat256.IsZero(x))
            {
                Nat256.Zero(z);
            }
            else
            {
                Nat256.Sub(P, x, z);
            }
        }

        public static void Reduce(uint[] xx, uint[] z)
        {
            long num = xx[8];
            long num2 = xx[9];
            long num3 = xx[10];
            long num4 = xx[11];
            long num5 = xx[12];
            long num6 = xx[13];
            long num7 = xx[14];
            long num8 = xx[15];
            num -= 6L;
            long num9 = num + num2;
            long num10 = num2 + num3;
            long num11 = (num3 + num4) - num8;
            long num12 = num4 + num5;
            long num13 = num5 + num6;
            long num14 = num6 + num7;
            long num15 = num7 + num8;
            long num16 = num14 - num9;
            long num17 = 0L;
            num17 = ((long) ((ulong) num17)) + ((xx[0] - num12) - ((ulong) num16));
            z[0] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + (((xx[1] + num10) - ((ulong) num13)) - ((ulong) num15));
            z[1] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + ((xx[2] + num11) - ((ulong) num14));
            z[2] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + (((xx[3] + (num12 << 1)) + ((ulong) num16)) - ((ulong) num15));
            z[3] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + (((xx[4] + (num13 << 1)) + ((ulong) num7)) - ((ulong) num10));
            z[4] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + ((xx[5] + (num14 << 1)) - ((ulong) num11));
            z[5] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + ((xx[6] + (num15 << 1)) + ((ulong) num16));
            z[6] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + ((((xx[7] + (num8 << 1)) + ((ulong) num)) - ((ulong) num11)) - ((ulong) num13));
            z[7] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 += 6L;
            Reduce32((uint) num17, z);
        }

        public static void Reduce32(uint x, uint[] z)
        {
            long num = 0L;
            if (x != 0)
            {
                long num2 = x;
                num = ((long) ((ulong) num)) + (z[0] + num2);
                z[0] = (uint) num;
                num = num >> 0x20;
                if (num != 0L)
                {
                    num += z[1];
                    z[1] = (uint) num;
                    num = num >> 0x20;
                    num += z[2];
                    z[2] = (uint) num;
                    num = num >> 0x20;
                }
                num = ((long) ((ulong) num)) + (z[3] - num2);
                z[3] = (uint) num;
                num = num >> 0x20;
                if (num != 0L)
                {
                    num += z[4];
                    z[4] = (uint) num;
                    num = num >> 0x20;
                    num += z[5];
                    z[5] = (uint) num;
                    num = num >> 0x20;
                }
                num = ((long) ((ulong) num)) + (z[6] - num2);
                z[6] = (uint) num;
                num = num >> 0x20;
                num = ((long) ((ulong) num)) + (z[7] + num2);
                z[7] = (uint) num;
                num = num >> 0x20;
            }
            if ((num != 0L) || ((z[7] == uint.MaxValue) && Nat256.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void Square(uint[] x, uint[] z)
        {
            uint[] zz = Nat256.CreateExt();
            Nat256.Square(x, zz);
            Reduce(zz, z);
        }

        public static void SquareN(uint[] x, int n, uint[] z)
        {
            uint[] zz = Nat256.CreateExt();
            Nat256.Square(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                Nat256.Square(z, zz);
                Reduce(zz, z);
            }
        }

        private static void SubPInvFrom(uint[] z)
        {
            long num = z[0] - 1L;
            z[0] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num += z[1];
                z[1] = (uint) num;
                num = num >> 0x20;
                num += z[2];
                z[2] = (uint) num;
                num = num >> 0x20;
            }
            num = ((long) ((ulong) num)) + (z[3] + 1L);
            z[3] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num += z[4];
                z[4] = (uint) num;
                num = num >> 0x20;
                num += z[5];
                z[5] = (uint) num;
                num = num >> 0x20;
            }
            num = ((long) ((ulong) num)) + (z[6] + 1L);
            z[6] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[7] - 1L);
            z[7] = (uint) num;
        }

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat256.Sub(x, y, z) != 0)
            {
                SubPInvFrom(z);
            }
        }

        public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if (Nat.Sub(0x10, xx, yy, zz) != 0)
            {
                Nat.AddTo(0x10, PExt, zz);
            }
        }

        public static void Twice(uint[] x, uint[] z)
        {
            if ((Nat.ShiftUpBit(8, x, 0, z) != 0) || ((z[7] == uint.MaxValue) && Nat256.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }
    }
}

