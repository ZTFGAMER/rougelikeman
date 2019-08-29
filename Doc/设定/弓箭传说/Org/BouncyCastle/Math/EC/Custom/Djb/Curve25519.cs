namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;

    internal class Curve25519 : AbstractFpCurve
    {
        public static readonly BigInteger q = Nat256.ToBigInteger(Curve25519Field.P);
        private const int Curve25519_DEFAULT_COORDS = 4;
        protected readonly Curve25519Point m_infinity;

        public Curve25519() : base(q)
        {
            this.m_infinity = new Curve25519Point(this, null, null);
            base.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("2AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA984914A144")));
            base.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("7B425ED097B425ED097B425ED097B425ED097B425ED097B4260B5E9C7710C864")));
            base.m_order = new BigInteger(1, Hex.Decode("1000000000000000000000000000000014DEF9DEA2F79CD65812631A5CF5D3ED"));
            base.m_cofactor = BigInteger.ValueOf(8L);
            base.m_coord = 4;
        }

        protected override ECCurve CloneCurve() => 
            new Curve25519();

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression) => 
            new Curve25519Point(this, x, y, withCompression);

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) => 
            new Curve25519Point(this, x, y, zs, withCompression);

        public override ECFieldElement FromBigInteger(BigInteger x) => 
            new Curve25519FieldElement(x);

        public override bool SupportsCoordinateSystem(int coord)
        {
            if (coord != 4)
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

