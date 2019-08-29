namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP224K1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP224K1Curve.q;
        private static readonly uint[] PRECOMP_POW2 = new uint[] { 0x33bfd202, 0xdcfad133, 0x2287624a, 0xc3811ba8, 0xa85558fc, 0x1eaef5d7, 0x8edf154c };
        protected internal readonly uint[] x;

        public SecP224K1FieldElement()
        {
            this.x = Nat224.Create();
        }

        public SecP224K1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP224K1FieldElement", "x");
            }
            this.x = SecP224K1Field.FromBigInteger(x);
        }

        protected internal SecP224K1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat224.Create();
            SecP224K1Field.Add(this.x, ((SecP224K1FieldElement) b).x, z);
            return new SecP224K1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat224.Create();
            SecP224K1Field.AddOne(this.x, z);
            return new SecP224K1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat224.Create();
            Mod.Invert(SecP224K1Field.P, ((SecP224K1FieldElement) b).x, z);
            SecP224K1Field.Multiply(z, this.x, z);
            return new SecP224K1FieldElement(z);
        }

        public virtual bool Equals(SecP224K1FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat224.Eq(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecP224K1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP224K1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 7));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat224.Create();
            Mod.Invert(SecP224K1Field.P, this.x, z);
            return new SecP224K1FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat224.Create();
            SecP224K1Field.Multiply(this.x, ((SecP224K1FieldElement) b).x, z);
            return new SecP224K1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat224.Create();
            SecP224K1Field.Negate(this.x, z);
            return new SecP224K1FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat224.IsZero(x) || Nat224.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat224.Create();
            SecP224K1Field.Square(x, z);
            SecP224K1Field.Multiply(z, x, z);
            uint[] numArray3 = z;
            SecP224K1Field.Square(z, numArray3);
            SecP224K1Field.Multiply(numArray3, x, numArray3);
            uint[] numArray4 = Nat224.Create();
            SecP224K1Field.Square(numArray3, numArray4);
            SecP224K1Field.Multiply(numArray4, x, numArray4);
            uint[] numArray5 = Nat224.Create();
            SecP224K1Field.SquareN(numArray4, 4, numArray5);
            SecP224K1Field.Multiply(numArray5, numArray4, numArray5);
            uint[] numArray6 = Nat224.Create();
            SecP224K1Field.SquareN(numArray5, 3, numArray6);
            SecP224K1Field.Multiply(numArray6, numArray3, numArray6);
            uint[] numArray7 = numArray6;
            SecP224K1Field.SquareN(numArray6, 8, numArray7);
            SecP224K1Field.Multiply(numArray7, numArray5, numArray7);
            uint[] numArray8 = numArray5;
            SecP224K1Field.SquareN(numArray7, 4, numArray8);
            SecP224K1Field.Multiply(numArray8, numArray4, numArray8);
            uint[] numArray9 = numArray4;
            SecP224K1Field.SquareN(numArray8, 0x13, numArray9);
            SecP224K1Field.Multiply(numArray9, numArray7, numArray9);
            uint[] numArray10 = Nat224.Create();
            SecP224K1Field.SquareN(numArray9, 0x2a, numArray10);
            SecP224K1Field.Multiply(numArray10, numArray9, numArray10);
            uint[] numArray11 = numArray9;
            SecP224K1Field.SquareN(numArray10, 0x17, numArray11);
            SecP224K1Field.Multiply(numArray11, numArray8, numArray11);
            uint[] numArray12 = numArray8;
            SecP224K1Field.SquareN(numArray11, 0x54, numArray12);
            SecP224K1Field.Multiply(numArray12, numArray10, numArray12);
            uint[] numArray13 = numArray12;
            SecP224K1Field.SquareN(numArray13, 20, numArray13);
            SecP224K1Field.Multiply(numArray13, numArray7, numArray13);
            SecP224K1Field.SquareN(numArray13, 3, numArray13);
            SecP224K1Field.Multiply(numArray13, x, numArray13);
            SecP224K1Field.SquareN(numArray13, 2, numArray13);
            SecP224K1Field.Multiply(numArray13, x, numArray13);
            SecP224K1Field.SquareN(numArray13, 4, numArray13);
            SecP224K1Field.Multiply(numArray13, numArray3, numArray13);
            SecP224K1Field.Square(numArray13, numArray13);
            uint[] numArray14 = numArray10;
            SecP224K1Field.Square(numArray13, numArray14);
            if (Nat224.Eq(x, numArray14))
            {
                return new SecP224K1FieldElement(numArray13);
            }
            SecP224K1Field.Multiply(numArray13, PRECOMP_POW2, numArray13);
            SecP224K1Field.Square(numArray13, numArray14);
            if (Nat224.Eq(x, numArray14))
            {
                return new SecP224K1FieldElement(numArray13);
            }
            return null;
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat224.Create();
            SecP224K1Field.Square(this.x, z);
            return new SecP224K1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat224.Create();
            SecP224K1Field.Subtract(this.x, ((SecP224K1FieldElement) b).x, z);
            return new SecP224K1FieldElement(z);
        }

        public override bool TestBitZero() => 
            (Nat224.GetBit(this.x, 0) == 1);

        public override BigInteger ToBigInteger() => 
            Nat224.ToBigInteger(this.x);

        public override bool IsZero =>
            Nat224.IsZero(this.x);

        public override bool IsOne =>
            Nat224.IsOne(this.x);

        public override string FieldName =>
            "SecP224K1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

