namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Security;
    using System;

    public class ECGost3410Signer : IDsa
    {
        private ECKeyParameters key;
        private SecureRandom random;

        protected virtual ECMultiplier CreateBasePointMultiplier() => 
            new FixedPointCombMultiplier();

        public virtual BigInteger[] GenerateSignature(byte[] message)
        {
            BigInteger integer6;
            byte[] bytes = new byte[message.Length];
            for (int i = 0; i != bytes.Length; i++)
            {
                bytes[i] = message[(bytes.Length - 1) - i];
            }
            BigInteger val = new BigInteger(1, bytes);
            ECDomainParameters parameters = this.key.Parameters;
            BigInteger n = parameters.N;
            BigInteger d = ((ECPrivateKeyParameters) this.key).D;
            BigInteger integer5 = null;
            ECMultiplier multiplier = this.CreateBasePointMultiplier();
        Label_0062:
            integer6 = new BigInteger(n.BitLength, this.random);
            if (integer6.SignValue == 0)
            {
                goto Label_0062;
            }
            BigInteger integer4 = multiplier.Multiply(parameters.G, integer6).Normalize().AffineXCoord.ToBigInteger().Mod(n);
            if (integer4.SignValue == 0)
            {
                goto Label_0062;
            }
            integer5 = integer6.Multiply(val).Add(d.Multiply(integer4)).Mod(n);
            if (integer5.SignValue == 0)
            {
                goto Label_0062;
            }
            return new BigInteger[] { integer4, integer5 };
        }

        public virtual void Init(bool forSigning, ICipherParameters parameters)
        {
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
            byte[] bytes = new byte[message.Length];
            for (int i = 0; i != bytes.Length; i++)
            {
                bytes[i] = message[(bytes.Length - 1) - i];
            }
            BigInteger integer = new BigInteger(1, bytes);
            BigInteger n = this.key.Parameters.N;
            if ((r.CompareTo(BigInteger.One) < 0) || (r.CompareTo(n) >= 0))
            {
                return false;
            }
            if ((s.CompareTo(BigInteger.One) < 0) || (s.CompareTo(n) >= 0))
            {
                return false;
            }
            BigInteger val = integer.ModInverse(n);
            BigInteger a = s.Multiply(val).Mod(n);
            BigInteger b = n.Subtract(r).Multiply(val).Mod(n);
            ECPoint g = this.key.Parameters.G;
            ECPoint q = ((ECPublicKeyParameters) this.key).Q;
            ECPoint point3 = ECAlgorithms.SumOfTwoMultiplies(g, a, q, b).Normalize();
            if (point3.IsInfinity)
            {
                return false;
            }
            return point3.AffineXCoord.ToBigInteger().Mod(n).Equals(r);
        }

        public virtual string AlgorithmName =>
            "ECGOST3410";
    }
}

