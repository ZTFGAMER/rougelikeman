namespace Org.BouncyCastle.Math.Raw
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Security;
    using System;

    internal abstract class Mod
    {
        private static readonly SecureRandom RandomSource = new SecureRandom();

        protected Mod()
        {
        }

        public static void Add(uint[] p, uint[] x, uint[] y, uint[] z)
        {
            int length = p.Length;
            if (Nat.Add(length, x, y, z) != 0)
            {
                Nat.SubFrom(length, p, z);
            }
        }

        private static int GetTrailingZeroes(uint x)
        {
            int num = 0;
            while ((x & 1) == 0)
            {
                x = x >> 1;
                num++;
            }
            return num;
        }

        private static void InversionResult(uint[] p, int ac, uint[] a, uint[] z)
        {
            if (ac < 0)
            {
                Nat.Add(p.Length, a, p, z);
            }
            else
            {
                Array.Copy(a, 0, z, 0, p.Length);
            }
        }

        private static void InversionStep(uint[] p, uint[] u, int uLen, uint[] x, ref int xc)
        {
            int length = p.Length;
            int num2 = 0;
            while (u[0] == 0)
            {
                Nat.ShiftDownWord(uLen, u, 0);
                num2 += 0x20;
            }
            int trailingZeroes = GetTrailingZeroes(u[0]);
            if (trailingZeroes > 0)
            {
                Nat.ShiftDownBits(uLen, u, trailingZeroes, 0);
                num2 += trailingZeroes;
            }
            for (int i = 0; i < num2; i++)
            {
                if ((x[0] & 1) != 0)
                {
                    if (xc < 0)
                    {
                        xc = (int) (xc + Nat.AddTo(length, p, x));
                    }
                    else
                    {
                        xc += Nat.SubFrom(length, p, x);
                    }
                }
                Nat.ShiftDownBit(length, x, (uint) xc);
            }
        }

        public static void Invert(uint[] p, uint[] x, uint[] z)
        {
            int length = p.Length;
            if (Nat.IsZero(length, x))
            {
                throw new ArgumentException("cannot be 0", "x");
            }
            if (Nat.IsOne(length, x))
            {
                Array.Copy(x, 0, z, 0, length);
                return;
            }
            uint[] u = Nat.Copy(length, x);
            uint[] numArray2 = Nat.Create(length);
            numArray2[0] = 1;
            int xc = 0;
            if ((u[0] & 1) == 0)
            {
                InversionStep(p, u, length, numArray2, ref xc);
            }
            if (Nat.IsOne(length, u))
            {
                InversionResult(p, xc, numArray2, z);
                return;
            }
            uint[] y = Nat.Copy(length, p);
            uint[] numArray4 = Nat.Create(length);
            int num3 = 0;
            int uLen = length;
        Label_009E:
            while ((u[uLen - 1] == 0) && (y[uLen - 1] == 0))
            {
                uLen--;
            }
            if (Nat.Gte(length, u, y))
            {
                Nat.SubFrom(length, y, u);
                xc += Nat.SubFrom(length, numArray4, numArray2) - num3;
                InversionStep(p, u, uLen, numArray2, ref xc);
                if (!Nat.IsOne(length, u))
                {
                    goto Label_009E;
                }
                InversionResult(p, xc, numArray2, z);
            }
            else
            {
                Nat.SubFrom(length, u, y);
                num3 += Nat.SubFrom(length, numArray2, numArray4) - xc;
                InversionStep(p, y, uLen, numArray4, ref num3);
                if (!Nat.IsOne(length, y))
                {
                    goto Label_009E;
                }
                InversionResult(p, num3, numArray4, z);
            }
        }

        public static uint[] Random(uint[] p)
        {
            int length = p.Length;
            uint[] x = Nat.Create(length);
            uint num2 = p[length - 1];
            num2 |= num2 >> 1;
            num2 |= num2 >> 2;
            num2 |= num2 >> 4;
            num2 |= num2 >> 8;
            num2 |= num2 >> 0x10;
            do
            {
                byte[] buffer = new byte[length << 2];
                RandomSource.NextBytes(buffer);
                Pack.BE_To_UInt32(buffer, 0, x);
                x[length - 1] &= num2;
            }
            while (Nat.Gte(length, x, p));
            return x;
        }

        public static void Subtract(uint[] p, uint[] x, uint[] y, uint[] z)
        {
            int length = p.Length;
            if (Nat.Sub(length, x, y, z) != 0)
            {
                Nat.AddTo(length, p, z);
            }
        }
    }
}

