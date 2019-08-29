namespace Org.BouncyCastle.Crypto.Generators
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class DHParametersHelper
    {
        private static readonly BigInteger Six = BigInteger.ValueOf(6L);
        private static readonly int[][] primeLists = BigInteger.primeLists;
        private static readonly int[] primeProducts = BigInteger.primeProducts;
        private static readonly BigInteger[] BigPrimeProducts = ConstructBigPrimeProducts(primeProducts);

        private static BigInteger[] ConstructBigPrimeProducts(int[] primeProducts)
        {
            BigInteger[] integerArray = new BigInteger[primeProducts.Length];
            for (int i = 0; i < integerArray.Length; i++)
            {
                integerArray[i] = BigInteger.ValueOf((long) primeProducts[i]);
            }
            return integerArray;
        }

        internal static BigInteger[] GenerateSafePrimes(int size, int certainty, SecureRandom random)
        {
            BigInteger integer;
            BigInteger integer2;
            int num3;
            int bitLength = size - 1;
            int num2 = size >> 2;
            if (size <= 0x20)
            {
                while (true)
                {
                    integer2 = new BigInteger(bitLength, 2, random);
                    integer = integer2.ShiftLeft(1).Add(BigInteger.One);
                    if (integer.IsProbablePrime(certainty, true) && ((certainty <= 2) || integer2.IsProbablePrime(certainty, true)))
                    {
                        goto Label_01B9;
                    }
                }
            }
        Label_006F:
            integer2 = new BigInteger(bitLength, 0, random);
        Label_0078:
            num3 = 0;
            while (num3 < primeLists.Length)
            {
                int intValue = integer2.Remainder(BigPrimeProducts[num3]).IntValue;
                if (num3 == 0)
                {
                    int num5 = intValue % 3;
                    if (num5 != 2)
                    {
                        int num6 = (2 * num5) + 2;
                        integer2 = integer2.Add(BigInteger.ValueOf((long) num6));
                        intValue = (intValue + num6) % primeProducts[num3];
                    }
                }
                foreach (int num8 in primeLists[num3])
                {
                    int num9 = intValue % num8;
                    if ((num9 == 0) || (num9 == (num8 >> 1)))
                    {
                        integer2 = integer2.Add(Six);
                        goto Label_0078;
                    }
                }
                num3++;
            }
            if ((integer2.BitLength != bitLength) || !integer2.RabinMillerTest(2, random, true))
            {
                goto Label_006F;
            }
            integer = integer2.ShiftLeft(1).Add(BigInteger.One);
            if ((!integer.RabinMillerTest(certainty, random, true) || ((certainty > 2) && !integer2.RabinMillerTest(certainty - 2, random, true))) || (WNafUtilities.GetNafWeight(integer) < num2))
            {
                goto Label_006F;
            }
        Label_01B9:
            return new BigInteger[] { integer, integer2 };
        }

        internal static BigInteger SelectGenerator(BigInteger p, BigInteger q, SecureRandom random)
        {
            BigInteger integer2;
            BigInteger max = p.Subtract(BigInteger.Two);
            do
            {
                integer2 = BigIntegers.CreateRandomInRange(BigInteger.Two, max, random).ModPow(BigInteger.Two, p);
            }
            while (integer2.Equals(BigInteger.One));
            return integer2;
        }
    }
}

