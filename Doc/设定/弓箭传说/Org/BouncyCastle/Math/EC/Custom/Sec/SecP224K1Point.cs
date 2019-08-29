namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class SecP224K1Point : AbstractFpPoint
    {
        public SecP224K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
        {
        }

        public SecP224K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
        {
            if ((x == null) != (y == null))
            {
                throw new ArgumentException("Exactly one of the field elements is null");
            }
        }

        internal SecP224K1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
        {
        }

        public override ECPoint Add(ECPoint b)
        {
            uint[] numArray5;
            uint[] numArray6;
            uint[] numArray7;
            uint[] numArray8;
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
            SecP224K1FieldElement rawXCoord = (SecP224K1FieldElement) base.RawXCoord;
            SecP224K1FieldElement rawYCoord = (SecP224K1FieldElement) base.RawYCoord;
            SecP224K1FieldElement element3 = (SecP224K1FieldElement) b.RawXCoord;
            SecP224K1FieldElement element4 = (SecP224K1FieldElement) b.RawYCoord;
            SecP224K1FieldElement element5 = (SecP224K1FieldElement) base.RawZCoords[0];
            SecP224K1FieldElement element6 = (SecP224K1FieldElement) b.RawZCoords[0];
            uint[] zz = Nat224.CreateExt();
            uint[] numArray2 = Nat224.Create();
            uint[] numArray3 = Nat224.Create();
            uint[] x = Nat224.Create();
            bool isOne = element5.IsOne;
            if (isOne)
            {
                numArray5 = element3.x;
                numArray6 = element4.x;
            }
            else
            {
                numArray6 = numArray3;
                SecP224K1Field.Square(element5.x, numArray6);
                numArray5 = numArray2;
                SecP224K1Field.Multiply(numArray6, element3.x, numArray5);
                SecP224K1Field.Multiply(numArray6, element5.x, numArray6);
                SecP224K1Field.Multiply(numArray6, element4.x, numArray6);
            }
            bool flag2 = element6.IsOne;
            if (flag2)
            {
                numArray7 = rawXCoord.x;
                numArray8 = rawYCoord.x;
            }
            else
            {
                numArray8 = x;
                SecP224K1Field.Square(element6.x, numArray8);
                numArray7 = zz;
                SecP224K1Field.Multiply(numArray8, rawXCoord.x, numArray7);
                SecP224K1Field.Multiply(numArray8, element6.x, numArray8);
                SecP224K1Field.Multiply(numArray8, rawYCoord.x, numArray8);
            }
            uint[] z = Nat224.Create();
            SecP224K1Field.Subtract(numArray7, numArray5, z);
            uint[] numArray10 = numArray2;
            SecP224K1Field.Subtract(numArray8, numArray6, numArray10);
            if (Nat224.IsZero(z))
            {
                if (Nat224.IsZero(numArray10))
                {
                    return this.Twice();
                }
                return curve.Infinity;
            }
            uint[] numArray11 = numArray3;
            SecP224K1Field.Square(z, numArray11);
            uint[] numArray12 = Nat224.Create();
            SecP224K1Field.Multiply(numArray11, z, numArray12);
            uint[] numArray13 = numArray3;
            SecP224K1Field.Multiply(numArray11, numArray7, numArray13);
            SecP224K1Field.Negate(numArray12, numArray12);
            Nat224.Mul(numArray8, numArray12, zz);
            SecP224K1Field.Reduce32(Nat224.AddBothTo(numArray13, numArray13, numArray12), numArray12);
            SecP224K1FieldElement element7 = new SecP224K1FieldElement(x);
            SecP224K1Field.Square(numArray10, element7.x);
            SecP224K1Field.Subtract(element7.x, numArray12, element7.x);
            SecP224K1FieldElement y = new SecP224K1FieldElement(numArray12);
            SecP224K1Field.Subtract(numArray13, element7.x, y.x);
            SecP224K1Field.MultiplyAddToExt(y.x, numArray10, zz);
            SecP224K1Field.Reduce(zz, y.x);
            SecP224K1FieldElement element9 = new SecP224K1FieldElement(z);
            if (!isOne)
            {
                SecP224K1Field.Multiply(element9.x, element5.x, element9.x);
            }
            if (!flag2)
            {
                SecP224K1Field.Multiply(element9.x, element6.x, element9.x);
            }
            ECFieldElement[] zs = new ECFieldElement[] { element9 };
            return new SecP224K1Point(curve, element7, y, zs, base.IsCompressed);
        }

        protected override ECPoint Detach() => 
            new SecP224K1Point(null, this.AffineXCoord, this.AffineYCoord);

        public override ECPoint Negate()
        {
            if (base.IsInfinity)
            {
                return this;
            }
            return new SecP224K1Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
        }

        public override ECPoint ThreeTimes()
        {
            if (!base.IsInfinity && !base.RawYCoord.IsZero)
            {
                return this.Twice().Add(this);
            }
            return this;
        }

        public override ECPoint Twice()
        {
            if (base.IsInfinity)
            {
                return this;
            }
            ECCurve curve = this.Curve;
            SecP224K1FieldElement rawYCoord = (SecP224K1FieldElement) base.RawYCoord;
            if (rawYCoord.IsZero)
            {
                return curve.Infinity;
            }
            SecP224K1FieldElement rawXCoord = (SecP224K1FieldElement) base.RawXCoord;
            SecP224K1FieldElement element3 = (SecP224K1FieldElement) base.RawZCoords[0];
            uint[] z = Nat224.Create();
            SecP224K1Field.Square(rawYCoord.x, z);
            uint[] numArray2 = Nat224.Create();
            SecP224K1Field.Square(z, numArray2);
            uint[] numArray3 = Nat224.Create();
            SecP224K1Field.Square(rawXCoord.x, numArray3);
            SecP224K1Field.Reduce32(Nat224.AddBothTo(numArray3, numArray3, numArray3), numArray3);
            uint[] numArray4 = z;
            SecP224K1Field.Multiply(z, rawXCoord.x, numArray4);
            SecP224K1Field.Reduce32(Nat.ShiftUpBits(7, numArray4, 2, 0), numArray4);
            uint[] numArray5 = Nat224.Create();
            SecP224K1Field.Reduce32(Nat.ShiftUpBits(7, numArray2, 3, 0, numArray5), numArray5);
            SecP224K1FieldElement x = new SecP224K1FieldElement(numArray2);
            SecP224K1Field.Square(numArray3, x.x);
            SecP224K1Field.Subtract(x.x, numArray4, x.x);
            SecP224K1Field.Subtract(x.x, numArray4, x.x);
            SecP224K1FieldElement y = new SecP224K1FieldElement(numArray4);
            SecP224K1Field.Subtract(numArray4, x.x, y.x);
            SecP224K1Field.Multiply(y.x, numArray3, y.x);
            SecP224K1Field.Subtract(y.x, numArray5, y.x);
            SecP224K1FieldElement element6 = new SecP224K1FieldElement(numArray3);
            SecP224K1Field.Twice(rawYCoord.x, element6.x);
            if (!element3.IsOne)
            {
                SecP224K1Field.Multiply(element6.x, element3.x, element6.x);
            }
            return new SecP224K1Point(curve, x, y, new ECFieldElement[] { element6 }, base.IsCompressed);
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
            if (base.RawYCoord.IsZero)
            {
                return b;
            }
            return this.Twice().Add(b);
        }
    }
}

