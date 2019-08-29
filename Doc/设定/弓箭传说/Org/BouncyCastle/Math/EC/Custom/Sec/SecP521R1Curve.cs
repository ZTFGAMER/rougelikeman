namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecP521R1Curve : AbstractFpCurve
    {
        public static readonly BigInteger q = new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"));
        private const int SecP521R1_DEFAULT_COORDS = 2;
        protected readonly SecP521R1Point m_infinity;

        public SecP521R1Curve() : base(q)
        {
            this.m_infinity = new SecP521R1Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0051953EB9618E1C9A1F929A21A0B68540EEA2DA725B99B315F3B8B489918EF109E156193951EC7E937B1652C0BD3BB1BF073573DF883D2C34F1EF451FD46B503F00")));
            base.m_order = new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFA51868783BF2F966B7FCC0148F709A5D03BB5C9B8899C47AEBB6FB71E91386409"));
            base.m_cofactor = BigInteger.One;
            base.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => 
            new SecP521R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecP521R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecP521R1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecP521R1FieldElement(x);

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

