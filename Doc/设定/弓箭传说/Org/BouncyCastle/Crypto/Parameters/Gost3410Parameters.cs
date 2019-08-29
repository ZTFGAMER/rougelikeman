namespace Org.BouncyCastle.Crypto.Parameters
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Math;
    using System;

    public class Gost3410Parameters : ICipherParameters
    {
        private readonly BigInteger p;
        private readonly BigInteger q;
        private readonly BigInteger a;
        private readonly Gost3410ValidationParameters validation;

        public Gost3410Parameters(BigInteger p, BigInteger q, BigInteger a) : this(p, q, a, null)
        {
        }

        public Gost3410Parameters(BigInteger p, BigInteger q, BigInteger a, Gost3410ValidationParameters validation)
        {
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }
            if (q == null)
            {
                throw new ArgumentNullException("q");
            }
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            this.p = p;
            this.q = q;
            this.a = a;
            this.validation = validation;
        }

        protected bool Equals(Gost3410Parameters other) => 
            ((this.p.Equals(other.p) && this.q.Equals(other.q)) && this.a.Equals(other.a));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            Gost3410Parameters other = obj as Gost3410Parameters;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            ((this.p.GetHashCode() ^ this.q.GetHashCode()) ^ this.a.GetHashCode());

        public BigInteger P =>
            this.p;

        public BigInteger Q =>
            this.q;

        public BigInteger A =>
            this.a;

        public Gost3410ValidationParameters ValidationParameters =>
            this.validation;
    }
}

