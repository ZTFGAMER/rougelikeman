namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class Curve25519Field
    {
        internal static readonly uint[] P = new uint[] { 0xffffffed, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, 0x7fffffff };
        private const uint P7 = 0x7fffffff;
        private static readonly uint[] PExt = new uint[] { 0x169, 0, 0, 0, 0, 0, 0, 0, 0xffffffed, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, 0x3fffffff };
        private const uint PInv = 0x13;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            Nat256.Add(x, y, z);
            if (Nat256.Gte(z, P))
            {
                SubPFrom(z);
            }
        }

        public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
        {
            Nat.Add(0x10, xx, yy, zz);
            if (Nat.Gte(0x10, zz, PExt))
            {
                SubPExtFrom(zz);
            }
        }

        public static void AddOne(uint[] x, uint[] z)
        {
            Nat.Inc(8, x, z);
            if (Nat256.Gte(z, P))
            {
                SubPFrom(z);
            }
        }

        private static uint AddPExtTo(uint[] zz)
        {
            long num = zz[0] + PExt[0];
            zz[0] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num = Nat.IncAt(8, zz, 1);
            }
            num = ((long) ((ulong) num)) + (zz[8] - 0x13L);
            zz[8] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num = Nat.DecAt(15, zz, 9);
            }
            num = (long) (((ulong) num) + (zz[15] + (PExt[15] + 1)));
            zz[15] = (uint) num;
            num = num >> 0x20;
            return (uint) num;
        }

        private static uint AddPTo(uint[] z)
        {
            long num = z[0] - 0x13L;
            z[0] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num = Nat.DecAt(7, z, 1);
            }
            num = (long) (((ulong) num) + (z[7] + 0x80000000L));
            z[7] = (uint) num;
            num = num >> 0x20;
            return (uint) num;
        }

        public static uint[] FromBigInteger(BigInteger x)
        {
            uint[] numArray = Nat256.FromBigInteger(x);
            while (Nat256.Gte(numArray, P))
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
                Nat256.Add(x, P, z);
                Nat.ShiftDownBit(8, z, 0);
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
            Nat256.MulAddTo(x, y, zz);
            if (Nat.Gte(0x10, zz, PExt))
            {
                SubPExtFrom(zz);
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
            uint c = xx[7];
            Nat.ShiftUpBit(8, xx, 8, c, z, 0);
            uint num2 = Nat256.MulByWordAddTo(0x13, xx, z) << 1;
            uint num3 = z[7];
            num2 += (num3 >> 0x1f) - (c >> 0x1f);
            num3 &= 0x7fffffff;
            num3 += Nat.AddWordTo(7, num2 * 0x13, z);
            z[7] = num3;
            if ((num3 >= 0x7fffffff) && Nat256.Gte(z, P))
            {
                SubPFrom(z);
            }
        }

        public static void Reduce27(uint x, uint[] z)
        {
            uint num = z[7];
            uint num2 = (x << 1) | (num >> 0x1f);
            num &= 0x7fffffff;
            num += Nat.AddWordTo(7, num2 * 0x13, z);
            z[7] = num;
            if ((num >= 0x7fffffff) && Nat256.Gte(z, P))
            {
                SubPFrom(z);
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

        private static int SubPExtFrom(uint[] zz)
        {
            long num = zz[0] - PExt[0];
            zz[0] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num = Nat.DecAt(8, zz, 1);
            }
            num = ((long) ((ulong) num)) + (zz[8] + 0x13L);
            zz[8] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num = Nat.IncAt(15, zz, 9);
            }
            num = (long) (((ulong) num) + (zz[15] - (PExt[15] + 1)));
            zz[15] = (uint) num;
            num = num >> 0x20;
            return (int) num;
        }

        private static int SubPFrom(uint[] z)
        {
            long num = z[0] + 0x13L;
            z[0] = (uint) num;
            num = num >> 0x20;
            if (num != 0L)
            {
                num = Nat.IncAt(7, z, 1);
            }
            num = (long) (((ulong) num) + (z[7] - 0x80000000L));
            z[7] = (uint) num;
            num = num >> 0x20;
            return (int) num;
        }

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat256.Sub(x, y, z) != 0)
            {
                AddPTo(z);
            }
        }

        public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if (Nat.Sub(0x10, xx, yy, zz) != 0)
            {
                AddPExtTo(zz);
            }
        }

        public static void Twice(uint[] x, uint[] z)
        {
            Nat.ShiftUpBit(8, x, 0, z);
            if (Nat256.Gte(z, P))
            {
                SubPFrom(z);
            }
        }
    }
}

