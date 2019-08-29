namespace Org.BouncyCastle.Crypto.Generators
{
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class DHKeyGeneratorHelper
    {
        internal static readonly DHKeyGeneratorHelper Instance = new DHKeyGeneratorHelper();

        private DHKeyGeneratorHelper()
        {
        }

        internal BigInteger CalculatePrivate(DHParameters dhParams, SecureRandom random)
        {
            BigInteger integer5;
            int l = dhParams.L;
            if (l != 0)
            {
                BigInteger integer;
                int num2 = l >> 2;
                do
                {
                    integer = new BigInteger(l, random).SetBit(l - 1);
                }
                while (WNafUtilities.GetNafWeight(integer) < num2);
                return integer;
            }
            BigInteger two = BigInteger.Two;
            int m = dhParams.M;
            if (m != 0)
            {
                two = BigInteger.One.ShiftLeft(m - 1);
            }
            BigInteger q = dhParams.Q;
            if (q == null)
            {
                q = dhParams.P;
            }
            BigInteger max = q.Subtract(BigInteger.Two);
            int num4 = max.BitLength >> 2;
            do
            {
                integer5 = BigIntegers.CreateRandomInRange(two, max, random);
            }
            while (WNafUtilities.GetNafWeight(integer5) < num4);
            return integer5;
        }

        internal BigInteger CalculatePublic(DHParameters dhParams, BigInteger x) => 
            dhParams.G.ModPow(x, dhParams.P);
    }
}

