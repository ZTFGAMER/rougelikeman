namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP128R1Field
    {
        internal static readonly uint[] P = new uint[] { uint.MaxValue, uint.MaxValue, uint.MaxValue, 0xfffffffd };
        internal static readonly uint[] PExt = new uint[] { 1, 0, 0, 4, 0xfffffffe, uint.MaxValue, 3, 0xfffffffc };
        private static readonly uint[] PExtInv = new uint[] { uint.MaxValue, uint.MaxValue, uint.MaxValue, 0xfffffffb, 1, 0, 0xfffffffc, 3 };
        private const uint P3 = 0xfffffffd;
        private const uint PExt7 = 0xfffffffc;

        public static void Add(uint[] x, uint[] y, uint[] z)
        {
            if ((Nat128.Add(x, y, z) != 0) || ((z[3] == 0xfffffffd) && Nat128.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }

        public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if ((Nat256.Add(xx, yy, zz) != 0) || ((zz[7] == 0xfffffffc) && Nat256.Gte(zz, PExt)))
            {
                Nat.AddTo(PExtInv.Length, PExtInv, zz);
            }
        }

        public static void AddOne(uint[] x, uint[] z)
        {
            if ((Nat.Inc(4, x, z) != 0) || ((z[3] == 0xfffffffd) && Nat128.Gte(z, P)))
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
            num = ((long) ((ulong) num)) + (z[3] + 2L);
            z[3] = (uint) num;
        }

        public static uint[] FromBigInteger(BigInteger x)
        {
            uint[] numArray = Nat128.FromBigInteger(x);
            if ((numArray[3] == 0xfffffffd) && Nat128.Gte(numArray, P))
            {
                Nat128.SubFrom(P, numArray);
            }
            return numArray;
        }

        public static void Half(uint[] x, uint[] z)
        {
            if ((x[0] & 1) == 0)
            {
                Nat.ShiftDownBit(4, x, 0, z);
            }
            else
            {
                uint c = Nat128.Add(x, P, z);
                Nat.ShiftDownBit(4, z, c);
            }
        }

        public static void Multiply(uint[] x, uint[] y, uint[] z)
        {
            uint[] zz = Nat128.CreateExt();
            Nat128.Mul(x, y, zz);
            Reduce(zz, z);
        }

        public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
        {
            if ((Nat128.MulAddTo(x, y, zz) != 0) || ((zz[7] == 0xfffffffc) && Nat256.Gte(zz, PExt)))
            {
                Nat.AddTo(PExtInv.Length, PExtInv, zz);
            }
        }

        public static void Negate(uint[] x, uint[] z)
        {
            if (Nat128.IsZero(x))
            {
                Nat128.Zero(z);
            }
            else
            {
                Nat128.Sub(P, x, z);
            }
        }

        public static void Reduce(uint[] xx, uint[] z)
        {
            ulong num = xx[0];
            ulong num2 = xx[1];
            ulong num3 = xx[2];
            ulong num4 = xx[3];
            ulong num5 = xx[4];
            ulong num6 = xx[5];
            ulong num7 = xx[6];
            ulong num8 = xx[7];
            num4 += num8;
            num7 += num8 << 1;
            num3 += num7;
            num6 += num7 << 1;
            num2 += num6;
            num5 += num6 << 1;
            num += num5;
            num4 += num5 << 1;
            z[0] = (uint) num;
            num2 += num >> 0x20;
            z[1] = (uint) num2;
            num3 += num2 >> 0x20;
            z[2] = (uint) num3;
            num4 += num3 >> 0x20;
            z[3] = (uint) num4;
            Reduce32((uint) (num4 >> 0x20), z);
        }

        public static void Reduce32(uint x, uint[] z)
        {
            while (x != 0)
            {
                ulong num2 = x;
                ulong num = z[0] + num2;
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
                num += z[3] + (num2 << 1);
                z[3] = (uint) num;
                num = num >> 0x20;
                x = (uint) num;
            }
        }

        public static void Square(uint[] x, uint[] z)
        {
            uint[] zz = Nat128.CreateExt();
            Nat128.Square(x, zz);
            Reduce(zz, z);
        }

        public static void SquareN(uint[] x, int n, uint[] z)
        {
            uint[] zz = Nat128.CreateExt();
            Nat128.Square(x, zz);
            Reduce(zz, z);
            while (--n > 0)
            {
                Nat128.Square(z, zz);
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
            num = ((long) ((ulong) num)) + (z[3] - 2L);
            z[3] = (uint) num;
        }

        public static void Subtract(uint[] x, uint[] y, uint[] z)
        {
            if (Nat128.Sub(x, y, z) != 0)
            {
                SubPInvFrom(z);
            }
        }

        public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
        {
            if (Nat.Sub(10, xx, yy, zz) != 0)
            {
                Nat.SubFrom(PExtInv.Length, PExtInv, zz);
            }
        }

        public static void Twice(uint[] x, uint[] z)
        {
            if ((Nat.ShiftUpBit(4, x, 0, z) != 0) || ((z[3] == 0xfffffffd) && Nat128.Gte(z, P)))
            {
                AddPInvTo(z);
            }
        }
    }
}

