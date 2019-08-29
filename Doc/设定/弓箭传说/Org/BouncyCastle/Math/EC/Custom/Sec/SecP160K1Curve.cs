namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecP160K1Curve : AbstractFpCurve
    {
        public static readonly BigInteger q = SecP160R2Curve.q;
        private const int SECP160K1_DEFAULT_COORDS = 2;
        protected readonly SecP160K1Point m_infinity;

        public SecP160K1Curve() : base(q)
        {
            this.m_infinity = new SecP160K1Point(this, null, null);
            base.m_a = this.FromBigInteger(BigInteger.Zero);
            base.m_b = this.FromBigInteger(BigInteger.ValueOf(7L));
            base.m_order = new BigInteger(1, Hex.Decode("0100000000000000000001B8FA16DFAB9ACA16B6B3"));
            base.m_cofactor = BigInteger.One;
            base.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => 
            new SecP160K1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecP160K1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecP160K1Point(this, x, y, zs, withCompression);

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

