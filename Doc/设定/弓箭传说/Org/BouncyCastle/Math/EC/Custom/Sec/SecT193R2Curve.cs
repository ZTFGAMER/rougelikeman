namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT193R2Curve : AbstractF2mCurve
    {
        private const int SecT193R2_DEFAULT_COORDS = 6;
        protected readonly SecT193R2Point m_infinity;

        public SecT193R2Curve() : base(0xc1, 15, 0, 0)
        {
            this.m_infinity = new SecT193R2Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("0163F35A5137C2CE3EA6ED8667190B0BC43ECD69977702709B")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("00C9BB9E8927D4D64C377E2AB2856A5B16E3EFB7F61D4316AE")));
            base.m_order = new BigInteger(1, Hex.Decode("010000000000000000000000015AAB561B005413CCD4EE99D5"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT193R2Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT193R2Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT193R2Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT193FieldElement(x);

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
            0xc1;

        public override bool IsKoblitz =>
            false;

        public virtual int M =>
            0xc1;

        public virtual bool IsTrinomial =>
            true;

        public virtual int K1 =>
            15;

        public virtual int K2 =>
            0;

        public virtual int K3 =>
            0;
    }
}

