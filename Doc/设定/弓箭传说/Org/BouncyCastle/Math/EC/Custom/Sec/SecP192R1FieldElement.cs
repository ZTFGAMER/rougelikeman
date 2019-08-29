namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP192R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP192R1Curve.q;
        protected internal readonly uint[] x;

        public SecP192R1FieldElement()
        {
            this.x = Nat192.Create();
        }

        public SecP192R1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP192R1FieldElement", "x");
            }
            this.x = SecP192R1Field.FromBigInteger(x);
        }

        protected internal SecP192R1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat192.Create();
            SecP192R1Field.Add(this.x, ((SecP192R1FieldElement) b).x, z);
            return new SecP192R1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat192.Create();
            SecP192R1Field.AddOne(this.x, z);
            return new SecP192R1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat192.Create();
            Mod.Invert(SecP192R1Field.P, ((SecP192R1FieldElement) b).x, z);
            SecP192R1Field.Multiply(z, this.x, z);
            return new SecP192R1FieldElement(z);
        }

        public virtual bool Equals(SecP192R1FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat192.Eq(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecP192R1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP192R1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 6));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat192.Create();
            Mod.Invert(SecP192R1Field.P, this.x, z);
            return new SecP192R1FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat192.Create();
            SecP192R1Field.Multiply(this.x, ((SecP192R1FieldElement) b).x, z);
            return new SecP192R1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat192.Create();
            SecP192R1Field.Negate(this.x, z);
            return new SecP192R1FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat192.IsZero(x) || Nat192.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat192.Create();
            uint[] numArray3 = Nat192.Create();
            SecP192R1Field.Square(x, z);
            SecP192R1Field.Multiply(z, x, z);
            SecP192R1Field.SquareN(z, 2, numArray3);
            SecP192R1Field.Multiply(numArray3, z, numArray3);
            SecP192R1Field.SquareN(numArray3, 4, z);
            SecP192R1Field.Multiply(z, numArray3, z);
            SecP192R1Field.SquareN(z, 8, numArray3);
            SecP192R1Field.Multiply(numArray3, z, numArray3);
            SecP192R1Field.SquareN(numArray3, 0x10, z);
            SecP192R1Field.Multiply(z, numArray3, z);
            SecP192R1Field.SquareN(z, 0x20, numArray3);
            SecP192R1Field.Multiply(numArray3, z, numArray3);
            SecP192R1Field.SquareN(numArray3, 0x40, z);
            SecP192R1Field.Multiply(z, numArray3, z);
            SecP192R1Field.SquareN(z, 0x3e, z);
            SecP192R1Field.Square(z, numArray3);
            return (!Nat192.Eq(x, numArray3) ? null : new SecP192R1FieldElement(z));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat192.Create();
            SecP192R1Field.Square(this.x, z);
            return new SecP192R1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat192.Create();
            SecP192R1Field.Subtract(this.x, ((SecP192R1FieldElement) b).x, z);
            return new SecP192R1FieldElement(z);
        }

        public override bool TestBitZero() => 
            (Nat192.GetBit(this.x, 0) == 1);

        public override BigInteger ToBigInteger() => 
            Nat192.ToBigInteger(this.x);

        public override bool IsZero =>
            Nat192.IsZero(this.x);

        public override bool IsOne =>
            Nat192.IsOne(this.x);

        public override string FieldName =>
            "SecP192R1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

