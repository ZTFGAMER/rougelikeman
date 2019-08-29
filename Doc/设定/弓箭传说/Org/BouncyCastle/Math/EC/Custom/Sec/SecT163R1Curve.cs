namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT163R1Curve : AbstractF2mCurve
    {
        private const int SecT163R1_DEFAULT_COORDS = 6;
        protected readonly SecT163R1Point m_infinity;

        public SecT163R1Curve() : base(0xa3, 3, 6, 7)
        {
            this.m_infinity = new SecT163R1Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("07B6882CAAEFA84F9554FF8428BD88E246D2782AE2")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0713612DCDDCB40AAB946BDA29CA91F73AF958AFD9")));
            base.m_order = new BigInteger(1, Hex.Decode("03FFFFFFFFFFFFFFFFFFFF48AAB689C29CA710279B"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT163R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT163R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT163R1Point(this, x, y, zs, withCompression);

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
            false;

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

