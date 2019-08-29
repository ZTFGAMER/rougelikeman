namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecP384R1Curve : AbstractFpCurve
    {
        public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFF"));
        private const int SecP384R1_DEFAULT_COORDS = 2;
        protected readonly SecP384R1Point m_infinity;

        public SecP384R1Curve() : base(q)
        {
            this.m_infinity = new SecP384R1Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFC")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("B3312FA7E23EE7E4988E056BE3F82D19181D9C6EFE8141120314088F5013875AC656398D8A2ED19D2A85C8EDD3EC2AEF")));
            base.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC7634D81F4372DDF581A0DB248B0A77AECEC196ACCC52973"));
            base.m_cofactor = BigInteger.One;
            base.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => 
            new SecP384R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecP384R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecP384R1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecP384R1FieldElement(x);

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

