namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC.Abc;
    using Org.BouncyCastle.Math.Field;
    using System;

    public abstract class AbstractF2mCurve : ECCurve
    {
        private BigInteger[] si;

        protected AbstractF2mCurve(int m, int k1, int k2, int k3) : base(BuildField(m, k1, k2, k3))
        {
        }

        private static IFiniteField BuildField(int m, int k1, int k2, int k3)
        {
            if (k1 == 0)
            {
                throw new ArgumentException("k1 must be > 0");
            }
            if (k2 == 0)
            {
                if (k3 != 0)
                {
                    throw new ArgumentException("k3 must be 0 if k2 == 0");
                }
                int[] numArray1 = new int[3];
                numArray1[1] = k1;
                numArray1[2] = m;
                return FiniteFields.GetBinaryExtensionField(numArray1);
            }
            if (k2 <= k1)
            {
                throw new ArgumentException("k2 must be > k1");
            }
            if (k3 <= k2)
            {
                throw new ArgumentException("k3 must be > k2");
            }
            int[] exponents = new int[5];
            exponents[1] = k1;
            exponents[2] = k2;
            exponents[3] = k3;
            exponents[4] = m;
            return FiniteFields.GetBinaryExtensionField(exponents);
        }

        public override ECPoint CreatePoint(BigInteger x, BigInteger y, bool withCompression)
        {
            ECFieldElement b = this.FromBigInteger(x);
            ECFieldElement element2 = this.FromBigInteger(y);
            switch (this.CoordinateSystem)
            {
                case 5:
                case 6:
                    if (b.IsZero)
                    {
                        if (!element2.Square().Equals(this.B))
                        {
                            throw new ArgumentException();
                        }
                    }
                    else
                    {
                        element2 = element2.Divide(b).Add(b);
                    }
                    break;
            }
            return this.CreateRawPoint(b, element2, withCompression);
        }

        protected override ECPoint DecompressPoint(int yTilde, BigInteger X1)
        {
            ECFieldElement b = this.FromBigInteger(X1);
            ECFieldElement y = null;
            if (b.IsZero)
            {
                y = this.B.Sqrt();
            }
            else
            {
                ECFieldElement beta = b.Square().Invert().Multiply(this.B).Add(this.A).Add(b);
                ECFieldElement element4 = this.SolveQuadradicEquation(beta);
                if (element4 != null)
                {
                    if (element4.TestBitZero() != (yTilde == 1))
                    {
                        element4 = element4.AddOne();
                    }
                    switch (this.CoordinateSystem)
                    {
                        case 5:
                        case 6:
                            y = element4.Add(b);
                            goto Label_00A9;
                    }
                    y = element4.Multiply(b);
                }
            }
        Label_00A9:
            if (y == null)
            {
                throw new ArgumentException("Invalid point compression");
            }
            return this.CreateRawPoint(b, y, true);
        }

        internal virtual BigInteger[] GetSi()
        {
            if (this.si == null)
            {
                object obj2 = this;
                lock (obj2)
                {
                    if (this.si == null)
                    {
                        this.si = Tnaf.GetSi(this);
                    }
                }
            }
            return this.si;
        }

        public static BigInteger Inverse(int m, int[] ks, BigInteger x) => 
            new LongArray(x).ModInverse(m, ks).ToBigInteger();

        public override bool IsValidFieldElement(BigInteger x) => 
            (((x != null) && (x.SignValue >= 0)) && (x.BitLength <= this.FieldSize));

        private ECFieldElement SolveQuadradicEquation(ECFieldElement beta)
        {
            ECFieldElement element2;
            if (beta.IsZero)
            {
                return beta;
            }
            ECFieldElement element3 = this.FromBigInteger(BigInteger.Zero);
            int fieldSize = this.FieldSize;
            do
            {
                ECFieldElement b = this.FromBigInteger(BigInteger.Arbitrary(fieldSize));
                element2 = element3;
                ECFieldElement element5 = beta;
                for (int i = 1; i < fieldSize; i++)
                {
                    ECFieldElement element6 = element5.Square();
                    element2 = element2.Square().Add(element6.Multiply(b));
                    element5 = element6.Add(beta);
                }
                if (!element5.IsZero)
                {
                    return null;
                }
            }
            while (element2.Square().Add(element2).IsZero);
            return element2;
        }

        public virtual bool IsKoblitz =>
            ((((base.m_order != null) && (base.m_cofactor != null)) && base.m_b.IsOne) && (base.m_a.IsZero || base.m_a.IsOne));
    }
}

