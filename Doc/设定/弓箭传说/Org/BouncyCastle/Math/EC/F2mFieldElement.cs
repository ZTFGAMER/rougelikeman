namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public class F2mFieldElement : ECFieldElement
    {
        public const int Gnb = 1;
        public const int Tpb = 2;
        public const int Ppb = 3;
        private int representation;
        private int m;
        private int[] ks;
        private LongArray x;

        public F2mFieldElement(int m, int k, BigInteger x) : this(m, k, 0, 0, x)
        {
        }

        private F2mFieldElement(int m, int[] ks, LongArray x)
        {
            this.m = m;
            this.representation = (ks.Length != 1) ? 3 : 2;
            this.ks = ks;
            this.x = x;
        }

        public F2mFieldElement(int m, int k1, int k2, int k3, BigInteger x)
        {
            if (((x == null) || (x.SignValue < 0)) || (x.BitLength > m))
            {
                throw new ArgumentException("value invalid in F2m field element", "x");
            }
            if ((k2 == 0) && (k3 == 0))
            {
                this.representation = 2;
                this.ks = new int[] { k1 };
            }
            else
            {
                if (k2 >= k3)
                {
                    throw new ArgumentException("k2 must be smaller than k3");
                }
                if (k2 <= 0)
                {
                    throw new ArgumentException("k2 must be larger than 0");
                }
                this.representation = 3;
                this.ks = new int[] { k1, k2, k3 };
            }
            this.m = m;
            this.x = new LongArray(x);
        }

        public override ECFieldElement Add(ECFieldElement b)
        {
            LongArray x = this.x.Copy();
            F2mFieldElement element = (F2mFieldElement) b;
            x.AddShiftedByWords(element.x, 0);
            return new F2mFieldElement(this.m, this.ks, x);
        }

        public override ECFieldElement AddOne() => 
            new F2mFieldElement(this.m, this.ks, this.x.AddOne());

        public static void CheckFieldElements(ECFieldElement a, ECFieldElement b)
        {
            if (!(a is F2mFieldElement) || !(b is F2mFieldElement))
            {
                throw new ArgumentException("Field elements are not both instances of F2mFieldElement");
            }
            F2mFieldElement element = (F2mFieldElement) a;
            F2mFieldElement element2 = (F2mFieldElement) b;
            if (element.representation != element2.representation)
            {
                throw new ArgumentException("One of the F2m field elements has incorrect representation");
            }
            if ((element.m != element2.m) || !Arrays.AreEqual(element.ks, element2.ks))
            {
                throw new ArgumentException("Field elements are not elements of the same field F2m");
            }
        }

        public override ECFieldElement Divide(ECFieldElement b)
        {
            ECFieldElement element = b.Invert();
            return this.Multiply(element);
        }

        public virtual bool Equals(F2mFieldElement other) => 
            ((((this.m == other.m) && (this.representation == other.representation)) && Arrays.AreEqual(this.ks, other.ks)) && this.x.Equals(other.x));

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            F2mFieldElement other = obj as F2mFieldElement;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode() => 
            ((this.x.GetHashCode() ^ this.m) ^ Arrays.GetHashCode(this.ks));

        public override ECFieldElement Invert() => 
            new F2mFieldElement(this.m, this.ks, this.x.ModInverse(this.m, this.ks));

        public override ECFieldElement Multiply(ECFieldElement b) => 
            new F2mFieldElement(this.m, this.ks, this.x.ModMultiply(((F2mFieldElement) b).x, this.m, this.ks));

        public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y) => 
            this.MultiplyPlusProduct(b, x, y);

        public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
        {
            LongArray array = this.x;
            LongArray other = ((F2mFieldElement) b).x;
            LongArray array3 = ((F2mFieldElement) x).x;
            LongArray array4 = ((F2mFieldElement) y).x;
            LongArray array5 = array.Multiply(other, this.m, this.ks);
            LongArray array6 = array3.Multiply(array4, this.m, this.ks);
            if ((array5 == array) || (array5 == other))
            {
                array5 = array5.Copy();
            }
            array5.AddShiftedByWords(array6, 0);
            array5.Reduce(this.m, this.ks);
            return new F2mFieldElement(this.m, this.ks, array5);
        }

        public override ECFieldElement Negate() => 
            this;

        public override ECFieldElement Sqrt() => 
            ((!this.x.IsZero() && !this.x.IsOne()) ? this.SquarePow(this.m - 1) : this);

        public override ECFieldElement Square() => 
            new F2mFieldElement(this.m, this.ks, this.x.ModSquare(this.m, this.ks));

        public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y) => 
            this.SquarePlusProduct(x, y);

        public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
        {
            LongArray array = this.x;
            LongArray array2 = ((F2mFieldElement) x).x;
            LongArray other = ((F2mFieldElement) y).x;
            LongArray array4 = array.Square(this.m, this.ks);
            LongArray array5 = array2.Multiply(other, this.m, this.ks);
            if (array4 == array)
            {
                array4 = array4.Copy();
            }
            array4.AddShiftedByWords(array5, 0);
            array4.Reduce(this.m, this.ks);
            return new F2mFieldElement(this.m, this.ks, array4);
        }

        public override ECFieldElement SquarePow(int pow) => 
            ((pow >= 1) ? new F2mFieldElement(this.m, this.ks, this.x.ModSquareN(pow, this.m, this.ks)) : this);

        public override ECFieldElement Subtract(ECFieldElement b) => 
            this.Add(b);

        public override bool TestBitZero() => 
            this.x.TestBitZero();

        public override BigInteger ToBigInteger() => 
            this.x.ToBigInteger();

        public override int BitLength =>
            this.x.Degree();

        public override bool IsOne =>
            this.x.IsOne();

        public override bool IsZero =>
            this.x.IsZero();

        public override string FieldName =>
            "F2m";

        public override int FieldSize =>
            this.m;

        public int Representation =>
            this.representation;

        public int M =>
            this.m;

        public int K1 =>
            this.ks[0];

        public int K2 =>
            ((this.ks.Length < 2) ? 0 : this.ks[1]);

        public int K3 =>
            ((this.ks.Length < 3) ? 0 : this.ks[2]);
    }
}

