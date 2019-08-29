namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP160R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP160R1Curve.q;
        protected internal readonly uint[] x;

        public SecP160R1FieldElement()
        {
            this.x = Nat160.Create();
        }

        public SecP160R1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP160R1FieldElement", "x");
            }
            this.x = SecP160R1Field.FromBigInteger(x);
        }

        protected internal SecP160R1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat160.Create();
            SecP160R1Field.Add(this.x, ((SecP160R1FieldElement) b).x, z);
            return new SecP160R1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat160.Create();
            SecP160R1Field.AddOne(this.x, z);
            return new SecP160R1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat160.Create();
            Mod.Invert(SecP160R1Field.P, ((SecP160R1FieldElement) b).x, z);
            SecP160R1Field.Multiply(z, this.x, z);
            return new SecP160R1FieldElement(z);
        }

        public virtual bool Equals(SecP160R1FieldElement other)
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
            this.Equals(other as SecP160R1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP160R1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 5));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat160.Create();
            Mod.Invert(SecP160R1Field.P, this.x, z);
            return new SecP160R1FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat160.Create();
            SecP160R1Field.Multiply(this.x, ((SecP160R1FieldElement) b).x, z);
            return new SecP160R1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat160.Create();
            SecP160R1Field.Negate(this.x, z);
            return new SecP160R1FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat160.IsZero(x) || Nat160.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat160.Create();
            SecP160R1Field.Square(x, z);
            SecP160R1Field.Multiply(z, x, z);
            uint[] numArray3 = Nat160.Create();
            SecP160R1Field.SquareN(z, 2, numArray3);
            SecP160R1Field.Multiply(numArray3, z, numArray3);
            uint[] numArray4 = z;
            SecP160R1Field.SquareN(numArray3, 4, numArray4);
            SecP160R1Field.Multiply(numArray4, numArray3, numArray4);
            uint[] numArray5 = numArray3;
            SecP160R1Field.SquareN(numArray4, 8, numArray5);
            SecP160R1Field.Multiply(numArray5, numArray4, numArray5);
            uint[] numArray6 = numArray4;
            SecP160R1Field.SquareN(numArray5, 0x10, numArray6);
            SecP160R1Field.Multiply(numArray6, numArray5, numArray6);
            uint[] numArray7 = numArray5;
            SecP160R1Field.SquareN(numArray6, 0x20, numArray7);
            SecP160R1Field.Multiply(numArray7, numArray6, numArray7);
            uint[] numArray8 = numArray6;
            SecP160R1Field.SquareN(numArray7, 0x40, numArray8);
            SecP160R1Field.Multiply(numArray8, numArray7, numArray8);
            uint[] numArray9 = numArray7;
            SecP160R1Field.Square(numArray8, numArray9);
            SecP160R1Field.Multiply(numArray9, x, numArray9);
            uint[] numArray10 = numArray9;
            SecP160R1Field.SquareN(numArray10, 0x1d, numArray10);
            uint[] numArray11 = numArray8;
            SecP160R1Field.Square(numArray10, numArray11);
            return (!Nat160.Eq(x, numArray11) ? null : new SecP160R1FieldElement(numArray10));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat160.Create();
            SecP160R1Field.Square(this.x, z);
            return new SecP160R1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat160.Create();
            SecP160R1Field.Subtract(this.x, ((SecP160R1FieldElement) b).x, z);
            return new SecP160R1FieldElement(z);
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
            "SecP160R1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

