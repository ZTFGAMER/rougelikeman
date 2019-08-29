namespace Org.BouncyCastle.Crypto.Signers
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Security;
    using System;

    public class ECDsaSigner : IDsa
    {
        private static readonly BigInteger Eight = BigInteger.ValueOf(8L);
        protected readonly IDsaKCalculator kCalculator;
        protected ECKeyParameters key;
        protected SecureRandom random;

        public ECDsaSigner()
        {
            this.kCalculator = new RandomDsaKCalculator();
        }

        public ECDsaSigner(IDsaKCalculator kCalculator)
        {
            this.kCalculator = kCalculator;
        }

        protected virtual BigInteger CalculateE(BigInteger n, byte[] message)
        {
            int num = message.Length * 8;
            BigInteger integer = new BigInteger(1, message);
            if (n.BitLength < num)
            {
                integer = integer.ShiftRight(num - n.BitLength);
            }
            return integer;
        }

        protected virtual ECMultiplier CreateBasePointMultiplier() => 
            new FixedPointCombMultiplier();

        public virtual BigInteger[] GenerateSignature(byte[] message)
        {
            BigInteger integer6;
            ECDomainParameters parameters = this.key.Parameters;
            BigInteger n = parameters.N;
            BigInteger integer2 = this.CalculateE(n, message);
            BigInteger d = ((ECPrivateKeyParameters) this.key).D;
            if (this.kCalculator.IsDeterministic)
            {
                this.kCalculator.Init(n, d, message);
            }
            else
            {
                this.kCalculator.Init(n, this.random);
            }
            ECMultiplier multiplier = this.CreateBasePointMultiplier();
        Label_006A:
            integer6 = this.kCalculator.NextK();
            BigInteger val = multiplier.Multiply(parameters.G, integer6).Normalize().AffineXCoord.ToBigInteger().Mod(n);
            if (val.SignValue == 0)
            {
                goto Label_006A;
            }
            BigInteger integer5 = integer6.ModInverse(n).Multiply(integer2.Add(d.Multiply(val))).Mod(n);
            if (integer5.SignValue == 0)
            {
                goto Label_006A;
            }
            return new BigInteger[] { val, integer5 };
        }

        protected virtual ECFieldElement GetDenominator(int coordinateSystem, ECPoint p)
        {
            switch (coordinateSystem)
            {
                case 1:
                case 6:
                case 7:
                    return p.GetZCoord(0);

                case 2:
                case 3:
                case 4:
                    return p.GetZCoord(0).Square();
            }
            return null;
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
            this.random = this.InitSecureRandom(forSigning && !this.kCalculator.IsDeterministic, provided);
        }

        protected virtual SecureRandom InitSecureRandom(bool needed, SecureRandom provided) => 
            (needed ? ((provided == null) ? new SecureRandom() : provided) : null);

        public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
        {
            BigInteger n = this.key.Parameters.N;
            if (((r.SignValue < 1) || (s.SignValue < 1)) || ((r.CompareTo(n) >= 0) || (s.CompareTo(n) >= 0)))
            {
                return false;
            }
            BigInteger integer2 = this.CalculateE(n, message);
            BigInteger val = s.ModInverse(n);
            BigInteger a = integer2.Multiply(val).Mod(n);
            BigInteger b = r.Multiply(val).Mod(n);
            ECPoint g = this.key.Parameters.G;
            ECPoint q = ((ECPublicKeyParameters) this.key).Q;
            ECPoint p = ECAlgorithms.SumOfTwoMultiplies(g, a, q, b);
            if (p.IsInfinity)
            {
                return false;
            }
            ECCurve curve = p.Curve;
            if (curve != null)
            {
                BigInteger cofactor = curve.Cofactor;
                if ((cofactor != null) && (cofactor.CompareTo(Eight) <= 0))
                {
                    ECFieldElement denominator = this.GetDenominator(curve.CoordinateSystem, p);
                    if ((denominator != null) && !denominator.IsZero)
                    {
                        ECFieldElement xCoord = p.XCoord;
                        while (curve.IsValidFieldElement(r))
                        {
                            if (curve.FromBigInteger(r).Multiply(denominator).Equals(xCoord))
                            {
                                return true;
                            }
                            r = r.Add(n);
                        }
                        return false;
                    }
                }
            }
            return p.Normalize().AffineXCoord.ToBigInteger().Mod(n).Equals(r);
        }

        public virtual string AlgorithmName =>
            "ECDSA";
    }
}

