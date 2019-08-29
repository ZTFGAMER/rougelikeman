namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecT193FieldElement : ECFieldElement
    {
        protected readonly ulong[] x;

        public SecT193FieldElement()
        {
            this.x = Nat256.Create64();
        }

        public SecT193FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.BitLength > 0xc1))
            {
                throw new ArgumentException("value invalid for SecT193FieldElement", "x");
            }
            this.x = SecT193Field.FromBigInteger(x);
        }

        protected internal SecT193FieldElement(ulong[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            ulong[] z = Nat256.Create64();
            SecT193Field.Add(this.x, ((SecT193FieldElement) b).x, z);
            return new SecT193FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            ulong[] z = Nat256.Create64();
            SecT193Field.AddOne(this.x, z);
            return new SecT193FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b) => 
            this.Multiply(b.Invert());

        public virtual bool Equals(SecT193FieldElement other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return Nat256.Eq64(this.x, other.x);
        }

        public override bool Equals(ECFieldElement other) => 
            this.Equals(other as SecT193FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecT193FieldElement);

        public override int GetHashCode() => 
            (0x1d731f ^ Arrays.GetHashCode(this.x, 0, 4));

        public override ECFieldElement Invert()
        {
            ulong[] z = Nat256.Create64();
            SecT193Field.Invert(this.x, z);
            return new SecT193FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            ulong[] z = Nat256.Create64();
            SecT193Field.Multiply(this.x, ((SecT193FieldElement) b).x, z);
            return new SecT193FieldElement(z);
        }

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.MultiplyPlusProduct(b, x, y);

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT193FieldElement) b).x;
            ulong[] numArray3 = ((SecT193FieldElement) x).x;
            ulong[] numArray4 = ((SecT193FieldElement) y).x;
            ulong[] zz = Nat256.CreateExt64();
            SecT193Field.MultiplyAddToExt(numArray, numArray2, zz);
            SecT193Field.MultiplyAddToExt(numArray3, numArray4, zz);
            ulong[] z = Nat256.Create64();
            SecT193Field.Reduce(zz, z);
            return new SecT193FieldElement(z);
        }

        public override ECFieldElement Negate() => 
            this;

        public override ECFieldElement Sqrt()
        {
            ulong[] z = Nat256.Create64();
            SecT193Field.Sqrt(this.x, z);
            return new SecT193FieldElement(z);
        }

        public override ECFieldElement Square()
        {
            ulong[] z = Nat256.Create64();
            SecT193Field.Square(this.x, z);
            return new SecT193FieldElement(z);
        }

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y) => 
            this.SquarePlusProduct(x, y);

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT193FieldElement) x).x;
            ulong[] numArray3 = ((SecT193FieldElement) y).x;
            ulong[] zz = Nat256.CreateExt64();
            SecT193Field.SquareAddToExt(numArray, zz);
            SecT193Field.MultiplyAddToExt(numArray2, numArray3, zz);
            ulong[] z = Nat256.Create64();
            SecT193Field.Reduce(zz, z);
            return new SecT193FieldElement(z);
        }

        public override ECFieldElement SquarePow(int pow)
        {
            if (pow < 1)
            {
                return this;
            }
            ulong[] z = Nat256.Create64();
            SecT193Field.SquareN(this.x, pow, z);
            return new SecT193FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b) => 
            this.Add(b);

        public override bool TestBitZero() => 
            ((this.x[0] & ((ulong) 1L)) != 0L);

        public override BigInteger ToBigInteger() => 
            Nat256.ToBigInteger64(this.x);

        public override bool IsOne =>
            Nat256.IsOne64(this.x);

        public override bool IsZero =>
            Nat256.IsZero64(this.x);

        public override string FieldName =>
            "SecT193Field";

        public override int FieldSize =>
            0xc1;

        public virtual int Representation =>
            2;

        public virtual int M =>
            0xc1;

        public virtual int K1 =>
            15;

        public virtual int K2 =>
            0;

        public virtual int K3 =>
            0;
    }
}

