namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP256K1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP256K1Curve.q;
        protected internal readonly uint[] x;

        public SecP256K1FieldElement()
        {
            this.x = Nat256.Create();
        }

        public SecP256K1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP256K1FieldElement", "x");
            }
            this.x = SecP256K1Field.FromBigInteger(x);
        }

        protected internal SecP256K1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            SecP256K1Field.Add(this.x, ((SecP256K1FieldElement) b).x, z);
            return new SecP256K1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat256.Create();
            SecP256K1Field.AddOne(this.x, z);
            return new SecP256K1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            Mod.Invert(SecP256K1Field.P, ((SecP256K1FieldElement) b).x, z);
            SecP256K1Field.Multiply(z, this.x, z);
            return new SecP256K1FieldElement(z);
        }

        public virtual bool Equals(SecP256K1FieldElement other)
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
            this.Equals(other as SecP256K1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP256K1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat256.Create();
            Mod.Invert(SecP256K1Field.P, this.x, z);
            return new SecP256K1FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            SecP256K1Field.Multiply(this.x, ((SecP256K1FieldElement) b).x, z);
            return new SecP256K1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat256.Create();
            SecP256K1Field.Negate(this.x, z);
            return new SecP256K1FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat256.IsZero(x) || Nat256.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat256.Create();
            SecP256K1Field.Square(x, z);
            SecP256K1Field.Multiply(z, x, z);
            uint[] numArray3 = Nat256.Create();
            SecP256K1Field.Square(z, numArray3);
            SecP256K1Field.Multiply(numArray3, x, numArray3);
            uint[] numArray4 = Nat256.Create();
            SecP256K1Field.SquareN(numArray3, 3, numArray4);
            SecP256K1Field.Multiply(numArray4, numArray3, numArray4);
            uint[] numArray5 = numArray4;
            SecP256K1Field.SquareN(numArray4, 3, numArray5);
            SecP256K1Field.Multiply(numArray5, numArray3, numArray5);
            uint[] numArray6 = numArray5;
            SecP256K1Field.SquareN(numArray5, 2, numArray6);
            SecP256K1Field.Multiply(numArray6, z, numArray6);
            uint[] numArray7 = Nat256.Create();
            SecP256K1Field.SquareN(numArray6, 11, numArray7);
            SecP256K1Field.Multiply(numArray7, numArray6, numArray7);
            uint[] numArray8 = numArray6;
            SecP256K1Field.SquareN(numArray7, 0x16, numArray8);
            SecP256K1Field.Multiply(numArray8, numArray7, numArray8);
            uint[] numArray9 = Nat256.Create();
            SecP256K1Field.SquareN(numArray8, 0x2c, numArray9);
            SecP256K1Field.Multiply(numArray9, numArray8, numArray9);
            uint[] numArray10 = Nat256.Create();
            SecP256K1Field.SquareN(numArray9, 0x58, numArray10);
            SecP256K1Field.Multiply(numArray10, numArray9, numArray10);
            uint[] numArray11 = numArray9;
            SecP256K1Field.SquareN(numArray10, 0x2c, numArray11);
            SecP256K1Field.Multiply(numArray11, numArray8, numArray11);
            uint[] numArray12 = numArray8;
            SecP256K1Field.SquareN(numArray11, 3, numArray12);
            SecP256K1Field.Multiply(numArray12, numArray3, numArray12);
            uint[] numArray13 = numArray12;
            SecP256K1Field.SquareN(numArray13, 0x17, numArray13);
            SecP256K1Field.Multiply(numArray13, numArray7, numArray13);
            SecP256K1Field.SquareN(numArray13, 6, numArray13);
            SecP256K1Field.Multiply(numArray13, z, numArray13);
            SecP256K1Field.SquareN(numArray13, 2, numArray13);
            uint[] numArray14 = z;
            SecP256K1Field.Square(numArray13, numArray14);
            return (!Nat256.Eq(x, numArray14) ? null : new SecP256K1FieldElement(numArray13));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat256.Create();
            SecP256K1Field.Square(this.x, z);
            return new SecP256K1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            SecP256K1Field.Subtract(this.x, ((SecP256K1FieldElement) b).x, z);
            return new SecP256K1FieldElement(z);
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
            "SecP256K1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

