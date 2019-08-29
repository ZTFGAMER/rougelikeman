namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT163K1Curve : AbstractF2mCurve
    {
        private const int SecT163K1_DEFAULT_COORDS = 6;
        protected readonly SecT163K1Point m_infinity;

        public SecT163K1Curve() : base(0xa3, 3, 6, 7)
        {
            this.m_infinity = new SecT163K1Point(this, null, null);
            base.m_a = this.FromBigInteger(BigInteger.One);
            base.m_b = base.m_a;
            base.m_order = new BigInteger(1, Hex.Decode("04000000000000000000020108A2E0CC0D99F8A5EF"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT163K1Curve();

        protected override ECMultiplier CreateDefaultMultiplier() => 
            new WTauNafMultiplier();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT163K1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT163K1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT163FieldElement(x);

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
            0xa3;

        public override bool IsKoblitz =>
            true;

        public virtual int M =>
            0xa3;

        public virtual bool IsTrinomial =>
            false;

        public virtual int K1 =>
            3;

        public virtual int K2 =>
            6;

        public virtual int K3 =>
            7;
    }
}

