namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    internal class SecT163K1Point : AbstractF2mPoint
    {
        public SecT163K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
        {
        }

        public SecT163K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
        {
            if ((x == null) != (y == null))
            {
                throw new ArgumentException("Exactly one of the field elements is null");
            }
        }

        internal SecT163K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
        {
        }

        public override ECPoint Add(ECPoint b)
        {
            ECFieldElement element13;
            ECFieldElement element14;
            ECFieldElement element15;
            if (base.IsInfinity)
            {
                return b;
            }
            if (b.IsInfinity)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            ECFieldElement rawXCoord = base.RawXCoord;
            ECFieldElement element2 = b.RawXCoord;
            if (rawXCoord.IsZero)
            {
                if (element2.IsZero)
                {
                    return curve.Infinity;
                }
                return b.Add(this);
            }
            ECFieldElement rawYCoord = base.RawYCoord;
            ECFieldElement element4 = base.RawZCoords[0];
            ECFieldElement element5 = b.RawYCoord;
            ECFieldElement element6 = b.RawZCoords[0];
            bool isOne = element4.IsOne;
            ECFieldElement element7 = element2;
            ECFieldElement element8 = element5;
            if (!isOne)
            {
                element7 = element7.Multiply(element4);
                element8 = element8.Multiply(element4);
            }
            bool flag2 = element6.IsOne;
            ECFieldElement element9 = rawXCoord;
            ECFieldElement element10 = rawYCoord;
            if (!flag2)
            {
                element9 = element9.Multiply(element6);
                element10 = element10.Multiply(element6);
            }
            ECFieldElement element11 = element10.Add(element8);
            ECFieldElement element12 = element9.Add(element7);
            if (element12.IsZero)
            {
                if (element11.IsZero)
                {
                    return this.Twice();
                }
                return curve.Infinity;
            }
            if (element2.IsZero)
            {
                ECPoint point = this.Normalize();
                rawXCoord = point.XCoord;
                ECFieldElement yCoord = point.YCoord;
                ECFieldElement element17 = element5;
                ECFieldElement element18 = yCoord.Add(element17).Divide(rawXCoord);
                element13 = element18.Square().Add(element18).Add(rawXCoord).AddOne();
                if (element13.IsZero)
                {
                    return new SecT163K1Point(curve, element13, curve.B, base.IsCompressed);
                }
                element14 = element18.Multiply(rawXCoord.Add(element13)).Add(element13).Add(yCoord).Divide(element13).Add(element13);
                element15 = curve.FromBigInteger(BigInteger.One);
            }
            else
            {
                element12 = element12.Square();
                ECFieldElement element20 = element11.Multiply(element9);
                ECFieldElement element21 = element11.Multiply(element7);
                element13 = element20.Multiply(element21);
                if (element13.IsZero)
                {
                    return new SecT163K1Point(curve, element13, curve.B, base.IsCompressed);
                }
                ECFieldElement x = element11.Multiply(element12);
                if (!flag2)
                {
                    x = x.Multiply(element6);
                }
                element14 = element21.Add(element12).SquarePlusProduct(x, rawYCoord.Add(element4));
                element15 = x;
                if (!isOne)
                {
                    element15 = element15.Multiply(element4);
                }
            }
            return new SecT163K1Point(curve, element13, element14, new ECFieldElement[] { element15 }, base.IsCompressed);
        }

        protected override ECPoint Detach() => 
            new SecT163K1Point(null, this.AffineXCoord, this.AffineYCoord);

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
            ECFieldElement rawYCoord = base.RawYCoord;
            ECFieldElement b = base.RawZCoords[0];
            return new SecT163K1Point(this.Curve, rawXCoord, rawYCoord.Add(b), new ECFieldElement[] { b }, base.IsCompressed);
        }

        public override ECPoint Twice()
        {
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
            ECFieldElement rawYCoord = base.RawYCoord;
            ECFieldElement b = base.RawZCoords[0];
            bool isOne = b.IsOne;
            ECFieldElement element4 = !isOne ? rawYCoord.Multiply(b) : rawYCoord;
            ECFieldElement element5 = !isOne ? b.Square() : b;
            ECFieldElement x = rawYCoord.Square().Add(element4).Add(element5);
            if (x.IsZero)
            {
                return new SecT163K1Point(curve, x, curve.B, base.IsCompressed);
            }
            ECFieldElement element7 = x.Square();
            ECFieldElement element8 = !isOne ? x.Multiply(element5) : x;
            ECFieldElement element9 = rawYCoord.Add(rawXCoord).Square();
            ECFieldElement y = element9.Add(x).Add(element5).Multiply(element9).Add(element7);
            return new SecT163K1Point(curve, element7, y, new ECFieldElement[] { element8 }, base.IsCompressed);
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
            ECFieldElement y = element5.Square();
            ECFieldElement element10 = rawYCoord.Multiply(element5);
            ECFieldElement element11 = y.Add(element8).Add(element10);
            ECFieldElement element12 = element6.AddOne();
            ECFieldElement element13 = element6.Multiply(y).Add(element8).MultiplyPlusProduct(element11, x, y);
            ECFieldElement element14 = element2.Multiply(y);
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
                return new SecT163K1Point(curve, element13, curve.B, base.IsCompressed);
            }
            ECFieldElement element16 = element13.Square().Multiply(element14);
            ECFieldElement element17 = element13.Multiply(element15).Multiply(y);
            ECFieldElement element18 = element13.Add(element15).Square().MultiplyPlusProduct(element11, element12, element17);
            return new SecT163K1Point(curve, element16, element18, new ECFieldElement[] { element17 }, base.IsCompressed);
        }

        public override ECFieldElement YCoord
        {
            get
            {
                ECFieldElement rawXCoord = base.RawXCoord;
                ECFieldElement rawYCoord = base.RawYCoord;
                if (base.IsInfinity || rawXCoord.IsZero)
                {
                    return rawYCoord;
                }
                ECFieldElement element3 = rawYCoord.Add(rawXCoord).Multiply(rawXCoord);
                ECFieldElement b = base.RawZCoords[0];
                if (!b.IsOne)
                {
                    element3 = element3.Divide(b);
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
                return (base.RawYCoord.TestBitZero() != rawXCoord.TestBitZero());
            }
        }
    }
}

