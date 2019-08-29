namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecT409FieldElement : ECFieldElement
    {
        protected ulong[] x;

        public SecT409FieldElement()
        {
            this.x = Nat448.Create64();
        }

        public SecT409FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.BitLength > 0x199))
            {
                throw new ArgumentException("value invalid for SecT409FieldElement", "x");
            }
            this.x = SecT409Field.FromBigInteger(x);
        }

        protected internal SecT409FieldElement(ulong[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            ulong[] z = Nat448.Create64();
            SecT409Field.Add(this.x, ((SecT409FieldElement) b).x, z);
            return new SecT409FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            ulong[] z = Nat448.Create64();
            SecT409Field.AddOne(this.x, z);
            return new SecT409FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b) => 
            this.Multiply(b.Invert());

        public virtual bool Equals(SecT409FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat448.Eq64(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecT409FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecT409FieldElement);

        public override int GetHashCode() => 
            (0x3e68e7 ^ Arrays.GetHashCode(this.x, 0, 7));

        public override ECFieldElement Invert()
        {
            ulong[] z = Nat448.Create64();
            SecT409Field.Invert(this.x, z);
            return new SecT409FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            ulong[] z = Nat448.Create64();
            SecT409Field.Multiply(this.x, ((SecT409FieldElement) b).x, z);
            return new SecT409FieldElement(z);
        }

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.MultiplyPlusProduct(b, x, y);

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT409FieldElement) b).x;
            ulong[] numArray3 = ((SecT409FieldElement) x).x;
            ulong[] numArray4 = ((SecT409FieldElement) y).x;
            ulong[] zz = Nat.Create64(13);
            SecT409Field.MultiplyAddToExt(numArray, numArray2, zz);
            SecT409Field.MultiplyAddToExt(numArray3, numArray4, zz);
            ulong[] z = Nat448.Create64();
            SecT409Field.Reduce(zz, z);
            return new SecT409FieldElement(z);
        }

        public override ECFieldElement Negate() => 
            this;

        public override ECFieldElement Sqrt()
        {
            ulong[] z = Nat448.Create64();
            SecT409Field.Sqrt(this.x, z);
            return new SecT409FieldElement(z);
        }

        public override ECFieldElement Square()
        {
            ulong[] z = Nat448.Create64();
            SecT409Field.Square(this.x, z);
            return new SecT409FieldElement(z);
        }

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y) => 
            this.SquarePlusProduct(x, y);

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT409FieldElement) x).x;
            ulong[] numArray3 = ((SecT409FieldElement) y).x;
            ulong[] zz = Nat.Create64(13);
            SecT409Field.SquareAddToExt(numArray, zz);
            SecT409Field.MultiplyAddToExt(numArray2, numArray3, zz);
            ulong[] z = Nat448.Create64();
            SecT409Field.Reduce(zz, z);
            return new SecT409FieldElement(z);
        }

        public override ECFieldElement SquarePow(int pow)
        {
            if (pow < 1)
            {
                return this;
            }
            ulong[] z = Nat448.Create64();
            SecT409Field.SquareN(this.x, pow, z);
            return new SecT409FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b) => 
            this.Add(b);

        public override bool TestBitZero() => 
            ((this.x[0] & ((ulong) 1L)) != 0L);

        public override BigInteger ToBigInteger() => 
            Nat448.ToBigInteger64(this.x);

        public override bool IsOne =>
            Nat448.IsOne64(this.x);

        public override bool IsZero =>
            Nat448.IsZero64(this.x);

        public override string FieldName =>
            "SecT409Field";

        public override int FieldSize =>
            0x199;

        public virtual int Representation =>
            2;

        public virtual int M =>
            0x199;

        public virtual int K1 =>
            0x57;

        public virtual int K2 =>
            0;

        public virtual int K3 =>
            0;
    }
}

