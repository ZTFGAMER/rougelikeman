namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using System;

    public abstract class AbstractFpCurve : ECCurve
    {
        protected AbstractFpCurve(BigInteger q) : base(FiniteFields.GetPrimeField(q))
        {
        }

        protected override ECPoint DecompressPoint(int yTilde, BigInteger X1)
        {
            ECFieldElement b = this.FromBigInteger(X1);
            ECFieldElement y = b.Square().Add(this.A).Multiply(b).Add(this.B).Sqrt();
            if (y == null)
            {
                throw new ArgumentException("Invalid point compression");
            }
            if (y.TestBitZero() != (yTilde == 1))
            {
                y = y.Negate();
            }
            return this.CreateRawPoint(b, y, true);
        }

        public override bool IsValidFieldElement(BigInteger x) => 
            (((x != null) && (x.SignValue >= 0)) && (x.CompareTo(this.Field.Characteristic) < 0));
    }
}

