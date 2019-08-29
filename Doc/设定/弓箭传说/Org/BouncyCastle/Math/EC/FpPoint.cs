namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using System;

    public class FpPoint : AbstractFpPoint
    {
        public FpPoint(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
        {
        }

        public FpPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
        {
            if ((x == null) != (y == null))
            {
                throw new ArgumentException("Exactly one of the field elements is null");
            }
        }

        internal FpPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
        {
        }

        public override ECPoint Add(ECPoint b)
        {
            ECFieldElement element28;
            ECFieldElement element29;
            ECFieldElement element30;
            ECFieldElement element31;
            ECFieldElement[] elementArray;
            if (base.IsInfinity)
            {
                return b;
            }
            if (b.IsInfinity)
            {
                return this;
            }
            if (this == b)
            {
                return this.Twice();
            }
            ECCurve curve = this.Curve;
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement rawXCoord = base.RawXCoord;
            ECFieldElement rawYCoord = base.RawYCoord;
            ECFieldElement element3 = b.RawXCoord;
            ECFieldElement element4 = b.RawYCoord;
            switch (coordinateSystem)
            {
                case 0:
                {
                    ECFieldElement element5 = element3.Subtract(rawXCoord);
                    ECFieldElement element6 = element4.Subtract(rawYCoord);
                    if (!element5.IsZero)
                    {
                        ECFieldElement element7 = element6.Divide(element5);
                        ECFieldElement x = element7.Square().Subtract(rawXCoord).Subtract(element3);
                        return new FpPoint(this.Curve, x, element7.Multiply(rawXCoord.Subtract(x)).Subtract(rawYCoord), base.IsCompressed);
                    }
                    if (!element6.IsZero)
                    {
                        return this.Curve.Infinity;
                    }
                    return this.Twice();
                }
                case 1:
                {
                    ECFieldElement element10 = base.RawZCoords[0];
                    ECFieldElement element11 = b.RawZCoords[0];
                    bool isOne = element10.IsOne;
                    bool flag2 = element11.IsOne;
                    ECFieldElement element12 = !isOne ? element4.Multiply(element10) : element4;
                    ECFieldElement element13 = !flag2 ? rawYCoord.Multiply(element11) : rawYCoord;
                    ECFieldElement element14 = element12.Subtract(element13);
                    ECFieldElement element15 = !isOne ? element3.Multiply(element10) : element3;
                    ECFieldElement element16 = !flag2 ? rawXCoord.Multiply(element11) : rawXCoord;
                    ECFieldElement element17 = element15.Subtract(element16);
                    if (!element17.IsZero)
                    {
                        ECFieldElement element18 = !isOne ? (!flag2 ? element10.Multiply(element11) : element10) : element11;
                        ECFieldElement element19 = element17.Square();
                        ECFieldElement element20 = element19.Multiply(element17);
                        ECFieldElement x = element19.Multiply(element16);
                        ECFieldElement element22 = element14.Square().Multiply(element18).Subtract(element20).Subtract(this.Two(x));
                        ECFieldElement element23 = element17.Multiply(element22);
                        ECFieldElement y = x.Subtract(element22).MultiplyMinusProduct(element14, element13, element20);
                        ECFieldElement element25 = element20.Multiply(element18);
                        return new FpPoint(curve, element23, y, new ECFieldElement[] { element25 }, base.IsCompressed);
                    }
                    if (!element14.IsZero)
                    {
                        return curve.Infinity;
                    }
                    return this.Twice();
                }
                case 2:
                case 4:
                {
                    ECFieldElement element26 = base.RawZCoords[0];
                    ECFieldElement other = b.RawZCoords[0];
                    bool isOne = element26.IsOne;
                    element31 = null;
                    if (isOne || !element26.Equals(other))
                    {
                        ECFieldElement element38;
                        ECFieldElement element39;
                        ECFieldElement element40;
                        ECFieldElement element42;
                        ECFieldElement element43;
                        ECFieldElement element44;
                        if (isOne)
                        {
                            element38 = element26;
                            element39 = element3;
                            element40 = element4;
                        }
                        else
                        {
                            element38 = element26.Square();
                            element39 = element38.Multiply(element3);
                            element40 = element38.Multiply(element26).Multiply(element4);
                        }
                        bool flag4 = other.IsOne;
                        if (flag4)
                        {
                            element42 = other;
                            element43 = rawXCoord;
                            element44 = rawYCoord;
                        }
                        else
                        {
                            element42 = other.Square();
                            element43 = element42.Multiply(rawXCoord);
                            element44 = element42.Multiply(other).Multiply(rawYCoord);
                        }
                        ECFieldElement element46 = element43.Subtract(element39);
                        ECFieldElement element47 = element44.Subtract(element40);
                        if (element46.IsZero)
                        {
                            if (element47.IsZero)
                            {
                                return this.Twice();
                            }
                            return curve.Infinity;
                        }
                        ECFieldElement element48 = element46.Square();
                        ECFieldElement element49 = element48.Multiply(element46);
                        ECFieldElement x = element48.Multiply(element43);
                        element28 = element47.Square().Add(element49).Subtract(this.Two(x));
                        element29 = x.Subtract(element28).MultiplyMinusProduct(element47, element49, element44);
                        element30 = element46;
                        if (!isOne)
                        {
                            element30 = element30.Multiply(element26);
                        }
                        if (!flag4)
                        {
                            element30 = element30.Multiply(other);
                        }
                        if (element30 == element46)
                        {
                            element31 = element48;
                        }
                        break;
                    }
                    ECFieldElement element32 = rawXCoord.Subtract(element3);
                    ECFieldElement element33 = rawYCoord.Subtract(element4);
                    if (!element32.IsZero)
                    {
                        ECFieldElement element34 = element32.Square();
                        ECFieldElement element35 = rawXCoord.Multiply(element34);
                        ECFieldElement element36 = element3.Multiply(element34);
                        ECFieldElement element37 = element35.Subtract(element36).Multiply(rawYCoord);
                        element28 = element33.Square().Subtract(element35).Subtract(element36);
                        element29 = element35.Subtract(element28).Multiply(element33).Subtract(element37);
                        element30 = element32;
                        if (isOne)
                        {
                            element31 = element34;
                        }
                        else
                        {
                            element30 = element30.Multiply(element26);
                        }
                        break;
                    }
                    if (!element33.IsZero)
                    {
                        return curve.Infinity;
                    }
                    return this.Twice();
                }
                default:
                    throw new InvalidOperationException("unsupported coordinate system");
            }
            if (coordinateSystem == 4)
            {
                ECFieldElement element51 = this.CalculateJacobianModifiedW(element30, element31);
                elementArray = new ECFieldElement[] { element30, element51 };
            }
            else
            {
                elementArray = new ECFieldElement[] { element30 };
            }
            return new FpPoint(curve, element28, element29, elementArray, base.IsCompressed);
        }

        protected virtual ECFieldElement CalculateJacobianModifiedW(ECFieldElement Z, ECFieldElement ZSquared)
        {
            ECFieldElement a = this.Curve.A;
            if (a.IsZero || Z.IsOne)
            {
                return a;
            }
            if (ZSquared == null)
            {
                ZSquared = Z.Square();
            }
            ECFieldElement element2 = ZSquared.Square();
            ECFieldElement b = a.Negate();
            if (b.BitLength < a.BitLength)
            {
                return element2.Multiply(b).Negate();
            }
            return element2.Multiply(a);
        }

        protected override ECPoint Detach() => 
            new FpPoint(null, this.AffineXCoord, this.AffineYCoord);

        protected virtual ECFieldElement DoubleProductFromSquares(ECFieldElement a, ECFieldElement b, ECFieldElement aSquared, ECFieldElement bSquared) => 
            a.Add(b).Square().Subtract(aSquared).Subtract(bSquared);

        protected virtual ECFieldElement Eight(ECFieldElement x) => 
            this.Four(this.Two(x));

        protected virtual ECFieldElement Four(ECFieldElement x) => 
            this.Two(this.Two(x));

        protected virtual ECFieldElement GetJacobianModifiedW()
        {
            ECFieldElement[] rawZCoords = base.RawZCoords;
            ECFieldElement element = rawZCoords[1];
            if (element == null)
            {
                rawZCoords[1] = element = this.CalculateJacobianModifiedW(rawZCoords[0], null);
            }
            return element;
        }

        public override ECFieldElement GetZCoord(int index)
        {
            if ((index == 1) && (this.CurveCoordinateSystem == 4))
            {
                return this.GetJacobianModifiedW();
            }
            return base.GetZCoord(index);
        }

        public override ECPoint Negate()
        {
            if (base.IsInfinity)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            if (curve.CoordinateSystem != 0)
            {
                return new FpPoint(curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
            }
            return new FpPoint(curve, base.RawXCoord, base.RawYCoord.Negate(), base.IsCompressed);
        }

        protected virtual ECFieldElement Three(ECFieldElement x) => 
            this.Two(x).Add(x);

        public override ECPoint ThreeTimes()
        {
            if (base.IsInfinity)
            {
                return this;
            }
            ECFieldElement rawYCoord = base.RawYCoord;
            if (rawYCoord.IsZero)
            {
                return this;
            }
            switch (this.Curve.CoordinateSystem)
            {
                case 0:
                {
                    ECFieldElement rawXCoord = base.RawXCoord;
                    ECFieldElement b = this.Two(rawYCoord);
                    ECFieldElement element4 = b.Square();
                    ECFieldElement element5 = this.Three(rawXCoord.Square()).Add(this.Curve.A);
                    ECFieldElement element6 = element5.Square();
                    ECFieldElement element7 = this.Three(rawXCoord).Multiply(element4).Subtract(element6);
                    if (element7.IsZero)
                    {
                        return this.Curve.Infinity;
                    }
                    ECFieldElement element9 = element7.Multiply(b).Invert();
                    ECFieldElement element10 = element7.Multiply(element9).Multiply(element5);
                    ECFieldElement element11 = element4.Square().Multiply(element9).Subtract(element10);
                    ECFieldElement x = element11.Subtract(element10).Multiply(element10.Add(element11)).Add(rawXCoord);
                    return new FpPoint(this.Curve, x, rawXCoord.Subtract(x).Multiply(element11).Subtract(rawYCoord), base.IsCompressed);
                }
                case 4:
                    return this.TwiceJacobianModified(false).Add(this);
            }
            return this.Twice().Add(this);
        }

        public override ECPoint TimesPow2(int e)
        {
            if (e < 0)
            {
                throw new ArgumentException("cannot be negative", "e");
            }
            if ((e == 0) || base.IsInfinity)
            {
                return this;
            }
            if (e == 1)
            {
                return this.Twice();
            }
            ECCurve curve = this.Curve;
            ECFieldElement rawYCoord = base.RawYCoord;
            if (rawYCoord.IsZero)
            {
                return curve.Infinity;
            }
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement a = curve.A;
            ECFieldElement rawXCoord = base.RawXCoord;
            ECFieldElement b = (base.RawZCoords.Length >= 1) ? base.RawZCoords[0] : curve.FromBigInteger(BigInteger.One);
            if (!b.IsOne)
            {
                switch (coordinateSystem)
                {
                    case 1:
                    {
                        ECFieldElement element5 = b.Square();
                        rawXCoord = rawXCoord.Multiply(b);
                        rawYCoord = rawYCoord.Multiply(element5);
                        a = this.CalculateJacobianModifiedW(b, element5);
                        break;
                    }
                    case 2:
                        a = this.CalculateJacobianModifiedW(b, null);
                        break;

                    case 4:
                        a = this.GetJacobianModifiedW();
                        break;
                }
            }
            for (int i = 0; i < e; i++)
            {
                if (rawYCoord.IsZero)
                {
                    return curve.Infinity;
                }
                ECFieldElement x = rawXCoord.Square();
                ECFieldElement element7 = this.Three(x);
                ECFieldElement element8 = this.Two(rawYCoord);
                ECFieldElement element9 = element8.Multiply(rawYCoord);
                ECFieldElement element10 = this.Two(rawXCoord.Multiply(element9));
                ECFieldElement element11 = element9.Square();
                ECFieldElement element12 = this.Two(element11);
                if (!a.IsZero)
                {
                    element7 = element7.Add(a);
                    a = this.Two(element12.Multiply(a));
                }
                rawXCoord = element7.Square().Subtract(this.Two(element10));
                rawYCoord = element7.Multiply(element10.Subtract(rawXCoord)).Subtract(element12);
                b = !b.IsOne ? element8.Multiply(b) : element8;
            }
            switch (coordinateSystem)
            {
                case 0:
                {
                    ECFieldElement element13 = b.Invert();
                    ECFieldElement element14 = element13.Square();
                    ECFieldElement element15 = element14.Multiply(element13);
                    return new FpPoint(curve, rawXCoord.Multiply(element14), rawYCoord.Multiply(element15), base.IsCompressed);
                }
                case 1:
                    rawXCoord = rawXCoord.Multiply(b);
                    b = b.Multiply(b.Square());
                    return new FpPoint(curve, rawXCoord, rawYCoord, new ECFieldElement[] { b }, base.IsCompressed);

                case 2:
                    return new FpPoint(curve, rawXCoord, rawYCoord, new ECFieldElement[] { b }, base.IsCompressed);

                case 4:
                    return new FpPoint(curve, rawXCoord, rawYCoord, new ECFieldElement[] { b, a }, base.IsCompressed);
            }
            throw new InvalidOperationException("unsupported coordinate system");
        }

        public override ECPoint Twice()
        {
            ECFieldElement element20;
            bool isOne;
            ECFieldElement element22;
            ECFieldElement element25;
            ECFieldElement element26;
            if (base.IsInfinity)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            ECFieldElement rawYCoord = base.RawYCoord;
            if (rawYCoord.IsZero)
            {
                return curve.Infinity;
            }
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement rawXCoord = base.RawXCoord;
            switch (coordinateSystem)
            {
                case 0:
                {
                    ECFieldElement x = rawXCoord.Square();
                    ECFieldElement element4 = this.Three(x).Add(this.Curve.A).Divide(this.Two(rawYCoord));
                    ECFieldElement element5 = element4.Square().Subtract(this.Two(rawXCoord));
                    return new FpPoint(this.Curve, element5, element4.Multiply(rawXCoord.Subtract(element5)).Subtract(rawYCoord), base.IsCompressed);
                }
                case 1:
                {
                    ECFieldElement element7 = base.RawZCoords[0];
                    bool isOne = element7.IsOne;
                    ECFieldElement a = curve.A;
                    if (!a.IsZero && !isOne)
                    {
                        a = a.Multiply(element7.Square());
                    }
                    a = a.Add(this.Three(rawXCoord.Square()));
                    ECFieldElement x = !isOne ? rawYCoord.Multiply(element7) : rawYCoord;
                    ECFieldElement element10 = !isOne ? x.Multiply(rawYCoord) : rawYCoord.Square();
                    ECFieldElement element11 = rawXCoord.Multiply(element10);
                    ECFieldElement element12 = this.Four(element11);
                    ECFieldElement element13 = a.Square().Subtract(this.Two(element12));
                    ECFieldElement element14 = this.Two(x);
                    ECFieldElement element15 = element13.Multiply(element14);
                    ECFieldElement element16 = this.Two(element10);
                    ECFieldElement element17 = element12.Subtract(element13).Multiply(a).Subtract(this.Two(element16.Square()));
                    ECFieldElement element18 = !isOne ? element14.Square() : this.Two(element16);
                    ECFieldElement element19 = this.Two(element18).Multiply(x);
                    return new FpPoint(curve, element15, element17, new ECFieldElement[] { element19 }, base.IsCompressed);
                }
                case 2:
                {
                    element20 = base.RawZCoords[0];
                    isOne = element20.IsOne;
                    ECFieldElement element21 = rawYCoord.Square();
                    element22 = element21.Square();
                    ECFieldElement a = curve.A;
                    ECFieldElement element24 = a.Negate();
                    if (!element24.ToBigInteger().Equals(BigInteger.ValueOf(3L)))
                    {
                        ECFieldElement x = rawXCoord.Square();
                        element25 = this.Three(x);
                        if (isOne)
                        {
                            element25 = element25.Add(a);
                        }
                        else if (!a.IsZero)
                        {
                            ECFieldElement element30 = (!isOne ? element20.Square() : element20).Square();
                            if (element24.BitLength < a.BitLength)
                            {
                                element25 = element25.Subtract(element30.Multiply(element24));
                            }
                            else
                            {
                                element25 = element25.Add(element30.Multiply(a));
                            }
                        }
                        element26 = this.Four(rawXCoord.Multiply(element21));
                        break;
                    }
                    ECFieldElement element27 = !isOne ? element20.Square() : element20;
                    element25 = this.Three(rawXCoord.Add(element27).Multiply(rawXCoord.Subtract(element27)));
                    element26 = this.Four(element21.Multiply(rawXCoord));
                    break;
                }
                case 4:
                    return this.TwiceJacobianModified(true);

                default:
                    throw new InvalidOperationException("unsupported coordinate system");
            }
            ECFieldElement b = element25.Square().Subtract(this.Two(element26));
            ECFieldElement y = element26.Subtract(b).Multiply(element25).Subtract(this.Eight(element22));
            ECFieldElement element33 = this.Two(rawYCoord);
            if (!isOne)
            {
                element33 = element33.Multiply(element20);
            }
            return new FpPoint(curve, b, y, new ECFieldElement[] { element33 }, base.IsCompressed);
        }

        protected virtual FpPoint TwiceJacobianModified(bool calculateW)
        {
            ECFieldElement rawXCoord = base.RawXCoord;
            ECFieldElement rawYCoord = base.RawYCoord;
            ECFieldElement b = base.RawZCoords[0];
            ECFieldElement jacobianModifiedW = this.GetJacobianModifiedW();
            ECFieldElement x = rawXCoord.Square();
            ECFieldElement element6 = this.Three(x).Add(jacobianModifiedW);
            ECFieldElement element7 = this.Two(rawYCoord);
            ECFieldElement element8 = element7.Multiply(rawYCoord);
            ECFieldElement element9 = this.Two(rawXCoord.Multiply(element8));
            ECFieldElement element10 = element6.Square().Subtract(this.Two(element9));
            ECFieldElement element11 = element8.Square();
            ECFieldElement element12 = this.Two(element11);
            ECFieldElement y = element6.Multiply(element9.Subtract(element10)).Subtract(element12);
            ECFieldElement element14 = !calculateW ? null : this.Two(element12.Multiply(jacobianModifiedW));
            ECFieldElement element15 = !b.IsOne ? element7.Multiply(b) : element7;
            return new FpPoint(this.Curve, element10, y, new ECFieldElement[] { element15, element14 }, base.IsCompressed);
        }

        public override ECPoint TwicePlus(ECPoint b)
        {
            if (this == b)
            {
                return this.ThreeTimes();
            }
            if (base.IsInfinity)
            {
                return b;
            }
            if (b.IsInfinity)
            {
                return this.Twice();
            }
            ECFieldElement rawYCoord = base.RawYCoord;
            if (rawYCoord.IsZero)
            {
                return b;
            }
            switch (this.Curve.CoordinateSystem)
            {
                case 0:
                {
                    ECFieldElement rawXCoord = base.RawXCoord;
                    ECFieldElement element3 = b.RawXCoord;
                    ECFieldElement element4 = b.RawYCoord;
                    ECFieldElement element5 = element3.Subtract(rawXCoord);
                    ECFieldElement element6 = element4.Subtract(rawYCoord);
                    if (!element5.IsZero)
                    {
                        ECFieldElement element7 = element5.Square();
                        ECFieldElement element8 = element6.Square();
                        ECFieldElement element9 = element7.Multiply(this.Two(rawXCoord).Add(element3)).Subtract(element8);
                        if (element9.IsZero)
                        {
                            return this.Curve.Infinity;
                        }
                        ECFieldElement element11 = element9.Multiply(element5).Invert();
                        ECFieldElement element12 = element9.Multiply(element11).Multiply(element6);
                        ECFieldElement element13 = this.Two(rawYCoord).Multiply(element7).Multiply(element5).Multiply(element11).Subtract(element12);
                        ECFieldElement x = element13.Subtract(element12).Multiply(element12.Add(element13)).Add(element3);
                        return new FpPoint(this.Curve, x, rawXCoord.Subtract(x).Multiply(element13).Subtract(rawYCoord), base.IsCompressed);
                    }
                    if (element6.IsZero)
                    {
                        return this.ThreeTimes();
                    }
                    return this;
                }
                case 4:
                    return this.TwiceJacobianModified(false).Add(b);
            }
            return this.Twice().Add(b);
        }

        protected virtual ECFieldElement Two(ECFieldElement x) => 
            x.Add(x);
    }
}

