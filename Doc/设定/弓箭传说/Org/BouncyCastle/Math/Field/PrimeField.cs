namespace Org.BouncyCastle.Math.Field
{
    using Org.BouncyCastle.Math;
    using System;

    internal class PrimeField : IFiniteField
    {
        protected readonly BigInteger characteristic;

        internal PrimeField(BigInteger characteristic)
        {
            this.characteristic = characteristic;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            PrimeField field = obj as PrimeField;
            if (field == null)
            {
                return false;
            }
            return this.characteristic.Equals(field.characteristic);
        }

        public override int GetHashCode() => 
            this.characteristic.GetHashCode();

        public virtual BigInteger Characteristic =>
            this.characteristic;

        public virtual int Dimension =>
            1;
    }
}

