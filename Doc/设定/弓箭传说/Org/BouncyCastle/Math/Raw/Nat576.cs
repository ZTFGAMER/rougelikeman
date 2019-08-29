namespace Org.BouncyCastle.Math.Raw
{
    using Org.BouncyCastle.Crypto.Utilities;
    using Org.BouncyCastle.Math;
    using System;

    internal abstract class Nat576
    {
        protected Nat576()
        {
        }

        public static void Copy64(ulong[] x, ulong[] z)
        {
            z[0] = x[0];
            z[1] = x[1];
            z[2] = x[2];
            z[3] = x[3];
            z[4] = x[4];
            z[5] = x[5];
            z[6] = x[6];
            z[7] = x[7];
            z[8] = x[8];
        }

        public static ulong[] Create64() => 
            new ulong[9];

        public static ulong[] CreateExt64() => 
            new ulong[0x12];

        public static bool Eq64(ulong[] x, ulong[] y)
        {
            for (int i = 8; i >= 0; i--)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static ulong[] FromBigInteger64(BigInteger x)
        {
            if ((x.SignValue < 0) || (x.BitLength > 0x240))
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

        public static bool IsOne64(ulong[] x)
        {
            if (x[0] != 1L)
            {
                return false;
            }
            for (int i = 1; i < 9; i++)
            {
                if (x[i] != 0L)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsZero64(ulong[] x)
        {
            for (int i = 0; i < 9; i++)
            {
                if (x[i] != 0L)
                {
                    return false;
                }
            }
            return true;
        }

        public static BigInteger ToBigInteger64(ulong[] x)
        {
            byte[] bs = new byte[0x48];
            for (int i = 0; i < 9; i++)
            {
                ulong n = x[i];
                if (n != 0L)
                {
                    Pack.UInt64_To_BE(n, bs, (8 - i) << 3);
                }
            }
            return new BigInteger(1, bs);
        }
    }
}

