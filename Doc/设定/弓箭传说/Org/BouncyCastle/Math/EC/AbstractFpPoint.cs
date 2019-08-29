namespace Org.BouncyCastle.Math.EC
{
    using System;

    public abstract class AbstractFpPoint : ECPointBase
    {
        protected AbstractFpPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
        {
        }

        protected AbstractFpPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
        {
        }

        protected override bool SatisfiesCurveEquation()
        {
            ECFieldElement rawXCoord = base.RawXCoord;
            ECFieldElement rawYCoord = base.RawYCoord;
            ECFieldElement a = this.Curve.A;
            ECFieldElement b = this.Curve.B;
            ECFieldElement element5 = rawYCoord.Square();
            switch (this.CurveCoordinateSystem)
            {
                case 0:
                    break;

                case 1:
                {
                    ECFieldElement element6 = base.RawZCoords[0];
                    if (!element6.IsOne)
                    {
                        ECFieldElement element7 = element6.Square();
                        ECFieldElement element8 = element6.Multiply(element7);
                        element5 = element5.Multiply(element6);
                        a = a.Multiply(element7);
                        b = b.Multiply(element8);
                    }
                    break;
                }
                case 2:
                case 3:
                case 4:
                {
                    ECFieldElement element9 = base.RawZCoords[0];
                    if (!element9.IsOne)
                    {
                        ECFieldElement element10 = element9.Square();
                        ECFieldElement element11 = element10.Square();
                        ECFieldElement element12 = element10.Multiply(element11);
                        a = a.Multiply(element11);
                        b = b.Multiply(element12);
                    }
                    break;
                }
                default:
                    throw new InvalidOperationException("unsupported coordinate system");
            }
            ECFieldElement other = rawXCoord.Square().Add(a).Multiply(rawXCoord).Add(b);
            return element5.Equals(other);
        }

        public override ECPoint Subtract(ECPoint b)
        {
            if (b.IsInfinity)
            {
                return this;
            }
            return this.Add(b.Negate());
        }

        protected internal override bool CompressionYTilde =>
            this.AffineYCoord.TestBitZero();
    }
}

