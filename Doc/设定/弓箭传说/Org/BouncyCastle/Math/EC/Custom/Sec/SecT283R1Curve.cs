namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT283R1Curve : AbstractF2mCurve
    {
        private const int SecT283R1_DEFAULT_COORDS = 6;
        protected readonly SecT283R1Point m_infinity;

        public SecT283R1Curve() : base(0x11b, 5, 7, 12)
        {
            this.m_infinity = new SecT283R1Point(this, null, null);
            base.m_a = this.FromBigInteger(BigInteger.One);
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("027B680AC8B8596DA5A4AF8A19A0303FCA97FD7645309FA2A581485AF6263E313B79A2F5")));
            base.m_order = new BigInteger(1, Hex.Decode("03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEF90399660FC938A90165B042A7CEFADB307"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT283R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT283R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT283R1Point(this, x, y, zs, withCompression);

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
            false;

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

