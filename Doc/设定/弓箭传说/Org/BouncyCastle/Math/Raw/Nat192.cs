namespace Org.BouncyCastle.Math.Raw
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Math;
    using System;

    internal abstract class Nat192
    {
        private const ulong M = 0xffffffffL;

        protected Nat192()
        {
        }

        public static uint Add(uint[] x, uint[] y, uint[] z)
        {
            ulong num = 0L;
            num += x[0] + y[0];
            z[0] = (uint) num;
            num = num >> 0x20;
            num += x[1] + y[1];
            z[1] = (uint) num;
            num = num >> 0x20;
            num += x[2] + y[2];
            z[2] = (uint) num;
            num = num >> 0x20;
            num += x[3] + y[3];
            z[3] = (uint) num;
            num = num >> 0x20;
            num += x[4] + y[4];
            z[4] = (uint) num;
            num = num >> 0x20;
            num += x[5] + y[5];
            z[5] = (uint) num;
            num = num >> 0x20;
            return (uint) num;
        }

        public static uint AddBothTo(uint[] x, uint[] y, uint[] z)
        {
            ulong num = 0L;
            num += (x[0] + y[0]) + z[0];
            z[0] = (uint) num;
            num = num >> 0x20;
            num += (x[1] + y[1]) + z[1];
            z[1] = (uint) num;
            num = num >> 0x20;
            num += (x[2] + y[2]) + z[2];
            z[2] = (uint) num;
            num = num >> 0x20;
            num += (x[3] + y[3]) + z[3];
            z[3] = (uint) num;
            num = num >> 0x20;
            num += (x[4] + y[4]) + z[4];
            z[4] = (uint) num;
            num = num >> 0x20;
            num += (x[5] + y[5]) + z[5];
            z[5] = (uint) num;
            num = num >> 0x20;
            return (uint) num;
        }

        public static uint AddTo(uint[] x, uint[] z)
        {
            ulong num = 0L;
            num += x[0] + z[0];
            z[0] = (uint) num;
            num = num >> 0x20;
            num += x[1] + z[1];
            z[1] = (uint) num;
            num = num >> 0x20;
            num += x[2] + z[2];
            z[2] = (uint) num;
            num = num >> 0x20;
            num += x[3] + z[3];
            z[3] = (uint) num;
            num = num >> 0x20;
            num += x[4] + z[4];
            z[4] = (uint) num;
            num = num >> 0x20;
            num += x[5] + z[5];
            z[5] = (uint) num;
            num = num >> 0x20;
            return (uint) num;
        }

        public static uint AddTo(uint[] x, int xOff, uint[] z, int zOff, uint cIn)
        {
            ulong num = cIn;
            num += x[xOff] + z[zOff];
            z[zOff] = (uint) num;
            num = num >> 0x20;
            num += x[xOff + 1] + z[zOff + 1];
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            num += x[xOff + 2] + z[zOff + 2];
            z[zOff + 2] = (uint) num;
            num = num >> 0x20;
            num += x[xOff + 3] + z[zOff + 3];
            z[zOff + 3] = (uint) num;
            num = num >> 0x20;
            num += x[xOff + 4] + z[zOff + 4];
            z[zOff + 4] = (uint) num;
            num = num >> 0x20;
            num += x[xOff + 5] + z[zOff + 5];
            z[zOff + 5] = (uint) num;
            num = num >> 0x20;
            return (uint) num;
        }

        public static uint AddToEachOther(uint[] u, int uOff, uint[] v, int vOff)
        {
            ulong num = 0L;
            num += u[uOff] + v[vOff];
            u[uOff] = (uint) num;
            v[vOff] = (uint) num;
            num = num >> 0x20;
            num += u[uOff + 1] + v[vOff + 1];
            u[uOff + 1] = (uint) num;
            v[vOff + 1] = (uint) num;
            num = num >> 0x20;
            num += u[uOff + 2] + v[vOff + 2];
            u[uOff + 2] = (uint) num;
            v[vOff + 2] = (uint) num;
            num = num >> 0x20;
            num += u[uOff + 3] + v[vOff + 3];
            u[uOff + 3] = (uint) num;
            v[vOff + 3] = (uint) num;
            num = num >> 0x20;
            num += u[uOff + 4] + v[vOff + 4];
            u[uOff + 4] = (uint) num;
            v[vOff + 4] = (uint) num;
            num = num >> 0x20;
            num += u[uOff + 5] + v[vOff + 5];
            u[uOff + 5] = (uint) num;
            v[vOff + 5] = (uint) num;
            num = num >> 0x20;
            return (uint) num;
        }

        public static void Copy(uint[] x, uint[] z)
        {
            z[0] = x[0];
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
            z[4] = x[4];
            z[5] = x[5];
        }

        public static void Copy64(ulong[] x, ulong[] z)
        {
            z[0] = x[0];
            z[1] = x[1];
            z[2] = x[2];
        }

        public static uint[] Create() => 
            new uint[6];

        public static ulong[] Create64() => 
            new ulong[3];

        public static uint[] CreateExt() => 
            new uint[12];

        public static ulong[] CreateExt64() => 
            new ulong[6];

        public static bool Diff(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
        {
            bool flag = Gte(x, xOff, y, yOff);
            if (flag)
            {
                Sub(x, xOff, y, yOff, z, zOff);
                return flag;
            }
            Sub(y, yOff, x, xOff, z, zOff);
            return flag;
        }

        public static bool Eq(uint[] x, uint[] y)
        {
            for (int i = 5; i >= 0; i--)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Eq64(ulong[] x, ulong[] y)
        {
            for (int i = 2; i >= 0; i--)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static uint[] FromBigInteger(BigInteger x)
        {
            if ((x.SignValue < 0) || (x.BitLength > 0xc0))
            {
                throw new ArgumentException();
            }
            uint[] numArray = Create();
            int num = 0;
            while (x.SignValue != 0)
            {
                numArray[num++] = (uint) x.IntValue;
                x = x.ShiftRight(0x20);
            }
            return numArray;
        }

        public static ulong[] FromBigInteger64(BigInteger x)
        {
            if ((x.SignValue < 0) || (x.BitLength > 0xc0))
            {
                throw new ArgumentException();
            }
            ulong[] numArray = Create64();
            int num = 0;
            while (x.SignValue != 0)
            {
                numArray[num++] = (ulong) x.LongValue;
                x = x.ShiftRight(0x40);
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
            if ((index < 0) || (index >= 6))
            {
                return 0;
            }
            int num2 = bit & 0x1f;
            return ((x[index] >> num2) & 1);
        }

        public static bool Gte(uint[] x, uint[] y)
        {
            for (int i = 5; i >= 0; i--)
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

        public static bool Gte(uint[] x, int xOff, uint[] y, int yOff)
        {
            for (int i = 5; i >= 0; i--)
            {
                uint num2 = x[xOff + i];
                uint num3 = y[yOff + i];
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

        public static bool IsOne(uint[] x)
        {
            if (x[0] != 1)
            {
                return false;
            }
            for (int i = 1; i < 6; i++)
            {
                if (x[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsOne64(ulong[] x)
        {
            if (x[0] != 1L)
            {
                return false;
            }
            for (int i = 1; i < 3; i++)
            {
                if (x[i] != 0L)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsZero(uint[] x)
        {
            for (int i = 0; i < 6; i++)
            {
                if (x[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsZero64(ulong[] x)
        {
            for (int i = 0; i < 3; i++)
            {
                if (x[i] != 0L)
                {
                    return false;
                }
            }
            return true;
        }

        public static void Mul(uint[] x, uint[] y, uint[] zz)
        {
            ulong num = y[0];
            ulong num2 = y[1];
            ulong num3 = y[2];
            ulong num4 = y[3];
            ulong num5 = y[4];
            ulong num6 = y[5];
            ulong num7 = 0L;
            ulong num8 = x[0];
            num7 += num8 * num;
            zz[0] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num2;
            zz[1] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num3;
            zz[2] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num4;
            zz[3] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num5;
            zz[4] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num6;
            zz[5] = (uint) num7;
            num7 = num7 >> 0x20;
            zz[6] = (uint) num7;
            for (int i = 1; i < 6; i++)
            {
                ulong num10 = 0L;
                ulong num11 = x[i];
                num10 += (num11 * num) + zz[i];
                zz[i] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num2) + zz[i + 1];
                zz[i + 1] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num3) + zz[i + 2];
                zz[i + 2] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num4) + zz[i + 3];
                zz[i + 3] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num5) + zz[i + 4];
                zz[i + 4] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num6) + zz[i + 5];
                zz[i + 5] = (uint) num10;
                num10 = num10 >> 0x20;
                zz[i + 6] = (uint) num10;
            }
        }

        public static void Mul(uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
        {
            ulong num = y[yOff];
            ulong num2 = y[yOff + 1];
            ulong num3 = y[yOff + 2];
            ulong num4 = y[yOff + 3];
            ulong num5 = y[yOff + 4];
            ulong num6 = y[yOff + 5];
            ulong num7 = 0L;
            ulong num8 = x[xOff];
            num7 += num8 * num;
            zz[zzOff] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num2;
            zz[zzOff + 1] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num3;
            zz[zzOff + 2] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num4;
            zz[zzOff + 3] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num5;
            zz[zzOff + 4] = (uint) num7;
            num7 = num7 >> 0x20;
            num7 += num8 * num6;
            zz[zzOff + 5] = (uint) num7;
            num7 = num7 >> 0x20;
            zz[zzOff + 6] = (uint) num7;
            for (int i = 1; i < 6; i++)
            {
                zzOff++;
                ulong num10 = 0L;
                ulong num11 = x[xOff + i];
                num10 += (num11 * num) + zz[zzOff];
                zz[zzOff] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num2) + zz[zzOff + 1];
                zz[zzOff + 1] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num3) + zz[zzOff + 2];
                zz[zzOff + 2] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num4) + zz[zzOff + 3];
                zz[zzOff + 3] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num5) + zz[zzOff + 4];
                zz[zzOff + 4] = (uint) num10;
                num10 = num10 >> 0x20;
                num10 += (num11 * num6) + zz[zzOff + 5];
                zz[zzOff + 5] = (uint) num10;
                num10 = num10 >> 0x20;
                zz[zzOff + 6] = (uint) num10;
            }
        }

        public static ulong Mul33Add(uint w, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
        {
            ulong num = 0L;
            ulong num2 = w;
            ulong num3 = x[xOff];
            num += (num2 * num3) + y[yOff];
            z[zOff] = (uint) num;
            num = num >> 0x20;
            ulong num4 = x[xOff + 1];
            num += ((num2 * num4) + num3) + y[yOff + 1];
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            ulong num5 = x[xOff + 2];
            num += ((num2 * num5) + num4) + y[yOff + 2];
            z[zOff + 2] = (uint) num;
            num = num >> 0x20;
            ulong num6 = x[xOff + 3];
            num += ((num2 * num6) + num5) + y[yOff + 3];
            z[zOff + 3] = (uint) num;
            num = num >> 0x20;
            ulong num7 = x[xOff + 4];
            num += ((num2 * num7) + num6) + y[yOff + 4];
            z[zOff + 4] = (uint) num;
            num = num >> 0x20;
            ulong num8 = x[xOff + 5];
            num += ((num2 * num8) + num7) + y[yOff + 5];
            z[zOff + 5] = (uint) num;
            num = num >> 0x20;
            return (num + num8);
        }

        public static uint Mul33DWordAdd(uint x, ulong y, uint[] z, int zOff)
        {
            ulong num = 0L;
            ulong num2 = x;
            ulong num3 = y & 0xffffffffL;
            num += (num2 * num3) + z[zOff];
            z[zOff] = (uint) num;
            num = num >> 0x20;
            ulong num4 = y >> 0x20;
            num += ((num2 * num4) + num3) + z[zOff + 1];
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            num += num4 + z[zOff + 2];
            z[zOff + 2] = (uint) num;
            num = num >> 0x20;
            num += z[zOff + 3];
            z[zOff + 3] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? Nat.IncAt(6, z, zOff, 4) : 0);
        }

        public static uint Mul33WordAdd(uint x, uint y, uint[] z, int zOff)
        {
            ulong num = 0L;
            ulong num2 = y;
            num += (num2 * x) + z[zOff];
            z[zOff] = (uint) num;
            num = num >> 0x20;
            num += num2 + z[zOff + 1];
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            num += z[zOff + 2];
            z[zOff + 2] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? Nat.IncAt(6, z, zOff, 3) : 0);
        }

        public static uint MulAddTo(uint[] x, uint[] y, uint[] zz)
        {
            ulong num = y[0];
            ulong num2 = y[1];
            ulong num3 = y[2];
            ulong num4 = y[3];
            ulong num5 = y[4];
            ulong num6 = y[5];
            ulong num7 = 0L;
            for (int i = 0; i < 6; i++)
            {
                ulong num9 = 0L;
                ulong num10 = x[i];
                num9 += (num10 * num) + zz[i];
                zz[i] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num2) + zz[i + 1];
                zz[i + 1] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num3) + zz[i + 2];
                zz[i + 2] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num4) + zz[i + 3];
                zz[i + 3] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num5) + zz[i + 4];
                zz[i + 4] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num6) + zz[i + 5];
                zz[i + 5] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += num7 + zz[i + 6];
                zz[i + 6] = (uint) num9;
                num7 = num9 >> 0x20;
            }
            return (uint) num7;
        }

        public static uint MulAddTo(uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
        {
            ulong num = y[yOff];
            ulong num2 = y[yOff + 1];
            ulong num3 = y[yOff + 2];
            ulong num4 = y[yOff + 3];
            ulong num5 = y[yOff + 4];
            ulong num6 = y[yOff + 5];
            ulong num7 = 0L;
            for (int i = 0; i < 6; i++)
            {
                ulong num9 = 0L;
                ulong num10 = x[xOff + i];
                num9 += (num10 * num) + zz[zzOff];
                zz[zzOff] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num2) + zz[zzOff + 1];
                zz[zzOff + 1] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num3) + zz[zzOff + 2];
                zz[zzOff + 2] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num4) + zz[zzOff + 3];
                zz[zzOff + 3] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num5) + zz[zzOff + 4];
                zz[zzOff + 4] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += (num10 * num6) + zz[zzOff + 5];
                zz[zzOff + 5] = (uint) num9;
                num9 = num9 >> 0x20;
                num9 += num7 + zz[zzOff + 6];
                zz[zzOff + 6] = (uint) num9;
                num7 = num9 >> 0x20;
                zzOff++;
            }
            return (uint) num7;
        }

        public static uint MulWord(uint x, uint[] y, uint[] z, int zOff)
        {
            ulong num = 0L;
            ulong num2 = x;
            int index = 0;
            do
            {
                num += num2 * y[index];
                z[zOff + index] = (uint) num;
                num = num >> 0x20;
            }
            while (++index < 6);
            return (uint) num;
        }

        public static uint MulWordAddExt(uint x, uint[] yy, int yyOff, uint[] zz, int zzOff)
        {
            ulong num = 0L;
            ulong num2 = x;
            num += (num2 * yy[yyOff]) + zz[zzOff];
            zz[zzOff] = (uint) num;
            num = num >> 0x20;
            num += (num2 * yy[yyOff + 1]) + zz[zzOff + 1];
            zz[zzOff + 1] = (uint) num;
            num = num >> 0x20;
            num += (num2 * yy[yyOff + 2]) + zz[zzOff + 2];
            zz[zzOff + 2] = (uint) num;
            num = num >> 0x20;
            num += (num2 * yy[yyOff + 3]) + zz[zzOff + 3];
            zz[zzOff + 3] = (uint) num;
            num = num >> 0x20;
            num += (num2 * yy[yyOff + 4]) + zz[zzOff + 4];
            zz[zzOff + 4] = (uint) num;
            num = num >> 0x20;
            num += (num2 * yy[yyOff + 5]) + zz[zzOff + 5];
            zz[zzOff + 5] = (uint) num;
            num = num >> 0x20;
            return (uint) num;
        }

        public static uint MulWordDwordAdd(uint x, ulong y, uint[] z, int zOff)
        {
            ulong num = 0L;
            ulong num2 = x;
            num += (num2 * y) + z[zOff];
            z[zOff] = (uint) num;
            num = num >> 0x20;
            num += (num2 * (y >> 0x20)) + z[zOff + 1];
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            num += z[zOff + 2];
            z[zOff + 2] = (uint) num;
            num = num >> 0x20;
            return ((num != 0L) ? Nat.IncAt(6, z, zOff, 3) : 0);
        }

        public static void Square(uint[] x, uint[] zz)
        {
            ulong num = x[0];
            uint num3 = 0;
            int num5 = 5;
            int num6 = 12;
            do
            {
                ulong num7 = x[num5--];
                ulong num8 = num7 * num7;
                zz[--num6] = (num3 << 0x1f) | ((uint) (num8 >> 0x21));
                zz[--num6] = (uint) (num8 >> 1);
                num3 = (uint) num8;
            }
            while (num5 > 0);
            ulong num9 = num * num;
            ulong num2 = (num3 << 0x1f) | (num9 >> 0x21);
            zz[0] = (uint) num9;
            num3 = ((uint) (num9 >> 0x20)) & 1;
            ulong num10 = x[1];
            ulong num11 = zz[2];
            num2 += num10 * num;
            uint num4 = (uint) num2;
            zz[1] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num11 += num2 >> 0x20;
            ulong num12 = x[2];
            ulong num13 = zz[3];
            ulong num14 = zz[4];
            num11 += num12 * num;
            num4 = (uint) num11;
            zz[2] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num13 += (num11 >> 0x20) + (num12 * num10);
            num14 += num13 >> 0x20;
            num13 &= 0xffffffffL;
            ulong num15 = x[3];
            ulong num16 = zz[5];
            ulong num17 = zz[6];
            num13 += num15 * num;
            num4 = (uint) num13;
            zz[3] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num14 += (num13 >> 0x20) + (num15 * num10);
            num16 += (num14 >> 0x20) + (num15 * num12);
            num14 &= 0xffffffffL;
            num17 += num16 >> 0x20;
            num16 &= 0xffffffffL;
            ulong num18 = x[4];
            ulong num19 = zz[7];
            ulong num20 = zz[8];
            num14 += num18 * num;
            num4 = (uint) num14;
            zz[4] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num16 += (num14 >> 0x20) + (num18 * num10);
            num17 += (num16 >> 0x20) + (num18 * num12);
            num16 &= 0xffffffffL;
            num19 += (num17 >> 0x20) + (num18 * num15);
            num17 &= 0xffffffffL;
            num20 += num19 >> 0x20;
            num19 &= 0xffffffffL;
            ulong num21 = x[5];
            ulong num22 = zz[9];
            ulong num23 = zz[10];
            num16 += num21 * num;
            num4 = (uint) num16;
            zz[5] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num17 += (num16 >> 0x20) + (num21 * num10);
            num19 += (num17 >> 0x20) + (num21 * num12);
            num20 += (num19 >> 0x20) + (num21 * num15);
            num22 += (num20 >> 0x20) + (num21 * num18);
            num23 += num22 >> 0x20;
            num4 = (uint) num17;
            zz[6] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = (uint) num19;
            zz[7] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = (uint) num20;
            zz[8] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = (uint) num22;
            zz[9] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = (uint) num23;
            zz[10] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = zz[11] + ((uint) (num23 >> 0x20));
            zz[11] = (num4 << 1) | num3;
        }

        public static void Square(uint[] x, int xOff, uint[] zz, int zzOff)
        {
            ulong num = x[xOff];
            uint num3 = 0;
            int num5 = 5;
            int num6 = 12;
            do
            {
                ulong num7 = x[xOff + num5--];
                ulong num8 = num7 * num7;
                zz[zzOff + --num6] = (num3 << 0x1f) | ((uint) (num8 >> 0x21));
                zz[zzOff + --num6] = (uint) (num8 >> 1);
                num3 = (uint) num8;
            }
            while (num5 > 0);
            ulong num9 = num * num;
            ulong num2 = (num3 << 0x1f) | (num9 >> 0x21);
            zz[zzOff] = (uint) num9;
            num3 = ((uint) (num9 >> 0x20)) & 1;
            ulong num10 = x[xOff + 1];
            ulong num11 = zz[zzOff + 2];
            num2 += num10 * num;
            uint num4 = (uint) num2;
            zz[zzOff + 1] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num11 += num2 >> 0x20;
            ulong num12 = x[xOff + 2];
            ulong num13 = zz[zzOff + 3];
            ulong num14 = zz[zzOff + 4];
            num11 += num12 * num;
            num4 = (uint) num11;
            zz[zzOff + 2] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num13 += (num11 >> 0x20) + (num12 * num10);
            num14 += num13 >> 0x20;
            num13 &= 0xffffffffL;
            ulong num15 = x[xOff + 3];
            ulong num16 = zz[zzOff + 5];
            ulong num17 = zz[zzOff + 6];
            num13 += num15 * num;
            num4 = (uint) num13;
            zz[zzOff + 3] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num14 += (num13 >> 0x20) + (num15 * num10);
            num16 += (num14 >> 0x20) + (num15 * num12);
            num14 &= 0xffffffffL;
            num17 += num16 >> 0x20;
            num16 &= 0xffffffffL;
            ulong num18 = x[xOff + 4];
            ulong num19 = zz[zzOff + 7];
            ulong num20 = zz[zzOff + 8];
            num14 += num18 * num;
            num4 = (uint) num14;
            zz[zzOff + 4] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num16 += (num14 >> 0x20) + (num18 * num10);
            num17 += (num16 >> 0x20) + (num18 * num12);
            num16 &= 0xffffffffL;
            num19 += (num17 >> 0x20) + (num18 * num15);
            num17 &= 0xffffffffL;
            num20 += num19 >> 0x20;
            num19 &= 0xffffffffL;
            ulong num21 = x[xOff + 5];
            ulong num22 = zz[zzOff + 9];
            ulong num23 = zz[zzOff + 10];
            num16 += num21 * num;
            num4 = (uint) num16;
            zz[zzOff + 5] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num17 += (num16 >> 0x20) + (num21 * num10);
            num19 += (num17 >> 0x20) + (num21 * num12);
            num20 += (num19 >> 0x20) + (num21 * num15);
            num22 += (num20 >> 0x20) + (num21 * num18);
            num23 += num22 >> 0x20;
            num4 = (uint) num17;
            zz[zzOff + 6] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = (uint) num19;
            zz[zzOff + 7] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = (uint) num20;
            zz[zzOff + 8] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = (uint) num22;
            zz[zzOff + 9] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = (uint) num23;
            zz[zzOff + 10] = (num4 << 1) | num3;
            num3 = num4 >> 0x1f;
            num4 = zz[zzOff + 11] + ((uint) (num23 >> 0x20));
            zz[zzOff + 11] = (num4 << 1) | num3;
        }

        public static int Sub(uint[] x, uint[] y, uint[] z)
        {
            long num = 0L;
            num = (long) (((ulong) num) + (x[0] - y[0]));
            z[0] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[1] - y[1]));
            z[1] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[2] - y[2]));
            z[2] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[3] - y[3]));
            z[3] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[4] - y[4]));
            z[4] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[5] - y[5]));
            z[5] = (uint) num;
            num = num >> 0x20;
            return (int) num;
        }

        public static int Sub(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
        {
            long num = 0L;
            num = (long) (((ulong) num) + (x[xOff] - y[yOff]));
            z[zOff] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[xOff + 1] - y[yOff + 1]));
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[xOff + 2] - y[yOff + 2]));
            z[zOff + 2] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[xOff + 3] - y[yOff + 3]));
            z[zOff + 3] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[xOff + 4] - y[yOff + 4]));
            z[zOff + 4] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (x[xOff + 5] - y[yOff + 5]));
            z[zOff + 5] = (uint) num;
            num = num >> 0x20;
            return (int) num;
        }

        public static int SubBothFrom(uint[] x, uint[] y, uint[] z)
        {
            long num = 0L;
            num = (long) (((ulong) num) + ((z[0] - x[0]) - y[0]));
            z[0] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + ((z[1] - x[1]) - y[1]));
            z[1] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + ((z[2] - x[2]) - y[2]));
            z[2] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + ((z[3] - x[3]) - y[3]));
            z[3] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + ((z[4] - x[4]) - y[4]));
            z[4] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + ((z[5] - x[5]) - y[5]));
            z[5] = (uint) num;
            num = num >> 0x20;
            return (int) num;
        }

        public static int SubFrom(uint[] x, uint[] z)
        {
            long num = 0L;
            num = (long) (((ulong) num) + (z[0] - x[0]));
            z[0] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[1] - x[1]));
            z[1] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[2] - x[2]));
            z[2] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[3] - x[3]));
            z[3] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[4] - x[4]));
            z[4] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[5] - x[5]));
            z[5] = (uint) num;
            num = num >> 0x20;
            return (int) num;
        }

        public static int SubFrom(uint[] x, int xOff, uint[] z, int zOff)
        {
            long num = 0L;
            num = (long) (((ulong) num) + (z[zOff] - x[xOff]));
            z[zOff] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[zOff + 1] - x[xOff + 1]));
            z[zOff + 1] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[zOff + 2] - x[xOff + 2]));
            z[zOff + 2] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[zOff + 3] - x[xOff + 3]));
            z[zOff + 3] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[zOff + 4] - x[xOff + 4]));
            z[zOff + 4] = (uint) num;
            num = num >> 0x20;
            num = (long) (((ulong) num) + (z[zOff + 5] - x[xOff + 5]));
            z[zOff + 5] = (uint) num;
            num = num >> 0x20;
            return (int) num;
        }

        public static BigInteger ToBigInteger(uint[] x)
        {
            byte[] bs = new byte[0x18];
            for (int i = 0; i < 6; i++)
            {
                uint n = x[i];
                if (n != 0)
                {
                    Pack.UInt32_To_BE(n, bs, (5 - i) << 2);
                }
            }
            return new BigInteger(1, bs);
        }

        public static BigInteger ToBigInteger64(ulong[] x)
        {
            byte[] bs = new byte[0x18];
            for (int i = 0; i < 3; i++)
            {
                ulong n = x[i];
                if (n != 0L)
                {
                    Pack.UInt64_To_BE(n, bs, (2 - i) << 3);
                }
            }
            return new BigInteger(1, bs);
        }

        public static void Zero(uint[] z)
        {
            z[0] = 0;
            z[1] = 0;
            z[2] = 0;
            z[3] = 0;
            z[4] = 0;
            z[5] = 0;
        }
    }
}

