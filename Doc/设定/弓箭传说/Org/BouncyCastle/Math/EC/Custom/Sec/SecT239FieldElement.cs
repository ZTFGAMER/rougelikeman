namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecT239FieldElement : ECFieldElement
    {
        protected ulong[] x;

        public SecT239FieldElement()
        {
            this.x = Nat256.Create64();
        }

        public SecT239FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.BitLength > 0xef))
            {
                throw new ArgumentException("value invalid for SecT239FieldElement", "x");
            }
            this.x = SecT239Field.FromBigInteger(x);
        }

        protected internal SecT239FieldElement(ulong[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            ulong[] z = Nat256.Create64();
            SecT239Field.Add(this.x, ((SecT239FieldElement) b).x, z);
            return new SecT239FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            ulong[] z = Nat256.Create64();
            SecT239Field.AddOne(this.x, z);
            return new SecT239FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b) => 
            this.Multiply(b.Invert());

        public virtual bool Equals(SecT239FieldElement other)
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
            this.Equals(other as SecT239FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecT239FieldElement);

        public override int GetHashCode() => 
            (0x16caffe ^ Arrays.GetHashCode(this.x, 0, 4));

        public override ECFieldElement Invert()
        {
            ulong[] z = Nat256.Create64();
            SecT239Field.Invert(this.x, z);
            return new SecT239FieldElement(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            ulong[] z = Nat256.Create64();
            SecT239Field.Multiply(this.x, ((SecT239FieldElement) b).x, z);
            return new SecT239FieldElement(z);
        }

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.MultiplyPlusProduct(b, x, y);

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT239FieldElement) b).x;
            ulong[] numArray3 = ((SecT239FieldElement) x).x;
            ulong[] numArray4 = ((SecT239FieldElement) y).x;
            ulong[] zz = Nat256.CreateExt64();
            SecT239Field.MultiplyAddToExt(numArray, numArray2, zz);
            SecT239Field.MultiplyAddToExt(numArray3, numArray4, zz);
            ulong[] z = Nat256.Create64();
            SecT239Field.Reduce(zz, z);
            return new SecT239FieldElement(z);
        }

        public override ECFieldElement Negate() => 
            this;

        public override ECFieldElement Sqrt()
        {
            ulong[] z = Nat256.Create64();
            SecT239Field.Sqrt(this.x, z);
            return new SecT239FieldElement(z);
        }

        public override ECFieldElement Square()
        {
            ulong[] z = Nat256.Create64();
            SecT239Field.Square(this.x, z);
            return new SecT239FieldElement(z);
        }

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y) => 
            this.SquarePlusProduct(x, y);

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            ulong[] numArray = this.x;
            ulong[] numArray2 = ((SecT239FieldElement) x).x;
            ulong[] numArray3 = ((SecT239FieldElement) y).x;
            ulong[] zz = Nat256.CreateExt64();
            SecT239Field.SquareAddToExt(numArray, zz);
            SecT239Field.MultiplyAddToExt(numArray2, numArray3, zz);
            ulong[] z = Nat256.Create64();
            SecT239Field.Reduce(zz, z);
            return new SecT239FieldElement(z);
        }

        public override ECFieldElement SquarePow(int pow)
        {
            if (pow < 1)
            {
                return this;
            }
            ulong[] z = Nat256.Create64();
            SecT239Field.SquareN(this.x, pow, z);
            return new SecT239FieldElement(z);
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
            "SecT239Field";

        public override int FieldSize =>
            0xef;

        public virtual int Representation =>
            2;

        public virtual int M =>
            0xef;

        public virtual int K1 =>
            0x9e;

        public virtual int K2 =>
            0;

        public virtual int K3 =>
            0;
    }
}

