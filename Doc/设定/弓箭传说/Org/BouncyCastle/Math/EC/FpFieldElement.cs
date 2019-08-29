namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.Raw;
    using Org.BouncyCastle.Utilities;
    using System;

    public class FpFieldElement : ECFieldElement
    {
        private readonly BigInteger q;
        private readonly BigInteger r;
        private readonly BigInteger x;

        public FpFieldElement(BigInteger q, BigInteger x) : this(q, CalculateResidue(q), x)
        {
        }

        internal FpFieldElement(BigInteger q, BigInteger r, BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.CompareTo(q) >= 0))
            {
                throw new ArgumentException("value invalid in Fp field element", "x");
            }
            this.q = q;
            this.r = r;
            this.x = x;
        }

        public override ECFieldElement Add(ECFieldElement b) => 
            new FpFieldElement(this.q, this.r, this.ModAdd(this.x, b.ToBigInteger()));

        public override ECFieldElement AddOne()
        {
            BigInteger x = this.x.Add(BigInteger.One);
            if (x.CompareTo(this.q) == 0)
            {
                x = BigInteger.Zero;
            }
            return new FpFieldElement(this.q, this.r, x);
        }

        internal static BigInteger CalculateResidue(BigInteger p)
        {
            int bitLength = p.BitLength;
            if (bitLength >= 0x60)
            {
                if (p.ShiftRight(bitLength - 0x40).LongValue == -1L)
                {
                    return BigInteger.One.ShiftLeft(bitLength).Subtract(p);
                }
                if ((bitLength & 7) == 0)
                {
                    return BigInteger.One.ShiftLeft(bitLength << 1).Divide(p).Negate();
                }
            }
            return null;
        }

        private ECFieldElement CheckSqrt(ECFieldElement z) => 
            (!z.Square().Equals((ECFieldElement) this) ? null : z);

        public override ECFieldElement Divide(ECFieldElement b) => 
            new FpFieldElement(this.q, this.r, this.ModMult(this.x, this.ModInverse(b.ToBigInteger())));

        public virtual bool Equals(FpFieldElement other) => 
            (this.q.Equals(other.q) && base.Equals((ECFieldElement) other));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            FpFieldElement other = obj as FpFieldElement;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            (this.q.GetHashCode() ^ base.GetHashCode());

        public override ECFieldElement Invert() => 
            new FpFieldElement(this.q, this.r, this.ModInverse(this.x));

        private BigInteger[] LucasSequence(BigInteger P, BigInteger Q, BigInteger k)
        {
            int bitLength = k.BitLength;
            int lowestSetBit = k.GetLowestSetBit();
            BigInteger one = BigInteger.One;
            BigInteger two = BigInteger.Two;
            BigInteger integer3 = P;
            BigInteger integer4 = BigInteger.One;
            BigInteger integer5 = BigInteger.One;
            for (int i = bitLength - 1; i >= (lowestSetBit + 1); i--)
            {
                integer4 = this.ModMult(integer4, integer5);
                if (k.TestBit(i))
                {
                    integer5 = this.ModMult(integer4, Q);
                    one = this.ModMult(one, integer3);
                    two = this.ModReduce(integer3.Multiply(two).Subtract(P.Multiply(integer4)));
                    integer3 = this.ModReduce(integer3.Multiply(integer3).Subtract(integer5.ShiftLeft(1)));
                }
                else
                {
                    integer5 = integer4;
                    one = this.ModReduce(one.Multiply(two).Subtract(integer4));
                    integer3 = this.ModReduce(integer3.Multiply(two).Subtract(P.Multiply(integer4)));
                    two = this.ModReduce(two.Multiply(two).Subtract(integer4.ShiftLeft(1)));
                }
            }
            integer4 = this.ModMult(integer4, integer5);
            integer5 = this.ModMult(integer4, Q);
            one = this.ModReduce(one.Multiply(two).Subtract(integer4));
            two = this.ModReduce(integer3.Multiply(two).Subtract(P.Multiply(integer4)));
            integer4 = this.ModMult(integer4, integer5);
            for (int j = 1; j <= lowestSetBit; j++)
            {
                one = this.ModMult(one, two);
                two = this.ModReduce(two.Multiply(two).Subtract(integer4.ShiftLeft(1)));
                integer4 = this.ModMult(integer4, integer4);
            }
            return new BigInteger[] { one, two };
        }

        protected virtual BigInteger ModAdd(BigInteger x1, BigInteger x2)
        {
            BigInteger integer = x1.Add(x2);
            if (integer.CompareTo(this.q) >= 0)
            {
                integer = integer.Subtract(this.q);
            }
            return integer;
        }

        protected virtual BigInteger ModDouble(BigInteger x)
        {
            BigInteger integer = x.ShiftLeft(1);
            if (integer.CompareTo(this.q) >= 0)
            {
                integer = integer.Subtract(this.q);
            }
            return integer;
        }

        protected virtual BigInteger ModHalf(BigInteger x)
        {
            if (x.TestBit(0))
            {
                x = this.q.Add(x);
            }
            return x.ShiftRight(1);
        }

        protected virtual BigInteger ModHalfAbs(BigInteger x)
        {
            if (x.TestBit(0))
            {
                x = this.q.Subtract(x);
            }
            return x.ShiftRight(1);
        }

        protected virtual BigInteger ModInverse(BigInteger x)
        {
            int fieldSize = this.FieldSize;
            int len = (fieldSize + 0x1f) >> 5;
            uint[] p = Nat.FromBigInteger(fieldSize, this.q);
            uint[] numArray2 = Nat.FromBigInteger(fieldSize, x);
            uint[] z = Nat.Create(len);
            Mod.Invert(p, numArray2, z);
            return Nat.ToBigInteger(len, z);
        }

        protected virtual BigInteger ModMult(BigInteger x1, BigInteger x2) => 
            this.ModReduce(x1.Multiply(x2));

        protected virtual BigInteger ModReduce(BigInteger x)
        {
            if (this.r == null)
            {
                x = x.Mod(this.q);
                return x;
            }
            bool flag = x.SignValue < 0;
            if (flag)
            {
                x = x.Abs();
            }
            int bitLength = this.q.BitLength;
            if (this.r.SignValue > 0)
            {
                BigInteger n = BigInteger.One.ShiftLeft(bitLength);
                bool flag2 = this.r.Equals(BigInteger.One);
                while (x.BitLength > (bitLength + 1))
                {
                    BigInteger integer2 = x.ShiftRight(bitLength);
                    BigInteger integer3 = x.Remainder(n);
                    if (!flag2)
                    {
                        integer2 = integer2.Multiply(this.r);
                    }
                    x = integer2.Add(integer3);
                }
            }
            else
            {
                int num2 = ((bitLength - 1) & 0x1f) + 1;
                BigInteger n = this.r.Negate().Multiply(x.ShiftRight(bitLength - num2)).ShiftRight(bitLength + num2).Multiply(this.q);
                BigInteger integer8 = BigInteger.One.ShiftLeft(bitLength + num2);
                n = n.Remainder(integer8);
                x = x.Remainder(integer8);
                x = x.Subtract(n);
                if (x.SignValue < 0)
                {
                    x = x.Add(integer8);
                }
            }
            while (x.CompareTo(this.q) >= 0)
            {
                x = x.Subtract(this.q);
            }
            if (flag && (x.SignValue != 0))
            {
                x = this.q.Subtract(x);
            }
            return x;
        }

        protected virtual BigInteger ModSubtract(BigInteger x1, BigInteger x2)
        {
            BigInteger integer = x1.Subtract(x2);
            if (integer.SignValue < 0)
            {
                integer = integer.Add(this.q);
            }
            return integer;
        }

        public override ECFieldElement Multiply(ECFieldElement b) => 
            new FpFieldElement(this.q, this.r, this.ModMult(this.x, b.ToBigInteger()));

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            BigInteger integer = this.x;
            BigInteger val = b.ToBigInteger();
            BigInteger integer3 = x.ToBigInteger();
            BigInteger integer4 = y.ToBigInteger();
            BigInteger integer5 = integer.Multiply(val);
            BigInteger n = integer3.Multiply(integer4);
            return new FpFieldElement(this.q, this.r, this.ModReduce(integer5.Subtract(n)));
        }

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            BigInteger integer = this.x;
            BigInteger val = b.ToBigInteger();
            BigInteger integer3 = x.ToBigInteger();
            BigInteger integer4 = y.ToBigInteger();
            BigInteger integer5 = integer.Multiply(val);
            BigInteger integer6 = integer3.Multiply(integer4);
            BigInteger integer7 = integer5.Add(integer6);
            if (((this.r != null) && (this.r.SignValue < 0)) && (integer7.BitLength > (this.q.BitLength << 1)))
            {
                integer7 = integer7.Subtract(this.q.ShiftLeft(this.q.BitLength));
            }
            return new FpFieldElement(this.q, this.r, this.ModReduce(integer7));
        }

        public override ECFieldElement Negate() => 
            ((this.x.SignValue != 0) ? new FpFieldElement(this.q, this.r, this.q.Subtract(this.x)) : this);

        public override ECFieldElement Sqrt()
        {
            BigInteger integer14;
            if (this.IsZero || this.IsOne)
            {
                return this;
            }
            if (!this.q.TestBit(0))
            {
                throw Platform.CreateNotImplementedException("even value of q");
            }
            if (this.q.TestBit(1))
            {
                BigInteger integer = this.q.ShiftRight(2).Add(BigInteger.One);
                return this.CheckSqrt(new FpFieldElement(this.q, this.r, this.x.ModPow(integer, this.q)));
            }
            if (this.q.TestBit(2))
            {
                BigInteger integer2 = this.x.ModPow(this.q.ShiftRight(3), this.q);
                BigInteger integer3 = this.ModMult(integer2, this.x);
                if (this.ModMult(integer3, integer2).Equals(BigInteger.One))
                {
                    return this.CheckSqrt(new FpFieldElement(this.q, this.r, integer3));
                }
                BigInteger integer5 = BigInteger.Two.ModPow(this.q.ShiftRight(2), this.q);
                BigInteger integer6 = this.ModMult(integer3, integer5);
                return this.CheckSqrt(new FpFieldElement(this.q, this.r, integer6));
            }
            BigInteger e = this.q.ShiftRight(1);
            if (!this.x.ModPow(e, this.q).Equals(BigInteger.One))
            {
                return null;
            }
            BigInteger x = this.x;
            BigInteger n = this.ModDouble(this.ModDouble(x));
            BigInteger k = e.Add(BigInteger.One);
            BigInteger integer11 = this.q.Subtract(BigInteger.One);
        Label_01A2:
            integer14 = BigInteger.Arbitrary(this.q.BitLength);
            if ((integer14.CompareTo(this.q) >= 0) || !this.ModReduce(integer14.Multiply(integer14).Subtract(n)).ModPow(e, this.q).Equals(integer11))
            {
                goto Label_01A2;
            }
            BigInteger[] integerArray = this.LucasSequence(integer14, x, k);
            BigInteger integer12 = integerArray[0];
            BigInteger integer13 = integerArray[1];
            if (this.ModMult(integer13, integer13).Equals(n))
            {
                return new FpFieldElement(this.q, this.r, this.ModHalfAbs(integer13));
            }
            if (integer12.Equals(BigInteger.One) || integer12.Equals(integer11))
            {
                goto Label_01A2;
            }
            return null;
        }

        public override ECFieldElement Square() => 
            new FpFieldElement(this.q, this.r, this.ModMult(this.x, this.x));

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
        {
            BigInteger val = this.x;
            BigInteger integer2 = x.ToBigInteger();
            BigInteger integer3 = y.ToBigInteger();
            BigInteger integer4 = val.Multiply(val);
            BigInteger n = integer2.Multiply(integer3);
            return new FpFieldElement(this.q, this.r, this.ModReduce(integer4.Subtract(n)));
        }

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            BigInteger val = this.x;
            BigInteger integer2 = x.ToBigInteger();
            BigInteger integer3 = y.ToBigInteger();
            BigInteger integer4 = val.Multiply(val);
            BigInteger integer5 = integer2.Multiply(integer3);
            BigInteger integer6 = integer4.Add(integer5);
            if (((this.r != null) && (this.r.SignValue < 0)) && (integer6.BitLength > (this.q.BitLength << 1)))
            {
                integer6 = integer6.Subtract(this.q.ShiftLeft(this.q.BitLength));
            }
            return new FpFieldElement(this.q, this.r, this.ModReduce(integer6));
        }

        public override ECFieldElement Subtract(ECFieldElement b) => 
            new FpFieldElement(this.q, this.r, this.ModSubtract(this.x, b.ToBigInteger()));

        public override BigInteger ToBigInteger() => 
            this.x;

        public override string FieldName =>
            "Fp";

        public override int FieldSize =>
            this.q.BitLength;

        public BigInteger Q =>
            this.q;
    }
}

