namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Math;
    using System;

    public class DsaParameters : ICipherParameters
    {
        private readonly BigInteger p;
        private readonly BigInteger q;
        private readonly BigInteger g;
        private readonly DsaValidationParameters validation;

        public DsaParameters(BigInteger p, BigInteger q, BigInteger g) : this(p, q, g, null)
        {
        }

        public DsaParameters(BigInteger p, BigInteger q, BigInteger g, DsaValidationParameters parameters)
        {
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }
            if (q == null)
            {
                throw new ArgumentNullException("q");
            }
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            this.p = p;
            this.q = q;
            this.g = g;
            this.validation = parameters;
        }

        protected bool Equals(DsaParameters other) => 
            ((this.p.Equals(other.p) && this.q.Equals(other.q)) && this.g.Equals(other.g));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            DsaParameters other = obj as DsaParameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            ((this.p.GetHashCode() ^ this.q.GetHashCode()) ^ this.g.GetHashCode());

        public BigInteger P =>
            this.p;

        public BigInteger Q =>
            this.q;

        public BigInteger G =>
            this.g;

        public DsaValidationParameters ValidationParameters =>
            this.validation;
    }
}

