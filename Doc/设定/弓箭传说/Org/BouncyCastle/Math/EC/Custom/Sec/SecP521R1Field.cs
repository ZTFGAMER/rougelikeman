namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP521R1Field
    {
        internal static readonly uint[] P = new uint[] { 
            uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue, uint.MaxValue,
            0x1ff
        };
        private const int P16 = 0x1ff;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            uint num = (Nat.Add(0x10, x, y, z) + x[0x10]) + y[0x10];
            if ((num > 0x1ff) || ((num == 0x1ff) && Nat.Eq(0x10, z, P)))
            {
                num += Nat.Inc(0x10, z);
                num &= 0x1ff;
            }
            z[0x10] = num;
        }

        public static void AddOne(uint[] x, uint[] z)
        {
            uint num = Nat.Inc(0x10, x, z) + x[0x10];
            if ((num > 0x1ff) || ((num == 0x1ff) && Nat.Eq(0x10, z, P)))
            {
                num += Nat.Inc(0x10, z);
                num &= 0x1ff;
            }
            z[0x10] = num;
        }

        public static uint[] FromBigInteger(BigInteger x)
        {
            uint[] numArray = Nat.FromBigInteger(0x209, x);
            if (Nat.Eq(0x11, numArray, P))
            {
                Nat.Zero(0x11, numArray);
            }
            return numArray;
        }

        public static void Half(uint[] x, uint[] z)
        {
            uint c = x[0x10];
            uint num2 = Nat.ShiftDownBit(0x10, x, c, z);
            z[0x10] = (c >> 1) | (num2 >> 0x17);
        }

        protected static void ImplMultiply(uint[] x, uint[] y, uint[] zz)
        {
            Nat512.Mul(x, y, zz);
            uint a = x[0x10];
            uint b = y[0x10];
            zz[0x20] = Nat.Mul31BothAdd(0x10, a, y, b, x, zz, 0x10) + (a * b);
        }

        protected static void ImplSquare(uint[] x, uint[] zz)
        {
            Nat512.Square(x, zz);
            uint num = x[0x10];
            zz[0x20] = Nat.MulWordAddTo(0x10, num << 1, x, 0, zz, 0x10) + (num * num);
        }

        public static void Multiply(uint[] x, uint[] y, uint[] z)
        {
            uint[] zz = Nat.Create(0x21);
            ImplMultiply(x, y, zz);
            Reduce(zz, z);
        }

        public static void Negate(uint[] x, uint[] z)
        {
            if (Nat.IsZero(0x11, x))
            {
                Nat.Zero(0x11, z);
            }
            else
            {
                Nat.Sub(0x11, P, x, z);
            }
        }

        public static void Reduce(uint[] xx, uint[] z)
        {
            uint c = xx[0x20];
            uint num2 = Nat.ShiftDownBits(0x10, xx, 0x10, 9, c, z, 0) >> 0x17;
            num2 += c >> 9;
            num2 += Nat.AddTo(0x10, xx, z);
            if ((num2 > 0x1ff) || ((num2 == 0x1ff) && Nat.Eq(0x10, z, P)))
            {
                num2 += Nat.Inc(0x10, z);
                num2 &= 0x1ff;
            }
            z[0x10] = num2;
        }

        public static void Reduce23(uint[] z)
        {
            uint num = z[0x10];
            uint num2 = Nat.AddWordTo(0x10, num >> 9, z) + (num & 0x1ff);
            if ((num2 > 0x1ff) || ((num2 == 0x1ff) && Nat.Eq(0x10, z, P)))
            {
                num2 += Nat.Inc(0x10, z);
                num2 &= 0x1ff;
            }
            z[0x10] = num2;
        }

        public static void Square(uint[] x, uint[] z)
        {
            uint[] zz = Nat.Create(0x21);
            ImplSquare(x, zz);
            Reduce(zz, z);
        }

        public static void SquareN(uint[] x, int n, uint[] z)
        {
            uint[] zz = Nat.Create(0x21);
            ImplSquare(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                ImplSquare(z, zz);
                Reduce(zz, z);
            }
        }

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            int num = Nat.Sub(0x10, x, y, z) + ((int) (x[0x10] - y[0x10]));
            if (num < 0)
            {
                num += Nat.Dec(0x10, z);
                num &= 0x1ff;
            }
            z[0x10] = (uint) num;
        }

        public static void Twice(uint[] x, uint[] z)
        {
            uint num = x[0x10];
            uint num2 = Nat.ShiftUpBit(0x10, x, num << 0x17, z) | (num << 1);
            z[0x10] = num2 & 0x1ff;
        }
    }
}

