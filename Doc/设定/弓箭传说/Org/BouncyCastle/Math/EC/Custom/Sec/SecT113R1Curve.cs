namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT113R1Curve : AbstractF2mCurve
    {
        private const int SecT113R1_DEFAULT_COORDS = 6;
        protected readonly SecT113R1Point m_infinity;

        public SecT113R1Curve() : base(0x71, 9, 0, 0)
        {
            this.m_infinity = new SecT113R1Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("003088250CA6E7C7FE649CE85820F7")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("00E8BEE4D3E2260744188BE0E9C723")));
            base.m_order = new BigInteger(1, Hex.Decode("0100000000000000D9CCEC8A39E56F"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT113R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT113R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT113R1Point(this, x, y, zs, withCompression);

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
                ECFieldElement element4 = this.SolveQuadraticEquation(beta);
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

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT113FieldElement(x);

        private ECFieldElement SolveQuadraticEquation(ECFieldElement beta)
        {
            if (beta.IsZero)
            {
                return beta;
            }
            ECFieldElement element = this.FromBigInteger(BigInteger.Zero);
            ECFieldElement b = null;
            Random random = new Random();
            do
            {
                ECFieldElement element4 = this.FromBigInteger(new BigInteger(0x71, random));
                b = element;
                ECFieldElement element5 = beta;
                for (int i = 1; i < 0x71; i++)
                {
                    ECFieldElement element6 = element5.Square();
                    b = b.Square().Add(element6.Multiply(element4));
                    element5 = element6.Add(beta);
                }
                if (!element5.IsZero)
                {
                    return null;
                }
            }
            while (b.Square().Add(b).IsZero);
            return b;
        }

        public override bool SupportsCoordinateSystem(int coord)
        {
            if (coord != 6)
            {
                return false;
            }
            return true;
        }

        public override ECPoint Infinity =>
            this.m_infinity;

        public override int FieldSize =>
            0x71;

        public override bool IsKoblitz =>
            false;

        public virtual int M =>
            0x71;

        public virtual bool IsTrinomial =>
            true;

        public virtual int K1 =>
            9;

        public virtual int K2 =>
            0;

        public virtual int K3 =>
            0;
    }
}

