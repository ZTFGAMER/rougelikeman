namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT131R1Curve : AbstractF2mCurve
    {
        private const int SecT131R1_DEFAULT_COORDS = 6;
        protected readonly SecT131R1Point m_infinity;

        public SecT131R1Curve() : base(0x83, 2, 3, 8)
        {
            this.m_infinity = new SecT131R1Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("07A11B09A76B562144418FF3FF8C2570B8")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0217C05610884B63B9C6C7291678F9D341")));
            base.m_order = new BigInteger(1, Hex.Decode("0400000000000000023123953A9464B54D"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT131R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT131R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT131R1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT131FieldElement(x);

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
            0x83;

        public override bool IsKoblitz =>
            false;

        public virtual int M =>
            0x83;

        public virtual bool IsTrinomial =>
            false;

        public virtual int K1 =>
            2;

        public virtual int K2 =>
            3;

        public virtual int K3 =>
            8;
    }
}

