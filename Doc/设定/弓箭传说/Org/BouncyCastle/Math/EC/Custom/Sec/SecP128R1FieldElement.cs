namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP128R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP128R1Curve.q;
        protected internal readonly uint[] x;

        public SecP128R1FieldElement()
        {
            this.x = Nat128.Create();
        }

        public SecP128R1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP128R1FieldElement", "x");
            }
            this.x = SecP128R1Field.FromBigInteger(x);
        }

        protected internal SecP128R1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat128.Create();
            SecP128R1Field.Add(this.x, ((SecP128R1FieldElement) b).x, z);
            return new SecP128R1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat128.Create();
            SecP128R1Field.AddOne(this.x, z);
            return new SecP128R1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat128.Create();
            Mod.Invert(SecP128R1Field.P, ((SecP128R1FieldElement) b).x, z);
            SecP128R1Field.Multiply(z, this.x, z);
            return new SecP128R1FieldElement(z);
        }

        public virtual bool Equals(SecP128R1FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat128.Eq(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecP128R1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP128R1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 4));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat128.Create();
            Mod.Invert(SecP128R1Field.P, this.x, z);
            return new SecP128R1FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat128.Create();
            SecP128R1Field.Multiply(this.x, ((SecP128R1FieldElement) b).x, z);
            return new SecP128R1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat128.Create();
            SecP128R1Field.Negate(this.x, z);
            return new SecP128R1FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat128.IsZero(x) || Nat128.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat128.Create();
            SecP128R1Field.Square(x, z);
            SecP128R1Field.Multiply(z, x, z);
            uint[] numArray3 = Nat128.Create();
            SecP128R1Field.SquareN(z, 2, numArray3);
            SecP128R1Field.Multiply(numArray3, z, numArray3);
            uint[] numArray4 = Nat128.Create();
            SecP128R1Field.SquareN(numArray3, 4, numArray4);
            SecP128R1Field.Multiply(numArray4, numArray3, numArray4);
            uint[] numArray5 = numArray3;
            SecP128R1Field.SquareN(numArray4, 2, numArray5);
            SecP128R1Field.Multiply(numArray5, z, numArray5);
            uint[] numArray6 = z;
            SecP128R1Field.SquareN(numArray5, 10, numArray6);
            SecP128R1Field.Multiply(numArray6, numArray5, numArray6);
            uint[] numArray7 = numArray4;
            SecP128R1Field.SquareN(numArray6, 10, numArray7);
            SecP128R1Field.Multiply(numArray7, numArray5, numArray7);
            uint[] numArray8 = numArray5;
            SecP128R1Field.Square(numArray7, numArray8);
            SecP128R1Field.Multiply(numArray8, x, numArray8);
            uint[] numArray9 = numArray8;
            SecP128R1Field.SquareN(numArray9, 0x5f, numArray9);
            uint[] numArray10 = numArray7;
            SecP128R1Field.Square(numArray9, numArray10);
            return (!Nat128.Eq(x, numArray10) ? null : new SecP128R1FieldElement(numArray9));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat128.Create();
            SecP128R1Field.Square(this.x, z);
            return new SecP128R1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat128.Create();
            SecP128R1Field.Subtract(this.x, ((SecP128R1FieldElement) b).x, z);
            return new SecP128R1FieldElement(z);
        }

        public override bool TestBitZero() => 
            (Nat128.GetBit(this.x, 0) == 1);

        public override BigInteger ToBigInteger() => 
            Nat128.ToBigInteger(this.x);

        public override bool IsZero =>
            Nat128.IsZero(this.x);

        public override bool IsOne =>
            Nat128.IsOne(this.x);

        public override string FieldName =>
            "SecP128R1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

