namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    using Org.BouncyCastle.Crypto.Utilities;
    using System;

    internal abstract class GcmUtilities
    {
        private const uint E1 = 0xe1000000;
        private const ulong E1L = 16_212_958_658_533_785_600L;
        private static readonly uint[] LOOKUP = GenerateLookup();

        protected GcmUtilities()
        {
        }

        internal static byte[] AsBytes(uint[] x) => 
            Pack.UInt32_To_BE(x);

        internal static byte[] AsBytes(ulong[] x)
        {
            byte[] bs = new byte[0x10];
            Pack.UInt64_To_BE(x, bs, 0);
            return bs;
        }

        internal static void AsBytes(uint[] x, byte[] z)
        {
            Pack.UInt32_To_BE(x, z, 0);
        }

        internal static void AsBytes(ulong[] x, byte[] z)
        {
            Pack.UInt64_To_BE(x, z, 0);
        }

        internal static uint[] AsUints(byte[] bs)
        {
            uint[] ns = new uint[4];
            Pack.BE_To_UInt32(bs, 0, ns);
            return ns;
        }

        internal static void AsUints(byte[] bs, uint[] output)
        {
            Pack.BE_To_UInt32(bs, 0, output);
        }

        internal static ulong[] AsUlongs(byte[] x)
        {
            ulong[] ns = new ulong[2];
            Pack.BE_To_UInt64(x, 0, ns);
            return ns;
        }

        public static void AsUlongs(byte[] x, ulong[] z)
        {
            Pack.BE_To_UInt64(x, 0, z);
        }

        private static uint[] GenerateLookup()
        {
            uint[] numArray = new uint[0x100];
            for (int i = 0; i < 0x100; i++)
            {
                uint num2 = 0;
                for (int j = 7; j >= 0; j--)
                {
                    if ((i & (((int) 1) << j)) != 0)
                    {
                        num2 ^= ((uint) (-520093696)) >> (7 - j);
                    }
                }
                numArray[i] = num2;
            }
            return numArray;
        }

        internal static void Multiply(byte[] x, byte[] y)
        {
            uint[] numArray = AsUints(x);
            uint[] numArray2 = AsUints(y);
            Multiply(numArray, numArray2);
            AsBytes(numArray, x);
        }

        internal static void Multiply(uint[] x, uint[] y)
        {
            uint num = x[0];
            uint num2 = x[1];
            uint num3 = x[2];
            uint num4 = x[3];
            uint num5 = 0;
            uint num6 = 0;
            uint num7 = 0;
            uint num8 = 0;
            for (int i = 0; i < 4; i++)
            {
                int num10 = (int) y[i];
                for (int j = 0; j < 0x20; j++)
                {
                    uint num12 = (uint) (num10 >> 0x1f);
                    num10 = num10 << 1;
                    num5 ^= num & num12;
                    num6 ^= num2 & num12;
                    num7 ^= num3 & num12;
                    num8 ^= num4 & num12;
                    uint num13 = (num4 << 0x1f) >> 8;
                    num4 = (num4 >> 1) | (num3 << 0x1f);
                    num3 = (num3 >> 1) | (num2 << 0x1f);
                    num2 = (num2 >> 1) | (num << 0x1f);
                    num = (num >> 1) ^ (num13 & 0xe1000000);
                }
            }
            x[0] = num5;
            x[1] = num6;
            x[2] = num7;
            x[3] = num8;
        }

        internal static void Multiply(ulong[] x, ulong[] y)
        {
            ulong num = x[0];
            ulong num2 = x[1];
            ulong num3 = 0L;
            ulong num4 = 0L;
            for (int i = 0; i < 2; i++)
            {
                long num6 = (long) y[i];
                for (int j = 0; j < 0x40; j++)
                {
                    ulong num8 = (ulong) (num6 >> 0x3f);
                    num6 = num6 << 1;
                    num3 ^= num & num8;
                    num4 ^= num2 & num8;
                    ulong num9 = (num2 << 0x3f) >> 8;
                    num2 = (num2 >> 1) | (num << 0x3f);
                    num = (num >> 1) ^ (num9 & 16_212_958_658_533_785_600L);
                }
            }
            x[0] = num3;
            x[1] = num4;
        }

        internal static void MultiplyP(uint[] x)
        {
            uint num = ShiftRight(x) >> 8;
            x[0] ^= num & 0xe1000000;
        }

        internal static void MultiplyP(uint[] x, uint[] z)
        {
            uint num = ShiftRight(x, z) >> 8;
            z[0] ^= num & 0xe1000000;
        }

        internal static void MultiplyP8(uint[] x)
        {
            uint num = ShiftRightN(x, 8);
            x[0] ^= LOOKUP[num >> 0x18];
        }

        internal static void MultiplyP8(uint[] x, uint[] y)
        {
            uint num = ShiftRightN(x, 8, y);
            y[0] ^= LOOKUP[num >> 0x18];
        }

        internal static byte[] OneAsBytes()
        {
            byte[] buffer = new byte[0x10];
            buffer[0] = 0x80;
            return buffer;
        }

        internal static uint[] OneAsUints()
        {
            uint[] numArray = new uint[4];
            numArray[0] = 0x80000000;
            return numArray;
        }

        internal static ulong[] OneAsUlongs()
        {
            ulong[] numArray = new ulong[2];
            numArray[0] = 9_223_372_036_854_775_808L;
            return numArray;
        }

        internal static uint ShiftRight(uint[] x)
        {
            uint num = x[0];
            x[0] = num >> 1;
            uint num2 = num << 0x1f;
            num = x[1];
            x[1] = (num >> 1) | num2;
            num2 = num << 0x1f;
            num = x[2];
            x[2] = (num >> 1) | num2;
            num2 = num << 0x1f;
            num = x[3];
            x[3] = (num >> 1) | num2;
            return (num << 0x1f);
        }

        internal static uint ShiftRight(uint[] x, uint[] z)
        {
            uint num = x[0];
            z[0] = num >> 1;
            uint num2 = num << 0x1f;
            num = x[1];
            z[1] = (num >> 1) | num2;
            num2 = num << 0x1f;
            num = x[2];
            z[2] = (num >> 1) | num2;
            num2 = num << 0x1f;
            num = x[3];
            z[3] = (num >> 1) | num2;
            return (num << 0x1f);
        }

        internal static uint ShiftRightN(uint[] x, int n)
        {
            uint num = x[0];
            int num2 = 0x20 - n;
            x[0] = num >> n;
            uint num3 = num << num2;
            num = x[1];
            x[1] = (num >> n) | num3;
            num3 = num << num2;
            num = x[2];
            x[2] = (num >> n) | num3;
            num3 = num << num2;
            num = x[3];
            x[3] = (num >> n) | num3;
            return (num << num2);
        }

        internal static uint ShiftRightN(uint[] x, int n, uint[] z)
        {
            uint num = x[0];
            int num2 = 0x20 - n;
            z[0] = num >> n;
            uint num3 = num << num2;
            num = x[1];
            z[1] = (num >> n) | num3;
            num3 = num << num2;
            num = x[2];
            z[2] = (num >> n) | num3;
            num3 = num << num2;
            num = x[3];
            z[3] = (num >> n) | num3;
            return (num << num2);
        }

        internal static void Xor(byte[] x, byte[] y)
        {
            int index = 0;
            do
            {
                x[index] = (byte) (x[index] ^ y[index]);
                index++;
                x[index] = (byte) (x[index] ^ y[index]);
                index++;
                x[index] = (byte) (x[index] ^ y[index]);
                index++;
                x[index] = (byte) (x[index] ^ y[index]);
                index++;
            }
            while (index < 0x10);
        }

        internal static void Xor(uint[] x, uint[] y)
        {
            x[0] ^= y[0];
            x[1] ^= y[1];
            x[2] ^= y[2];
            x[3] ^= y[3];
        }

        internal static void Xor(ulong[] x, ulong[] y)
        {
            x[0] ^= y[0];
            x[1] ^= y[1];
        }

        internal static void Xor(byte[] x, byte[] y, byte[] z)
        {
            int index = 0;
            do
            {
                z[index] = (byte) (x[index] ^ y[index]);
                index++;
                z[index] = (byte) (x[index] ^ y[index]);
                index++;
                z[index] = (byte) (x[index] ^ y[index]);
                index++;
                z[index] = (byte) (x[index] ^ y[index]);
                index++;
            }
            while (index < 0x10);
        }

        internal static void Xor(uint[] x, uint[] y, uint[] z)
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
            z[2] = x[2] ^ y[2];
            z[3] = x[3] ^ y[3];
        }

        internal static void Xor(ulong[] x, ulong[] y, ulong[] z)
        {
            z[0] = x[0] ^ y[0];
            z[1] = x[1] ^ y[1];
        }

        internal static void Xor(byte[] x, byte[] y, int yOff, int yLen)
        {
            while (--yLen >= 0)
            {
                x[yLen] = (byte) (x[yLen] ^ y[yOff + yLen]);
            }
        }
    }
}

