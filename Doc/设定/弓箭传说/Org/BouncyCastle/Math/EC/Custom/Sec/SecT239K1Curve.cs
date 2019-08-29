namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT239K1Curve : AbstractF2mCurve
    {
        private const int SecT239K1_DEFAULT_COORDS = 6;
        protected readonly SecT239K1Point m_infinity;

        public SecT239K1Curve() : base(0xef, 0x9e, 0, 0)
        {
            this.m_infinity = new SecT239K1Point(this, null, null);
            base.m_a = this.FromBigInteger(BigInteger.Zero);
            base.m_b = this.FromBigInteger(BigInteger.One);
            base.m_order = new BigInteger(1, Hex.Decode("2000000000000000000000000000005A79FEC67CB6E91F1C1DA800E478A5"));
            base.m_cofactor = BigInteger.ValueOf(4L);
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT239K1Curve();

        protected override ECMultiplier CreateDefaultMultiplier() => 
            new WTauNafMultiplier();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT239K1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT239K1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT239FieldElement(x);

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
            0xef;

        public override bool IsKoblitz =>
            true;

        public virtual int M =>
            0xef;

        public virtual bool IsTrinomial =>
            true;

        public virtual int K1 =>
            0x9e;

        public virtual int K2 =>
            0;

        public virtual int K3 =>
            0;
    }
}

