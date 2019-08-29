namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    public class FixedPointUtilities
    {
        public static readonly string PRECOMP_NAME = "bc_fixed_point";

        public static int GetCombSize(ECCurve c)
        {
            BigInteger order = c.Order;
            return ((order != null) ? order.BitLength : (c.FieldSize + 1));
        }

        public static FixedPointPreCompInfo GetFixedPointPreCompInfo(PreCompInfo preCompInfo)
        {
            if ((preCompInfo != null) && (preCompInfo is FixedPointPreCompInfo))
            {
                return (FixedPointPreCompInfo) preCompInfo;
            }
            return new FixedPointPreCompInfo();
        }

        public static FixedPointPreCompInfo Precompute(ECPoint p, int minWidth)
        {
            ECCurve c = p.Curve;
            int num = ((int) 1) << minWidth;
            FixedPointPreCompInfo fixedPointPreCompInfo = GetFixedPointPreCompInfo(c.GetPreCompInfo(p, PRECOMP_NAME));
            ECPoint[] preComp = fixedPointPreCompInfo.PreComp;
            if ((preComp == null) || (preComp.Length < num))
            {
                int e = ((GetCombSize(c) + minWidth) - 1) / minWidth;
                ECPoint[] points = new ECPoint[minWidth];
                points[0] = p;
                for (int i = 1; i < minWidth; i++)
                {
                    points[i] = points[i - 1].TimesPow2(e);
                }
                c.NormalizeAll(points);
                preComp = new ECPoint[num];
                preComp[0] = c.Infinity;
                for (int j = minWidth - 1; j >= 0; j--)
                {
                    ECPoint b = points[j];
                    int num6 = ((int) 1) << j;
                    for (int k = num6; k < num; k += num6 << 1)
                    {
                        preComp[k] = preComp[k - num6].Add(b);
                    }
                }
                c.NormalizeAll(preComp);
                fixedPointPreCompInfo.PreComp = preComp;
                fixedPointPreCompInfo.Width = minWidth;
                c.SetPreCompInfo(p, PRECOMP_NAME, fixedPointPreCompInfo);
            }
            return fixedPointPreCompInfo;
        }
    }
}

