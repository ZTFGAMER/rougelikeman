namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecP256R1Curve : AbstractFpCurve
    {
        public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF"));
        private const int SecP256R1_DEFAULT_COORDS = 2;
        protected readonly SecP256R1Point m_infinity;

        public SecP256R1Curve() : base(q)
        {
            this.m_infinity = new SecP256R1Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFC")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("5AC635D8AA3A93E7B3EBBD55769886BC651D06B0CC53B0F63BCE3C3E27D2604B")));
            base.m_order = new BigInteger(1, Hex.Decode("FFFFFFFF00000000FFFFFFFFFFFFFFFFBCE6FAADA7179E84F3B9CAC2FC632551"));
            base.m_cofactor = BigInteger.One;
            base.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => 
            new SecP256R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecP256R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecP256R1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecP256R1FieldElement(x);

        public override bool SupportsCoordinateSystem(int coord)
        {
            if (coord != 2)
            {
                return false;
            }
            return true;
        }

        public virtual BigInteger Q =>
            q;

        public override ECPoint Infinity =>
            this.m_infinity;

        public override int FieldSize =>
            q.BitLength;
    }
}

