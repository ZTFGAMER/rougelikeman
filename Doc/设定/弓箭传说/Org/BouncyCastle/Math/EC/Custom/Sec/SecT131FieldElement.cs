namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecT131FieldElement : ECFieldElement
    {
        protected readonly ulong[] x;

        public SecT131FieldElement()
        {
            this.x = Nat192.Create64();
        }

        public SecT131FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.BitLength > 0x83))
            {
                throw new ArgumentException("value invalid for SecT131FieldElement", "x");
            }
            this.x = SecT131Field.FromBigInteger(x);
        }

        protected internal SecT131FieldElement(ulong[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            ulong[] z = Nat192.Create64();
            SecT131Field.Add(this.x, ((SecT131FieldElement) b).x, z);
            return new SecT131FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            ulong[] z = Nat192.Create64();
            SecT131Field.AddOne(this.x, z);
            return new SecT131FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b) => 
            this.Multiply(b.Invert());

        public virtual bool Equals(SecT131FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat192.Eq64(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecT131FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecT131FieldElement);

        public override int GetHashCode() => 
            (0x202f8 ^ Arrays.GetHashCode(this.x, 0, 3));

        public override ECFieldElement Invert()
        {
            ulong[] z = Nat192.Create64();
            SecT131Field.Invert(this.x, z);
            return new SecT131FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            ulong[] z = Nat192.Create64();
            SecT131Field.Multiply(this.x, ((SecT131FieldElement) b).x, z);
            return new SecT131FieldElement(z);
        }

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.MultiplyPlusProduct(b, x, y);

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT131FieldElement) b).x;
            ulong[] numArray3 = ((SecT131FieldElement) x).x;
            ulong[] numArray4 = ((SecT131FieldElement) y).x;
            ulong[] zz = Nat.Create64(5);
            SecT131Field.MultiplyAddToExt(numArray, numArray2, zz);
            SecT131Field.MultiplyAddToExt(numArray3, numArray4, zz);
            ulong[] z = Nat192.Create64();
            SecT131Field.Reduce(zz, z);
            return new SecT131FieldElement(z);
        }

        public override ECFieldElement Negate() => 
            this;

        public override ECFieldElement Sqrt()
        {
            ulong[] z = Nat192.Create64();
            SecT131Field.Sqrt(this.x, z);
            return new SecT131FieldElement(z);
        }

        public override ECFieldElement Square()
        {
            ulong[] z = Nat192.Create64();
            SecT131Field.Square(this.x, z);
            return new SecT131FieldElement(z);
        }

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y) => 
            this.SquarePlusProduct(x, y);

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT131FieldElement) x).x;
            ulong[] numArray3 = ((SecT131FieldElement) y).x;
            ulong[] zz = Nat.Create64(5);
            SecT131Field.SquareAddToExt(numArray, zz);
            SecT131Field.MultiplyAddToExt(numArray2, numArray3, zz);
            ulong[] z = Nat192.Create64();
            SecT131Field.Reduce(zz, z);
            return new SecT131FieldElement(z);
        }

        public override ECFieldElement SquarePow(int pow)
        {
            if (pow < 1)
            {
                return this;
            }
            ulong[] z = Nat192.Create64();
            SecT131Field.SquareN(this.x, pow, z);
            return new SecT131FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b) => 
            this.Add(b);

        public override bool TestBitZero() => 
            ((this.x[0] & ((ulong) 1L)) != 0L);

        public override BigInteger ToBigInteger() => 
            Nat192.ToBigInteger64(this.x);

        public override bool IsOne =>
            Nat192.IsOne64(this.x);

        public override bool IsZero =>
            Nat192.IsZero64(this.x);

        public override string FieldName =>
            "SecT131Field";

        public override int FieldSize =>
            0x83;

        public virtual int Representation =>
            3;

        public virtual int M =>
            0x83;

        public virtual int K1 =>
            2;

        public virtual int K2 =>
            3;

        public virtual int K3 =>
            8;
    }
}

