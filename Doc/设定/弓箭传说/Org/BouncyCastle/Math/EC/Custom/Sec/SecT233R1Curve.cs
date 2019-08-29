namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class SecT233R1Curve : AbstractF2mCurve
    {
        private const int SecT233R1_DEFAULT_COORDS = 6;
        protected readonly SecT233R1Point m_infinity;

        public SecT233R1Curve() : base(0xe9, 0x4a, 0, 0)
        {
            this.m_infinity = new SecT233R1Point(this, null, null);
            base.m_a = this.FromBigInteger(BigInteger.One);
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0066647EDE6C332C7F8C0923BB58213B333B20E9CE4281FE115F7D8F90AD")));
            base.m_order = new BigInteger(1, Hex.Decode("01000000000000000000000000000013E974E72F8A6922031D2603CFE0D7"));
            base.m_cofactor = BigInteger.Two;
            base.m_coord = 6;
        }

        protected override ECCurve CloneCurve() => 
            new SecT233R1Curve();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new SecT233R1Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new SecT233R1Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new SecT233FieldElement(x);

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
            0xe9;

        public override bool IsKoblitz =>
            false;

        public virtual int M =>
            0xe9;

        public virtual bool IsTrinomial =>
            true;

        public virtual int K1 =>
            0x4a;

        public virtual int K2 =>
            0;

        public virtual int K3 =>
            0;
    }
}

