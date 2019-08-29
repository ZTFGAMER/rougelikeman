namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecT113FieldElement : ECFieldElement
    {
        protected internal readonly ulong[] x;

        public SecT113FieldElement()
        {
            this.x = Nat128.Create64();
        }

        public SecT113FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.BitLength > 0x71))
            {
                throw new ArgumentException("value invalid for SecT113FieldElement", "x");
            }
            this.x = SecT113Field.FromBigInteger(x);
        }

        protected internal SecT113FieldElement(ulong[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            ulong[] z = Nat128.Create64();
            SecT113Field.Add(this.x, ((SecT113FieldElement) b).x, z);
            return new SecT113FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            ulong[] z = Nat128.Create64();
            SecT113Field.AddOne(this.x, z);
            return new SecT113FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b) => 
            this.Multiply(b.Invert());

        public virtual bool Equals(SecT113FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat128.Eq64(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecT113FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecT113FieldElement);

        public override int GetHashCode() => 
            (0x1b971 ^ Arrays.GetHashCode(this.x, 0, 2));

        public override ECFieldElement Invert()
        {
            ulong[] z = Nat128.Create64();
            SecT113Field.Invert(this.x, z);
            return new SecT113FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            ulong[] z = Nat128.Create64();
            SecT113Field.Multiply(this.x, ((SecT113FieldElement) b).x, z);
            return new SecT113FieldElement(z);
        }

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.MultiplyPlusProduct(b, x, y);

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT113FieldElement) b).x;
            ulong[] numArray3 = ((SecT113FieldElement) x).x;
            ulong[] numArray4 = ((SecT113FieldElement) y).x;
            ulong[] zz = Nat128.CreateExt64();
            SecT113Field.MultiplyAddToExt(numArray, numArray2, zz);
            SecT113Field.MultiplyAddToExt(numArray3, numArray4, zz);
            ulong[] z = Nat128.Create64();
            SecT113Field.Reduce(zz, z);
            return new SecT113FieldElement(z);
        }

        public override ECFieldElement Negate() => 
            this;

        public override ECFieldElement Sqrt()
        {
            ulong[] z = Nat128.Create64();
            SecT113Field.Sqrt(this.x, z);
            return new SecT113FieldElement(z);
        }

        public override ECFieldElement Square()
        {
            ulong[] z = Nat128.Create64();
            SecT113Field.Square(this.x, z);
            return new SecT113FieldElement(z);
        }

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y) => 
            this.SquarePlusProduct(x, y);

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT113FieldElement) x).x;
            ulong[] numArray3 = ((SecT113FieldElement) y).x;
            ulong[] zz = Nat128.CreateExt64();
            SecT113Field.SquareAddToExt(numArray, zz);
            SecT113Field.MultiplyAddToExt(numArray2, numArray3, zz);
            ulong[] z = Nat128.Create64();
            SecT113Field.Reduce(zz, z);
            return new SecT113FieldElement(z);
        }

        public override ECFieldElement SquarePow(int pow)
        {
            if (pow < 1)
            {
                return this;
            }
            ulong[] z = Nat128.Create64();
            SecT113Field.SquareN(this.x, pow, z);
            return new SecT113FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b) => 
            this.Add(b);

        public override bool TestBitZero() => 
            ((this.x[0] & ((ulong) 1L)) != 0L);

        public override BigInteger ToBigInteger() => 
            Nat128.ToBigInteger64(this.x);

        public override bool IsOne =>
            Nat128.IsOne64(this.x);

        public override bool IsZero =>
            Nat128.IsZero64(this.x);

        public override string FieldName =>
            "SecT113Field";

        public override int FieldSize =>
            0x71;

        public virtual int Representation =>
            2;

        public virtual int M =>
            0x71;

        public virtual int K1 =>
            9;

        public virtual int K2 =>
            0;

        public virtual int K3 =>
            0;
    }
}

