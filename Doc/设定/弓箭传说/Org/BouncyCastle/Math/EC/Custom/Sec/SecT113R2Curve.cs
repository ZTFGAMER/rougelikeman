namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT113R2Curve : AbstractF2mCurve
    {
        private const int SecT113R2_DEFAULT_COORDS = 6;
        protected readonly SecT113R2Point m_infinity;

        public SecT113R2Curve() : base(0x71, 9, 0, 0)
        {
            this.m_infinity = new SecT113R2Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("00689918DBEC7E5A0DD6DFC0AA55C7")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0095E9A9EC9B297BD4BF36E059184F")));
            base.m_order = new BigInteger(1, Hex.Decode("010000000000000108789B2496AF93"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT113R2Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT113R2Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT113R2Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT113FieldElement(x);

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

