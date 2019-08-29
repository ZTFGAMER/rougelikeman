namespace Org.BouncyCastle.Crypto.Generators
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RsaKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
    {
        private static readonly int[] SPECIAL_E_VALUES = new int[] { 3, 5, 0x11, 0x101, 0x10001 };
        private static readonly int SPECIAL_E_HIGHEST = SPECIAL_E_VALUES[SPECIAL_E_VALUES.Length - 1];
        private static readonly int SPECIAL_E_BITS = BigInteger.ValueOf((long) SPECIAL_E_HIGHEST).BitLength;
        protected static readonly BigInteger One = BigInteger.One;
        protected static readonly BigInteger DefaultPublicExponent = BigInteger.ValueOf(0x10001L);
        protected const int DefaultTests = 100;
        protected RsaKeyGenerationParameters parameters;

        protected virtual BigInteger ChooseRandomPrime(int bitlength, BigInteger e)
        {
            BigInteger integer;
            bool flag = (e.BitLength <= SPECIAL_E_BITS) && Arrays.Contains(SPECIAL_E_VALUES, e.IntValue);
            do
            {
                integer = new BigInteger(bitlength, 1, this.parameters.Random);
            }
            while ((integer.Mod(e).Equals(One) || !integer.IsProbablePrime(this.parameters.Certainty, true)) || (!flag && !e.Gcd(integer.Subtract(One)).Equals(One)));
            return integer;
        }

        public virtual AsymmetricCipherKeyPair GenerateKeyPair()
        {
            int num;
            BigInteger integer3;
        Label_0005:
            num = this.parameters.Strength;
            int bitlength = (num + 1) / 2;
            int num3 = num - bitlength;
            int num4 = num / 3;
            int num5 = num >> 2;
            BigInteger publicExponent = this.parameters.PublicExponent;
            BigInteger n = this.ChooseRandomPrime(bitlength, publicExponent);
        Label_0041:
            integer3 = this.ChooseRandomPrime(num3, publicExponent);
            if (integer3.Subtract(n).Abs().BitLength < num4)
            {
                goto Label_0041;
            }
            BigInteger k = n.Multiply(integer3);
            if (k.BitLength != num)
            {
                n = n.Max(integer3);
                goto Label_0041;
            }
            if (WNafUtilities.GetNafWeight(k) < num5)
            {
                n = this.ChooseRandomPrime(bitlength, publicExponent);
                goto Label_0041;
            }
            if (n.CompareTo(integer3) < 0)
            {
                BigInteger integer6 = n;
                n = integer3;
                integer3 = integer6;
            }
            BigInteger integer7 = n.Subtract(One);
            BigInteger integer8 = integer3.Subtract(One);
            BigInteger val = integer7.Gcd(integer8);
            BigInteger m = integer7.Divide(val).Multiply(integer8);
            BigInteger privateExponent = publicExponent.ModInverse(m);
            if (privateExponent.BitLength <= num3)
            {
                goto Label_0005;
            }
            BigInteger dP = privateExponent.Remainder(integer7);
            BigInteger dQ = privateExponent.Remainder(integer8);
            BigInteger qInv = integer3.ModInverse(n);
            return new AsymmetricCipherKeyPair(new RsaKeyParameters(false, k, publicExponent), new RsaPrivateCrtKeyParameters(k, publicExponent, privateExponent, n, integer3, dP, dQ, qInv));
        }

        public virtual void Init(KeyGenerationParameters parameters)
        {
            if (parameters is RsaKeyGenerationParameters)
            {
                this.parameters = (RsaKeyGenerationParameters) parameters;
            }
            else
            {
                this.parameters = new RsaKeyGenerationParameters(DefaultPublicExponent, parameters.Random, parameters.Strength, 100);
            }
        }
    }
}

