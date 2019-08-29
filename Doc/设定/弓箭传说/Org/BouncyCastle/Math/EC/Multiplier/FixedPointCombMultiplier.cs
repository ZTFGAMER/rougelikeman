namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    public class FixedPointCombMultiplier : AbstractECMultiplier
    {
        protected virtual int GetWidthForCombSize(int combSize) => 
            ((combSize <= 0x101) ? 5 : 6);

        protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
        {
            ECCurve c = p.Curve;
            int combSize = FixedPointUtilities.GetCombSize(c);
            if (k.BitLength > combSize)
            {
                throw new InvalidOperationException("fixed-point comb doesn't support scalars larger than the curve order");
            }
            int widthForCombSize = this.GetWidthForCombSize(combSize);
            FixedPointPreCompInfo info = FixedPointUtilities.Precompute(p, widthForCombSize);
            ECPoint[] preComp = info.PreComp;
            int width = info.Width;
            int num4 = ((combSize + width) - 1) / width;
            ECPoint infinity = c.Infinity;
            int num5 = (num4 * width) - 1;
            for (int i = 0; i < num4; i++)
            {
                int index = 0;
                for (int j = num5 - i; j >= 0; j -= num4)
                {
                    index = index << 1;
                    if (k.TestBit(j))
                    {
                        index |= 1;
                    }
                }
                infinity = infinity.TwicePlus(preComp[index]);
            }
            return infinity;
        }
    }
}

