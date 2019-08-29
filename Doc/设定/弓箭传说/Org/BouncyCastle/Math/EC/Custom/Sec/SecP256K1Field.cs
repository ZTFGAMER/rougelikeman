namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP256K1Field
    {
        internal static readonly uint[] P = new uint[] { 0xfffffc2f, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        internal static readonly uint[] PExt = new uint[] { 0xe90a1, 0x7a2, 1, 0, 0, 0, 0, 0, 0xfffff85e, 0xfffffffd, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        private static readonly uint[] PExtInv = new uint[] { 0xfff16f5f, 0xfffff85d, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, 0x7a1, 2 };
        private const uint P7 = uint.MaxValue;
        private const uint PExt15 = uint.MaxValue;
        private const uint PInv33 = 0x3d1;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            if ((Nat256.Add(x, y, z) != 0) || ((z[7] == uint.MaxValue) && Nat256.Gte(z, P)))
            {
                Nat.Add33To(8, 0x3d1, z);
            }
        }

        public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if (((Nat.Add(0x10, xx, yy, zz) != 0) || ((zz[15] == uint.MaxValue) && Nat.Gte(0x10, zz, PExt))) && (Nat.AddTo(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.IncAt(0x10, zz, PExtInv.Length);
            }
        }

        public static void AddOne(uint[] x, uint[] z)
        {
            if ((Nat.Inc(8, x, z) != 0) || ((z[7] == uint.MaxValue) && Nat256.Gte(z, P)))
            {
                Nat.Add33To(8, 0x3d1, z);
            }
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
            if (((Nat256.MulAddTo(x, y, zz) != 0) || ((zz[15] == uint.MaxValue) && Nat.Gte(0x10, zz, PExt))) && (Nat.AddTo(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.IncAt(0x10, zz, PExtInv.Length);
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
            ulong y = Nat256.Mul33Add(0x3d1, xx, 8, xx, 0, z, 0);
            if ((Nat256.Mul33DWordAdd(0x3d1, y, z, 0) != 0) || ((z[7] == uint.MaxValue) && Nat256.Gte(z, P)))
            {
                Nat.Add33To(8, 0x3d1, z);
            }
        }

        public static void Reduce32(uint x, uint[] z)
        {
            if (((x != 0) && (Nat256.Mul33WordAdd(0x3d1, x, z, 0) != 0)) || ((z[7] == uint.MaxValue) && Nat256.Gte(z, P)))
            {
                Nat.Add33To(8, 0x3d1, z);
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

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat256.Sub(x, y, z) != 0)
            {
                Nat.Sub33From(8, 0x3d1, z);
            }
        }

        public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if ((Nat.Sub(0x10, xx, yy, zz) != 0) && (Nat.SubFrom(PExtInv.Length, PExtInv, zz) != 0))
            {
                Nat.DecAt(0x10, zz, PExtInv.Length);
            }
        }

        public static void Twice(uint[] x, uint[] z)
        {
            if ((Nat.ShiftUpBit(8, x, 0, z) != 0) || ((z[7] == uint.MaxValue) && Nat256.Gte(z, P)))
            {
                Nat.Add33To(8, 0x3d1, z);
            }
        }
    }
}

