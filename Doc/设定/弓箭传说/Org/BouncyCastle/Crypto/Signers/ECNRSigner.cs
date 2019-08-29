namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Security;
    using System;

    public class ECNRSigner : IDsa
    {
        private bool forSigning;
        private ECKeyParameters key;
        private SecureRandom random;

        public virtual BigInteger[] GenerateSignature(byte[] message)
        {
            AsymmetricCipherKeyPair pair;
            if (!this.forSigning)
            {
                throw new InvalidOperationException("not initialised for signing");
            }
            BigInteger n = ((ECPrivateKeyParameters) this.key).Parameters.N;
            int bitLength = n.BitLength;
            BigInteger integer2 = new BigInteger(1, message);
            int num2 = integer2.BitLength;
            ECPrivateKeyParameters key = (ECPrivateKeyParameters) this.key;
            if (num2 > bitLength)
            {
                throw new DataLengthException("input too large for ECNR key.");
            }
            BigInteger integer3 = null;
            BigInteger integer4 = null;
            do
            {
                ECKeyPairGenerator generator = new ECKeyPairGenerator();
                generator.Init(new ECKeyGenerationParameters(key.Parameters, this.random));
                pair = generator.GenerateKeyPair();
                ECPublicKeyParameters @public = (ECPublicKeyParameters) pair.Public;
                integer3 = @public.Q.AffineXCoord.ToBigInteger().Add(integer2).Mod(n);
            }
            while (integer3.SignValue == 0);
            BigInteger d = key.D;
            integer4 = ((ECPrivateKeyParameters) pair.Private).D.Subtract(integer3.Multiply(d)).Mod(n);
            return new BigInteger[] { integer3, integer4 };
        }

        public virtual void Init(bool forSigning, ICipherParameters parameters)
        {
            this.forSigning = forSigning;
            if (forSigning)
            {
                if (parameters is ParametersWithRandom)
                {
                    ParametersWithRandom random = (ParametersWithRandom) parameters;
                    this.random = random.Random;
                    parameters = random.Parameters;
                }
                else
                {
                    this.random = new SecureRandom();
                }
                if (!(parameters is ECPrivateKeyParameters))
                {
                    throw new InvalidKeyException("EC private key required for signing");
                }
                this.key = (ECPrivateKeyParameters) parameters;
            }
            else
            {
                if (!(parameters is ECPublicKeyParameters))
                {
                    throw new InvalidKeyException("EC public key required for verification");
                }
                this.key = (ECPublicKeyParameters) parameters;
            }
        }

        public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
        {
            if (this.forSigning)
            {
                throw new InvalidOperationException("not initialised for verifying");
            }
            ECPublicKeyParameters key = (ECPublicKeyParameters) this.key;
            BigInteger n = key.Parameters.N;
            int bitLength = n.BitLength;
            BigInteger integer2 = new BigInteger(1, message);
            if (integer2.BitLength > bitLength)
            {
                throw new DataLengthException("input too large for ECNR key.");
            }
            if ((r.CompareTo(BigInteger.One) < 0) || (r.CompareTo(n) >= 0))
            {
                return false;
            }
            if ((s.CompareTo(BigInteger.Zero) < 0) || (s.CompareTo(n) >= 0))
            {
                return false;
            }
            ECPoint g = key.Parameters.G;
            ECPoint q = key.Q;
            ECPoint point3 = ECAlgorithms.SumOfTwoMultiplies(g, s, q, r).Normalize();
            if (point3.IsInfinity)
            {
                return false;
            }
            BigInteger integer3 = point3.AffineXCoord.ToBigInteger();
            return r.Subtract(integer3).Mod(n).Equals(integer2);
        }

        public virtual string AlgorithmName =>
            "ECNR";
    }
}

