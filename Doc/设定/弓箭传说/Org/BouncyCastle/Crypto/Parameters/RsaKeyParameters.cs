namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Math;
    using System;

    public class RsaKeyParameters : AsymmetricKeyParameter
    {
        private readonly BigInteger modulus;
        private readonly BigInteger exponent;

        public RsaKeyParameters(bool isPrivate, BigInteger modulus, BigInteger exponent) : base(isPrivate)
        {
            if (modulus == null)
            {
                throw new ArgumentNullException("modulus");
            }
            if (exponent == null)
            {
                throw new ArgumentNullException("exponent");
            }
            if (modulus.SignValue <= 0)
            {
                throw new ArgumentException("Not a valid RSA modulus", "modulus");
            }
            if (exponent.SignValue <= 0)
            {
                throw new ArgumentException("Not a valid RSA exponent", "exponent");
            }
            this.modulus = modulus;
            this.exponent = exponent;
        }

        public override bool Equals(object obj)
        {
            RsaKeyParameters parameters = obj as RsaKeyParameters;
            return (((parameters?.IsPrivate == base.IsPrivate) && parameters.Modulus.Equals(this.modulus)) && parameters.Exponent.Equals(this.exponent));
        }

        public override int GetHashCode() => 
            ((this.modulus.GetHashCode() ^ this.exponent.GetHashCode()) ^ base.IsPrivate.GetHashCode());

        public BigInteger Modulus =>
            this.modulus;

        public BigInteger Exponent =>
            this.exponent;
    }
}

