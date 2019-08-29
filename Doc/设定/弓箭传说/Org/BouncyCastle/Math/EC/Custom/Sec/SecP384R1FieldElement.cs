namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP384R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP384R1Curve.q;
        protected internal readonly uint[] x;

        public SecP384R1FieldElement()
        {
            this.x = Nat.Create(12);
        }

        public SecP384R1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP384R1FieldElement", "x");
            }
            this.x = SecP384R1Field.FromBigInteger(x);
        }

        protected internal SecP384R1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat.Create(12);
            SecP384R1Field.Add(this.x, ((SecP384R1FieldElement) b).x, z);
            return new SecP384R1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat.Create(12);
            SecP384R1Field.AddOne(this.x, z);
            return new SecP384R1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat.Create(12);
            Mod.Invert(SecP384R1Field.P, ((SecP384R1FieldElement) b).x, z);
            SecP384R1Field.Multiply(z, this.x, z);
            return new SecP384R1FieldElement(z);
        }

        public virtual bool Equals(SecP384R1FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat.Eq(12, this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecP384R1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP384R1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 12));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat.Create(12);
            Mod.Invert(SecP384R1Field.P, this.x, z);
            return new SecP384R1FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat.Create(12);
            SecP384R1Field.Multiply(this.x, ((SecP384R1FieldElement) b).x, z);
            return new SecP384R1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat.Create(12);
            SecP384R1Field.Negate(this.x, z);
            return new SecP384R1FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat.IsZero(12, x) || Nat.IsOne(12, x))
            {
                return this;
            }
            uint[] z = Nat.Create(12);
            uint[] numArray3 = Nat.Create(12);
            uint[] numArray4 = Nat.Create(12);
            uint[] numArray5 = Nat.Create(12);
            SecP384R1Field.Square(x, z);
            SecP384R1Field.Multiply(z, x, z);
            SecP384R1Field.SquareN(z, 2, numArray3);
            SecP384R1Field.Multiply(numArray3, z, numArray3);
            SecP384R1Field.Square(numArray3, numArray3);
            SecP384R1Field.Multiply(numArray3, x, numArray3);
            SecP384R1Field.SquareN(numArray3, 5, numArray4);
            SecP384R1Field.Multiply(numArray4, numArray3, numArray4);
            SecP384R1Field.SquareN(numArray4, 5, numArray5);
            SecP384R1Field.Multiply(numArray5, numArray3, numArray5);
            SecP384R1Field.SquareN(numArray5, 15, numArray3);
            SecP384R1Field.Multiply(numArray3, numArray5, numArray3);
            SecP384R1Field.SquareN(numArray3, 2, numArray4);
            SecP384R1Field.Multiply(z, numArray4, z);
            SecP384R1Field.SquareN(numArray4, 0x1c, numArray4);
            SecP384R1Field.Multiply(numArray3, numArray4, numArray3);
            SecP384R1Field.SquareN(numArray3, 60, numArray4);
            SecP384R1Field.Multiply(numArray4, numArray3, numArray4);
            uint[] numArray6 = numArray3;
            SecP384R1Field.SquareN(numArray4, 120, numArray6);
            SecP384R1Field.Multiply(numArray6, numArray4, numArray6);
            SecP384R1Field.SquareN(numArray6, 15, numArray6);
            SecP384R1Field.Multiply(numArray6, numArray5, numArray6);
            SecP384R1Field.SquareN(numArray6, 0x21, numArray6);
            SecP384R1Field.Multiply(numArray6, z, numArray6);
            SecP384R1Field.SquareN(numArray6, 0x40, numArray6);
            SecP384R1Field.Multiply(numArray6, x, numArray6);
            SecP384R1Field.SquareN(numArray6, 30, z);
            SecP384R1Field.Square(z, numArray3);
            return (!Nat.Eq(12, x, numArray3) ? null : new SecP384R1FieldElement(z));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat.Create(12);
            SecP384R1Field.Square(this.x, z);
            return new SecP384R1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat.Create(12);
            SecP384R1Field.Subtract(this.x, ((SecP384R1FieldElement) b).x, z);
            return new SecP384R1FieldElement(z);
        }

        public override bool TestBitZero() => 
            (Nat.GetBit(this.x, 0) == 1);

        public override BigInteger ToBigInteger() => 
            Nat.ToBigInteger(12, this.x);

        public override bool IsZero =>
            Nat.IsZero(12, this.x);

        public override bool IsOne =>
            Nat.IsOne(12, this.x);

        public override string FieldName =>
            "SecP384R1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

