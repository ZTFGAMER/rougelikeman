namespace Org.BouncyCastle.Math.Raw
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Math;
    using System;

    internal abstract class Nat
    {
        private const ulong M = 0xffffffffL;

        protected Nat()
        {
        }

        public static uint Add(int len, uint[] x, uint[] y, uint[] z)
        {
            ulong num = 0L;
            for (int i = 0; i < len; i++)
            {
                num += x[i] + y[i];
                z[i] = (uint) num;
                num = num >> 0x20;
            }
            return (uint) num;
        }

        public static uint Add33At(int len, uint x, uint[] z, int zPos)
        {
            ulong num = z[zPos] + x;
            z[zPos] = (uint) num;
            num = num >> 0x20;
            num += (ulong) (z[zPos + 1] + 1L);
            z[zPos + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zPos + 2) : 0);
        }

        public static uint Add33At(int len, uint x, uint[] z, int zOff, int zPos)
        {
            ulong num = z[zOff + zPos] + x;
            z[zOff + zPos] = (uint) num;
            num = num >> 0x20;
            num += (ulong) (z[(zOff + zPos) + 1] + 1L);
            z[(zOff + zPos) + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zOff, zPos + 2) : 0);
        }

        public static uint Add33To(int len, uint x, uint[] z)
        {
            ulong num = z[0] + x;
            z[0] = (uint) num;
            num = num >> 0x20;
            num += (ulong) (z[1] + 1L);
            z[1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, 2) : 0);
        }

        public static uint Add33To(int len, uint x, uint[] z, int zOff)
        {
            ulong num = z[zOff] + x;
            z[zOff] = (uint) num;
            num = num >> 0x20;
            num += (ulong) (z[zOff + 1] + 1L);
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zOff, 2) : 0);
        }

        public static uint AddBothTo(int len, uint[] x, uint[] y, uint[] z)
        {
            ulong num = 0L;
            for (int i = 0; i < len; i++)
            {
                num += (x[i] + y[i]) + z[i];
                z[i] = (uint) num;
                num = num >> 0x20;
            }
            return (uint) num;
        }

        public static uint AddBothTo(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
        {
            ulong num = 0L;
            for (int i = 0; i < len; i++)
            {
                num += (x[xOff + i] + y[yOff + i]) + z[zOff + i];
                z[zOff + i] = (uint) num;
                num = num >> 0x20;
            }
            return (uint) num;
        }

        public static uint AddDWordAt(int len, ulong x, uint[] z, int zPos)
        {
            ulong num = z[zPos] + (x & 0xffffffffL);
            z[zPos] = (uint) num;
            num = num >> 0x20;
            num += z[zPos + 1] + (x >> 0x20);
            z[zPos + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zPos + 2) : 0);
        }

        public static uint AddDWordAt(int len, ulong x, uint[] z, int zOff, int zPos)
        {
            ulong num = z[zOff + zPos] + (x & 0xffffffffL);
            z[zOff + zPos] = (uint) num;
            num = num >> 0x20;
            num += z[(zOff + zPos) + 1] + (x >> 0x20);
            z[(zOff + zPos) + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zOff, zPos + 2) : 0);
        }

        public static uint AddDWordTo(int len, ulong x, uint[] z)
        {
            ulong num = z[0] + (x & 0xffffffffL);
            z[0] = (uint) num;
            num = num >> 0x20;
            num += z[1] + (x >> 0x20);
            z[1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, 2) : 0);
        }

        public static uint AddDWordTo(int len, ulong x, uint[] z, int zOff)
        {
            ulong num = z[zOff] + (x & 0xffffffffL);
            z[zOff] = (uint) num;
            num = num >> 0x20;
            num += z[zOff + 1] + (x >> 0x20);
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zOff, 2) : 0);
        }

        public static uint AddTo(int len, uint[] x, uint[] z)
        {
            ulong num = 0L;
            for (int i = 0; i < len; i++)
            {
                num += x[i] + z[i];
                z[i] = (uint) num;
                num = num >> 0x20;
            }
            return (uint) num;
        }

        public static uint AddTo(int len, uint[] x, int xOff, uint[] z, int zOff)
        {
            ulong num = 0L;
            for (int i = 0; i < len; i++)
            {
                num += x[xOff + i] + z[zOff + i];
                z[zOff + i] = (uint) num;
                num = num >> 0x20;
            }
            return (uint) num;
        }

        public static uint AddWordAt(int len, uint x, uint[] z, int zPos)
        {
            ulong num = x + z[zPos];
            z[zPos] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zPos + 1) : 0);
        }

        public static uint AddWordAt(int len, uint x, uint[] z, int zOff, int zPos)
        {
            ulong num = x + z[zOff + zPos];
            z[zOff + zPos] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zOff, zPos + 1) : 0);
        }

        public static uint AddWordTo(int len, uint x, uint[] z)
        {
            ulong num = x + z[0];
            z[0] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, 1) : 0);
        }

        public static uint AddWordTo(int len, uint x, uint[] z, int zOff)
        {
            ulong num = x + z[zOff];
            z[zOff] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zOff, 1) : 0);
        }

        public static uint[] Copy(int len, uint[] x)
        {
            uint[] destinationArray = new uint[len];
            Array.Copy(x, 0, destinationArray, 0, len);
            return destinationArray;
        }

        public static void Copy(int len, uint[] x, uint[] z)
        {
            Array.Copy(x, 0, z, 0, len);
        }

        public static uint[] Create(int len) => 
            new uint[len];

        public static ulong[] Create64(int len) => 
            new ulong[len];

        public static int Dec(int len, uint[] z)
        {
            for (int i = 0; i < len; i++)
            {
                if (--z[i] != uint.MaxValue)
                {
                    return 0;
                }
            }
            return -1;
        }

        public static int Dec(int len, uint[] x, uint[] z)
        {
            int index = 0;
            while (index < len)
            {
                uint num2 = x[index] - 1;
                z[index] = num2;
                index++;
                if (num2 != uint.MaxValue)
                {
                    while (index < len)
                    {
                        z[index] = x[index];
                        index++;
                    }
                    return 0;
                }
            }
            return -1;
        }

        public static int DecAt(int len, uint[] z, int zPos)
        {
            for (int i = zPos; i < len; i++)
            {
                if (--z[i] != uint.MaxValue)
                {
                    return 0;
                }
            }
            return -1;
        }

        public static int DecAt(int len, uint[] z, int zOff, int zPos)
        {
            for (int i = zPos; i < len; i++)
            {
                if (--z[zOff + i] != uint.MaxValue)
                {
                    return 0;
                }
            }
            return -1;
        }

        public static bool Eq(int len, uint[] x, uint[] y)
        {
            for (int i = len - 1; i >= 0; i--)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static uint[] FromBigInteger(int bits, BigInteger x)
        {
            if ((x.SignValue < 0) || (x.BitLength > bits))
            {
                throw new ArgumentException();
            }
            int len = (bits + 0x1f) >> 5;
            uint[] numArray = Create(len);
            int num2 = 0;
            while (x.SignValue != 0)
            {
                numArray[num2++] = (uint) x.IntValue;
                x = x.ShiftRight(0x20);
            }
            return numArray;
        }

        public static uint GetBit(uint[] x, int bit)
        {
            if (bit == 0)
            {
                return (x[0] & 1);
            }
            int index = bit >> 5;
            if ((index < 0) || (index >= x.Length))
            {
                return 0;
            }
            int num2 = bit & 0x1f;
            return ((x[index] >> num2) & 1);
        }

        public static bool Gte(int len, uint[] x, uint[] y)
        {
            for (int i = len - 1; i >= 0; i--)
            {
                uint num2 = x[i];
                uint num3 = y[i];
                if (num2 < num3)
                {
                    return false;
                }
                if (num2 > num3)
                {
                    return true;
                }
            }
            return true;
        }

        public static uint Inc(int len, uint[] z)
        {
            for (int i = 0; i < len; i++)
            {
                if (++z[i] != 0)
                {
                    return 0;
                }
            }
            return 1;
        }

        public static uint Inc(int len, uint[] x, uint[] z)
        {
            int index = 0;
            while (index < len)
            {
                uint num2 = x[index] + 1;
                z[index] = num2;
                index++;
                if (num2 != 0)
                {
                    while (index < len)
                    {
                        z[index] = x[index];
                        index++;
                    }
                    return 0;
                }
            }
            return 1;
        }

        public static uint IncAt(int len, uint[] z, int zPos)
        {
            for (int i = zPos; i < len; i++)
            {
                if (++z[i] != 0)
                {
                    return 0;
                }
            }
            return 1;
        }

        public static uint IncAt(int len, uint[] z, int zOff, int zPos)
        {
            for (int i = zPos; i < len; i++)
            {
                if (++z[zOff + i] != 0)
                {
                    return 0;
                }
            }
            return 1;
        }

        public static bool IsOne(int len, uint[] x)
        {
            if (x[0] != 1)
            {
                return false;
            }
            for (int i = 1; i < len; i++)
            {
                if (x[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsZero(int len, uint[] x)
        {
            if (x[0] != 0)
            {
                return false;
            }
            for (int i = 1; i < len; i++)
            {
                if (x[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static void Mul(int len, uint[] x, uint[] y, uint[] zz)
        {
            zz[len] = MulWord(len, x[0], y, zz);
            for (int i = 1; i < len; i++)
            {
                zz[i + len] = MulWordAddTo(len, x[i], y, 0, zz, i);
            }
        }

        public static void Mul(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
        {
            zz[zzOff + len] = MulWord(len, x[xOff], y, yOff, zz, zzOff);
            for (int i = 1; i < len; i++)
            {
                zz[(zzOff + i) + len] = MulWordAddTo(len, x[xOff + i], y, yOff, zz, zzOff + i);
            }
        }

        public static uint Mul31BothAdd(int len, uint a, uint[] x, uint b, uint[] y, uint[] z, int zOff)
        {
            ulong num = 0L;
            ulong num2 = a;
            ulong num3 = b;
            int index = 0;
            do
            {
                num += ((num2 * x[index]) + (num3 * y[index])) + z[zOff + index];
                z[zOff + index] = (uint) num;
                num = num >> 0x20;
            }
            while (++index < len);
            return (uint) num;
        }

        public static uint MulWord(int len, uint x, uint[] y, uint[] z)
        {
            ulong num = 0L;
            ulong num2 = x;
            int index = 0;
            do
            {
                num += num2 * y[index];
                z[index] = (uint) num;
                num = num >> 0x20;
            }
            while (++index < len);
            return (uint) num;
        }

        public static uint MulWord(int len, uint x, uint[] y, int yOff, uint[] z, int zOff)
        {
            ulong num = 0L;
            ulong num2 = x;
            int num3 = 0;
            do
            {
                num += num2 * y[yOff + num3];
                z[zOff + num3] = (uint) num;
                num = num >> 0x20;
            }
            while (++num3 < len);
            return (uint) num;
        }

        public static uint MulWordAddTo(int len, uint x, uint[] y, int yOff, uint[] z, int zOff)
        {
            ulong num = 0L;
            ulong num2 = x;
            int num3 = 0;
            do
            {
                num += (num2 * y[yOff + num3]) + z[zOff + num3];
                z[zOff + num3] = (uint) num;
                num = num >> 0x20;
            }
            while (++num3 < len);
            return (uint) num;
        }

        public static uint MulWordDwordAddAt(int len, uint x, ulong y, uint[] z, int zPos)
        {
            ulong num = 0L;
            ulong num2 = x;
            num += (num2 * ((uint) y)) + z[zPos];
            z[zPos] = (uint) num;
            num = num >> 0x20;
            num += (num2 * (y >> 0x20)) + z[zPos + 1];
            z[zPos + 1] = (uint) num;
            num = num >> 0x20;
            num += z[zPos + 2];
            z[zPos + 2] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? IncAt(len, z, zPos + 3) : 0);
        }

        public static uint ShiftDownBit(int len, uint[] z, uint c)
        {
            int index = len;
            while (--index >= 0)
            {
                uint num2 = z[index];
                z[index] = (num2 >> 1) | (c << 0x1f);
                c = num2;
            }
            return (c << 0x1f);
        }

        public static uint ShiftDownBit(int len, uint[] z, int zOff, uint c)
        {
            int num = len;
            while (--num >= 0)
            {
                uint num2 = z[zOff + num];
                z[zOff + num] = (num2 >> 1) | (c << 0x1f);
                c = num2;
            }
            return (c << 0x1f);
        }

        public static uint ShiftDownBit(int len, uint[] x, uint c, uint[] z)
        {
            int index = len;
            while (--index >= 0)
            {
                uint num2 = x[index];
                z[index] = (num2 >> 1) | (c << 0x1f);
                c = num2;
            }
            return (c << 0x1f);
        }

        public static uint ShiftDownBit(int len, uint[] x, int xOff, uint c, uint[] z, int zOff)
        {
            int num = len;
            while (--num >= 0)
            {
                uint num2 = x[xOff + num];
                z[zOff + num] = (num2 >> 1) | (c << 0x1f);
                c = num2;
            }
            return (c << 0x1f);
        }

        public static uint ShiftDownBits(int len, uint[] z, int bits, uint c)
        {
            int index = len;
            while (--index >= 0)
            {
                uint num2 = z[index];
                z[index] = (num2 >> bits) | (c << -bits);
                c = num2;
            }
            return (c << -bits);
        }

        public static uint ShiftDownBits(int len, uint[] z, int zOff, int bits, uint c)
        {
            int num = len;
            while (--num >= 0)
            {
                uint num2 = z[zOff + num];
                z[zOff + num] = (num2 >> bits) | (c << -bits);
                c = num2;
            }
            return (c << -bits);
        }

        public static uint ShiftDownBits(int len, uint[] x, int bits, uint c, uint[] z)
        {
            int index = len;
            while (--index >= 0)
            {
                uint num2 = x[index];
                z[index] = (num2 >> bits) | (c << -bits);
                c = num2;
            }
            return (c << -bits);
        }

        public static uint ShiftDownBits(int len, uint[] x, int xOff, int bits, uint c, uint[] z, int zOff)
        {
            int num = len;
            while (--num >= 0)
            {
                uint num2 = x[xOff + num];
                z[zOff + num] = (num2 >> bits) | (c << -bits);
                c = num2;
            }
            return (c << -bits);
        }

        public static uint ShiftDownWord(int len, uint[] z, uint c)
        {
            int index = len;
            while (--index >= 0)
            {
                uint num2 = z[index];
                z[index] = c;
                c = num2;
            }
            return c;
        }

        public static uint ShiftUpBit(int len, uint[] z, uint c)
        {
            for (int i = 0; i < len; i++)
            {
                uint num2 = z[i];
                z[i] = (num2 << 1) | (c >> 0x1f);
                c = num2;
            }
            return (c >> 0x1f);
        }

        public static uint ShiftUpBit(int len, uint[] z, int zOff, uint c)
        {
            for (int i = 0; i < len; i++)
            {
                uint num2 = z[zOff + i];
                z[zOff + i] = (num2 << 1) | (c >> 0x1f);
                c = num2;
            }
            return (c >> 0x1f);
        }

        public static uint ShiftUpBit(int len, uint[] x, uint c, uint[] z)
        {
            for (int i = 0; i < len; i++)
            {
                uint num2 = x[i];
                z[i] = (num2 << 1) | (c >> 0x1f);
                c = num2;
            }
            return (c >> 0x1f);
        }

        public static uint ShiftUpBit(int len, uint[] x, int xOff, uint c, uint[] z, int zOff)
        {
            for (int i = 0; i < len; i++)
            {
                uint num2 = x[xOff + i];
                z[zOff + i] = (num2 << 1) | (c >> 0x1f);
                c = num2;
            }
            return (c >> 0x1f);
        }

        public static ulong ShiftUpBit64(int len, ulong[] x, int xOff, ulong c, ulong[] z, int zOff)
        {
            for (int i = 0; i < len; i++)
            {
                ulong num2 = x[xOff + i];
                z[zOff + i] = (num2 << 1) | (c >> 0x3f);
                c = num2;
            }
            return (c >> 0x3f);
        }

        public static uint ShiftUpBits(int len, uint[] z, int bits, uint c)
        {
            for (int i = 0; i < len; i++)
            {
                uint num2 = z[i];
                z[i] = (num2 << bits) | (c >> -bits);
                c = num2;
            }
            return (c >> -bits);
        }

        public static uint ShiftUpBits(int len, uint[] z, int zOff, int bits, uint c)
        {
            for (int i = 0; i < len; i++)
            {
                uint num2 = z[zOff + i];
                z[zOff + i] = (num2 << bits) | (c >> -bits);
                c = num2;
            }
            return (c >> -bits);
        }

        public static uint ShiftUpBits(int len, uint[] x, int bits, uint c, uint[] z)
        {
            for (int i = 0; i < len; i++)
            {
                uint num2 = x[i];
                z[i] = (num2 << bits) | (c >> -bits);
                c = num2;
            }
            return (c >> -bits);
        }

        public static uint ShiftUpBits(int len, uint[] x, int xOff, int bits, uint c, uint[] z, int zOff)
        {
            for (int i = 0; i < len; i++)
            {
                uint num2 = x[xOff + i];
                z[zOff + i] = (num2 << bits) | (c >> -bits);
                c = num2;
            }
            return (c >> -bits);
        }

        public static ulong ShiftUpBits64(int len, ulong[] z, int zOff, int bits, ulong c)
        {
            for (int i = 0; i < len; i++)
            {
                ulong num2 = z[zOff + i];
                z[zOff + i] = (num2 << bits) | (c >> -bits);
                c = num2;
            }
            return (c >> -bits);
        }

        public static ulong ShiftUpBits64(int len, ulong[] x, int xOff, int bits, ulong c, ulong[] z, int zOff)
        {
            for (int i = 0; i < len; i++)
            {
                ulong num2 = x[xOff + i];
                z[zOff + i] = (num2 << bits) | (c >> -bits);
                c = num2;
            }
            return (c >> -bits);
        }

        public static void Square(int len, uint[] x, uint[] zz)
        {
            int num = len << 1;
            uint num2 = 0;
            int num3 = len;
            int num4 = num;
            do
            {
                ulong num5 = x[--num3];
                ulong num6 = num5 * num5;
                zz[--num4] = (num2 << 0x1f) | ((uint) (num6 >> 0x21));
                zz[--num4] = (uint) (num6 >> 1);
                num2 = (uint) num6;
            }
            while (num3 > 0);
            for (int i = 1; i < len; i++)
            {
                num2 = SquareWordAdd(x, i, zz);
                AddWordAt(num, num2, zz, i << 1);
            }
            ShiftUpBit(num, zz, x[0] << 0x1f);
        }

        public static void Square(int len, uint[] x, int xOff, uint[] zz, int zzOff)
        {
            int num = len << 1;
            uint num2 = 0;
            int num3 = len;
            int num4 = num;
            do
            {
                ulong num5 = x[xOff + --num3];
                ulong num6 = num5 * num5;
                zz[zzOff + --num4] = (num2 << 0x1f) | ((uint) (num6 >> 0x21));
                zz[zzOff + --num4] = (uint) (num6 >> 1);
                num2 = (uint) num6;
            }
            while (num3 > 0);
            for (int i = 1; i < len; i++)
            {
                num2 = SquareWordAdd(x, xOff, i, zz, zzOff);
                AddWordAt(num, num2, zz, zzOff, i << 1);
            }
            ShiftUpBit(num, zz, zzOff, x[xOff] << 0x1f);
        }

        public static uint SquareWordAdd(uint[] x, int xPos, uint[] z)
        {
            ulong num = 0L;
            ulong num2 = x[xPos];
            int index = 0;
            do
            {
                num += (num2 * x[index]) + z[xPos + index];
                z[xPos + index] = (uint) num;
                num = num >> 0x20;
            }
            while (++index < xPos);
            return (uint) num;
        }

        public static uint SquareWordAdd(uint[] x, int xOff, int xPos, uint[] z, int zOff)
        {
            ulong num = 0L;
            ulong num2 = x[xOff + xPos];
            int num3 = 0;
            do
            {
                num += (num2 * (x[xOff + num3] & 0xffffffffL)) + (z[xPos + zOff] & 0xffffffffL);
                z[xPos + zOff] = (uint) num;
                num = num >> 0x20;
                zOff++;
            }
            while (++num3 < xPos);
            return (uint) num;
        }

        public static int Sub(int len, uint[] x, uint[] y, uint[] z)
        {
            long num = 0L;
            for (int i = 0; i < len; i++)
            {
                num = (long) (((ulong) num) + (x[i] - y[i]));
                z[i] = (uint) num;
                num = num >> 0x20;
            }
            return (int) num;
        }

        public static int Sub(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
        {
            long num = 0L;
            for (int i = 0; i < len; i++)
            {
                num = (long) (((ulong) num) + (x[xOff + i] - y[yOff + i]));
                z[zOff + i] = (uint) num;
                num = num >> 0x20;
            }
            return (int) num;
        }

        public static int Sub33At(int len, uint x, uint[] z, int zPos)
        {
            long num = z[zPos] - x;
            z[zPos] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[zPos + 1] - 1L);
            z[zPos + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, zPos + 2) : 0);
        }

        public static int Sub33At(int len, uint x, uint[] z, int zOff, int zPos)
        {
            long num = z[zOff + zPos] - x;
            z[zOff + zPos] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[(zOff + zPos) + 1] - 1L);
            z[(zOff + zPos) + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, zOff, zPos + 2) : 0);
        }

        public static int Sub33From(int len, uint x, uint[] z)
        {
            long num = z[0] - x;
            z[0] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[1] - 1L);
            z[1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, 2) : 0);
        }

        public static int Sub33From(int len, uint x, uint[] z, int zOff)
        {
            long num = z[zOff] - x;
            z[zOff] = (uint) num;
            num = num >> 0x20;
            num = ((long) ((ulong) num)) + (z[zOff + 1] - 1L);
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, zOff, 2) : 0);
        }

        public static int SubBothFrom(int len, uint[] x, uint[] y, uint[] z)
        {
            long num = 0L;
            for (int i = 0; i < len; i++)
            {
                num = (long) (((ulong) num) + ((z[i] - x[i]) - y[i]));
                z[i] = (uint) num;
                num = num >> 0x20;
            }
            return (int) num;
        }

        public static int SubBothFrom(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
        {
            long num = 0L;
            for (int i = 0; i < len; i++)
            {
                num = (long) (((ulong) num) + ((z[zOff + i] - x[xOff + i]) - y[yOff + i]));
                z[zOff + i] = (uint) num;
                num = num >> 0x20;
            }
            return (int) num;
        }

        public static int SubDWordAt(int len, ulong x, uint[] z, int zPos)
        {
            long num = (long) (z[zPos] - (x & 0xffffffffL));
            z[zPos] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[zPos + 1] - (x >> 0x20)));
            z[zPos + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, zPos + 2) : 0);
        }

        public static int SubDWordAt(int len, ulong x, uint[] z, int zOff, int zPos)
        {
            long num = (long) (z[zOff + zPos] - (x & 0xffffffffL));
            z[zOff + zPos] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[(zOff + zPos) + 1] - (x >> 0x20)));
            z[(zOff + zPos) + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, zOff, zPos + 2) : 0);
        }

        public static int SubDWordFrom(int len, ulong x, uint[] z)
        {
            long num = (long) (z[0] - (x & 0xffffffffL));
            z[0] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[1] - (x >> 0x20)));
            z[1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, 2) : 0);
        }

        public static int SubDWordFrom(int len, ulong x, uint[] z, int zOff)
        {
            long num = (long) (z[zOff] - (x & 0xffffffffL));
            z[zOff] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[zOff + 1] - (x >> 0x20)));
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, zOff, 2) : 0);
        }

        public static int SubFrom(int len, uint[] x, uint[] z)
        {
            long num = 0L;
            for (int i = 0; i < len; i++)
            {
                num = (long) (((ulong) num) + (z[i] - x[i]));
                z[i] = (uint) num;
                num = num >> 0x20;
            }
            return (int) num;
        }

        public static int SubFrom(int len, uint[] x, int xOff, uint[] z, int zOff)
        {
            long num = 0L;
            for (int i = 0; i < len; i++)
            {
                num = (long) (((ulong) num) + (z[zOff + i] - x[xOff + i]));
                z[zOff + i] = (uint) num;
                num = num >> 0x20;
            }
            return (int) num;
        }

        public static int SubWordAt(int len, uint x, uint[] z, int zPos)
        {
            long num = z[zPos] - x;
            z[zPos] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, zPos + 1) : 0);
        }

        public static int SubWordAt(int len, uint x, uint[] z, int zOff, int zPos)
        {
            long num = z[zOff + zPos] - x;
            z[zOff + zPos] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, zOff, zPos + 1) : 0);
        }

        public static int SubWordFrom(int len, uint x, uint[] z)
        {
            long num = z[0] - x;
            z[0] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, 1) : 0);
        }

        public static int SubWordFrom(int len, uint x, uint[] z, int zOff)
        {
            long num = z[zOff] - x;
            z[zOff] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? DecAt(len, z, zOff, 1) : 0);
        }

        public static BigInteger ToBigInteger(int len, uint[] x)
        {
            byte[] bs = new byte[len << 2];
            for (int i = 0; i < len; i++)
            {
                uint n = x[i];
                if (n != 0)
                {
                    Pack.UInt32_To_BE(n, bs, ((len - 1) - i) << 2);
                }
            }
            return new BigInteger(1, bs);
        }

        public static void Zero(int len, uint[] z)
        {
            for (int i = 0; i < len; i++)
            {
                z[i] = 0;
            }
        }
    }
}

