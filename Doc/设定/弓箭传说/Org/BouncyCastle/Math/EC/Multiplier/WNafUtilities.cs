namespace Org.BouncyCastle.Math.EC.Multiplier
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    public abstract class WNafUtilities
    {
        public static readonly string PRECOMP_NAME = "bc_wnaf";
        private static readonly int[] DEFAULT_WINDOW_SIZE_CUTOFFS = new int[] { 13, 0x29, 0x79, 0x151, 0x381, 0x901 };
        private static readonly byte[] EMPTY_BYTES = new byte[0];
        private static readonly int[] EMPTY_INTS = new int[0];
        private static readonly ECPoint[] EMPTY_POINTS = new ECPoint[0];

        protected WNafUtilities()
        {
        }

        public static int[] GenerateCompactNaf(BigInteger k)
        {
            if ((k.BitLength >> 0x10) != 0)
            {
                throw new ArgumentException("must have bitlength < 2^16", "k");
            }
            if (k.SignValue == 0)
            {
                return EMPTY_INTS;
            }
            BigInteger integer = k.ShiftLeft(1).Add(k);
            int bitLength = integer.BitLength;
            int[] a = new int[bitLength >> 1];
            BigInteger integer2 = integer.Xor(k);
            int num2 = bitLength - 1;
            int length = 0;
            int num4 = 0;
            for (int i = 1; i < num2; i++)
            {
                if (!integer2.TestBit(i))
                {
                    num4++;
                }
                else
                {
                    int num6 = !k.TestBit(i) ? 1 : -1;
                    a[length++] = (num6 << 0x10) | num4;
                    num4 = 1;
                    i++;
                }
            }
            a[length++] = 0x10000 | num4;
            if (a.Length > length)
            {
                a = Trim(a, length);
            }
            return a;
        }

        public static int[] GenerateCompactWindowNaf(int width, BigInteger k)
        {
            if (width == 2)
            {
                return GenerateCompactNaf(k);
            }
            if ((width < 2) || (width > 0x10))
            {
                throw new ArgumentException("must be in the range [2, 16]", "width");
            }
            if ((k.BitLength >> 0x10) != 0)
            {
                throw new ArgumentException("must have bitlength < 2^16", "k");
            }
            if (k.SignValue == 0)
            {
                return EMPTY_INTS;
            }
            int[] a = new int[(k.BitLength / width) + 1];
            int num = ((int) 1) << width;
            int num2 = num - 1;
            int num3 = num >> 1;
            bool flag = false;
            int length = 0;
            int n = 0;
            while (n <= k.BitLength)
            {
                if (k.TestBit(n) == flag)
                {
                    n++;
                }
                else
                {
                    k = k.ShiftRight(n);
                    int num6 = k.IntValue & num2;
                    if (flag)
                    {
                        num6++;
                    }
                    if ((num6 & num3) != 0)
                    {
                        num6 -= num;
                    }
                    int num7 = (length <= 0) ? n : (n - 1);
                    a[length++] = (num6 << 0x10) | num7;
                    n = width;
                }
            }
            if (a.Length > length)
            {
                a = Trim(a, length);
            }
            return a;
        }

        public static byte[] GenerateJsf(BigInteger g, BigInteger h)
        {
            int num = Math.Max(g.BitLength, h.BitLength) + 1;
            byte[] a = new byte[num];
            BigInteger integer = g;
            BigInteger integer2 = h;
            int length = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            while ((((num3 | num4) != 0) || (integer.BitLength > num5)) || (integer2.BitLength > num5))
            {
                int num6 = ((integer.IntValue >> num5) + num3) & 7;
                int num7 = ((integer2.IntValue >> num5) + num4) & 7;
                int num8 = num6 & 1;
                if (num8 != 0)
                {
                    num8 -= num6 & 2;
                    if (((num6 + num8) == 4) && ((num7 & 3) == 2))
                    {
                        num8 = -num8;
                    }
                }
                int num9 = num7 & 1;
                if (num9 != 0)
                {
                    num9 -= num7 & 2;
                    if (((num7 + num9) == 4) && ((num6 & 3) == 2))
                    {
                        num9 = -num9;
                    }
                }
                if ((num3 << 1) == (1 + num8))
                {
                    num3 ^= 1;
                }
                if ((num4 << 1) == (1 + num9))
                {
                    num4 ^= 1;
                }
                if (++num5 == 30)
                {
                    num5 = 0;
                    integer = integer.ShiftRight(30);
                    integer2 = integer2.ShiftRight(30);
                }
                a[length++] = (byte) ((num8 << 4) | (num9 & 15));
            }
            if (a.Length > length)
            {
                a = Trim(a, length);
            }
            return a;
        }

        public static byte[] GenerateNaf(BigInteger k)
        {
            if (k.SignValue == 0)
            {
                return EMPTY_BYTES;
            }
            BigInteger integer = k.ShiftLeft(1).Add(k);
            int num = integer.BitLength - 1;
            byte[] buffer = new byte[num];
            BigInteger integer2 = integer.Xor(k);
            for (int i = 1; i < num; i++)
            {
                if (integer2.TestBit(i))
                {
                    buffer[i - 1] = !k.TestBit(i) ? ((byte) 1) : ((byte) (-1));
                    i++;
                }
            }
            buffer[num - 1] = 1;
            return buffer;
        }

        public static byte[] GenerateWindowNaf(int width, BigInteger k)
        {
            if (width == 2)
            {
                return GenerateNaf(k);
            }
            if ((width < 2) || (width > 8))
            {
                throw new ArgumentException("must be in the range [2, 8]", "width");
            }
            if (k.SignValue == 0)
            {
                return EMPTY_BYTES;
            }
            byte[] a = new byte[k.BitLength + 1];
            int num = ((int) 1) << width;
            int num2 = num - 1;
            int num3 = num >> 1;
            bool flag = false;
            int length = 0;
            int n = 0;
            while (n <= k.BitLength)
            {
                if (k.TestBit(n) == flag)
                {
                    n++;
                }
                else
                {
                    k = k.ShiftRight(n);
                    int num6 = k.IntValue & num2;
                    if (flag)
                    {
                        num6++;
                    }
                    if ((num6 & num3) != 0)
                    {
                        num6 -= num;
                    }
                    length += (length <= 0) ? n : (n - 1);
                    a[length++] = (byte) num6;
                    n = width;
                }
            }
            if (a.Length > length)
            {
                a = Trim(a, length);
            }
            return a;
        }

        public static int GetNafWeight(BigInteger k)
        {
            if (k.SignValue == 0)
            {
                return 0;
            }
            return k.ShiftLeft(1).Add(k).Xor(k).BitCount;
        }

        public static int GetWindowSize(int bits) => 
            GetWindowSize(bits, DEFAULT_WINDOW_SIZE_CUTOFFS);

        public static int GetWindowSize(int bits, int[] windowSizeCutoffs)
        {
            int index = 0;
            while (index < windowSizeCutoffs.Length)
            {
                if (bits < windowSizeCutoffs[index])
                {
                    break;
                }
                index++;
            }
            return (index + 2);
        }

        public static WNafPreCompInfo GetWNafPreCompInfo(ECPoint p) => 
            GetWNafPreCompInfo(p.Curve.GetPreCompInfo(p, PRECOMP_NAME));

        public static WNafPreCompInfo GetWNafPreCompInfo(PreCompInfo preCompInfo)
        {
            if ((preCompInfo != null) && (preCompInfo is WNafPreCompInfo))
            {
                return (WNafPreCompInfo) preCompInfo;
            }
            return new WNafPreCompInfo();
        }

        public static ECPoint MapPointWithPrecomp(ECPoint p, int width, bool includeNegated, ECPointMap pointMap)
        {
            ECCurve curve = p.Curve;
            WNafPreCompInfo info = Precompute(p, width, includeNegated);
            ECPoint point = pointMap.Map(p);
            WNafPreCompInfo wNafPreCompInfo = GetWNafPreCompInfo(curve.GetPreCompInfo(point, PRECOMP_NAME));
            ECPoint twice = info.Twice;
            if (twice != null)
            {
                ECPoint point3 = pointMap.Map(twice);
                wNafPreCompInfo.Twice = point3;
            }
            ECPoint[] preComp = info.PreComp;
            ECPoint[] pointArray2 = new ECPoint[preComp.Length];
            for (int i = 0; i < preComp.Length; i++)
            {
                pointArray2[i] = pointMap.Map(preComp[i]);
            }
            wNafPreCompInfo.PreComp = pointArray2;
            if (includeNegated)
            {
                ECPoint[] pointArray3 = new ECPoint[pointArray2.Length];
                for (int j = 0; j < pointArray3.Length; j++)
                {
                    pointArray3[j] = pointArray2[j].Negate();
                }
                wNafPreCompInfo.PreCompNeg = pointArray3;
            }
            curve.SetPreCompInfo(point, PRECOMP_NAME, wNafPreCompInfo);
            return point;
        }

        public static WNafPreCompInfo Precompute(ECPoint p, int width, bool includeNegated)
        {
            ECCurve c = p.Curve;
            WNafPreCompInfo wNafPreCompInfo = GetWNafPreCompInfo(c.GetPreCompInfo(p, PRECOMP_NAME));
            int off = 0;
            int length = ((int) 1) << Math.Max(0, width - 2);
            ECPoint[] preComp = wNafPreCompInfo.PreComp;
            if (preComp == null)
            {
                preComp = EMPTY_POINTS;
            }
            else
            {
                off = preComp.Length;
            }
            if (off < length)
            {
                preComp = ResizeTable(preComp, length);
                if (length == 1)
                {
                    preComp[0] = p.Normalize();
                }
                else
                {
                    int num3 = off;
                    if (num3 == 0)
                    {
                        preComp[0] = p;
                        num3 = 1;
                    }
                    ECFieldElement b = null;
                    if (length == 2)
                    {
                        preComp[1] = p.ThreeTimes();
                    }
                    else
                    {
                        ECPoint twice = wNafPreCompInfo.Twice;
                        ECPoint point2 = preComp[num3 - 1];
                        if (twice == null)
                        {
                            twice = preComp[0].Twice();
                            wNafPreCompInfo.Twice = twice;
                            if (ECAlgorithms.IsFpCurve(c) && (c.FieldSize >= 0x40))
                            {
                                switch (c.CoordinateSystem)
                                {
                                    case 2:
                                    case 3:
                                    case 4:
                                    {
                                        b = twice.GetZCoord(0);
                                        twice = c.CreatePoint(twice.XCoord.ToBigInteger(), twice.YCoord.ToBigInteger());
                                        ECFieldElement scale = b.Square();
                                        ECFieldElement element3 = scale.Multiply(b);
                                        point2 = point2.ScaleX(scale).ScaleY(element3);
                                        if (off == 0)
                                        {
                                            preComp[0] = point2;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        while (num3 < length)
                        {
                            preComp[num3++] = point2 = point2.Add(twice);
                        }
                    }
                    c.NormalizeAll(preComp, off, length - off, b);
                }
            }
            wNafPreCompInfo.PreComp = preComp;
            if (includeNegated)
            {
                int num5;
                ECPoint[] preCompNeg = wNafPreCompInfo.PreCompNeg;
                if (preCompNeg == null)
                {
                    num5 = 0;
                    preCompNeg = new ECPoint[length];
                }
                else
                {
                    num5 = preCompNeg.Length;
                    if (num5 < length)
                    {
                        preCompNeg = ResizeTable(preCompNeg, length);
                    }
                }
                while (num5 < length)
                {
                    preCompNeg[num5] = preComp[num5].Negate();
                    num5++;
                }
                wNafPreCompInfo.PreCompNeg = preCompNeg;
            }
            c.SetPreCompInfo(p, PRECOMP_NAME, wNafPreCompInfo);
            return wNafPreCompInfo;
        }

        private static ECPoint[] ResizeTable(ECPoint[] a, int length)
        {
            ECPoint[] destinationArray = new ECPoint[length];
            Array.Copy(a, 0, destinationArray, 0, a.Length);
            return destinationArray;
        }

        private static byte[] Trim(byte[] a, int length)
        {
            byte[] destinationArray = new byte[length];
            Array.Copy(a, 0, destinationArray, 0, destinationArray.Length);
            return destinationArray;
        }

        private static int[] Trim(int[] a, int length)
        {
            int[] destinationArray = new int[length];
            Array.Copy(a, 0, destinationArray, 0, destinationArray.Length);
            return destinationArray;
        }
    }
}

