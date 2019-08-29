namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.EC.Endo;
    using System;

    public class GlvMultiplier : AbstractECMultiplier
    {
        protected readonly ECCurve curve;
        protected readonly GlvEndomorphism glvEndomorphism;

        public GlvMultiplier(ECCurve curve, GlvEndomorphism glvEndomorphism)
        {
            if ((curve == null) || (curve.Order == null))
            {
                throw new ArgumentException("Need curve with known group order", "curve");
            }
            this.curve = curve;
            this.glvEndomorphism = glvEndomorphism;
        }

        protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
        {
            if (!this.curve.Equals(p.Curve))
            {
                throw new InvalidOperationException();
            }
            BigInteger order = p.Curve.Order;
            BigInteger[] integerArray = this.glvEndomorphism.DecomposeScalar(k.Mod(order));
            BigInteger integer2 = integerArray[0];
            BigInteger l = integerArray[1];
            ECPointMap pointMap = this.glvEndomorphism.PointMap;
            if (this.glvEndomorphism.HasEfficientPointMap)
            {
                return ECAlgorithms.ImplShamirsTrickWNaf(p, integer2, pointMap, l);
            }
            return ECAlgorithms.ImplShamirsTrickWNaf(p, integer2, pointMap.Map(p), l);
        }
    }
}

