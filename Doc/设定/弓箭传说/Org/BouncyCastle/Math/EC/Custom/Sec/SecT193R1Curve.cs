namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT193R1Curve : AbstractF2mCurve
    {
        private const int SecT193R1_DEFAULT_COORDS = 6;
        protected readonly SecT193R1Point m_infinity;

        public SecT193R1Curve() : base(0xc1, 15, 0, 0)
        {
            this.m_infinity = new SecT193R1Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("0017858FEB7A98975169E171F77B4087DE098AC8A911DF7B01")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("00FDFB49BFE6C3A89FACADAA7A1E5BBC7CC1C2E5D831478814")));
            base.m_order = new BigInteger(1, Hex.Decode("01000000000000000000000000C7F34A778F443ACC920EBA49"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT193R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT193R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT193R1Point(this, x, y, zs, withCompression);

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

