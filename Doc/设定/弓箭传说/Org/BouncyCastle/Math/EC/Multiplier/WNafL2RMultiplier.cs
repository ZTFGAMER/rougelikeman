namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    public class WNafL2RMultiplier : AbstractECMultiplier
    {
        protected virtual int GetWindowSize(int bits) => 
            WNafUtilities.GetWindowSize(bits);

        protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
        {
            int width = Math.Max(2, Math.Min(0x10, this.GetWindowSize(k.BitLength)));
            WNafPreCompInfo info = WNafUtilities.Precompute(p, width, true);
            ECPoint[] preComp = info.PreComp;
            ECPoint[] preCompNeg = info.PreCompNeg;
            int[] numArray = WNafUtilities.GenerateCompactWindowNaf(width, k);
            ECPoint infinity = p.Curve.Infinity;
            int length = numArray.Length;
            if (length > 1)
            {
                int num3 = numArray[--length];
                int num4 = num3 >> 0x10;
                int e = num3 & 0xffff;
                int index = Math.Abs(num4);
                ECPoint[] pointArray3 = (num4 >= 0) ? preComp : preCompNeg;
                if ((index << 2) < (((int) 1) << width))
                {
                    int num7 = LongArray.BitLengths[index];
                    int num8 = width - num7;
                    int num9 = index ^ (((int) 1) << (num7 - 1));
                    int num10 = (((int) 1) << (width - 1)) - 1;
                    int num11 = (num9 << num8) + 1;
                    infinity = pointArray3[num10 >> 1].Add(pointArray3[num11 >> 1]);
                    e -= num8;
                }
                else
                {
                    infinity = pointArray3[index >> 1];
                }
                infinity = infinity.TimesPow2(e);
            }
            while (length > 0)
            {
                int num12 = numArray[--length];
                int num13 = num12 >> 0x10;
                int e = num12 & 0xffff;
                int num15 = Math.Abs(num13);
                ECPoint[] pointArray4 = (num13 >= 0) ? preComp : preCompNeg;
                ECPoint b = pointArray4[num15 >> 1];
                infinity = infinity.TwicePlus(b).TimesPow2(e);
            }
            return infinity;
        }
    }
}

