namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT409R1Curve : AbstractF2mCurve
    {
        private const int SecT409R1_DEFAULT_COORDS = 6;
        protected readonly SecT409R1Point m_infinity;

        public SecT409R1Curve() : base(0x199, 0x57, 0, 0)
        {
            this.m_infinity = new SecT409R1Point(this, null, null);
            base.m_a = this.FromBigInteger(BigInteger.One);
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0021A5C2C8EE9FEB5C4B9A753B7B476B7FD6422EF1F3DD674761FA99D6AC27C8A9A197B272822F6CD57A55AA4F50AE317B13545F")));
            base.m_order = new BigInteger(1, Hex.Decode("010000000000000000000000000000000000000000000000000001E2AAD6A612F33307BE5FA47C3C9E052F838164CD37D9A21173"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT409R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT409R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT409R1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT409FieldElement(x);

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
            0x199;

        public override bool IsKoblitz =>
            false;

        public virtual int M =>
            0x199;

        public virtual bool IsTrinomial =>
            true;

        public virtual int K1 =>
            0x57;

        public virtual int K2 =>
            0;

        public virtual int K3 =>
            0;
    }
}

