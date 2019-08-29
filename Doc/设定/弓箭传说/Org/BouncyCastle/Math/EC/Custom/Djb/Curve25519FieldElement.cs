namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class Curve25519FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = Curve25519.q;
        private static readonly uint[] PRECOMP_POW2 = new uint[] { 0x4a0ea0b0, 0xc4ee1b27, 0xad2fe478, 0x2f431806, 0x3dfbd7a7, 0x2b4d0099, 0x4fc1df0b, 0x2b832480 };
        protected internal readonly uint[] x;

        public Curve25519FieldElement()
        {
            this.x = Nat256.Create();
        }

        public Curve25519FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for Curve25519FieldElement", "x");
            }
            this.x = Curve25519Field.FromBigInteger(x);
        }

        protected internal Curve25519FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            Curve25519Field.Add(this.x, ((Curve25519FieldElement) b).x, z);
            return new Curve25519FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat256.Create();
            Curve25519Field.AddOne(this.x, z);
            return new Curve25519FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            Mod.Invert(Curve25519Field.P, ((Curve25519FieldElement) b).x, z);
            Curve25519Field.Multiply(z, this.x, z);
            return new Curve25519FieldElement(z);
        }

        public virtual bool Equals(Curve25519FieldElement other)
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
            this.Equals(other as Curve25519FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as Curve25519FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat256.Create();
            Mod.Invert(Curve25519Field.P, this.x, z);
            return new Curve25519FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            Curve25519Field.Multiply(this.x, ((Curve25519FieldElement) b).x, z);
            return new Curve25519FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat256.Create();
            Curve25519Field.Negate(this.x, z);
            return new Curve25519FieldElement(z);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat256.IsZero(x) || Nat256.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat256.Create();
            Curve25519Field.Square(x, z);
            Curve25519Field.Multiply(z, x, z);
            uint[] numArray3 = z;
            Curve25519Field.Square(z, numArray3);
            Curve25519Field.Multiply(numArray3, x, numArray3);
            uint[] numArray4 = Nat256.Create();
            Curve25519Field.Square(numArray3, numArray4);
            Curve25519Field.Multiply(numArray4, x, numArray4);
            uint[] numArray5 = Nat256.Create();
            Curve25519Field.SquareN(numArray4, 3, numArray5);
            Curve25519Field.Multiply(numArray5, numArray3, numArray5);
            uint[] numArray6 = numArray3;
            Curve25519Field.SquareN(numArray5, 4, numArray6);
            Curve25519Field.Multiply(numArray6, numArray4, numArray6);
            uint[] numArray7 = numArray5;
            Curve25519Field.SquareN(numArray6, 4, numArray7);
            Curve25519Field.Multiply(numArray7, numArray4, numArray7);
            uint[] numArray8 = numArray4;
            Curve25519Field.SquareN(numArray7, 15, numArray8);
            Curve25519Field.Multiply(numArray8, numArray7, numArray8);
            uint[] numArray9 = numArray7;
            Curve25519Field.SquareN(numArray8, 30, numArray9);
            Curve25519Field.Multiply(numArray9, numArray8, numArray9);
            uint[] numArray10 = numArray8;
            Curve25519Field.SquareN(numArray9, 60, numArray10);
            Curve25519Field.Multiply(numArray10, numArray9, numArray10);
            uint[] numArray11 = numArray9;
            Curve25519Field.SquareN(numArray10, 11, numArray11);
            Curve25519Field.Multiply(numArray11, numArray6, numArray11);
            uint[] numArray12 = numArray6;
            Curve25519Field.SquareN(numArray11, 120, numArray12);
            Curve25519Field.Multiply(numArray12, numArray10, numArray12);
            uint[] numArray13 = numArray12;
            Curve25519Field.Square(numArray13, numArray13);
            uint[] numArray14 = numArray10;
            Curve25519Field.Square(numArray13, numArray14);
            if (Nat256.Eq(x, numArray14))
            {
                return new Curve25519FieldElement(numArray13);
            }
            Curve25519Field.Multiply(numArray13, PRECOMP_POW2, numArray13);
            Curve25519Field.Square(numArray13, numArray14);
            if (Nat256.Eq(x, numArray14))
            {
                return new Curve25519FieldElement(numArray13);
            }
            return null;
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat256.Create();
            Curve25519Field.Square(this.x, z);
            return new Curve25519FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat256.Create();
            Curve25519Field.Subtract(this.x, ((Curve25519FieldElement) b).x, z);
            return new Curve25519FieldElement(z);
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
            "Curve25519Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

