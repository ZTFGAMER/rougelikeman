namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP192R1Field
    {
        internal static readonly uint[] P = new uint[] { uint.MaxValue, uint.MaxValue, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        internal static readonly uint[] PExt = new uint[] { 1, 0, 2, 0, 1, 0, 0xfffffffe, uint.MaxValue, 0xfffffffd, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        private static readonly uint[] PExtInv = new uint[] { uint.MaxValue, uint.MaxValue, 0xfffffffd, uint.MaxValue, 0xfffffffe, uint.MaxValue, 1, 0, 2 };
        private const uint P5 = uint.MaxValue;
        private const uint PExt11 = uint.MaxValue;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            if ((Nat192.Add(x, y, z) != 0) || ((z[5] == uint.MaxValue) && Nat192.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if (((Nat.Add(12, xx, yy, zz) != 0) || ((zz[11] == uint.MaxValue) && Nat.Gte(12, zz, PExt))) && (Nat.AddTo(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.IncAt(12, zz, PExtInv.Length);
            }
        }

        public static void AddOne(uint[] x, uint[] z)
        {
            if ((Nat.Inc(6, x, z) != 0) || ((z[5] == uint.MaxValue) && Nat192.Gte(z, P)))
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
            }
            num = ((long) ((ulong) num)) + (z[2] + 1L);
            z[2] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                Nat.IncAt(6, z, 3);
            }
        }

        public static uint[] FromBigInteger(BigInteger x)
        {
            uint[] numArray = Nat192.FromBigInteger(x);
            if ((numArray[5] == uint.MaxValue) && Nat192.Gte(numArray, P))
            {
                Nat192.SubFrom(P, numArray);
            }
            return numArray;
        }

        public static void Half(uint[] x, uint[] z)
        {
            if ((x[0] & 1) == 0)
            {
                Nat.ShiftDownBit(6, x, 0, z);
            }
            else
            {
                uint c = Nat192.Add(x, P, z);
                Nat.ShiftDownBit(6, z, c);
            }
        }

        public static void Multiply(uint[] x, uint[] y, uint[] z)
        {
            uint[] zz = Nat192.CreateExt();
            Nat192.Mul(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
        {
            if (((Nat192.MulAddTo(x, y, zz) != 0) || ((zz[11] == uint.MaxValue) && Nat.Gte(12, zz, PExt))) && (Nat.AddTo(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.IncAt(12, zz, PExtInv.Length);
            }
        }

        public static void Negate(uint[] x, uint[] z)
        {
            if (Nat192.IsZero(x))
            {
                Nat192.Zero(z);
            }
            else
            {
                Nat192.Sub(P, x, z);
            }
        }

        public static void Reduce(uint[] xx, uint[] z)
        {
            ulong num = xx[6];
            ulong num2 = xx[7];
            ulong num3 = xx[8];
            ulong num4 = xx[9];
            ulong num5 = xx[10];
            ulong num6 = xx[11];
            ulong num7 = num + num5;
            ulong num8 = num2 + num6;
            ulong num9 = 0L;
            num9 += xx[0] + num7;
            uint num10 = (uint) num9;
            num9 = num9 >> 0x20;
            num9 += xx[1] + num8;
            z[1] = (uint) num9;
            num9 = num9 >> 0x20;
            num7 += num3;
            num8 += num4;
            num9 += xx[2] + num7;
            ulong num11 = (uint) num9;
            num9 = num9 >> 0x20;
            num9 += xx[3] + num8;
            z[3] = (uint) num9;
            num9 = num9 >> 0x20;
            num7 -= num;
            num8 -= num2;
            num9 += xx[4] + num7;
            z[4] = (uint) num9;
            num9 = num9 >> 0x20;
            num9 += xx[5] + num8;
            z[5] = (uint) num9;
            num9 = num9 >> 0x20;
            num11 += num9;
            num9 += num10;
            z[0] = (uint) num9;
            num9 = num9 >> 0x20;
            if (num9 != 0L)
            {
                num9 += z[1];
                z[1] = (uint) num9;
                num11 += num9 >> 0x20;
            }
            z[2] = (uint) num11;
            num9 = num11 >> 0x20;
            if (((num9 != 0L) && (Nat.IncAt(6, z, 3) != 0)) || ((z[5] == uint.MaxValue) && Nat192.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void Reduce32(uint x, uint[] z)
        {
            ulong num = 0L;
            if (x != 0)
            {
                num += z[0] + x;
                z[0] = (uint) num;
                num = num >> 0x20;
                if (num != 0L)
                {
                    num += z[1];
                    z[1] = (uint) num;
                    num = num >> 0x20;
                }
                num += z[2] + x;
                z[2] = (uint) num;
                num = num >> 0x20;
            }
            if (((num != 0L) && (Nat.IncAt(6, z, 3) != 0)) || ((z[5] == uint.MaxValue) && Nat192.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void Square(uint[] x, uint[] z)
        {
            uint[] zz = Nat192.CreateExt();
            Nat192.Square(x, zz);
            Reduce(zz, z);
        }

        public static void SquareN(uint[] x, int n, uint[] z)
        {
            uint[] zz = Nat192.CreateExt();
            Nat192.Square(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                Nat192.Square(z, zz);
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
            }
            num = ((long) ((ulong) num)) + (z[2] - 1L);
            z[2] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                Nat.DecAt(6, z, 3);
            }
        }

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat192.Sub(x, y, z) != 0)
            {
                SubPInvFrom(z);
            }
        }

        public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if ((Nat.Sub(12, xx, yy, zz) != 0) && (Nat.SubFrom(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.DecAt(12, zz, PExtInv.Length);
            }
        }

        public static void Twice(uint[] x, uint[] z)
        {
            if ((Nat.ShiftUpBit(6, x, 0, z) != 0) || ((z[5] == uint.MaxValue) && Nat192.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }
    }
}

