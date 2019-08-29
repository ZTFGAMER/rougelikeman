namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecT283FieldElement : ECFieldElement
    {
        protected readonly ulong[] x;

        public SecT283FieldElement()
        {
            this.x = Nat320.Create64();
        }

        public SecT283FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.BitLength > 0x11b))
            {
                throw new ArgumentException("value invalid for SecT283FieldElement", "x");
            }
            this.x = SecT283Field.FromBigInteger(x);
        }

        protected internal SecT283FieldElement(ulong[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            ulong[] z = Nat320.Create64();
            SecT283Field.Add(this.x, ((SecT283FieldElement) b).x, z);
            return new SecT283FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            ulong[] z = Nat320.Create64();
            SecT283Field.AddOne(this.x, z);
            return new SecT283FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b) => 
            this.Multiply(b.Invert());

        public virtual bool Equals(SecT283FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat320.Eq64(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecT283FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecT283FieldElement);

        public override int GetHashCode() => 
            (0x2b33ab ^ Arrays.GetHashCode(this.x, 0, 5));

        public override ECFieldElement Invert()
        {
            ulong[] z = Nat320.Create64();
            SecT283Field.Invert(this.x, z);
            return new SecT283FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            ulong[] z = Nat320.Create64();
            SecT283Field.Multiply(this.x, ((SecT283FieldElement) b).x, z);
            return new SecT283FieldElement(z);
        }

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.MultiplyPlusProduct(b, x, y);

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT283FieldElement) b).x;
            ulong[] numArray3 = ((SecT283FieldElement) x).x;
            ulong[] numArray4 = ((SecT283FieldElement) y).x;
            ulong[] zz = Nat.Create64(9);
            SecT283Field.MultiplyAddToExt(numArray, numArray2, zz);
            SecT283Field.MultiplyAddToExt(numArray3, numArray4, zz);
            ulong[] z = Nat320.Create64();
            SecT283Field.Reduce(zz, z);
            return new SecT283FieldElement(z);
        }

        public override ECFieldElement Negate() => 
            this;

        public override ECFieldElement Sqrt()
        {
            ulong[] z = Nat320.Create64();
            SecT283Field.Sqrt(this.x, z);
            return new SecT283FieldElement(z);
        }

        public override ECFieldElement Square()
        {
            ulong[] z = Nat320.Create64();
            SecT283Field.Square(this.x, z);
            return new SecT283FieldElement(z);
        }

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y) => 
            this.SquarePlusProduct(x, y);

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT283FieldElement) x).x;
            ulong[] numArray3 = ((SecT283FieldElement) y).x;
            ulong[] zz = Nat.Create64(9);
            SecT283Field.SquareAddToExt(numArray, zz);
            SecT283Field.MultiplyAddToExt(numArray2, numArray3, zz);
            ulong[] z = Nat320.Create64();
            SecT283Field.Reduce(zz, z);
            return new SecT283FieldElement(z);
        }

        public override ECFieldElement SquarePow(int pow)
        {
            if (pow < 1)
            {
                return this;
            }
            ulong[] z = Nat320.Create64();
            SecT283Field.SquareN(this.x, pow, z);
            return new SecT283FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b) => 
            this.Add(b);

        public override bool TestBitZero() => 
            ((this.x[0] & ((ulong) 1L)) != 0L);

        public override BigInteger ToBigInteger() => 
            Nat320.ToBigInteger64(this.x);

        public override bool IsOne =>
            Nat320.IsOne64(this.x);

        public override bool IsZero =>
            Nat320.IsZero64(this.x);

        public override string FieldName =>
            "SecT283Field";

        public override int FieldSize =>
            0x11b;

        public virtual int Representation =>
            3;

        public virtual int M =>
            0x11b;

        public virtual int K1 =>
            5;

        public virtual int K2 =>
            7;

        public virtual int K3 =>
            12;
    }
}

