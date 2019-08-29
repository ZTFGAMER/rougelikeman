namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    internal class SecP224R1FieldElement : ECFieldElement
    {
        public static readonly BigInteger Q = SecP224R1Curve.q;
        protected internal readonly uint[] x;

        public SecP224R1FieldElement()
        {
            this.x = Nat224.Create();
        }

        public SecP224R1FieldElement(BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(Q) >= 0))
            {
                throw new ArgumentException("value invalid for SecP224R1FieldElement", "x");
            }
            this.x = SecP224R1Field.FromBigInteger(x);
        }

        protected internal SecP224R1FieldElement(uint[] x)
        {
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            uint[] z = Nat224.Create();
            SecP224R1Field.Add(this.x, ((SecP224R1FieldElement) b).x, z);
            return new SecP224R1FieldElement(z);
        }

        public override ECFieldElement AddOne()
        {
            uint[] z = Nat224.Create();
            SecP224R1Field.AddOne(this.x, z);
            return new SecP224R1FieldElement(z);
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            uint[] z = Nat224.Create();
            Mod.Invert(SecP224R1Field.P, ((SecP224R1FieldElement) b).x, z);
            SecP224R1Field.Multiply(z, this.x, z);
            return new SecP224R1FieldElement(z);
        }

        public virtual bool Equals(SecP224R1FieldElement other)
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
            this.Equals(other as SecP224R1FieldElement);

        public override bool Equals(object obj) => 
            this.Equals(obj as SecP224R1FieldElement);

        public override int GetHashCode() => 
            (Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 7));

        public override ECFieldElement Invert()
        {
            uint[] z = Nat224.Create();
            Mod.Invert(SecP224R1Field.P, this.x, z);
            return new SecP224R1FieldElement(z);
        }

        private static bool IsSquare(uint[] x)
        {
            uint[] z = Nat224.Create();
            uint[] numArray2 = Nat224.Create();
            Nat224.Copy(x, z);
            for (int i = 0; i < 7; i++)
            {
                Nat224.Copy(z, numArray2);
                SecP224R1Field.SquareN(z, ((int) 1) << i, z);
                SecP224R1Field.Multiply(z, numArray2, z);
            }
            SecP224R1Field.SquareN(z, 0x5f, z);
            return Nat224.IsOne(z);
        }

        public override ECFieldElement Multiply(ECFieldElement b)
        {
            uint[] z = Nat224.Create();
            SecP224R1Field.Multiply(this.x, ((SecP224R1FieldElement) b).x, z);
            return new SecP224R1FieldElement(z);
        }

        public override ECFieldElement Negate()
        {
            uint[] z = Nat224.Create();
            SecP224R1Field.Negate(this.x, z);
            return new SecP224R1FieldElement(z);
        }

        private static void RM(uint[] nc, uint[] d0, uint[] e0, uint[] d1, uint[] e1, uint[] f1, uint[] t)
        {
            SecP224R1Field.Multiply(e1, e0, t);
            SecP224R1Field.Multiply(t, nc, t);
            SecP224R1Field.Multiply(d1, d0, f1);
            SecP224R1Field.Add(f1, t, f1);
            SecP224R1Field.Multiply(d1, e0, t);
            Nat224.Copy(f1, d1);
            SecP224R1Field.Multiply(e1, d0, e1);
            SecP224R1Field.Add(e1, t, e1);
            SecP224R1Field.Square(e1, f1);
            SecP224R1Field.Multiply(f1, nc, f1);
        }

        private static void RP(uint[] nc, uint[] d1, uint[] e1, uint[] f1, uint[] t)
        {
            Nat224.Copy(nc, f1);
            uint[] z = Nat224.Create();
            uint[] numArray2 = Nat224.Create();
            for (int i = 0; i < 7; i++)
            {
                Nat224.Copy(d1, z);
                Nat224.Copy(e1, numArray2);
                int num2 = ((int) 1) << i;
                while (--num2 >= 0)
                {
                    RS(d1, e1, f1, t);
                }
                RM(nc, z, numArray2, d1, e1, f1, t);
            }
        }

        private static void RS(uint[] d, uint[] e, uint[] f, uint[] t)
        {
            SecP224R1Field.Multiply(e, d, e);
            SecP224R1Field.Twice(e, e);
            SecP224R1Field.Square(d, t);
            SecP224R1Field.Add(f, t, d);
            SecP224R1Field.Multiply(f, t, f);
            SecP224R1Field.Reduce32(Nat.ShiftUpBits(7, f, 2, 0), f);
        }

        public override ECFieldElement Sqrt()
        {
            uint[] x = this.x;
            if (Nat224.IsZero(x) || Nat224.IsOne(x))
            {
                return this;
            }
            uint[] z = Nat224.Create();
            SecP224R1Field.Negate(x, z);
            uint[] r = Mod.Random(SecP224R1Field.P);
            uint[] t = Nat224.Create();
            if (!IsSquare(x))
            {
                return null;
            }
            while (!TrySqrt(z, r, t))
            {
                SecP224R1Field.AddOne(r, r);
            }
            SecP224R1Field.Square(t, r);
            return (!Nat224.Eq(x, r) ? null : new SecP224R1FieldElement(t));
        }

        public override ECFieldElement Square()
        {
            uint[] z = Nat224.Create();
            SecP224R1Field.Square(this.x, z);
            return new SecP224R1FieldElement(z);
        }

        public override ECFieldElement Subtract(ECFieldElement b)
        {
            uint[] z = Nat224.Create();
            SecP224R1Field.Subtract(this.x, ((SecP224R1FieldElement) b).x, z);
            return new SecP224R1FieldElement(z);
        }

        public override bool TestBitZero() => 
            (Nat224.GetBit(this.x, 0) == 1);

        public override BigInteger ToBigInteger() => 
            Nat224.ToBigInteger(this.x);

        private static bool TrySqrt(uint[] nc, uint[] r, uint[] t)
        {
            uint[] z = Nat224.Create();
            Nat224.Copy(r, z);
            uint[] numArray2 = Nat224.Create();
            numArray2[0] = 1;
            uint[] numArray3 = Nat224.Create();
            RP(nc, z, numArray2, numArray3, t);
            uint[] numArray4 = Nat224.Create();
            uint[] numArray5 = Nat224.Create();
            for (int i = 1; i < 0x60; i++)
            {
                Nat224.Copy(z, numArray4);
                Nat224.Copy(numArray2, numArray5);
                RS(z, numArray2, numArray3, t);
                if (Nat224.IsZero(z))
                {
                    Mod.Invert(SecP224R1Field.P, numArray5, t);
                    SecP224R1Field.Multiply(t, numArray4, t);
                    return true;
                }
            }
            return false;
        }

        public override bool IsZero =>
            Nat224.IsZero(this.x);

        public override bool IsOne =>
            Nat224.IsOne(this.x);

        public override string FieldName =>
            "SecP224R1Field";

        public override int FieldSize =>
            Q.BitLength;
    }
}

