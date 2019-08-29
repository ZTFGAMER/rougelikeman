namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP256R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP256R1Curve.q;
        protected internal readonly uint[] x;

        public SecP256R1FieldElement()
        {
            this.x = Nat256.Create();
        }

        public SecP256R1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP256R1FieldElement", "x");
            }
            this.x = SecP256R1Field.FromBigInteger(x);
        }

        protected internal SecP256R1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            SecP256R1Field.Add(this.x, ((SecP256R1FieldElement) b).x, z);
            return new SecP256R1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat256.Create();
            SecP256R1Field.AddOne(this.x, z);
            return new SecP256R1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            Mod.Invert(SecP256R1Field.P, ((SecP256R1FieldElement) b).x, z);
            SecP256R1Field.Multiply(z, this.x, z);
            return new SecP256R1FieldElement(z);
        }

        public virtual bool Equals(SecP256R1FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat256.Eq(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecP256R1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP256R1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat256.Create();
            Mod.Invert(SecP256R1Field.P, this.x, z);
            return new SecP256R1FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            SecP256R1Field.Multiply(this.x, ((SecP256R1FieldElement) b).x, z);
            return new SecP256R1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat256.Create();
            SecP256R1Field.Negate(this.x, z);
            return new SecP256R1FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat256.IsZero(x) || Nat256.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat256.Create();
            uint[] numArray3 = Nat256.Create();
            SecP256R1Field.Square(x, z);
            SecP256R1Field.Multiply(z, x, z);
            SecP256R1Field.SquareN(z, 2, numArray3);
            SecP256R1Field.Multiply(numArray3, z, numArray3);
            SecP256R1Field.SquareN(numArray3, 4, z);
            SecP256R1Field.Multiply(z, numArray3, z);
            SecP256R1Field.SquareN(z, 8, numArray3);
            SecP256R1Field.Multiply(numArray3, z, numArray3);
            SecP256R1Field.SquareN(numArray3, 0x10, z);
            SecP256R1Field.Multiply(z, numArray3, z);
            SecP256R1Field.SquareN(z, 0x20, z);
            SecP256R1Field.Multiply(z, x, z);
            SecP256R1Field.SquareN(z, 0x60, z);
            SecP256R1Field.Multiply(z, x, z);
            SecP256R1Field.SquareN(z, 0x5e, z);
            SecP256R1Field.Multiply(z, z, numArray3);
            return (!Nat256.Eq(x, numArray3) ? null : new SecP256R1FieldElement(z));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat256.Create();
            SecP256R1Field.Square(this.x, z);
            return new SecP256R1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            SecP256R1Field.Subtract(this.x, ((SecP256R1FieldElement) b).x, z);
            return new SecP256R1FieldElement(z);
        }

        public override bool TestBitZero() => 
            (Nat256.GetBit(this.x, 0) == 1);

        public override BigInteger ToBigInteger() => 
            Nat256.ToBigInteger(this.x);

        public override bool IsZero =>
            Nat256.IsZero(this.x);

        public override bool IsOne =>
            Nat256.IsOne(this.x);

        public override string FieldName =>
            "SecP256R1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

