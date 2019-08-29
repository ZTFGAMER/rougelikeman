namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP521R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP521R1Curve.q;
        protected internal readonly uint[] x;

        public SecP521R1FieldElement()
        {
            this.x = Nat.Create(0x11);
        }

        public SecP521R1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP521R1FieldElement", "x");
            }
            this.x = SecP521R1Field.FromBigInteger(x);
        }

        protected internal SecP521R1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat.Create(0x11);
            SecP521R1Field.Add(this.x, ((SecP521R1FieldElement) b).x, z);
            return new SecP521R1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat.Create(0x11);
            SecP521R1Field.AddOne(this.x, z);
            return new SecP521R1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat.Create(0x11);
            Mod.Invert(SecP521R1Field.P, ((SecP521R1FieldElement) b).x, z);
            SecP521R1Field.Multiply(z, this.x, z);
            return new SecP521R1FieldElement(z);
        }

        public virtual bool Equals(SecP521R1FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat.Eq(0x11, this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecP521R1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP521R1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 0x11));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat.Create(0x11);
            Mod.Invert(SecP521R1Field.P, this.x, z);
            return new SecP521R1FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat.Create(0x11);
            SecP521R1Field.Multiply(this.x, ((SecP521R1FieldElement) b).x, z);
            return new SecP521R1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat.Create(0x11);
            SecP521R1Field.Negate(this.x, z);
            return new SecP521R1FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat.IsZero(0x11, x) || Nat.IsOne(0x11, x))
            {
                return this;
            }
            uint[] z = Nat.Create(0x11);
            uint[] numArray3 = Nat.Create(0x11);
            SecP521R1Field.SquareN(x, 0x207, z);
            SecP521R1Field.Square(z, numArray3);
            return (!Nat.Eq(0x11, x, numArray3) ? null : new SecP521R1FieldElement(z));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat.Create(0x11);
            SecP521R1Field.Square(this.x, z);
            return new SecP521R1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat.Create(0x11);
            SecP521R1Field.Subtract(this.x, ((SecP521R1FieldElement) b).x, z);
            return new SecP521R1FieldElement(z);
        }

        public override bool TestBitZero() => 
            (Nat.GetBit(this.x, 0) == 1);

        public override BigInteger ToBigInteger() => 
            Nat.ToBigInteger(0x11, this.x);

        public override bool IsZero =>
            Nat.IsZero(0x11, this.x);

        public override bool IsOne =>
            Nat.IsOne(0x11, this.x);

        public override string FieldName =>
            "SecP521R1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

