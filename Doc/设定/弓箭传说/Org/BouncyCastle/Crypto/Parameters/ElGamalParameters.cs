namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Math;
    using System;

    public class ElGamalParameters : ICipherParameters
    {
        private readonly BigInteger p;
        private readonly BigInteger g;
        private readonly int l;

        public ElGamalParameters(BigInteger p, BigInteger g) : this(p, g, 0)
        {
        }

        public ElGamalParameters(BigInteger p, BigInteger g, int l)
        {
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.p = p;
            this.g = g;
            this.l = l;
        }

        protected bool Equals(ElGamalParameters other) => 
            ((this.p.Equals(other.p) && this.g.Equals(other.g)) && (this.l == other.l));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            ElGamalParameters other = obj as ElGamalParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            ((this.p.GetHashCode() ^ this.g.GetHashCode()) ^ this.l);

        public BigInteger P =>
            this.p;

        public BigInteger G =>
            this.g;

        public int L =>
            this.l;
    }
}

