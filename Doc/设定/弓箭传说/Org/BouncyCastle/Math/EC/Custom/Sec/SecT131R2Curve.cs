namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT131R2Curve : AbstractF2mCurve
    {
        private const int SecT131R2_DEFAULT_COORDS = 6;
        protected readonly SecT131R2Point m_infinity;

        public SecT131R2Curve() : base(0x83, 2, 3, 8)
        {
            this.m_infinity = new SecT131R2Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("03E5A88919D7CAFCBF415F07C2176573B2")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("04B8266A46C55657AC734CE38F018F2192")));
            base.m_order = new BigInteger(1, Hex.Decode("0400000000000000016954A233049BA98F"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT131R2Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT131R2Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT131R2Point(this, x, y, zs, withCompression);

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

        public override int FieldSize =>
            0x83;

        public override ECPoint Infinity =>
            this.m_infinity;

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

