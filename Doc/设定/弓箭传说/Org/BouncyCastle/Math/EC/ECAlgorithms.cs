namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC.Endo;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Math.Field;
    using System;

    public class ECAlgorithms
    {
        internal static ECPoint ImplShamirsTrickJsf(ECPoint P, BigInteger k, ECPoint Q, BigInteger l)
        {
            ECCurve curve = P.Curve;
            ECPoint infinity = curve.Infinity;
            ECPoint point2 = P.Add(Q);
            ECPoint point3 = P.Subtract(Q);
            ECPoint[] points = new ECPoint[] { Q, point3, P, point2 };
            curve.NormalizeAll(points);
            ECPoint[] pointArray2 = new ECPoint[] { points[3].Negate(), points[2].Negate(), points[1].Negate(), points[0].Negate(), infinity, points[0], points[1], points[2], points[3] };
            byte[] buffer = WNafUtilities.GenerateJsf(k, l);
            ECPoint point4 = infinity;
            int length = buffer.Length;
            while (--length >= 0)
            {
                int num2 = buffer[length];
                int num3 = (num2 << 0x18) >> 0x1c;
                int num4 = (num2 << 0x1c) >> 0x1c;
                int index = (4 + (num3 * 3)) + num4;
                point4 = point4.TwicePlus(pointArray2[index]);
            }
            return point4;
        }

        internal static ECPoint ImplShamirsTrickWNaf(ECPoint P, BigInteger k, ECPoint Q, BigInteger l)
        {
            bool flag = k.SignValue < 0;
            bool flag2 = l.SignValue < 0;
            k = k.Abs();
            l = l.Abs();
            int width = Math.Max(2, Math.Min(0x10, WNafUtilities.GetWindowSize(k.BitLength)));
            int num2 = Math.Max(2, Math.Min(0x10, WNafUtilities.GetWindowSize(l.BitLength)));
            WNafPreCompInfo info = WNafUtilities.Precompute(P, width, true);
            WNafPreCompInfo info2 = WNafUtilities.Precompute(Q, num2, true);
            ECPoint[] preCompP = !flag ? info.PreComp : info.PreCompNeg;
            ECPoint[] preCompQ = !flag2 ? info2.PreComp : info2.PreCompNeg;
            ECPoint[] preCompNegP = !flag ? info.PreCompNeg : info.PreComp;
            ECPoint[] preCompNegQ = !flag2 ? info2.PreCompNeg : info2.PreComp;
            byte[] wnafP = WNafUtilities.GenerateWindowNaf(width, k);
            byte[] wnafQ = WNafUtilities.GenerateWindowNaf(num2, l);
            return ImplShamirsTrickWNaf(preCompP, preCompNegP, wnafP, preCompQ, preCompNegQ, wnafQ);
        }

        internal static ECPoint ImplShamirsTrickWNaf(ECPoint P, BigInteger k, ECPointMap pointMapQ, BigInteger l)
        {
            bool flag = k.SignValue < 0;
            bool flag2 = l.SignValue < 0;
            k = k.Abs();
            l = l.Abs();
            int width = Math.Max(2, Math.Min(0x10, WNafUtilities.GetWindowSize(Math.Max(k.BitLength, l.BitLength))));
            ECPoint p = WNafUtilities.MapPointWithPrecomp(P, width, true, pointMapQ);
            WNafPreCompInfo wNafPreCompInfo = WNafUtilities.GetWNafPreCompInfo(P);
            WNafPreCompInfo info2 = WNafUtilities.GetWNafPreCompInfo(p);
            ECPoint[] preCompP = !flag ? wNafPreCompInfo.PreComp : wNafPreCompInfo.PreCompNeg;
            ECPoint[] preCompQ = !flag2 ? info2.PreComp : info2.PreCompNeg;
            ECPoint[] preCompNegP = !flag ? wNafPreCompInfo.PreCompNeg : wNafPreCompInfo.PreComp;
            ECPoint[] preCompNegQ = !flag2 ? info2.PreCompNeg : info2.PreComp;
            byte[] wnafP = WNafUtilities.GenerateWindowNaf(width, k);
            byte[] wnafQ = WNafUtilities.GenerateWindowNaf(width, l);
            return ImplShamirsTrickWNaf(preCompP, preCompNegP, wnafP, preCompQ, preCompNegQ, wnafQ);
        }

        private static ECPoint ImplShamirsTrickWNaf(ECPoint[] preCompP, ECPoint[] preCompNegP, byte[] wnafP, ECPoint[] preCompQ, ECPoint[] preCompNegQ, byte[] wnafQ)
        {
            int num = Math.Max(wnafP.Length, wnafQ.Length);
            ECPoint infinity = preCompP[0].Curve.Infinity;
            ECPoint point2 = infinity;
            int e = 0;
            for (int i = num - 1; i >= 0; i--)
            {
                int num4 = (i >= wnafP.Length) ? 0 : ((int) ((sbyte) wnafP[i]));
                int num5 = (i >= wnafQ.Length) ? 0 : ((int) ((sbyte) wnafQ[i]));
                if ((num4 | num5) == 0)
                {
                    e++;
                }
                else
                {
                    ECPoint b = infinity;
                    if (num4 != 0)
                    {
                        int num6 = Math.Abs(num4);
                        ECPoint[] pointArray = (num4 >= 0) ? preCompP : preCompNegP;
                        b = b.Add(pointArray[num6 >> 1]);
                    }
                    if (num5 != 0)
                    {
                        int num7 = Math.Abs(num5);
                        ECPoint[] pointArray2 = (num5 >= 0) ? preCompQ : preCompNegQ;
                        b = b.Add(pointArray2[num7 >> 1]);
                    }
                    if (e > 0)
                    {
                        point2 = point2.TimesPow2(e);
                        e = 0;
                    }
                    point2 = point2.TwicePlus(b);
                }
            }
            if (e > 0)
            {
                point2 = point2.TimesPow2(e);
            }
            return point2;
        }

        internal static ECPoint ImplSumOfMultiplies(ECPoint[] ps, BigInteger[] ks)
        {
            int length = ps.Length;
            bool[] negs = new bool[length];
            WNafPreCompInfo[] infos = new WNafPreCompInfo[length];
            byte[][] wnafs = new byte[length][];
            for (int i = 0; i < length; i++)
            {
                BigInteger k = ks[i];
                negs[i] = k.SignValue < 0;
                k = k.Abs();
                int width = Math.Max(2, Math.Min(0x10, WNafUtilities.GetWindowSize(k.BitLength)));
                infos[i] = WNafUtilities.Precompute(ps[i], width, true);
                wnafs[i] = WNafUtilities.GenerateWindowNaf(width, k);
            }
            return ImplSumOfMultiplies(negs, infos, wnafs);
        }

        internal static ECPoint ImplSumOfMultiplies(ECPoint[] ps, ECPointMap pointMap, BigInteger[] ks)
        {
            int length = ps.Length;
            int num2 = length << 1;
            bool[] negs = new bool[num2];
            WNafPreCompInfo[] infos = new WNafPreCompInfo[num2];
            byte[][] wnafs = new byte[num2][];
            for (int i = 0; i < length; i++)
            {
                int index = i << 1;
                int num5 = index + 1;
                BigInteger k = ks[index];
                negs[index] = k.SignValue < 0;
                k = k.Abs();
                BigInteger integer2 = ks[num5];
                negs[num5] = integer2.SignValue < 0;
                integer2 = integer2.Abs();
                int width = Math.Max(2, Math.Min(0x10, WNafUtilities.GetWindowSize(Math.Max(k.BitLength, integer2.BitLength))));
                ECPoint p = ps[i];
                ECPoint point2 = WNafUtilities.MapPointWithPrecomp(p, width, true, pointMap);
                infos[index] = WNafUtilities.GetWNafPreCompInfo(p);
                infos[num5] = WNafUtilities.GetWNafPreCompInfo(point2);
                wnafs[index] = WNafUtilities.GenerateWindowNaf(width, k);
                wnafs[num5] = WNafUtilities.GenerateWindowNaf(width, integer2);
            }
            return ImplSumOfMultiplies(negs, infos, wnafs);
        }

        private static ECPoint ImplSumOfMultiplies(bool[] negs, WNafPreCompInfo[] infos, byte[][] wnafs)
        {
            int num = 0;
            int length = wnafs.Length;
            for (int i = 0; i < length; i++)
            {
                num = Math.Max(num, wnafs[i].Length);
            }
            ECPoint infinity = infos[0].PreComp[0].Curve.Infinity;
            ECPoint point2 = infinity;
            int e = 0;
            for (int j = num - 1; j >= 0; j--)
            {
                ECPoint b = infinity;
                for (int k = 0; k < length; k++)
                {
                    byte[] buffer = wnafs[k];
                    int num7 = (j >= buffer.Length) ? 0 : ((int) ((sbyte) buffer[j]));
                    if (num7 != 0)
                    {
                        int num8 = Math.Abs(num7);
                        WNafPreCompInfo info = infos[k];
                        ECPoint[] pointArray = ((num7 < 0) != negs[k]) ? info.PreCompNeg : info.PreComp;
                        b = b.Add(pointArray[num8 >> 1]);
                    }
                }
                if (b == infinity)
                {
                    e++;
                }
                else
                {
                    if (e > 0)
                    {
                        point2 = point2.TimesPow2(e);
                        e = 0;
                    }
                    point2 = point2.TwicePlus(b);
                }
            }
            if (e > 0)
            {
                point2 = point2.TimesPow2(e);
            }
            return point2;
        }

        internal static ECPoint ImplSumOfMultipliesGlv(ECPoint[] ps, BigInteger[] ks, GlvEndomorphism glvEndomorphism)
        {
            BigInteger order = ps[0].Curve.Order;
            int length = ps.Length;
            BigInteger[] integerArray = new BigInteger[length << 1];
            int index = 0;
            int num3 = 0;
            while (index < length)
            {
                BigInteger[] integerArray2 = glvEndomorphism.DecomposeScalar(ks[index].Mod(order));
                integerArray[num3++] = integerArray2[0];
                integerArray[num3++] = integerArray2[1];
                index++;
            }
            ECPointMap pointMap = glvEndomorphism.PointMap;
            if (glvEndomorphism.HasEfficientPointMap)
            {
                return ImplSumOfMultiplies(ps, pointMap, integerArray);
            }
            ECPoint[] pointArray = new ECPoint[length << 1];
            int num4 = 0;
            int num5 = 0;
            while (num4 < length)
            {
                ECPoint p = ps[num4];
                ECPoint point2 = pointMap.Map(p);
                pointArray[num5++] = p;
                pointArray[num5++] = point2;
                num4++;
            }
            return ImplSumOfMultiplies(pointArray, integerArray);
        }

        public static ECPoint ImportPoint(ECCurve c, ECPoint p)
        {
            ECCurve other = p.Curve;
            if (!c.Equals(other))
            {
                throw new ArgumentException("Point must be on the same curve");
            }
            return c.ImportPoint(p);
        }

        public static bool IsF2mCurve(ECCurve c) => 
            IsF2mField(c.Field);

        public static bool IsF2mField(IFiniteField field) => 
            (((field.Dimension > 1) && field.Characteristic.Equals(BigInteger.Two)) && (field is IPolynomialExtensionField));

        public static bool IsFpCurve(ECCurve c) => 
            IsFpField(c.Field);

        public static bool IsFpField(IFiniteField field) => 
            (field.Dimension == 1);

        public static void MontgomeryTrick(ECFieldElement[] zs, int off, int len)
        {
            MontgomeryTrick(zs, off, len, null);
        }

        public static void MontgomeryTrick(ECFieldElement[] zs, int off, int len, ECFieldElement scale)
        {
            ECFieldElement[] elementArray = new ECFieldElement[len];
            elementArray[0] = zs[off];
            int index = 0;
            while (++index < len)
            {
                elementArray[index] = elementArray[index - 1].Multiply(zs[off + index]);
            }
            index--;
            if (scale != null)
            {
                elementArray[index] = elementArray[index].Multiply(scale);
            }
            ECFieldElement b = elementArray[index].Invert();
            while (index > 0)
            {
                int num2 = off + index--;
                ECFieldElement element2 = zs[num2];
                zs[num2] = elementArray[index].Multiply(b);
                b = b.Multiply(element2);
            }
            zs[off] = b;
        }

        public static ECPoint ReferenceMultiply(ECPoint p, BigInteger k)
        {
            BigInteger integer = k.Abs();
            ECPoint infinity = p.Curve.Infinity;
            int bitLength = integer.BitLength;
            if (bitLength > 0)
            {
                if (integer.TestBit(0))
                {
                    infinity = p;
                }
                for (int i = 1; i < bitLength; i++)
                {
                    p = p.Twice();
                    if (integer.TestBit(i))
                    {
                        infinity = infinity.Add(p);
                    }
                }
            }
            return ((k.SignValue >= 0) ? infinity : infinity.Negate());
        }

        public static ECPoint ShamirsTrick(ECPoint P, BigInteger k, ECPoint Q, BigInteger l)
        {
            Q = ImportPoint(P.Curve, Q);
            return ValidatePoint(ImplShamirsTrickJsf(P, k, Q, l));
        }

        public static ECPoint SumOfMultiplies(ECPoint[] ps, BigInteger[] ks)
        {
            if (((ps == null) || (ks == null)) || ((ps.Length != ks.Length) || (ps.Length < 1)))
            {
                throw new ArgumentException("point and scalar arrays should be non-null, and of equal, non-zero, length");
            }
            int length = ps.Length;
            switch (length)
            {
                case 1:
                    return ps[0].Multiply(ks[0]);

                case 2:
                    return SumOfTwoMultiplies(ps[0], ks[0], ps[1], ks[1]);
            }
            ECPoint point = ps[0];
            ECCurve c = point.Curve;
            ECPoint[] pointArray = new ECPoint[length];
            pointArray[0] = point;
            for (int i = 1; i < length; i++)
            {
                pointArray[i] = ImportPoint(c, ps[i]);
            }
            GlvEndomorphism glvEndomorphism = c.GetEndomorphism() as GlvEndomorphism;
            if (glvEndomorphism != null)
            {
                return ValidatePoint(ImplSumOfMultipliesGlv(pointArray, ks, glvEndomorphism));
            }
            return ValidatePoint(ImplSumOfMultiplies(pointArray, ks));
        }

        public static ECPoint SumOfTwoMultiplies(ECPoint P, BigInteger a, ECPoint Q, BigInteger b)
        {
            ECCurve c = P.Curve;
            Q = ImportPoint(c, Q);
            AbstractF2mCurve curve2 = c as AbstractF2mCurve;
            if ((curve2 != null) && curve2.IsKoblitz)
            {
                return ValidatePoint(P.Multiply(a).Add(Q.Multiply(b)));
            }
            GlvEndomorphism glvEndomorphism = c.GetEndomorphism() as GlvEndomorphism;
            if (glvEndomorphism != null)
            {
                ECPoint[] ps = new ECPoint[] { P, Q };
                BigInteger[] ks = new BigInteger[] { a, b };
                return ValidatePoint(ImplSumOfMultipliesGlv(ps, ks, glvEndomorphism));
            }
            return ValidatePoint(ImplShamirsTrickWNaf(P, a, Q, b));
        }

        public static ECPoint ValidatePoint(ECPoint p)
        {
            if (!p.IsValid())
            {
                throw new ArgumentException("Invalid point", "p");
            }
            return p;
        }
    }
}

