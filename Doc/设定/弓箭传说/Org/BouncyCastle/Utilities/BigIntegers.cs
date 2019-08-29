namespace Org.BouncyCastle.Utilities
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;

    public abstract class BigIntegers
    {
        private const int MaxIterations = 0x3e8;

        protected BigIntegers()
        {
        }

        public static byte[] AsUnsignedByteArray(BigInteger n) => 
            n.ToByteArrayUnsigned();

        public static byte[] AsUnsignedByteArray(int length, BigInteger n)
        {
            byte[] sourceArray = n.ToByteArrayUnsigned();
            if (sourceArray.Length > length)
            {
                throw new ArgumentException("standard length exceeded", "n");
            }
            if (sourceArray.Length == length)
            {
                return sourceArray;
            }
            byte[] destinationArray = new byte[length];
            Array.Copy(sourceArray, 0, destinationArray, destinationArray.Length - sourceArray.Length, sourceArray.Length);
            return destinationArray;
        }

        public static BigInteger CreateRandomInRange(BigInteger min, BigInteger max, SecureRandom random)
        {
            int num = min.CompareTo(max);
            if (num >= 0)
            {
                if (num > 0)
                {
                    throw new ArgumentException("'min' may not be greater than 'max'");
                }
                return min;
            }
            if (min.BitLength > (max.BitLength / 2))
            {
                return CreateRandomInRange(BigInteger.Zero, max.Subtract(min), random).Add(min);
            }
            for (int i = 0; i < 0x3e8; i++)
            {
                BigInteger integer = new BigInteger(max.BitLength, random);
                if ((integer.CompareTo(min) >= 0) && (integer.CompareTo(max) <= 0))
                {
                    return integer;
                }
            }
            return new BigInteger(max.Subtract(min).BitLength - 1, random).Add(min);
        }
    }
}

