namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP192K1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP192K1Curve.q;
        protected internal readonly uint[] x;

        public SecP192K1FieldElement()
        {
            this.x = Nat192.Create();
        }

        public SecP192K1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP192K1FieldElement", "x");
            }
            this.x = SecP192K1Field.FromBigInteger(x);
        }

        protected internal SecP192K1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat192.Create();
            SecP192K1Field.Add(this.x, ((SecP192K1FieldElement) b).x, z);
            return new SecP192K1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat192.Create();
            SecP192K1Field.AddOne(this.x, z);
            return new SecP192K1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat192.Create();
            Mod.Invert(SecP192K1Field.P, ((SecP192K1FieldElement) b).x, z);
            SecP192K1Field.Multiply(z, this.x, z);
            return new SecP192K1FieldElement(z);
        }

        public virtual bool Equals(SecP192K1FieldElement other)
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
            this.Equals(other as SecP192K1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP192K1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 6));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat192.Create();
            Mod.Invert(SecP192K1Field.P, this.x, z);
            return new SecP192K1FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat192.Create();
            SecP192K1Field.Multiply(this.x, ((SecP192K1FieldElement) b).x, z);
            return new SecP192K1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat192.Create();
            SecP192K1Field.Negate(this.x, z);
            return new SecP192K1FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat192.IsZero(x) || Nat192.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat192.Create();
            SecP192K1Field.Square(x, z);
            SecP192K1Field.Multiply(z, x, z);
            uint[] numArray3 = Nat192.Create();
            SecP192K1Field.Square(z, numArray3);
            SecP192K1Field.Multiply(numArray3, x, numArray3);
            uint[] numArray4 = Nat192.Create();
            SecP192K1Field.SquareN(numArray3, 3, numArray4);
            SecP192K1Field.Multiply(numArray4, numArray3, numArray4);
            uint[] numArray5 = numArray4;
            SecP192K1Field.SquareN(numArray4, 2, numArray5);
            SecP192K1Field.Multiply(numArray5, z, numArray5);
            uint[] numArray6 = z;
            SecP192K1Field.SquareN(numArray5, 8, numArray6);
            SecP192K1Field.Multiply(numArray6, numArray5, numArray6);
            uint[] numArray7 = numArray5;
            SecP192K1Field.SquareN(numArray6, 3, numArray7);
            SecP192K1Field.Multiply(numArray7, numArray3, numArray7);
            uint[] numArray8 = Nat192.Create();
            SecP192K1Field.SquareN(numArray7, 0x10, numArray8);
            SecP192K1Field.Multiply(numArray8, numArray6, numArray8);
            uint[] numArray9 = numArray6;
            SecP192K1Field.SquareN(numArray8, 0x23, numArray9);
            SecP192K1Field.Multiply(numArray9, numArray8, numArray9);
            uint[] numArray10 = numArray8;
            SecP192K1Field.SquareN(numArray9, 70, numArray10);
            SecP192K1Field.Multiply(numArray10, numArray9, numArray10);
            uint[] numArray11 = numArray9;
            SecP192K1Field.SquareN(numArray10, 0x13, numArray11);
            SecP192K1Field.Multiply(numArray11, numArray7, numArray11);
            uint[] numArray12 = numArray11;
            SecP192K1Field.SquareN(numArray12, 20, numArray12);
            SecP192K1Field.Multiply(numArray12, numArray7, numArray12);
            SecP192K1Field.SquareN(numArray12, 4, numArray12);
            SecP192K1Field.Multiply(numArray12, numArray3, numArray12);
            SecP192K1Field.SquareN(numArray12, 6, numArray12);
            SecP192K1Field.Multiply(numArray12, numArray3, numArray12);
            SecP192K1Field.Square(numArray12, numArray12);
            uint[] numArray13 = numArray3;
            SecP192K1Field.Square(numArray12, numArray13);
            return (!Nat192.Eq(x, numArray13) ? null : new SecP192K1FieldElement(numArray12));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat192.Create();
            SecP192K1Field.Square(this.x, z);
            return new SecP192K1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat192.Create();
            SecP192K1Field.Subtract(this.x, ((SecP192K1FieldElement) b).x, z);
            return new SecP192K1FieldElement(z);
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
            "SecP192K1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

