namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecT571FieldElement : ECFieldElement
    {
        protected readonly ulong[] x;

        public SecT571FieldElement()
        {
            this.x = Nat576.Create64();
        }

        public SecT571FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.BitLength > 0x23b))
            {
                throw new ArgumentException("value invalid for SecT571FieldElement", "x");
            }
            this.x = SecT571Field.FromBigInteger(x);
        }

        protected internal SecT571FieldElement(ulong[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            ulong[] z = Nat576.Create64();
            SecT571Field.Add(this.x, ((SecT571FieldElement) b).x, z);
            return new SecT571FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            ulong[] z = Nat576.Create64();
            SecT571Field.AddOne(this.x, z);
            return new SecT571FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b) => 
            this.Multiply(b.Invert());

        public virtual bool Equals(SecT571FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat576.Eq64(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecT571FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecT571FieldElement);

        public override int GetHashCode() => 
            (0x5724cc ^ Arrays.GetHashCode(this.x, 0, 9));

        public override ECFieldElement Invert()
        {
            ulong[] z = Nat576.Create64();
            SecT571Field.Invert(this.x, z);
            return new SecT571FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            ulong[] z = Nat576.Create64();
            SecT571Field.Multiply(this.x, ((SecT571FieldElement) b).x, z);
            return new SecT571FieldElement(z);
        }

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.MultiplyPlusProduct(b, x, y);

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT571FieldElement) b).x;
            ulong[] numArray3 = ((SecT571FieldElement) x).x;
            ulong[] numArray4 = ((SecT571FieldElement) y).x;
            ulong[] zz = Nat576.CreateExt64();
            SecT571Field.MultiplyAddToExt(numArray, numArray2, zz);
            SecT571Field.MultiplyAddToExt(numArray3, numArray4, zz);
            ulong[] z = Nat576.Create64();
            SecT571Field.Reduce(zz, z);
            return new SecT571FieldElement(z);
        }

        public override ECFieldElement Negate() => 
            this;

        public override ECFieldElement Sqrt()
        {
            ulong[] z = Nat576.Create64();
            SecT571Field.Sqrt(this.x, z);
            return new SecT571FieldElement(z);
        }

        public override ECFieldElement Square()
        {
            ulong[] z = Nat576.Create64();
            SecT571Field.Square(this.x, z);
            return new SecT571FieldElement(z);
        }

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y) => 
            this.SquarePlusProduct(x, y);

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT571FieldElement) x).x;
            ulong[] numArray3 = ((SecT571FieldElement) y).x;
            ulong[] zz = Nat576.CreateExt64();
            SecT571Field.SquareAddToExt(numArray, zz);
            SecT571Field.MultiplyAddToExt(numArray2, numArray3, zz);
            ulong[] z = Nat576.Create64();
            SecT571Field.Reduce(zz, z);
            return new SecT571FieldElement(z);
        }

        public override ECFieldElement SquarePow(int pow)
        {
            if (pow < 1)
            {
                return this;
            }
            ulong[] z = Nat576.Create64();
            SecT571Field.SquareN(this.x, pow, z);
            return new SecT571FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b) => 
            this.Add(b);

        public override bool TestBitZero() => 
            ((this.x[0] & ((ulong) 1L)) != 0L);

        public override BigInteger ToBigInteger() => 
            Nat576.ToBigInteger64(this.x);

        public override bool IsOne =>
            Nat576.IsOne64(this.x);

        public override bool IsZero =>
            Nat576.IsZero64(this.x);

        public override string FieldName =>
            "SecT571Field";

        public override int FieldSize =>
            0x23b;

        public virtual int Representation =>
            3;

        public virtual int M =>
            0x23b;

        public virtual int K1 =>
            2;

        public virtual int K2 =>
            5;

        public virtual int K3 =>
            10;
    }
}

