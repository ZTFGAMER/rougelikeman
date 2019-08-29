namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT283K1Curve : AbstractF2mCurve
    {
        private const int SecT283K1_DEFAULT_COORDS = 6;
        protected readonly SecT283K1Point m_infinity;

        public SecT283K1Curve() : base(0x11b, 5, 7, 12)
        {
            this.m_infinity = new SecT283K1Point(this, null, null);
            base.m_a = this.FromBigInteger(BigInteger.Zero);
            base.m_b = this.FromBigInteger(BigInteger.One);
            base.m_order = new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE9AE2ED07577265DFF7F94451E061E163C61"));
            base.m_cofactor = BigInteger.ValueOf(4L);
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT283K1Curve();

        protected override ECMultiplier CreateDefaultMultiplier() => 
            new WTauNafMultiplier();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT283K1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT283K1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT283FieldElement(x);

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
            0x11b;

        public override bool IsKoblitz =>
            true;

        public virtual int M =>
            0x11b;

        public virtual bool IsTrinomial =>
            false;

        public virtual int K1 =>
            5;

        public virtual int K2 =>
            7;

        public virtual int K3 =>
            12;
    }
}

