namespace Org.BouncyCastle.Math.EC.Endo
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    public class GlvTypeBEndomorphism : GlvEndomorphism, ECEndomorphism
    {
        protected readonly ECCurve m_curve;
        protected readonly GlvTypeBParameters m_parameters;
        protected readonly ECPointMap m_pointMap;

        public GlvTypeBEndomorphism(ECCurve curve, GlvTypeBParameters parameters)
        {
            this.m_curve = curve;
            this.m_parameters = parameters;
            this.m_pointMap = new ScaleXPointMap(curve.FromBigInteger(parameters.Beta));
        }

        protected virtual BigInteger CalculateB(BigInteger k, BigInteger g, int t)
        {
            bool flag = g.SignValue < 0;
            BigInteger integer = k.Multiply(g.Abs());
            bool flag2 = integer.TestBit(t - 1);
            integer = integer.ShiftRight(t);
            if (flag2)
            {
                integer = integer.Add(BigInteger.One);
            }
            return (!flag ? integer : integer.Negate());
        }

        public virtual BigInteger[] DecomposeScalar(BigInteger k)
        {
            int bits = this.m_parameters.Bits;
            BigInteger integer = this.CalculateB(k, this.m_parameters.G1, bits);
            BigInteger integer2 = this.CalculateB(k, this.m_parameters.G2, bits);
            BigInteger[] integerArray = this.m_parameters.V1;
            BigInteger[] integerArray2 = this.m_parameters.V2;
            BigInteger integer3 = k.Subtract(integer.Multiply(integerArray[0]).Add(integer2.Multiply(integerArray2[0])));
            BigInteger integer4 = integer.Multiply(integerArray[1]).Add(integer2.Multiply(integerArray2[1])).Negate();
            return new BigInteger[] { integer3, integer4 };
        }

        public virtual ECPointMap PointMap =>
            this.m_pointMap;

        public virtual bool HasEfficientPointMap =>
            true;
    }
}

