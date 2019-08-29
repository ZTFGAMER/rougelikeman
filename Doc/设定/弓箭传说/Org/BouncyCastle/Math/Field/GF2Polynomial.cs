namespace Org.BouncyCastle.Math.Field
{
    using Org.BouncyCastle.Utilities;
    using System;

    internal class GF2Polynomial : IPolynomial
    {
        protected readonly int[] exponents;

        internal GF2Polynomial(int[] exponents)
        {
            this.exponents = Arrays.Clone(exponents);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            GF2Polynomial polynomial = obj as GF2Polynomial;
            if (polynomial == null)
            {
                return false;
            }
            return Arrays.AreEqual(this.exponents, polynomial.exponents);
        }

        public virtual int[] GetExponentsPresent() => 
            Arrays.Clone(this.exponents);

        public override int GetHashCode() => 
            Arrays.GetHashCode(this.exponents);

        public virtual int Degree =>
            this.exponents[this.exponents.Length - 1];
    }
}

