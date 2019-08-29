namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;

    public class DsaSigner : IDsa
    {
        protected readonly IDsaKCalculator kCalculator;
        protected DsaKeyParameters key;
        protected SecureRandom random;

        public DsaSigner()
        {
            this.kCalculator = new RandomDsaKCalculator();
        }

        public DsaSigner(IDsaKCalculator kCalculator)
        {
            this.kCalculator = kCalculator;
        }

        protected virtual BigInteger CalculateE(BigInteger n, byte[] message) => 
            new BigInteger(1, message, 0, Math.Min(message.Length, n.BitLength / 8));

        public virtual BigInteger[] GenerateSignature(byte[] message)
        {
            DsaParameters parameters = this.key.Parameters;
            BigInteger q = parameters.Q;
            BigInteger integer2 = this.CalculateE(q, message);
            BigInteger x = ((DsaPrivateKeyParameters) this.key).X;
            if (this.kCalculator.IsDeterministic)
            {
                this.kCalculator.Init(q, x, message);
            }
            else
            {
                this.kCalculator.Init(q, this.random);
            }
            BigInteger e = this.kCalculator.NextK();
            BigInteger val = parameters.G.ModPow(e, parameters.P).Mod(q);
            BigInteger integer6 = e.ModInverse(q).Multiply(integer2.Add(x.Multiply(val))).Mod(q);
            return new BigInteger[] { val, integer6 };
        }

        public virtual void Init(bool forSigning, ICipherParameters parameters)
        {
            SecureRandom provided = null;
            if (forSigning)
            {
                if (parameters is ParametersWithRandom)
                {
                    ParametersWithRandom random2 = (ParametersWithRandom) parameters;
                    provided = random2.Random;
                    parameters = random2.Parameters;
                }
                if (!(parameters is DsaPrivateKeyParameters))
                {
                    throw new InvalidKeyException("DSA private key required for signing");
                }
                this.key = (DsaPrivateKeyParameters) parameters;
            }
            else
            {
                if (!(parameters is DsaPublicKeyParameters))
                {
                    throw new InvalidKeyException("DSA public key required for verification");
                }
                this.key = (DsaPublicKeyParameters) parameters;
            }
            this.random = this.InitSecureRandom(forSigning && !this.kCalculator.IsDeterministic, provided);
        }

        protected virtual SecureRandom InitSecureRandom(bool needed, SecureRandom provided) => 
            (needed ? ((provided == null) ? new SecureRandom() : provided) : null);

        public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
        {
            DsaParameters parameters = this.key.Parameters;
            BigInteger q = parameters.Q;
            BigInteger integer2 = this.CalculateE(q, message);
            if ((r.SignValue <= 0) || (q.CompareTo(r) <= 0))
            {
                return false;
            }
            if ((s.SignValue <= 0) || (q.CompareTo(s) <= 0))
            {
                return false;
            }
            BigInteger val = s.ModInverse(q);
            BigInteger e = integer2.Multiply(val).Mod(q);
            BigInteger integer5 = r.Multiply(val).Mod(q);
            BigInteger p = parameters.P;
            e = parameters.G.ModPow(e, p);
            integer5 = ((DsaPublicKeyParameters) this.key).Y.ModPow(integer5, p);
            return e.Multiply(integer5).Mod(p).Mod(q).Equals(r);
        }

        public virtual string AlgorithmName =>
            "DSA";
    }
}

