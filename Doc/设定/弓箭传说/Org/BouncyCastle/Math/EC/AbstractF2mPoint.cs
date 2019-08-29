namespace Org.BouncyCastle.Math.EC
{
    using System;

    public abstract class AbstractF2mPoint : ECPointBase
    {
        protected AbstractF2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
        {
        }

        protected AbstractF2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
        {
        }

        protected override bool SatisfiesCurveEquation()
        {
            ECFieldElement element5;
            ECFieldElement element6;
            ECCurve curve = this.Curve;
            ECFieldElement rawXCoord = base.RawXCoord;
            ECFieldElement rawYCoord = base.RawYCoord;
            ECFieldElement a = curve.A;
            ECFieldElement b = curve.B;
            int coordinateSystem = curve.CoordinateSystem;
            if (coordinateSystem == 6)
            {
                ECFieldElement element7 = base.RawZCoords[0];
                bool isOne = element7.IsOne;
                if (rawXCoord.IsZero)
                {
                    element5 = rawYCoord.Square();
                    element6 = b;
                    if (!isOne)
                    {
                        ECFieldElement element8 = element7.Square();
                        element6 = element6.Multiply(element8);
                    }
                }
                else
                {
                    ECFieldElement element9 = rawYCoord;
                    ECFieldElement element10 = rawXCoord.Square();
                    if (isOne)
                    {
                        element5 = element9.Square().Add(element9).Add(a);
                        element6 = element10.Square().Add(b);
                    }
                    else
                    {
                        ECFieldElement y = element7.Square();
                        ECFieldElement element12 = y.Square();
                        element5 = element9.Add(element7).MultiplyPlusProduct(element9, a, y);
                        element6 = element10.SquarePlusProduct(b, element12);
                    }
                    element5 = element5.Multiply(element10);
                }
            }
            else
            {
                element5 = rawYCoord.Add(rawXCoord).Multiply(rawYCoord);
                if (coordinateSystem != 0)
                {
                    if (coordinateSystem != 1)
                    {
                        throw new InvalidOperationException("unsupported coordinate system");
                    }
                    ECFieldElement element13 = base.RawZCoords[0];
                    if (!element13.IsOne)
                    {
                        ECFieldElement element14 = element13.Square();
                        ECFieldElement element15 = element13.Multiply(element14);
                        element5 = element5.Multiply(element13);
                        a = a.Multiply(element13);
                        b = b.Multiply(element15);
                    }
                }
                element6 = rawXCoord.Add(a).Multiply(rawXCoord.Square()).Add(b);
            }
            return element5.Equals(element6);
        }

        public override ECPoint ScaleX(ECFieldElement scale)
        {
            if (base.IsInfinity)
            {
                return this;
            }
            switch (this.CurveCoordinateSystem)
            {
                case 5:
                {
                    ECFieldElement rawXCoord = base.RawXCoord;
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement b = rawXCoord.Multiply(scale);
                    ECFieldElement y = rawYCoord.Add(rawXCoord).Divide(scale).Add(b);
                    return this.Curve.CreateRawPoint(rawXCoord, y, base.RawZCoords, base.IsCompressed);
                }
                case 6:
                {
                    ECFieldElement rawXCoord = base.RawXCoord;
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement element7 = base.RawZCoords[0];
                    ECFieldElement b = rawXCoord.Multiply(scale.Square());
                    ECFieldElement y = rawYCoord.Add(rawXCoord).Add(b);
                    ECFieldElement element10 = element7.Multiply(scale);
                    ECFieldElement[] zs = new ECFieldElement[] { element10 };
                    return this.Curve.CreateRawPoint(rawXCoord, y, zs, base.IsCompressed);
                }
            }
            return base.ScaleX(scale);
        }

        public override ECPoint ScaleY(ECFieldElement scale)
        {
            if (base.IsInfinity)
            {
                return this;
            }
            int curveCoordinateSystem = this.CurveCoordinateSystem;
            if ((curveCoordinateSystem != 5) && (curveCoordinateSystem != 6))
            {
                return base.ScaleY(scale);
            }
            ECFieldElement rawXCoord = base.RawXCoord;
            ECFieldElement y = base.RawYCoord.Add(rawXCoord).Multiply(scale).Add(rawXCoord);
            return this.Curve.CreateRawPoint(rawXCoord, y, base.RawZCoords, base.IsCompressed);
        }

        public override ECPoint Subtract(ECPoint b)
        {
            if (b.IsInfinity)
            {
                return this;
            }
            return this.Add(b.Negate());
        }

        public virtual AbstractF2mPoint Tau()
        {
            if (base.IsInfinity)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement rawXCoord = base.RawXCoord;
            switch (coordinateSystem)
            {
                case 0:
                case 5:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    return (AbstractF2mPoint) curve.CreateRawPoint(rawXCoord.Square(), rawYCoord.Square(), base.IsCompressed);
                }
                case 1:
                case 6:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement element4 = base.RawZCoords[0];
                    ECFieldElement[] zs = new ECFieldElement[] { element4.Square() };
                    return (AbstractF2mPoint) curve.CreateRawPoint(rawXCoord.Square(), rawYCoord.Square(), zs, base.IsCompressed);
                }
            }
            throw new InvalidOperationException("unsupported coordinate system");
        }

        public virtual AbstractF2mPoint TauPow(int pow)
        {
            if (base.IsInfinity)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            int coordinateSystem = curve.CoordinateSystem;
            ECFieldElement rawXCoord = base.RawXCoord;
            switch (coordinateSystem)
            {
                case 0:
                case 5:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    return (AbstractF2mPoint) curve.CreateRawPoint(rawXCoord.SquarePow(pow), rawYCoord.SquarePow(pow), base.IsCompressed);
                }
                case 1:
                case 6:
                {
                    ECFieldElement rawYCoord = base.RawYCoord;
                    ECFieldElement element4 = base.RawZCoords[0];
                    ECFieldElement[] zs = new ECFieldElement[] { element4.SquarePow(pow) };
                    return (AbstractF2mPoint) curve.CreateRawPoint(rawXCoord.SquarePow(pow), rawYCoord.SquarePow(pow), zs, base.IsCompressed);
                }
            }
            throw new InvalidOperationException("unsupported coordinate system");
        }
    }
}

