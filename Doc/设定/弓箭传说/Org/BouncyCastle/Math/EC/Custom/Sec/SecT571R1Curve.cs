namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT571R1Curve : AbstractF2mCurve
    {
        private const int SecT571R1_DEFAULT_COORDS = 6;
        protected readonly SecT571R1Point m_infinity;
        internal static readonly SecT571FieldElement SecT571R1_B = new SecT571FieldElement(new BigInteger(1, Hex.Decode("02F40E7E2221F295DE297117B7F3D62F5C6A97FFCB8CEFF1CD6BA8CE4A9A18AD84FFABBD8EFA59332BE7AD6756A66E294AFD185A78FF12AA520E4DE739BACA0C7FFEFF7F2955727A")));
        internal static readonly SecT571FieldElement SecT571R1_B_SQRT = ((SecT571FieldElement) SecT571R1_B.Sqrt());

        public SecT571R1Curve() : base(0x23b, 2, 5, 10)
        {
            this.m_infinity = new SecT571R1Point(this, null, null);
            base.m_a = this.FromBigInteger(BigInteger.One);
            base.m_b = SecT571R1_B;
            base.m_order = new BigInteger(1, Hex.Decode("03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE661CE18FF55987308059B186823851EC7DD9CA1161DE93D5174D66E8382E9BB2FE84E47"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT571R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT571R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT571R1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT571FieldElement(x);

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
            0x23b;

        public override bool IsKoblitz =>
            false;

        public virtual int M =>
            0x23b;

        public virtual bool IsTrinomial =>
            false;

        public virtual int K1 =>
            2;

        public virtual int K2 =>
            5;

        public virtual int K3 =>
            10;
    }
}

