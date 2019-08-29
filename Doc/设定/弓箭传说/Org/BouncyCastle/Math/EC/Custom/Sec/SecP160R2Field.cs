namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP160R2Field
    {
        internal static readonly uint[] P = new uint[] { 0xffffac73, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        internal static readonly uint[] PExt = new uint[] { 0x1b44bba9, 0xa71a, 1, 0, 0, 0xffff58e6, 0xfffffffd, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        private static readonly uint[] PExtInv = new uint[] { 0xe4bb4457, 0xffff58e5, 0xfffffffe, uint.MaxValue, uint.MaxValue, 0xa719, 2 };
        private const uint P4 = uint.MaxValue;
        private const uint PExt9 = uint.MaxValue;
        private const uint PInv33 = 0x538d;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            if ((Nat160.Add(x, y, z) != 0) || ((z[4] == uint.MaxValue) && Nat160.Gte(z, P)))
            {
                Nat.Add33To(5, 0x538d, z);
            }
        }

        public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if (((Nat.Add(10, xx, yy, zz) != 0) || ((zz[9] == uint.MaxValue) && Nat.Gte(10, zz, PExt))) && (Nat.AddTo(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.IncAt(10, zz, PExtInv.Length);
            }
        }

        public static void AddOne(uint[] x, uint[] z)
        {
            if ((Nat.Inc(5, x, z) != 0) || ((z[4] == uint.MaxValue) && Nat160.Gte(z, P)))
            {
                Nat.Add33To(5, 0x538d, z);
            }
        }

        public static uint[] FromBigInteger(BigInteger x)
        {
            uint[] numArray = Nat160.FromBigInteger(x);
            if ((numArray[4] == uint.MaxValue) && Nat160.Gte(numArray, P))
            {
                Nat160.SubFrom(P, numArray);
            }
            return numArray;
        }

        public static void Half(uint[] x, uint[] z)
        {
            if ((x[0] & 1) == 0)
            {
                Nat.ShiftDownBit(5, x, 0, z);
            }
            else
            {
                uint c = Nat160.Add(x, P, z);
                Nat.ShiftDownBit(5, z, c);
            }
        }

        public static void Multiply(uint[] x, uint[] y, uint[] z)
        {
            uint[] zz = Nat160.CreateExt();
            Nat160.Mul(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
        {
            if (((Nat160.MulAddTo(x, y, zz) != 0) || ((zz[9] == uint.MaxValue) && Nat.Gte(10, zz, PExt))) && (Nat.AddTo(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.IncAt(10, zz, PExtInv.Length);
            }
        }

        public static void Negate(uint[] x, uint[] z)
        {
            if (Nat160.IsZero(x))
            {
                Nat160.Zero(z);
            }
            else
            {
                Nat160.Sub(P, x, z);
            }
        }

        public static void Reduce(uint[] xx, uint[] z)
        {
            ulong y = Nat160.Mul33Add(0x538d, xx, 5, xx, 0, z, 0);
            if ((Nat160.Mul33DWordAdd(0x538d, y, z, 0) != 0) || ((z[4] == uint.MaxValue) && Nat160.Gte(z, P)))
            {
                Nat.Add33To(5, 0x538d, z);
            }
        }

        public static void Reduce32(uint x, uint[] z)
        {
            if (((x != 0) && (Nat160.Mul33WordAdd(0x538d, x, z, 0) != 0)) || ((z[4] == uint.MaxValue) && Nat160.Gte(z, P)))
            {
                Nat.Add33To(5, 0x538d, z);
            }
        }

        public static void Square(uint[] x, uint[] z)
        {
            uint[] zz = Nat160.CreateExt();
            Nat160.Square(x, zz);
            Reduce(zz, z);
        }

        public static void SquareN(uint[] x, int n, uint[] z)
        {
            uint[] zz = Nat160.CreateExt();
            Nat160.Square(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                Nat160.Square(z, zz);
                Reduce(zz, z);
            }
        }

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat160.Sub(x, y, z) != 0)
            {
                Nat.Sub33From(5, 0x538d, z);
            }
        }

        public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if ((Nat.Sub(10, xx, yy, zz) != 0) && (Nat.SubFrom(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.DecAt(10, zz, PExtInv.Length);
            }
        }

        public static void Twice(uint[] x, uint[] z)
        {
            if ((Nat.ShiftUpBit(5, x, 0, z) != 0) || ((z[4] == uint.MaxValue) && Nat160.Gte(z, P)))
            {
                Nat.Add33To(5, 0x538d, z);
            }
        }
    }
}

