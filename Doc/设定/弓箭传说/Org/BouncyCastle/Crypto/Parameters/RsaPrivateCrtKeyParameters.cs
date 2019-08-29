namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Math;
    using System;

    public class RsaPrivateCrtKeyParameters : RsaKeyParameters
    {
        private readonly BigInteger e;
        private readonly BigInteger p;
        private readonly BigInteger q;
        private readonly BigInteger dP;
        private readonly BigInteger dQ;
        private readonly BigInteger qInv;

        public RsaPrivateCrtKeyParameters(BigInteger modulus, BigInteger publicExponent, BigInteger privateExponent, BigInteger p, BigInteger q, BigInteger dP, BigInteger dQ, BigInteger qInv) : base(true, modulus, privateExponent)
        {
            ValidateValue(publicExponent, "publicExponent", "exponent");
            ValidateValue(p, "p", "P value");
            ValidateValue(q, "q", "Q value");
            ValidateValue(dP, "dP", "DP value");
            ValidateValue(dQ, "dQ", "DQ value");
            ValidateValue(qInv, "qInv", "InverseQ value");
            this.e = publicExponent;
            this.p = p;
            this.q = q;
            this.dP = dP;
            this.dQ = dQ;
            this.qInv = qInv;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            RsaPrivateCrtKeyParameters parameters = obj as RsaPrivateCrtKeyParameters;
            return ((((parameters?.DP.Equals(this.dP) && parameters.DQ.Equals(this.dQ)) && (parameters.Exponent.Equals(base.Exponent) && parameters.Modulus.Equals(base.Modulus))) && ((parameters.P.Equals(this.p) && parameters.Q.Equals(this.q)) && parameters.PublicExponent.Equals(this.e))) && parameters.QInv.Equals(this.qInv));
        }

        public override int GetHashCode() => 
            (((((((this.DP.GetHashCode() ^ this.DQ.GetHashCode()) ^ base.Exponent.GetHashCode()) ^ base.Modulus.GetHashCode()) ^ this.P.GetHashCode()) ^ this.Q.GetHashCode()) ^ this.PublicExponent.GetHashCode()) ^ this.QInv.GetHashCode());

        private static void ValidateValue(BigInteger x, string name, string desc)
        {
            if (x == null)
            {
                throw new ArgumentNullException(name);
            }
            if (x.SignValue <= 0)
            {
                throw new ArgumentException("Not a valid RSA " + desc, name);
            }
        }

        public BigInteger PublicExponent =>
            this.e;

        public BigInteger P =>
            this.p;

        public BigInteger Q =>
            this.q;

        public BigInteger DP =>
            this.dP;

        public BigInteger DQ =>
            this.dQ;

        public BigInteger QInv =>
            this.qInv;
    }
}

