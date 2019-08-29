namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP192K1Field
    {
        internal static readonly uint[] P = new uint[] { 0xffffee37, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        internal static readonly uint[] PExt = new uint[] { 0x13c4fd1, 0x2392, 1, 0, 0, 0, 0xffffdc6e, 0xfffffffd, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        private static readonly uint[] PExtInv = new uint[] { 0xfec3b02f, 0xffffdc6d, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue, 0x2391, 2 };
        private const uint P5 = uint.MaxValue;
        private const uint PExt11 = uint.MaxValue;
        private const uint PInv33 = 0x11c9;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            if ((Nat192.Add(x, y, z) != 0) || ((z[5] == uint.MaxValue) && Nat192.Gte(z, P)))
            {
                Nat.Add33To(6, 0x11c9, z);
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
                Nat.Add33To(6, 0x11c9, z);
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
            ulong y = Nat192.Mul33Add(0x11c9, xx, 6, xx, 0, z, 0);
            if ((Nat192.Mul33DWordAdd(0x11c9, y, z, 0) != 0) || ((z[5] == uint.MaxValue) && Nat192.Gte(z, P)))
            {
                Nat.Add33To(6, 0x11c9, z);
            }
        }

        public static void Reduce32(uint x, uint[] z)
        {
            if (((x != 0) && (Nat192.Mul33WordAdd(0x11c9, x, z, 0) != 0)) || ((z[5] == uint.MaxValue) && Nat192.Gte(z, P)))
            {
                Nat.Add33To(6, 0x11c9, z);
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

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat192.Sub(x, y, z) != 0)
            {
                Nat.Sub33From(6, 0x11c9, z);
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
                Nat.Add33To(6, 0x11c9, z);
            }
        }
    }
}

