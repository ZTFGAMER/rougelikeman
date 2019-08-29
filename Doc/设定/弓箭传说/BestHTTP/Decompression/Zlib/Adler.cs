namespace BestHTTP.Decompression.Zlib
{
    using System;

    public sealed class Adler
    {
        private static readonly uint BASE = 0xfff1;
        private static readonly int NMAX = 0x15b0;

        public static uint Adler32(uint adler, byte[] buf, int index, int len)
        {
            if (buf == null)
            {
                return 1;
            }
            uint num = adler & 0xffff;
            uint num2 = (adler >> 0x10) & 0xffff;
            while (len > 0)
            {
                int num3 = (len >= NMAX) ? NMAX : len;
                len -= num3;
                while (num3 >= 0x10)
                {
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num += buf[index++];
                    num2 += num;
                    num3 -= 0x10;
                }
                if (num3 != 0)
                {
                    do
                    {
                        num += buf[index++];
                        num2 += num;
                    }
                    while (--num3 != 0);
                }
                num = num % BASE;
                num2 = num2 % BASE;
            }
            return ((num2 << 0x10) | num);
        }
    }
}

