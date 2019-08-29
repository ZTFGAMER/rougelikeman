namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public abstract class ECFieldElement
    {
        protected ECFieldElement()
        {
        }

        public abstract ECFieldElement Add(ECFieldElement b);
        public abstract ECFieldElement AddOne();
        public abstract ECFieldElement Divide(ECFieldElement b);
        public virtual bool Equals(ECFieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return this.ToBigInteger().Equals(other.ToBigInteger());
        }

        public override bool Equals(object obj) => 
            this.Equals(obj as ECFieldElement);

        public virtual byte[] GetEncoded() => 
            BigIntegers.AsUnsignedByteArray((this.FieldSize + 7) / 8, this.ToBigInteger());

        public override int GetHashCode() => 
            this.ToBigInteger().GetHashCode();

        public abstract ECFieldElement Invert();
        public abstract ECFieldElement Multiply(ECFieldElement b);
        public virtual ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.Multiply(b).Subtract(x.Multiply(y));

        public virtual ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.Multiply(b).Add(x.Multiply(y));

        public abstract ECFieldElement Negate();
        public abstract ECFieldElement Sqrt();
        public abstract ECFieldElement Square();
        public virtual ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y) => 
            this.Square().Subtract(x.Multiply(y));

        public virtual ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y) => 
            this.Square().Add(x.Multiply(y));

        public virtual ECFieldElement SquarePow(int pow)
        {
            ECFieldElement element = this;
            for (int i = 0; i < pow; i++)
            {
                element = element.Square();
            }
            return element;
        }

        public abstract ECFieldElement Subtract(ECFieldElement b);
        public virtual bool TestBitZero() => 
            this.ToBigInteger().TestBit(0);

        public abstract BigInteger ToBigInteger();
        public override string ToString() => 
            this.ToBigInteger().ToString(0x10);

        public abstract string FieldName { get; }

        public abstract int FieldSize { get; }

        public virtual int BitLength =>
            this.ToBigInteger().BitLength;

        public virtual bool IsOne =>
            (this.BitLength == 1);

        public virtual bool IsZero =>
            (0 == this.ToBigInteger().SignValue);
    }
}

