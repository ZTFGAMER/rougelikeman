namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP224R1Field
    {
        internal static readonly uint[] P = new uint[] { 1, 0, 0, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        internal static readonly uint[] PExt = new uint[] { 1, 0, 0, 0xfffffffe, uint.MaxValue, uint.MaxValue, 0, 2, 0, 0, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        private static readonly uint[] PExtInv = new uint[] { uint.MaxValue, uint.MaxValue, uint.MaxValue, 1, 0, 0, uint.MaxValue, 0xfffffffd, uint.MaxValue, uint.MaxValue, 1 };
        private const uint P6 = uint.MaxValue;
        private const uint PExt13 = uint.MaxValue;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            if ((Nat224.Add(x, y, z) != 0) || ((z[6] == uint.MaxValue) && Nat224.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if (((Nat.Add(14, xx, yy, zz) != 0) || ((zz[13] == uint.MaxValue) && Nat.Gte(14, zz, PExt))) && (Nat.AddTo(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.IncAt(14, zz, PExtInv.Length);
            }
        }

        public static void AddOne(uint[] x, uint[] z)
        {
            if ((Nat.Inc(7, x, z) != 0) || ((z[6] == uint.MaxValue) && Nat224.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        private static void AddPInvTo(uint[] z)
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
                Nat.IncAt(7, z, 4);
            }
        }

        public static uint[] FromBigInteger(BigInteger x)
        {
            uint[] numArray = Nat224.FromBigInteger(x);
            if ((numArray[6] == uint.MaxValue) && Nat224.Gte(numArray, P))
            {
                Nat224.SubFrom(P, numArray);
            }
            return numArray;
        }

        public static void Half(uint[] x, uint[] z)
        {
            if ((x[0] & 1) == 0)
            {
                Nat.ShiftDownBit(7, x, 0, z);
            }
            else
            {
                uint c = Nat224.Add(x, P, z);
                Nat.ShiftDownBit(7, z, c);
            }
        }

        public static void Multiply(uint[] x, uint[] y, uint[] z)
        {
            uint[] zz = Nat224.CreateExt();
            Nat224.Mul(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
        {
            if (((Nat224.MulAddTo(x, y, zz) != 0) || ((zz[13] == uint.MaxValue) && Nat.Gte(14, zz, PExt))) && (Nat.AddTo(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.IncAt(14, zz, PExtInv.Length);
            }
        }

        public static void Negate(uint[] x, uint[] z)
        {
            if (Nat224.IsZero(x))
            {
                Nat224.Zero(z);
            }
            else
            {
                Nat224.Sub(P, x, z);
            }
        }

        public static void Reduce(uint[] xx, uint[] z)
        {
            long num = xx[10];
            long num2 = xx[11];
            long num3 = xx[12];
            long num4 = xx[13];
            long num5 = (xx[7] + num2) - 1L;
            long num6 = xx[8] + num3;
            long num7 = xx[9] + num4;
            long num8 = 0L;
            num8 = ((long) ((ulong) num8)) + (xx[0] - num5);
            long num9 = (uint) num8;
            num8 = num8 >> 0x20;
            num8 = ((long) ((ulong) num8)) + (xx[1] - num6);
            z[1] = (uint) num8;
            num8 = num8 >> 0x20;
            num8 = ((long) ((ulong) num8)) + (xx[2] - num7);
            z[2] = (uint) num8;
            num8 = num8 >> 0x20;
            num8 = ((long) ((ulong) num8)) + ((xx[3] + num5) - ((ulong) num));
            long num10 = (uint) num8;
            num8 = num8 >> 0x20;
            num8 = ((long) ((ulong) num8)) + ((xx[4] + num6) - ((ulong) num2));
            z[4] = (uint) num8;
            num8 = num8 >> 0x20;
            num8 = ((long) ((ulong) num8)) + ((xx[5] + num7) - ((ulong) num3));
            z[5] = (uint) num8;
            num8 = num8 >> 0x20;
            num8 = ((long) ((ulong) num8)) + ((xx[6] + num) - ((ulong) num4));
            z[6] = (uint) num8;
            num8 = num8 >> 0x20;
            num8 += 1L;
            num10 += num8;
            num9 -= num8;
            z[0] = (uint) num9;
            num8 = num9 >> 0x20;
            if (num8 != 0L)
            {
                num8 += z[1];
                z[1] = (uint) num8;
                num8 = num8 >> 0x20;
                num8 += z[2];
                z[2] = (uint) num8;
                num10 += num8 >> 0x20;
            }
            z[3] = (uint) num10;
            num8 = num10 >> 0x20;
            if (((num8 != 0L) && (Nat.IncAt(7, z, 4) != 0)) || ((z[6] == uint.MaxValue) && Nat224.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void Reduce32(uint x, uint[] z)
        {
            long num = 0L;
            if (x != 0)
            {
                long num2 = x;
                num = ((long) ((ulong) num)) + (z[0] - num2);
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
                num = ((long) ((ulong) num)) + (z[3] + num2);
                z[3] = (uint) num;
                num = num >> 0x20;
            }
            if (((num != 0L) && (Nat.IncAt(7, z, 4) != 0)) || ((z[6] == uint.MaxValue) && Nat224.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void Square(uint[] x, uint[] z)
        {
            uint[] zz = Nat224.CreateExt();
            Nat224.Square(x, zz);
            Reduce(zz, z);
        }

        public static void SquareN(uint[] x, int n, uint[] z)
        {
            uint[] zz = Nat224.CreateExt();
            Nat224.Square(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                Nat224.Square(z, zz);
                Reduce(zz, z);
            }
        }

        private static void SubPInvFrom(uint[] z)
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
                Nat.DecAt(7, z, 4);
            }
        }

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat224.Sub(x, y, z) != 0)
            {
                SubPInvFrom(z);
            }
        }

        public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if ((Nat.Sub(14, xx, yy, zz) != 0) && (Nat.SubFrom(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.DecAt(14, zz, PExtInv.Length);
            }
        }

        public static void Twice(uint[] x, uint[] z)
        {
            if ((Nat.ShiftUpBit(7, x, 0, z) != 0) || ((z[6] == uint.MaxValue) && Nat224.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }
    }
}

