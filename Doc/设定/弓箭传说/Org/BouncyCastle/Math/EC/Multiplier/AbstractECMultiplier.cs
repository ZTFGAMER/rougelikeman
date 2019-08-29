namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    public abstract class AbstractECMultiplier : ECMultiplier
    {
        protected AbstractECMultiplier()
        {
        }

        public virtual ECPoint Multiply(ECPoint p, BigInteger k)
        {
            int signValue = k.SignValue;
            if ((signValue == 0) || p.IsInfinity)
            {
                return p.Curve.Infinity;
            }
            ECPoint point = this.MultiplyPositive(p, k.Abs());
            ECPoint point2 = (signValue <= 0) ? point.Negate() : point;
            return ECAlgorithms.ValidatePoint(point2);
        }

        protected abstract ECPoint MultiplyPositive(ECPoint p, BigInteger k);
    }
}

