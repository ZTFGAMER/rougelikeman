namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using System;

    public class FpCurve : AbstractFpCurve
    {
        private const int FP_DEFAULT_COORDS = 4;
        protected readonly BigInteger m_q;
        protected readonly BigInteger m_r;
        protected readonly FpPoint m_infinity;

        public FpCurve(BigInteger q, BigInteger a, BigInteger b) : this(q, a, b, null, null)
        {
        }

        protected FpCurve(BigInteger q, BigInteger r, ECFieldElement a, ECFieldElement b) : this(q, r, a, b, null, null)
        {
        }

        public FpCurve(BigInteger q, BigInteger a, BigInteger b, BigInteger order, BigInteger cofactor) : base(q)
        {
            this.m_q = q;
            this.m_r = FpFieldElement.CalculateResidue(q);
            this.m_infinity = new FpPoint(this, null, null);
            base.m_a = this.FromBigInteger(a);
            base.m_b = this.FromBigInteger(b);
            base.m_order = order;
            base.m_cofactor = cofactor;
            base.m_coord = 4;
        }

        protected FpCurve(BigInteger q, BigInteger r, ECFieldElement a, ECFieldElement b, BigInteger order, BigInteger cofactor) : base(q)
        {
            this.m_q = q;
            this.m_r = r;
            this.m_infinity = new FpPoint(this, null, null);
            base.m_a = a;
            base.m_b = b;
            base.m_order = order;
            base.m_cofactor = cofactor;
            base.m_coord = 4;
        }

        protected override ECCurve CloneCurve() => 
            new FpCurve(this.m_q, this.m_r, base.m_a, base.m_b, base.m_order, base.m_cofactor);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new FpPoint(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new FpPoint(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new FpFieldElement(this.m_q, this.m_r, x);

        public override ECPoint ImportPoint(ECPoint p)
        {
            if (((this != p.Curve) && (this.CoordinateSystem == 2)) && !p.IsInfinity)
            {
                switch (p.Curve.CoordinateSystem)
                {
                    case 2:
                    case 3:
                    case 4:
                        return new FpPoint(this, this.FromBigInteger(p.RawXCoord.ToBigInteger()), this.FromBigInteger(p.RawYCoord.ToBigInteger()), new ECFieldElement[] { this.FromBigInteger(p.GetZCoord(0).ToBigInteger()) }, p.IsCompressed);
                }
            }
            return base.ImportPoint(p);
        }

        public override bool SupportsCoordinateSystem(int coord)
        {
            switch (coord)
            {
                case 0:
                case 1:
                case 2:
                case 4:
                    return true;
            }
            return false;
        }

        public virtual BigInteger Q =>
            this.m_q;

        public override ECPoint Infinity =>
            this.m_infinity;

        public override int FieldSize =>
            this.m_q.BitLength;
    }
}

