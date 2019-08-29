namespace Org.BouncyCastle.Math.Field
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class GenericPolynomialExtensionField : IPolynomialExtensionField, IExtensionField, IFiniteField
    {
        protected readonly IFiniteField subfield;
        protected readonly IPolynomial minimalPolynomial;

        internal GenericPolynomialExtensionField(IFiniteField subfield, IPolynomial polynomial)
        {
            this.subfield = subfield;
            this.minimalPolynomial = polynomial;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            GenericPolynomialExtensionField field = obj as GenericPolynomialExtensionField;
            if (field == null)
            {
                return false;
            }
            return (this.subfield.Equals(field.subfield) && this.minimalPolynomial.Equals(field.minimalPolynomial));
        }

        public override int GetHashCode() => 
            (this.subfield.GetHashCode() ^ Integers.RotateLeft(this.minimalPolynomial.GetHashCode(), 0x10));

        public virtual BigInteger Characteristic =>
            this.subfield.Characteristic;

        public virtual int Dimension =>
            (this.subfield.Dimension * this.minimalPolynomial.Degree);

        public virtual IFiniteField Subfield =>
            this.subfield;

        public virtual int Degree =>
            this.minimalPolynomial.Degree;

        public virtual IPolynomial MinimalPolynomial =>
            this.minimalPolynomial;
    }
}

