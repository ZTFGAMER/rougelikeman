namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP384R1Field
    {
        internal static readonly uint[] P = new uint[] { uint.MaxValue, 0, 0, uint.MaxValue, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        internal static readonly uint[] PExt = new uint[] { 
            1, 0xfffffffe, 0, 2, 0, 0xfffffffe, 0, 2, 1, 0, 0, 0, 0xfffffffe, 1, 0, 0xfffffffe,
            0xfffffffd, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue
        };
        private static readonly uint[] PExtInv = new uint[] { 
            uint.MaxValue, 1, uint.MaxValue, 0xfffffffd, uint.MaxValue, 1, uint.MaxValue, 0xfffffffd, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue, 1, 0xfffffffe, uint.MaxValue, 1,
            2
        };
        private const uint P11 = uint.MaxValue;
        private const uint PExt23 = uint.MaxValue;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            if ((Nat.Add(12, x, y, z) != 0) || ((z[11] == uint.MaxValue) && Nat.Gte(12, z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if (((Nat.Add(0x18, xx, yy, zz) != 0) || ((zz[0x17] == uint.MaxValue) && Nat.Gte(0x18, zz, PExt))) && (Nat.AddTo(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.IncAt(0x18, zz, PExtInv.Length);
            }
        }

        public static void AddOne(uint[] x, uint[] z)
        {
            if ((Nat.Inc(12, x, z) != 0) || ((z[11] == uint.MaxValue) && Nat.Gte(12, z, P)))
            {
                AddPInvTo(z);
            }
        }

        private static void AddPInvTo(uint[] z)
        {
            long num = z[0] + 1L;
            z[0] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[1] - 1L);
            z[1] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num += z[2];
                z[2] = (uint) num;
                num = num >> 0x20;
            }
            num = ((long) ((ulong) num)) + (z[3] + 1L);
            z[3] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[4] + 1L);
            z[4] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                Nat.IncAt(12, z, 5);
            }
        }

        public static uint[] FromBigInteger(BigInteger x)
        {
            uint[] numArray = Nat.FromBigInteger(0x180, x);
            if ((numArray[11] == uint.MaxValue) && Nat.Gte(12, numArray, P))
            {
                Nat.SubFrom(12, P, numArray);
            }
            return numArray;
        }

        public static void Half(uint[] x, uint[] z)
        {
            if ((x[0] & 1) == 0)
            {
                Nat.ShiftDownBit(12, x, 0, z);
            }
            else
            {
                uint c = Nat.Add(12, x, P, z);
                Nat.ShiftDownBit(12, z, c);
            }
        }

        public static void Multiply(uint[] x, uint[] y, uint[] z)
        {
            uint[] zz = Nat.Create(0x18);
            Nat384.Mul(x, y, zz);
            Reduce(zz, z);
        }

        public static void Negate(uint[] x, uint[] z)
        {
            if (Nat.IsZero(12, x))
            {
                Nat.Zero(12, z);
            }
            else
            {
                Nat.Sub(12, P, x, z);
            }
        }

        public static void Reduce(uint[] xx, uint[] z)
        {
            long num = xx[0x10];
            long num2 = xx[0x11];
            long num3 = xx[0x12];
            long num4 = xx[0x13];
            long num5 = xx[20];
            long num6 = xx[0x15];
            long num7 = xx[0x16];
            long num8 = xx[0x17];
            long num9 = (xx[12] + num5) - 1L;
            long num10 = xx[13] + num7;
            long num11 = (xx[14] + num7) + num8;
            long num12 = xx[15] + num8;
            long num13 = num2 + num6;
            long num14 = num6 - num8;
            long num15 = num7 - num8;
            long num16 = num9 + num14;
            long num17 = 0L;
            num17 = ((long) ((ulong) num17)) + (xx[0] + num16);
            z[0] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + (((xx[1] + num8) - ((ulong) num9)) + ((ulong) num10));
            z[1] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + (((xx[2] - num6) - ((ulong) num10)) + ((ulong) num11));
            z[2] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + (((xx[3] - num11) + ((ulong) num12)) + ((ulong) num16));
            z[3] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + (((((xx[4] + num) + ((ulong) num6)) + ((ulong) num10)) - ((ulong) num12)) + ((ulong) num16));
            z[4] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + ((((xx[5] - num) + ((ulong) num10)) + ((ulong) num11)) + ((ulong) num13));
            z[5] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + ((((xx[6] + num3) - ((ulong) num2)) + ((ulong) num11)) + ((ulong) num12));
            z[6] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + ((((xx[7] + num) + ((ulong) num4)) - ((ulong) num3)) + ((ulong) num12));
            z[7] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + ((((xx[8] + num) + ((ulong) num2)) + ((ulong) num5)) - ((ulong) num4));
            z[8] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + (((xx[9] + num3) - ((ulong) num5)) + ((ulong) num13));
            z[9] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + ((((xx[10] + num3) + ((ulong) num4)) - ((ulong) num14)) + ((ulong) num15));
            z[10] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 = ((long) ((ulong) num17)) + (((xx[11] + num4) + ((ulong) num5)) - ((ulong) num15));
            z[11] = (uint) num17;
            num17 = num17 >> 0x20;
            num17 += 1L;
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
                num = ((long) ((ulong) num)) + (z[1] - num2);
                z[1] = (uint) num;
                num = num >> 0x20;
                if (num != 0L)
                {
                    num += z[2];
                    z[2] = (uint) num;
                    num = num >> 0x20;
                }
                num = ((long) ((ulong) num)) + (z[3] + num2);
                z[3] = (uint) num;
                num = num >> 0x20;
                num = ((long) ((ulong) num)) + (z[4] + num2);
                z[4] = (uint) num;
                num = num >> 0x20;
            }
            if (((num != 0L) && (Nat.IncAt(12, z, 5) != 0)) || ((z[11] == uint.MaxValue) && Nat.Gte(12, z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void Square(uint[] x, uint[] z)
        {
            uint[] zz = Nat.Create(0x18);
            Nat384.Square(x, zz);
            Reduce(zz, z);
        }

        public static void SquareN(uint[] x, int n, uint[] z)
        {
            uint[] zz = Nat.Create(0x18);
            Nat384.Square(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                Nat384.Square(z, zz);
                Reduce(zz, z);
            }
        }

        private static void SubPInvFrom(uint[] z)
        {
            long num = z[0] - 1L;
            z[0] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[1] + 1L);
            z[1] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num += z[2];
                z[2] = (uint) num;
                num = num >> 0x20;
            }
            num = ((long) ((ulong) num)) + (z[3] - 1L);
            z[3] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[4] - 1L);
            z[4] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                Nat.DecAt(12, z, 5);
            }
        }

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat.Sub(12, x, y, z) != 0)
            {
                SubPInvFrom(z);
            }
        }

        public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if ((Nat.Sub(0x18, xx, yy, zz) != 0) && (Nat.SubFrom(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.DecAt(0x18, zz, PExtInv.Length);
            }
        }

        public static void Twice(uint[] x, uint[] z)
        {
            if ((Nat.ShiftUpBit(12, x, 0, z) != 0) || ((z[11] == uint.MaxValue) && Nat.Gte(12, z, P)))
            {
                AddPInvTo(z);
            }
        }
    }
}

