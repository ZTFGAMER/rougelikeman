namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP160R2FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP160R2Curve.q;
        protected internal readonly uint[] x;

        public SecP160R2FieldElement()
        {
            this.x = Nat160.Create();
        }

        public SecP160R2FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP160R2FieldElement", "x");
            }
            this.x = SecP160R2Field.FromBigInteger(x);
        }

        protected internal SecP160R2FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat160.Create();
            SecP160R2Field.Add(this.x, ((SecP160R2FieldElement) b).x, z);
            return new SecP160R2FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat160.Create();
            SecP160R2Field.AddOne(this.x, z);
            return new SecP160R2FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat160.Create();
            Mod.Invert(SecP160R2Field.P, ((SecP160R2FieldElement) b).x, z);
            SecP160R2Field.Multiply(z, this.x, z);
            return new SecP160R2FieldElement(z);
        }

        public virtual bool Equals(SecP160R2FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat160.Eq(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecP160R2FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP160R2FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 5));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat160.Create();
            Mod.Invert(SecP160R2Field.P, this.x, z);
            return new SecP160R2FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat160.Create();
            SecP160R2Field.Multiply(this.x, ((SecP160R2FieldElement) b).x, z);
            return new SecP160R2FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat160.Create();
            SecP160R2Field.Negate(this.x, z);
            return new SecP160R2FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat160.IsZero(x) || Nat160.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat160.Create();
            SecP160R2Field.Square(x, z);
            SecP160R2Field.Multiply(z, x, z);
            uint[] numArray3 = Nat160.Create();
            SecP160R2Field.Square(z, numArray3);
            SecP160R2Field.Multiply(numArray3, x, numArray3);
            uint[] numArray4 = Nat160.Create();
            SecP160R2Field.Square(numArray3, numArray4);
            SecP160R2Field.Multiply(numArray4, x, numArray4);
            uint[] numArray5 = Nat160.Create();
            SecP160R2Field.SquareN(numArray4, 3, numArray5);
            SecP160R2Field.Multiply(numArray5, numArray3, numArray5);
            uint[] numArray6 = numArray4;
            SecP160R2Field.SquareN(numArray5, 7, numArray6);
            SecP160R2Field.Multiply(numArray6, numArray5, numArray6);
            uint[] numArray7 = numArray5;
            SecP160R2Field.SquareN(numArray6, 3, numArray7);
            SecP160R2Field.Multiply(numArray7, numArray3, numArray7);
            uint[] numArray8 = Nat160.Create();
            SecP160R2Field.SquareN(numArray7, 14, numArray8);
            SecP160R2Field.Multiply(numArray8, numArray6, numArray8);
            uint[] numArray9 = numArray6;
            SecP160R2Field.SquareN(numArray8, 0x1f, numArray9);
            SecP160R2Field.Multiply(numArray9, numArray8, numArray9);
            uint[] numArray10 = numArray8;
            SecP160R2Field.SquareN(numArray9, 0x3e, numArray10);
            SecP160R2Field.Multiply(numArray10, numArray9, numArray10);
            uint[] numArray11 = numArray9;
            SecP160R2Field.SquareN(numArray10, 3, numArray11);
            SecP160R2Field.Multiply(numArray11, numArray3, numArray11);
            uint[] numArray12 = numArray11;
            SecP160R2Field.SquareN(numArray12, 0x12, numArray12);
            SecP160R2Field.Multiply(numArray12, numArray7, numArray12);
            SecP160R2Field.SquareN(numArray12, 2, numArray12);
            SecP160R2Field.Multiply(numArray12, x, numArray12);
            SecP160R2Field.SquareN(numArray12, 3, numArray12);
            SecP160R2Field.Multiply(numArray12, z, numArray12);
            SecP160R2Field.SquareN(numArray12, 6, numArray12);
            SecP160R2Field.Multiply(numArray12, numArray3, numArray12);
            SecP160R2Field.SquareN(numArray12, 2, numArray12);
            SecP160R2Field.Multiply(numArray12, x, numArray12);
            uint[] numArray13 = z;
            SecP160R2Field.Square(numArray12, numArray13);
            return (!Nat160.Eq(x, numArray13) ? null : new SecP160R2FieldElement(numArray12));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat160.Create();
            SecP160R2Field.Square(this.x, z);
            return new SecP160R2FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat160.Create();
            SecP160R2Field.Subtract(this.x, ((SecP160R2FieldElement) b).x, z);
            return new SecP160R2FieldElement(z);
        }

        public override bool TestBitZero() => 
            (Nat160.GetBit(this.x, 0) == 1);

        public override BigInteger ToBigInteger() => 
            Nat160.ToBigInteger(this.x);

        public override bool IsZero =>
            Nat160.IsZero(this.x);

        public override bool IsOne =>
            Nat160.IsOne(this.x);

        public override string FieldName =>
            "SecP160R2Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

