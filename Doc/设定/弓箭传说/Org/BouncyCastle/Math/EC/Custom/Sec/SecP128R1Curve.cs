namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecP128R1Curve : AbstractFpCurve
    {
        public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFF"));
        private const int SecP128R1_DEFAULT_COORDS = 2;
        protected readonly SecP128R1Point m_infinity;

        public SecP128R1Curve() : base(q)
        {
            this.m_infinity = new SecP128R1Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFC")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("E87579C11079F43DD824993C2CEE5ED3")));
            base.m_order = new BigInteger(1, Hex.Decode("FFFFFFFE0000000075A30D1B9038A115"));
            base.m_cofactor = BigInteger.One;
            base.m_coord = 2;
        }

        protected override ECCurve CloneCurve() => 
            new SecP128R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecP128R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecP128R1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecP128R1FieldElement(x);

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

