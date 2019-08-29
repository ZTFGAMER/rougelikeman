namespace Org.BouncyCastle.Math.Raw
{
    using System;

    internal abstract class Interleave
    {
        private const ulong M32 = 0x55555555L;
        private const ulong M64 = 0x5555555555555555L;

        protected Interleave()
        {
        }

        internal static uint Expand16to32(uint x)
        {
            x &= 0xffff;
            x = (x | (x << 8)) & 0xff00ff;
            x = (x | (x << 4)) & 0xf0f0f0f;
            x = (x | (x << 2)) & 0x33333333;
            x = (x | (x << 1)) & 0x55555555;
            return x;
        }

        internal static ulong Expand32to64(uint x)
        {
            uint num = (x ^ (x >> 8)) & 0xff00;
            x ^= num ^ (num << 8);
            num = (x ^ (x >> 4)) & 0xf000f0;
            x ^= num ^ (num << 4);
            num = (x ^ (x >> 2)) & 0xc0c0c0c;
            x ^= num ^ (num << 2);
            num = (x ^ (x >> 1)) & 0x22222222;
            x ^= num ^ (num << 1);
            return (ulong) ((((x >> 1) & 0x55555555L) << 0x20) | (x & 0x55555555L));
        }

        internal static void Expand64To128(ulong x, ulong[] z, int zOff)
        {
            ulong num = (x ^ (x >> 0x10)) & 0xffff0000L;
            x ^= num ^ (num << 0x10);
            num = (x ^ (x >> 8)) & ((ulong) 0xff000000ff00L);
            x ^= num ^ (num << 8);
            num = (x ^ (x >> 4)) & ((ulong) 0xf000f000f000f0L);
            x ^= num ^ (num << 4);
            num = (x ^ (x >> 2)) & ((ulong) 0xc0c0c0c0c0c0c0cL);
            x ^= num ^ (num << 2);
            num = (x ^ (x >> 1)) & ((ulong) 0x2222222222222222L);
            x ^= num ^ (num << 1);
            z[zOff] = x & ((ulong) 0x5555555555555555L);
            z[zOff + 1] = (x >> 1) & ((ulong) 0x5555555555555555L);
        }

        internal static uint Expand8to16(uint x)
        {
            x &= 0xff;
            x = (x | (x << 4)) & 0xf0f;
            x = (x | (x << 2)) & 0x3333;
            x = (x | (x << 1)) & 0x5555;
            return x;
        }

        internal static ulong Unshuffle(ulong x)
        {
            ulong num = (x ^ (x >> 1)) & ((ulong) 0x2222222222222222L);
            x ^= num ^ (num << 1);
            num = (x ^ (x >> 2)) & ((ulong) 0xc0c0c0c0c0c0c0cL);
            x ^= num ^ (num << 2);
            num = (x ^ (x >> 4)) & ((ulong) 0xf000f000f000f0L);
            x ^= num ^ (num << 4);
            num = (x ^ (x >> 8)) & ((ulong) 0xff000000ff00L);
            x ^= num ^ (num << 8);
            num = (x ^ (x >> 0x10)) & 0xffff0000L;
            x ^= num ^ (num << 0x10);
            return x;
        }
    }
}

