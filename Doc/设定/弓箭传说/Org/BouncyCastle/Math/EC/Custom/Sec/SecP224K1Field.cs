namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP224K1Field
    {
        internal static readonly uint[] P = new uint[] { 0xffffe56d, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        internal static readonly uint[] PExt = new uint[] { 0x2c23069, 0x3526, 1, 0, 0, 0, 0, 0xffffcada, 0xfffffffd, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue };
        private static readonly uint[] PExtInv = new uint[] { 0xfd3dcf97, 0xffffcad9, 0xfffffffe, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, 0x3525, 2 };
        private const uint P6 = uint.MaxValue;
        private const uint PExt13 = uint.MaxValue;
        private const uint PInv33 = 0x1a93;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            if ((Nat224.Add(x, y, z) != 0) || ((z[6] == uint.MaxValue) && Nat224.Gte(z, P)))
            {
                Nat.Add33To(7, 0x1a93, z);
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
                Nat.Add33To(7, 0x1a93, z);
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
            ulong y = Nat224.Mul33Add(0x1a93, xx, 7, xx, 0, z, 0);
            if ((Nat224.Mul33DWordAdd(0x1a93, y, z, 0) != 0) || ((z[6] == uint.MaxValue) && Nat224.Gte(z, P)))
            {
                Nat.Add33To(7, 0x1a93, z);
            }
        }

        public static void Reduce32(uint x, uint[] z)
        {
            if (((x != 0) && (Nat224.Mul33WordAdd(0x1a93, x, z, 0) != 0)) || ((z[6] == uint.MaxValue) && Nat224.Gte(z, P)))
            {
                Nat.Add33To(7, 0x1a93, z);
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

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat224.Sub(x, y, z) != 0)
            {
                Nat.Sub33From(7, 0x1a93, z);
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
                Nat.Add33To(7, 0x1a93, z);
            }
        }
    }
}

