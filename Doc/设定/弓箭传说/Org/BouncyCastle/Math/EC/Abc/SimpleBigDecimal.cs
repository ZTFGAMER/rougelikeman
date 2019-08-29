namespace Org.BouncyCastle.Math.EC.Abc
{
    using Org.BouncyCastle.Math;
    using System;
    using System.Text;

    internal class SimpleBigDecimal
    {
        private readonly BigInteger bigInt;
        private readonly int scale;

        private SimpleBigDecimal(SimpleBigDecimal limBigDec)
        {
            this.bigInt = limBigDec.bigInt;
            this.scale = limBigDec.scale;
        }

        public SimpleBigDecimal(BigInteger bigInt, int scale)
        {
            if (scale < 0)
            {
                throw new ArgumentException("scale may not be negative");
            }
            this.bigInt = bigInt;
            this.scale = scale;
        }

        public SimpleBigDecimal Add(BigInteger b) => 
            new SimpleBigDecimal(this.bigInt.Add(b.ShiftLeft(this.scale)), this.scale);

        public SimpleBigDecimal Add(SimpleBigDecimal b)
        {
            this.CheckScale(b);
            return new SimpleBigDecimal(this.bigInt.Add(b.bigInt), this.scale);
        }

        public SimpleBigDecimal AdjustScale(int newScale)
        {
            if (newScale < 0)
            {
                throw new ArgumentException("scale may not be negative");
            }
            if (newScale == this.scale)
            {
                return this;
            }
            return new SimpleBigDecimal(this.bigInt.ShiftLeft(newScale - this.scale), newScale);
        }

        private void CheckScale(SimpleBigDecimal b)
        {
            if (this.scale != b.scale)
            {
                throw new ArgumentException("Only SimpleBigDecimal of same scale allowed in arithmetic operations");
            }
        }

        public int CompareTo(BigInteger val) => 
            this.bigInt.CompareTo(val.ShiftLeft(this.scale));

        public int CompareTo(SimpleBigDecimal val)
        {
            this.CheckScale(val);
            return this.bigInt.CompareTo(val.bigInt);
        }

        public SimpleBigDecimal Divide(BigInteger b) => 
            new SimpleBigDecimal(this.bigInt.Divide(b), this.scale);

        public SimpleBigDecimal Divide(SimpleBigDecimal b)
        {
            this.CheckScale(b);
            return new SimpleBigDecimal(this.bigInt.ShiftLeft(this.scale).Divide(b.bigInt), this.scale);
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            SimpleBigDecimal num = obj as SimpleBigDecimal;
            if (num == null)
            {
                return false;
            }
            return (this.bigInt.Equals(num.bigInt) && (this.scale == num.scale));
        }

        public BigInteger Floor() => 
            this.bigInt.ShiftRight(this.scale);

        public override int GetHashCode() => 
            (this.bigInt.GetHashCode() ^ this.scale);

        public static SimpleBigDecimal GetInstance(BigInteger val, int scale) => 
            new SimpleBigDecimal(val.ShiftLeft(scale), scale);

        public SimpleBigDecimal Multiply(BigInteger b) => 
            new SimpleBigDecimal(this.bigInt.Multiply(b), this.scale);

        public SimpleBigDecimal Multiply(SimpleBigDecimal b)
        {
            this.CheckScale(b);
            return new SimpleBigDecimal(this.bigInt.Multiply(b.bigInt), this.scale + this.scale);
        }

        public SimpleBigDecimal Negate() => 
            new SimpleBigDecimal(this.bigInt.Negate(), this.scale);

        public BigInteger Round()
        {
            SimpleBigDecimal num = new SimpleBigDecimal(BigInteger.One, 1);
            return this.Add(num.AdjustScale(this.scale)).Floor();
        }

        public SimpleBigDecimal ShiftLeft(int n) => 
            new SimpleBigDecimal(this.bigInt.ShiftLeft(n), this.scale);

        public SimpleBigDecimal Subtract(BigInteger b) => 
            new SimpleBigDecimal(this.bigInt.Subtract(b.ShiftLeft(this.scale)), this.scale);

        public SimpleBigDecimal Subtract(SimpleBigDecimal b) => 
            this.Add(b.Negate());

        public override string ToString()
        {
            if (this.scale == 0)
            {
                return this.bigInt.ToString();
            }
            BigInteger integer = this.Floor();
            BigInteger n = this.bigInt.Subtract(integer.ShiftLeft(this.scale));
            if (this.bigInt.SignValue < 0)
            {
                n = BigInteger.One.ShiftLeft(this.scale).Subtract(n);
            }
            if ((integer.SignValue == -1) && !n.Equals(BigInteger.Zero))
            {
                integer = integer.Add(BigInteger.One);
            }
            string str = integer.ToString();
            char[] chArray = new char[this.scale];
            string str2 = n.ToString(2);
            int length = str2.Length;
            int num2 = this.scale - length;
            for (int i = 0; i < num2; i++)
            {
                chArray[i] = '0';
            }
            for (int j = 0; j < length; j++)
            {
                chArray[num2 + j] = str2[j];
            }
            string str3 = new string(chArray);
            StringBuilder builder = new StringBuilder(str);
            builder.Append(".");
            builder.Append(str3);
            return builder.ToString();
        }

        public int IntValue =>
            this.Floor().IntValue;

        public long LongValue =>
            this.Floor().LongValue;

        public int Scale =>
            this.scale;
    }
}

