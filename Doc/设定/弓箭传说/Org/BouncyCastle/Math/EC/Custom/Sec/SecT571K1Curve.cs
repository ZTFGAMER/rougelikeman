namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT571K1Curve : AbstractF2mCurve
    {
        private const int SecT571K1_DEFAULT_COORDS = 6;
        protected readonly SecT571K1Point m_infinity;

        public SecT571K1Curve() : base(0x23b, 2, 5, 10)
        {
            this.m_infinity = new SecT571K1Point(this, null, null);
            base.m_a = this.FromBigInteger(BigInteger.Zero);
            base.m_b = this.FromBigInteger(BigInteger.One);
            base.m_order = new BigInteger(1, Hex.Decode("020000000000000000000000000000000000000000000000000000000000000000000000131850E1F19A63E4B391A8DB917F4138B630D84BE5D639381E91DEB45CFE778F637C1001"));
            base.m_cofactor = BigInteger.ValueOf(4L);
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT571K1Curve();

        protected override ECMultiplier CreateDefaultMultiplier() => 
            new WTauNafMultiplier();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT571K1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT571K1Point(this, x, y, zs, withCompression);

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
            true;

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

