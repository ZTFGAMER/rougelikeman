namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities;
    using System;

    public class ECDomainParameters
    {
        internal ECCurve curve;
        internal byte[] seed;
        internal ECPoint g;
        internal BigInteger n;
        internal BigInteger h;

        public ECDomainParameters(ECCurve curve, ECPoint g, BigInteger n) : this(curve, g, n, BigInteger.One)
        {
        }

        public ECDomainParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h) : this(curve, g, n, h, null)
        {
        }

        public ECDomainParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h, byte[] seed)
        {
            if (curve == null)
            {
                throw new ArgumentNullException("curve");
            }
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (n == null)
            {
                throw new ArgumentNullException("n");
            }
            if (h == null)
            {
                throw new ArgumentNullException("h");
            }
            this.curve = curve;
            this.g = g.Normalize();
            this.n = n;
            this.h = h;
            this.seed = Arrays.Clone(seed);
        }

        protected virtual bool Equals(ECDomainParameters other) => 
            (((this.curve.Equals(other.curve) && this.g.Equals(other.g)) && this.n.Equals(other.n)) && this.h.Equals(other.h));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            ECDomainParameters other = obj as ECDomainParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            int num = this.curve.GetHashCode() * 0x25;
            num ^= this.g.GetHashCode();
            num *= 0x25;
            num ^= this.n.GetHashCode();
            num *= 0x25;
            return (num ^ this.h.GetHashCode());
        }

        public byte[] GetSeed() => 
            Arrays.Clone(this.seed);

        public ECCurve Curve =>
            this.curve;

        public ECPoint G =>
            this.g;

        public BigInteger N =>
            this.n;

        public BigInteger H =>
            this.h;
    }
}

