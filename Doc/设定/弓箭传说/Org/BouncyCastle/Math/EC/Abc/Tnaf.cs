namespace Org.BouncyCastle.Math.EC.Abc
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    internal class Tnaf
    {
        private static readonly BigInteger MinusOne = BigInteger.One.Negate();
        private static readonly BigInteger MinusTwo = BigInteger.Two.Negate();
        private static readonly BigInteger MinusThree = BigInteger.Three.Negate();
        private static readonly BigInteger Four = BigInteger.ValueOf(4L);
        public const sbyte Width = 4;
        public const sbyte Pow2Width = 0x10;
        public static readonly ZTauElement[] Alpha0;
        public static readonly sbyte[][] Alpha0Tnaf;
        public static readonly ZTauElement[] Alpha1;
        public static readonly sbyte[][] Alpha1Tnaf;

        static Tnaf()
        {
            ZTauElement[] elementArray1 = new ZTauElement[9];
            elementArray1[1] = new ZTauElement(BigInteger.One, BigInteger.Zero);
            elementArray1[3] = new ZTauElement(MinusThree, MinusOne);
            elementArray1[5] = new ZTauElement(MinusOne, MinusOne);
            elementArray1[7] = new ZTauElement(BigInteger.One, MinusOne);
            Alpha0 = elementArray1;
            sbyte[][] numArrayArray1 = new sbyte[8][];
            numArrayArray1[1] = new sbyte[] { 1 };
            sbyte[] numArray2 = new sbyte[3];
            numArray2[0] = -1;
            numArray2[2] = 1;
            numArrayArray1[3] = numArray2;
            sbyte[] numArray3 = new sbyte[3];
            numArray3[0] = 1;
            numArray3[2] = 1;
            numArrayArray1[5] = numArray3;
            sbyte[] numArray4 = new sbyte[4];
            numArray4[0] = -1;
            numArray4[3] = 1;
            numArrayArray1[7] = numArray4;
            Alpha0Tnaf = numArrayArray1;
            ZTauElement[] elementArray2 = new ZTauElement[9];
            elementArray2[1] = new ZTauElement(BigInteger.One, BigInteger.Zero);
            elementArray2[3] = new ZTauElement(MinusThree, BigInteger.One);
            elementArray2[5] = new ZTauElement(MinusOne, BigInteger.One);
            elementArray2[7] = new ZTauElement(BigInteger.One, BigInteger.One);
            Alpha1 = elementArray2;
            sbyte[][] numArrayArray2 = new sbyte[8][];
            numArrayArray2[1] = new sbyte[] { 1 };
            sbyte[] numArray6 = new sbyte[3];
            numArray6[0] = -1;
            numArray6[2] = 1;
            numArrayArray2[3] = numArray6;
            sbyte[] numArray7 = new sbyte[3];
            numArray7[0] = 1;
            numArray7[2] = 1;
            numArrayArray2[5] = numArray7;
            sbyte[] numArray8 = new sbyte[4];
            numArray8[0] = -1;
            numArray8[3] = -1;
            numArrayArray2[7] = numArray8;
            Alpha1Tnaf = numArrayArray2;
        }

        public static SimpleBigDecimal ApproximateDivisionByN(BigInteger k, BigInteger s, BigInteger vm, sbyte a, int m, int c)
        {
            int num = ((m + 5) / 2) + c;
            BigInteger val = k.ShiftRight(((m - num) - 2) + a);
            BigInteger integer2 = s.Multiply(val);
            BigInteger integer3 = integer2.ShiftRight(m);
            BigInteger integer4 = vm.Multiply(integer3);
            BigInteger integer5 = integer2.Add(integer4);
            BigInteger bigInt = integer5.ShiftRight(num - c);
            if (integer5.TestBit((num - c) - 1))
            {
                bigInt = bigInt.Add(BigInteger.One);
            }
            return new SimpleBigDecimal(bigInt, c);
        }

        public static BigInteger[] GetLucas(sbyte mu, int k, bool doV)
        {
            BigInteger two;
            BigInteger one;
            if ((mu != 1) && (mu != -1))
            {
                throw new ArgumentException("mu must be 1 or -1");
            }
            if (doV)
            {
                two = BigInteger.Two;
                one = BigInteger.ValueOf((long) mu);
            }
            else
            {
                two = BigInteger.Zero;
                one = BigInteger.One;
            }
            for (int i = 1; i < k; i++)
            {
                BigInteger integer4 = null;
                if (mu == 1)
                {
                    integer4 = one;
                }
                else
                {
                    integer4 = one.Negate();
                }
                BigInteger integer3 = integer4.Subtract(two.ShiftLeft(1));
                two = one;
                one = integer3;
            }
            return new BigInteger[] { two, one };
        }

        public static sbyte GetMu(AbstractF2mCurve curve)
        {
            BigInteger integer = curve.A.ToBigInteger();
            if (integer.SignValue == 0)
            {
                return -1;
            }
            if (!integer.Equals(BigInteger.One))
            {
                throw new ArgumentException("No Koblitz curve (ABC), TNAF multiplication not possible");
            }
            return 1;
        }

        public static sbyte GetMu(ECFieldElement curveA) => 
            (!curveA.IsZero ? ((sbyte) 1) : ((sbyte) (-1)));

        public static sbyte GetMu(int curveA) => 
            ((curveA != 0) ? ((sbyte) 1) : ((sbyte) (-1)));

        public static AbstractF2mPoint[] GetPreComp(AbstractF2mPoint p, sbyte a)
        {
            sbyte[][] numArray = (a != 0) ? Alpha1Tnaf : Alpha0Tnaf;
            AbstractF2mPoint[] points = new AbstractF2mPoint[(numArray.Length + 1) >> 1];
            points[0] = p;
            uint length = (uint) numArray.Length;
            for (uint i = 3; i < length; i += 2)
            {
                points[i >> 1] = MultiplyFromTnaf(p, numArray[i]);
            }
            p.Curve.NormalizeAll(points);
            return points;
        }

        protected static int GetShiftsForCofactor(BigInteger h)
        {
            if ((h != null) && (h.BitLength < 4))
            {
                switch (h.IntValue)
                {
                    case 2:
                        return 1;

                    case 4:
                        return 2;
                }
            }
            throw new ArgumentException("h (Cofactor) must be 2 or 4");
        }

        public static BigInteger[] GetSi(AbstractF2mCurve curve)
        {
            if (!curve.IsKoblitz)
            {
                throw new ArgumentException("si is defined for Koblitz curves only");
            }
            int fieldSize = curve.FieldSize;
            int intValue = curve.A.ToBigInteger().IntValue;
            sbyte mu = GetMu(intValue);
            int shiftsForCofactor = GetShiftsForCofactor(curve.Cofactor);
            int k = (fieldSize + 3) - intValue;
            BigInteger[] integerArray = GetLucas(mu, k, false);
            if (mu == 1)
            {
                integerArray[0] = integerArray[0].Negate();
                integerArray[1] = integerArray[1].Negate();
            }
            BigInteger integer = BigInteger.One.Add(integerArray[1]).ShiftRight(shiftsForCofactor);
            BigInteger integer2 = BigInteger.One.Add(integerArray[0]).ShiftRight(shiftsForCofactor).Negate();
            return new BigInteger[] { integer, integer2 };
        }

        public static BigInteger[] GetSi(int fieldSize, int curveA, BigInteger cofactor)
        {
            sbyte mu = GetMu(curveA);
            int shiftsForCofactor = GetShiftsForCofactor(cofactor);
            int k = (fieldSize + 3) - curveA;
            BigInteger[] integerArray = GetLucas(mu, k, false);
            if (mu == 1)
            {
                integerArray[0] = integerArray[0].Negate();
                integerArray[1] = integerArray[1].Negate();
            }
            BigInteger integer = BigInteger.One.Add(integerArray[1]).ShiftRight(shiftsForCofactor);
            BigInteger integer2 = BigInteger.One.Add(integerArray[0]).ShiftRight(shiftsForCofactor).Negate();
            return new BigInteger[] { integer, integer2 };
        }

        public static BigInteger GetTw(sbyte mu, int w)
        {
            if (w == 4)
            {
                if (mu == 1)
                {
                    return BigInteger.ValueOf(6L);
                }
                return BigInteger.ValueOf(10L);
            }
            BigInteger[] integerArray = GetLucas(mu, w, false);
            BigInteger m = BigInteger.Zero.SetBit(w);
            BigInteger val = integerArray[1].ModInverse(m);
            return BigInteger.Two.Multiply(integerArray[0]).Multiply(val).Mod(m);
        }

        public static AbstractF2mPoint MultiplyFromTnaf(AbstractF2mPoint p, sbyte[] u)
        {
            AbstractF2mPoint infinity = (AbstractF2mPoint) p.Curve.Infinity;
            AbstractF2mPoint point2 = (AbstractF2mPoint) p.Negate();
            int pow = 0;
            for (int i = u.Length - 1; i >= 0; i--)
            {
                pow++;
                sbyte num3 = u[i];
                if (num3 != 0)
                {
                    infinity = infinity.TauPow(pow);
                    pow = 0;
                    ECPoint b = (num3 <= 0) ? point2 : p;
                    infinity = (AbstractF2mPoint) infinity.Add(b);
                }
            }
            if (pow > 0)
            {
                infinity = infinity.TauPow(pow);
            }
            return infinity;
        }

        public static AbstractF2mPoint MultiplyRTnaf(AbstractF2mPoint p, BigInteger k)
        {
            AbstractF2mCurve curve = (AbstractF2mCurve) p.Curve;
            int fieldSize = curve.FieldSize;
            int intValue = curve.A.ToBigInteger().IntValue;
            sbyte mu = GetMu(intValue);
            BigInteger[] si = curve.GetSi();
            ZTauElement lambda = PartModReduction(k, fieldSize, (sbyte) intValue, si, mu, 10);
            return MultiplyTnaf(p, lambda);
        }

        public static AbstractF2mPoint MultiplyTnaf(AbstractF2mPoint p, ZTauElement lambda)
        {
            AbstractF2mCurve curve = (AbstractF2mCurve) p.Curve;
            sbyte[] u = TauAdicNaf(GetMu(curve.A), lambda);
            return MultiplyFromTnaf(p, u);
        }

        public static BigInteger Norm(sbyte mu, ZTauElement lambda)
        {
            BigInteger integer2 = lambda.u.Multiply(lambda.u);
            BigInteger integer3 = lambda.u.Multiply(lambda.v);
            BigInteger integer4 = lambda.v.Multiply(lambda.v).ShiftLeft(1);
            if (mu == 1)
            {
                return integer2.Add(integer3).Add(integer4);
            }
            if (mu != -1)
            {
                throw new ArgumentException("mu must be 1 or -1");
            }
            return integer2.Subtract(integer3).Add(integer4);
        }

        public static SimpleBigDecimal Norm(sbyte mu, SimpleBigDecimal u, SimpleBigDecimal v)
        {
            SimpleBigDecimal num2 = u.Multiply(u);
            SimpleBigDecimal b = u.Multiply(v);
            SimpleBigDecimal num4 = v.Multiply(v).ShiftLeft(1);
            if (mu == 1)
            {
                return num2.Add(b).Add(num4);
            }
            if (mu != -1)
            {
                throw new ArgumentException("mu must be 1 or -1");
            }
            return num2.Subtract(b).Add(num4);
        }

        public static ZTauElement PartModReduction(BigInteger k, int m, sbyte a, BigInteger[] s, sbyte mu, sbyte c)
        {
            BigInteger integer;
            if (mu == 1)
            {
                integer = s[0].Add(s[1]);
            }
            else
            {
                integer = s[0].Subtract(s[1]);
            }
            BigInteger vm = GetLucas(mu, m, true)[1];
            SimpleBigDecimal num = ApproximateDivisionByN(k, s[0], vm, a, m, (int) c);
            SimpleBigDecimal num2 = ApproximateDivisionByN(k, s[1], vm, a, m, (int) c);
            ZTauElement element = Round(num, num2, mu);
            BigInteger u = k.Subtract(integer.Multiply(element.u)).Subtract(BigInteger.ValueOf(2L).Multiply(s[1]).Multiply(element.v));
            return new ZTauElement(u, s[1].Multiply(element.u).Subtract(s[0].Multiply(element.v)));
        }

        public static ZTauElement Round(SimpleBigDecimal lambda0, SimpleBigDecimal lambda1, sbyte mu)
        {
            SimpleBigDecimal num7;
            SimpleBigDecimal num8;
            int scale = lambda0.Scale;
            if (lambda1.Scale != scale)
            {
                throw new ArgumentException("lambda0 and lambda1 do not have same scale");
            }
            if ((mu != 1) && (mu != -1))
            {
                throw new ArgumentException("mu must be 1 or -1");
            }
            BigInteger b = lambda0.Round();
            BigInteger integer2 = lambda1.Round();
            SimpleBigDecimal num2 = lambda0.Subtract(b);
            SimpleBigDecimal num3 = lambda1.Subtract(integer2);
            SimpleBigDecimal num4 = num2.Add(num2);
            if (mu == 1)
            {
                num4 = num4.Add(num3);
            }
            else
            {
                num4 = num4.Subtract(num3);
            }
            SimpleBigDecimal num5 = num3.Add(num3).Add(num3);
            SimpleBigDecimal num6 = num5.Add(num3);
            if (mu == 1)
            {
                num7 = num2.Subtract(num5);
                num8 = num2.Add(num6);
            }
            else
            {
                num7 = num2.Add(num5);
                num8 = num2.Subtract(num6);
            }
            sbyte num9 = 0;
            sbyte num10 = 0;
            if (num4.CompareTo(BigInteger.One) >= 0)
            {
                if (num7.CompareTo(MinusOne) < 0)
                {
                    num10 = mu;
                }
                else
                {
                    num9 = 1;
                }
            }
            else if (num8.CompareTo(BigInteger.Two) >= 0)
            {
                num10 = mu;
            }
            if (num4.CompareTo(MinusOne) < 0)
            {
                if (num7.CompareTo(BigInteger.One) >= 0)
                {
                    num10 = (sbyte) -((int) mu);
                }
                else
                {
                    num9 = -1;
                }
            }
            else if (num8.CompareTo(MinusTwo) < 0)
            {
                num10 = (sbyte) -((int) mu);
            }
            BigInteger u = b.Add(BigInteger.ValueOf((long) num9));
            return new ZTauElement(u, integer2.Add(BigInteger.ValueOf((long) num10)));
        }

        public static AbstractF2mPoint Tau(AbstractF2mPoint p) => 
            p.Tau();

        public static sbyte[] TauAdicNaf(sbyte mu, ZTauElement lambda)
        {
            if ((mu != 1) && (mu != -1))
            {
                throw new ArgumentException("mu must be 1 or -1");
            }
            int bitLength = Norm(mu, lambda).BitLength;
            int num2 = (bitLength <= 30) ? 0x22 : (bitLength + 4);
            sbyte[] sourceArray = new sbyte[num2];
            int index = 0;
            int length = 0;
            BigInteger u = lambda.u;
            BigInteger v = lambda.v;
            while (!u.Equals(BigInteger.Zero) || !v.Equals(BigInteger.Zero))
            {
                if (u.TestBit(0))
                {
                    sourceArray[index] = (sbyte) BigInteger.Two.Subtract(u.Subtract(v.ShiftLeft(1)).Mod(Four)).IntValue;
                    if (sourceArray[index] == 1)
                    {
                        u = u.ClearBit(0);
                    }
                    else
                    {
                        u = u.Add(BigInteger.One);
                    }
                    length = index;
                }
                else
                {
                    sourceArray[index] = 0;
                }
                BigInteger integer4 = u;
                BigInteger integer5 = u.ShiftRight(1);
                if (mu == 1)
                {
                    u = v.Add(integer5);
                }
                else
                {
                    u = v.Subtract(integer5);
                }
                v = integer4.ShiftRight(1).Negate();
                index++;
            }
            length++;
            sbyte[] destinationArray = new sbyte[length];
            Array.Copy(sourceArray, 0, destinationArray, 0, length);
            return destinationArray;
        }

        public static sbyte[] TauAdicWNaf(sbyte mu, ZTauElement lambda, sbyte width, BigInteger pow2w, BigInteger tw, ZTauElement[] alpha)
        {
            if ((mu != 1) && (mu != -1))
            {
                throw new ArgumentException("mu must be 1 or -1");
            }
            int bitLength = Norm(mu, lambda).BitLength;
            int num2 = (bitLength <= 30) ? (0x22 + width) : ((bitLength + 4) + width);
            sbyte[] numArray = new sbyte[num2];
            BigInteger integer2 = pow2w.ShiftRight(1);
            BigInteger u = lambda.u;
            BigInteger v = lambda.v;
            for (int i = 0; !u.Equals(BigInteger.Zero) || !v.Equals(BigInteger.Zero); i++)
            {
                if (u.TestBit(0))
                {
                    sbyte intValue;
                    BigInteger integer5 = u.Add(v.Multiply(tw)).Mod(pow2w);
                    if (integer5.CompareTo(integer2) >= 0)
                    {
                        intValue = (sbyte) integer5.Subtract(pow2w).IntValue;
                    }
                    else
                    {
                        intValue = (sbyte) integer5.IntValue;
                    }
                    numArray[i] = intValue;
                    bool flag = true;
                    if (intValue < 0)
                    {
                        flag = false;
                        intValue = (sbyte) -((int) intValue);
                    }
                    if (flag)
                    {
                        u = u.Subtract(alpha[(int) intValue].u);
                        v = v.Subtract(alpha[(int) intValue].v);
                    }
                    else
                    {
                        u = u.Add(alpha[(int) intValue].u);
                        v = v.Add(alpha[(int) intValue].v);
                    }
                }
                else
                {
                    numArray[i] = 0;
                }
                BigInteger integer6 = u;
                if (mu == 1)
                {
                    u = v.Add(u.ShiftRight(1));
                }
                else
                {
                    u = v.Subtract(u.ShiftRight(1));
                }
                v = integer6.ShiftRight(1).Negate();
            }
            return numArray;
        }
    }
}

