namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecP160R2Curve : AbstractFpCurve
    {
        public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFAC73"));
        private const int SecP160R2_DEFAULT_COORDS = 2;
        protected readonly SecP160R2Point m_infinity;

        public SecP160R2Curve() : base(q)
        {
            this.m_infinity = new SecP160R2Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFAC70")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("B4E134D3FB59EB8BAB57274904664D5AF50388BA")));
            base.m_order = new BigInteger(1, Hex.Decode("0100000000000000000000351EE786A818F3A1A16B"));
            base.m_cofactor = BigInteger.One;
            base.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => 
            new SecP160R2Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecP160R2Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecP160R2Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecP160R2FieldElement(x);

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

