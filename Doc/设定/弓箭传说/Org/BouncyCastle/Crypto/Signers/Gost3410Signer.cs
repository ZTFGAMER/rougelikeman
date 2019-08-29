namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;

    public class Gost3410Signer : IDsa
    {
        private Gost3410KeyParameters key;
        private SecureRandom random;

        public virtual BigInteger[] GenerateSignature(byte[] message)
        {
            BigInteger integer2;
            byte[] bytes = new byte[message.Length];
            for (int i = 0; i != bytes.Length; i++)
            {
                bytes[i] = message[(bytes.Length - 1) - i];
            }
            BigInteger val = new BigInteger(1, bytes);
            Gost3410Parameters parameters = this.key.Parameters;
            do
            {
                integer2 = new BigInteger(parameters.Q.BitLength, this.random);
            }
            while (integer2.CompareTo(parameters.Q) >= 0);
            BigInteger integer3 = parameters.A.ModPow(integer2, parameters.P).Mod(parameters.Q);
            BigInteger integer4 = integer2.Multiply(val).Add(((Gost3410PrivateKeyParameters) this.key).X.Multiply(integer3)).Mod(parameters.Q);
            return new BigInteger[] { integer3, integer4 };
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
                if (!(parameters is Gost3410PrivateKeyParameters))
                {
                    throw new InvalidKeyException("GOST3410 private key required for signing");
                }
                this.key = (Gost3410PrivateKeyParameters) parameters;
            }
            else
            {
                if (!(parameters is Gost3410PublicKeyParameters))
                {
                    throw new InvalidKeyException("GOST3410 public key required for signing");
                }
                this.key = (Gost3410PublicKeyParameters) parameters;
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
            Gost3410Parameters parameters = this.key.Parameters;
            if ((r.SignValue < 0) || (parameters.Q.CompareTo(r) <= 0))
            {
                return false;
            }
            if ((s.SignValue < 0) || (parameters.Q.CompareTo(s) <= 0))
            {
                return false;
            }
            BigInteger val = integer.ModPow(parameters.Q.Subtract(BigInteger.Two), parameters.Q);
            BigInteger e = s.Multiply(val).Mod(parameters.Q);
            BigInteger integer4 = parameters.Q.Subtract(r).Multiply(val).Mod(parameters.Q);
            e = parameters.A.ModPow(e, parameters.P);
            integer4 = ((Gost3410PublicKeyParameters) this.key).Y.ModPow(integer4, parameters.P);
            return e.Multiply(integer4).Mod(parameters.P).Mod(parameters.Q).Equals(r);
        }

        public virtual string AlgorithmName =>
            "GOST3410";
    }
}

