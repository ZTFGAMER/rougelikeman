namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using System;

    public class F2mPoint : AbstractF2mPoint
    {
        [Obsolete("Use ECCurve.Infinity property")]
        public F2mPoint(ECCurve curve) : this(curve, null, null)
        {
        }

        public F2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
        {
        }

        public F2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
        {
            if ((x == null) != (y == null))
            {
                throw new ArgumentException("Exactly one of the field elements is null");
            }
            if (x != null)
            {
                F2mFieldElement.CheckFieldElements(x, y);
                if (curve != null)
                {
                    F2mFieldElement.CheckFieldElements(x, curve.A);
                }
            }
        }

        internal F2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
        {
        }

        public override ECPoint Add(ECPoint b)
        {
            if (base.IsInfinity)
            {
                return b;
            }
            if (b.IsInfinity)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement rawXCoord = base.RawXCoord;
            ECFieldElement element2 = b.RawXCoord;
            switch (coordinateSystem)
            {
                case 0:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement element4 = b.RawYCoord;
                    ECFieldElement element5 = rawXCoord.Add(element2);
                    ECFieldElement element6 = rawYCoord.Add(element4);
                    if (!element5.IsZero)
                    {
                        ECFieldElement element7 = element6.Divide(element5);
                        ECFieldElement x = element7.Square().Add(element7).Add(element5).Add(curve.A);
                        return new F2mPoint(curve, x, element7.Multiply(rawXCoord.Add(x)).Add(x).Add(rawYCoord), base.IsCompressed);
                    }
                    if (element6.IsZero)
                    {
                        return this.Twice();
                    }
                    return curve.Infinity;
                }
                case 1:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement element11 = base.RawZCoords[0];
                    ECFieldElement element12 = b.RawYCoord;
                    ECFieldElement element13 = b.RawZCoords[0];
                    bool isOne = element11.IsOne;
                    ECFieldElement element14 = element12;
                    ECFieldElement element15 = element2;
                    if (!isOne)
                    {
                        element14 = element14.Multiply(element11);
                        element15 = element15.Multiply(element11);
                    }
                    bool flag2 = element13.IsOne;
                    ECFieldElement element16 = rawYCoord;
                    ECFieldElement element17 = rawXCoord;
                    if (!flag2)
                    {
                        element16 = element16.Multiply(element13);
                        element17 = element17.Multiply(element13);
                    }
                    ECFieldElement element18 = element14.Add(element16);
                    ECFieldElement element19 = element15.Add(element17);
                    if (!element19.IsZero)
                    {
                        ECFieldElement x = element19.Square();
                        ECFieldElement element21 = x.Multiply(element19);
                        ECFieldElement element22 = !isOne ? (!flag2 ? element11.Multiply(element13) : element11) : element13;
                        ECFieldElement element23 = element18.Add(element19);
                        ECFieldElement element24 = element23.MultiplyPlusProduct(element18, x, curve.A).Multiply(element22).Add(element21);
                        ECFieldElement element25 = element19.Multiply(element24);
                        ECFieldElement element26 = !flag2 ? x.Multiply(element13) : x;
                        ECFieldElement y = element18.MultiplyPlusProduct(rawXCoord, element19, rawYCoord).MultiplyPlusProduct(element26, element23, element24);
                        ECFieldElement element28 = element21.Multiply(element22);
                        return new F2mPoint(curve, element25, y, new ECFieldElement[] { element28 }, base.IsCompressed);
                    }
                    if (element18.IsZero)
                    {
                        return this.Twice();
                    }
                    return curve.Infinity;
                }
                case 6:
                    if (!rawXCoord.IsZero)
                    {
                        ECFieldElement element39;
                        ECFieldElement element40;
                        ECFieldElement element41;
                        ECFieldElement rawYCoord = base.RawYCoord;
                        ECFieldElement element30 = base.RawZCoords[0];
                        ECFieldElement element31 = b.RawYCoord;
                        ECFieldElement element32 = b.RawZCoords[0];
                        bool isOne = element30.IsOne;
                        ECFieldElement element33 = element2;
                        ECFieldElement element34 = element31;
                        if (!isOne)
                        {
                            element33 = element33.Multiply(element30);
                            element34 = element34.Multiply(element30);
                        }
                        bool flag4 = element32.IsOne;
                        ECFieldElement element35 = rawXCoord;
                        ECFieldElement element36 = rawYCoord;
                        if (!flag4)
                        {
                            element35 = element35.Multiply(element32);
                            element36 = element36.Multiply(element32);
                        }
                        ECFieldElement element37 = element36.Add(element34);
                        ECFieldElement element38 = element35.Add(element33);
                        if (element38.IsZero)
                        {
                            if (element37.IsZero)
                            {
                                return this.Twice();
                            }
                            return curve.Infinity;
                        }
                        if (element2.IsZero)
                        {
                            ECPoint point = this.Normalize();
                            rawXCoord = point.RawXCoord;
                            ECFieldElement yCoord = point.YCoord;
                            ECFieldElement element43 = element31;
                            ECFieldElement element44 = yCoord.Add(element43).Divide(rawXCoord);
                            element39 = element44.Square().Add(element44).Add(rawXCoord).Add(curve.A);
                            if (element39.IsZero)
                            {
                                return new F2mPoint(curve, element39, curve.B.Sqrt(), base.IsCompressed);
                            }
                            element40 = element44.Multiply(rawXCoord.Add(element39)).Add(element39).Add(yCoord).Divide(element39).Add(element39);
                            element41 = curve.FromBigInteger(BigInteger.One);
                        }
                        else
                        {
                            element38 = element38.Square();
                            ECFieldElement element46 = element37.Multiply(element35);
                            ECFieldElement element47 = element37.Multiply(element33);
                            element39 = element46.Multiply(element47);
                            if (element39.IsZero)
                            {
                                return new F2mPoint(curve, element39, curve.B.Sqrt(), base.IsCompressed);
                            }
                            ECFieldElement x = element37.Multiply(element38);
                            if (!flag4)
                            {
                                x = x.Multiply(element32);
                            }
                            element40 = element47.Add(element38).SquarePlusProduct(x, rawYCoord.Add(element30));
                            element41 = x;
                            if (!isOne)
                            {
                                element41 = element41.Multiply(element30);
                            }
                        }
                        return new F2mPoint(curve, element39, element40, new ECFieldElement[] { element41 }, base.IsCompressed);
                    }
                    if (element2.IsZero)
                    {
                        return curve.Infinity;
                    }
                    return b.Add(this);
            }
            throw new InvalidOperationException("unsupported coordinate system");
        }

        protected override ECPoint Detach() => 
            new F2mPoint(null, this.AffineXCoord, this.AffineYCoord);

        public override ECPoint Negate()
        {
            if (base.IsInfinity)
            {
                return this;
            }
            ECFieldElement rawXCoord = base.RawXCoord;
            if (rawXCoord.IsZero)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            switch (curve.CoordinateSystem)
            {
                case 0:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    return new F2mPoint(curve, rawXCoord, rawYCoord.Add(rawXCoord), base.IsCompressed);
                }
                case 1:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement element4 = base.RawZCoords[0];
                    return new F2mPoint(curve, rawXCoord, rawYCoord.Add(rawXCoord), new ECFieldElement[] { element4 }, base.IsCompressed);
                }
                case 5:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    return new F2mPoint(curve, rawXCoord, rawYCoord.AddOne(), base.IsCompressed);
                }
                case 6:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement b = base.RawZCoords[0];
                    return new F2mPoint(curve, rawXCoord, rawYCoord.Add(b), new ECFieldElement[] { b }, base.IsCompressed);
                }
            }
            throw new InvalidOperationException("unsupported coordinate system");
        }

        public override ECPoint Twice()
        {
            ECFieldElement element26;
            ECFieldElement element27;
            ECFieldElement element29;
            if (base.IsInfinity)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            ECFieldElement rawXCoord = base.RawXCoord;
            if (rawXCoord.IsZero)
            {
                return curve.Infinity;
            }
            switch (curve.CoordinateSystem)
            {
                case 0:
                {
                    ECFieldElement b = base.RawYCoord.Divide(rawXCoord).Add(rawXCoord);
                    ECFieldElement x = b.Square().Add(b).Add(curve.A);
                    return new F2mPoint(curve, x, rawXCoord.SquarePlusProduct(x, b.AddOne()), base.IsCompressed);
                }
                case 1:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement b = base.RawZCoords[0];
                    bool isOne = b.IsOne;
                    ECFieldElement element8 = !isOne ? rawXCoord.Multiply(b) : rawXCoord;
                    ECFieldElement element9 = !isOne ? rawYCoord.Multiply(b) : rawYCoord;
                    ECFieldElement element10 = rawXCoord.Square();
                    ECFieldElement element11 = element10.Add(element9);
                    ECFieldElement element12 = element8;
                    ECFieldElement x = element12.Square();
                    ECFieldElement y = element11.Add(element12);
                    ECFieldElement element15 = y.MultiplyPlusProduct(element11, x, curve.A);
                    ECFieldElement element16 = element12.Multiply(element15);
                    ECFieldElement element17 = element10.Square().MultiplyPlusProduct(element12, element15, y);
                    ECFieldElement element18 = element12.Multiply(x);
                    return new F2mPoint(curve, element16, element17, new ECFieldElement[] { element18 }, base.IsCompressed);
                }
                case 6:
                {
                    ECFieldElement element31;
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement b = base.RawZCoords[0];
                    bool isOne = b.IsOne;
                    ECFieldElement element21 = !isOne ? rawYCoord.Multiply(b) : rawYCoord;
                    ECFieldElement element22 = !isOne ? b.Square() : b;
                    ECFieldElement a = curve.A;
                    ECFieldElement element24 = !isOne ? a.Multiply(element22) : a;
                    ECFieldElement x = rawYCoord.Square().Add(element21).Add(element24);
                    if (x.IsZero)
                    {
                        return new F2mPoint(curve, x, curve.B.Sqrt(), base.IsCompressed);
                    }
                    element26 = x.Square();
                    element27 = !isOne ? x.Multiply(element22) : x;
                    ECFieldElement element28 = curve.B;
                    if (element28.BitLength >= (curve.FieldSize >> 1))
                    {
                        ECFieldElement element32 = !isOne ? rawXCoord.Multiply(b) : rawXCoord;
                        element29 = element32.SquarePlusProduct(x, element21).Add(element26).Add(element27);
                        break;
                    }
                    ECFieldElement element30 = rawYCoord.Add(rawXCoord).Square();
                    if (element28.IsOne)
                    {
                        element31 = element24.Add(element22).Square();
                    }
                    else
                    {
                        element31 = element24.SquarePlusProduct(element28, element22.Square());
                    }
                    element29 = element30.Add(x).Add(element22).Multiply(element30).Add(element31).Add(element26);
                    if (a.IsZero)
                    {
                        element29 = element29.Add(element27);
                    }
                    else if (!a.IsOne)
                    {
                        element29 = element29.Add(a.AddOne().Multiply(element27));
                    }
                    break;
                }
                default:
                    throw new InvalidOperationException("unsupported coordinate system");
            }
            return new F2mPoint(curve, element26, element29, new ECFieldElement[] { element27 }, base.IsCompressed);
        }

        public override ECPoint TwicePlus(ECPoint b)
        {
            if (base.IsInfinity)
            {
                return b;
            }
            if (b.IsInfinity)
            {
                return this.Twice();
            }
            ECCurve curve = this.Curve;
            ECFieldElement rawXCoord = base.RawXCoord;
            if (rawXCoord.IsZero)
            {
                return b;
            }
            if (curve.CoordinateSystem != 6)
            {
                return this.Twice().Add(b);
            }
            ECFieldElement element2 = b.RawXCoord;
            ECFieldElement element3 = b.RawZCoords[0];
            if (element2.IsZero || !element3.IsOne)
            {
                return this.Twice().Add(b);
            }
            ECFieldElement rawYCoord = base.RawYCoord;
            ECFieldElement element5 = base.RawZCoords[0];
            ECFieldElement element6 = b.RawYCoord;
            ECFieldElement x = rawXCoord.Square();
            ECFieldElement element8 = rawYCoord.Square();
            ECFieldElement element9 = element5.Square();
            ECFieldElement element10 = rawYCoord.Multiply(element5);
            ECFieldElement element11 = curve.A.Multiply(element9).Add(element8).Add(element10);
            ECFieldElement element12 = element6.AddOne();
            ECFieldElement element13 = curve.A.Add(element12).Multiply(element9).Add(element8).MultiplyPlusProduct(element11, x, element9);
            ECFieldElement element14 = element2.Multiply(element9);
            ECFieldElement element15 = element14.Add(element11).Square();
            if (element15.IsZero)
            {
                if (element13.IsZero)
                {
                    return b.Twice();
                }
                return curve.Infinity;
            }
            if (element13.IsZero)
            {
                return new F2mPoint(curve, element13, curve.B.Sqrt(), base.IsCompressed);
            }
            ECFieldElement element16 = element13.Square().Multiply(element14);
            ECFieldElement y = element13.Multiply(element15).Multiply(element9);
            ECFieldElement element18 = element13.Add(element15).Square().MultiplyPlusProduct(element11, element12, y);
            return new F2mPoint(curve, element16, element18, new ECFieldElement[] { y }, base.IsCompressed);
        }

        public override ECFieldElement YCoord
        {
            get
            {
                int curveCoordinateSystem = this.CurveCoordinateSystem;
                if ((curveCoordinateSystem != 5) && (curveCoordinateSystem != 6))
                {
                    return base.RawYCoord;
                }
                ECFieldElement rawXCoord = base.RawXCoord;
                ECFieldElement rawYCoord = base.RawYCoord;
                if (base.IsInfinity || rawXCoord.IsZero)
                {
                    return rawYCoord;
                }
                ECFieldElement element3 = rawYCoord.Add(rawXCoord).Multiply(rawXCoord);
                if (curveCoordinateSystem == 6)
                {
                    ECFieldElement b = base.RawZCoords[0];
                    if (!b.IsOne)
                    {
                        element3 = element3.Divide(b);
                    }
                }
                return element3;
            }
        }

        protected internal override bool CompressionYTilde
        {
            get
            {
                ECFieldElement rawXCoord = base.RawXCoord;
                if (rawXCoord.IsZero)
                {
                    return false;
                }
                ECFieldElement rawYCoord = base.RawYCoord;
                int curveCoordinateSystem = this.CurveCoordinateSystem;
                if ((curveCoordinateSystem != 5) && (curveCoordinateSystem != 6))
                {
                    return rawYCoord.Divide(rawXCoord).TestBitZero();
                }
                return (rawYCoord.TestBitZero() != rawXCoord.TestBitZero());
            }
        }
    }
}

