namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Raw;
    using System;

    internal class Curve25519Point : AbstractFpPoint
    {
        public Curve25519Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
        {
        }

        public Curve25519Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
        {
            if ((x == null) != (y == null))
            {
                throw new ArgumentException("Exactly one of the field elements is null");
            }
        }

        internal Curve25519Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
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
            Curve25519FieldElement rawXCoord = (Curve25519FieldElement) base.RawXCoord;
            Curve25519FieldElement rawYCoord = (Curve25519FieldElement) base.RawYCoord;
            Curve25519FieldElement element3 = (Curve25519FieldElement) base.RawZCoords[0];
            Curve25519FieldElement element4 = (Curve25519FieldElement) b.RawXCoord;
            Curve25519FieldElement element5 = (Curve25519FieldElement) b.RawYCoord;
            Curve25519FieldElement element6 = (Curve25519FieldElement) b.RawZCoords[0];
            uint[] zz = Nat256.CreateExt();
            uint[] numArray2 = Nat256.Create();
            uint[] numArray3 = Nat256.Create();
            uint[] x = Nat256.Create();
            bool isOne = element3.IsOne;
            if (isOne)
            {
                numArray5 = element4.x;
                numArray6 = element5.x;
            }
            else
            {
                numArray6 = numArray3;
                Curve25519Field.Square(element3.x, numArray6);
                numArray5 = numArray2;
                Curve25519Field.Multiply(numArray6, element4.x, numArray5);
                Curve25519Field.Multiply(numArray6, element3.x, numArray6);
                Curve25519Field.Multiply(numArray6, element5.x, numArray6);
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
                Curve25519Field.Square(element6.x, numArray8);
                numArray7 = zz;
                Curve25519Field.Multiply(numArray8, rawXCoord.x, numArray7);
                Curve25519Field.Multiply(numArray8, element6.x, numArray8);
                Curve25519Field.Multiply(numArray8, rawYCoord.x, numArray8);
            }
            uint[] z = Nat256.Create();
            Curve25519Field.Subtract(numArray7, numArray5, z);
            uint[] numArray10 = numArray2;
            Curve25519Field.Subtract(numArray8, numArray6, numArray10);
            if (Nat256.IsZero(z))
            {
                if (Nat256.IsZero(numArray10))
                {
                    return this.Twice();
                }
                return curve.Infinity;
            }
            uint[] numArray11 = Nat256.Create();
            Curve25519Field.Square(z, numArray11);
            uint[] numArray12 = Nat256.Create();
            Curve25519Field.Multiply(numArray11, z, numArray12);
            uint[] numArray13 = numArray3;
            Curve25519Field.Multiply(numArray11, numArray7, numArray13);
            Curve25519Field.Negate(numArray12, numArray12);
            Nat256.Mul(numArray8, numArray12, zz);
            Curve25519Field.Reduce27(Nat256.AddBothTo(numArray13, numArray13, numArray12), numArray12);
            Curve25519FieldElement element7 = new Curve25519FieldElement(x);
            Curve25519Field.Square(numArray10, element7.x);
            Curve25519Field.Subtract(element7.x, numArray12, element7.x);
            Curve25519FieldElement y = new Curve25519FieldElement(numArray12);
            Curve25519Field.Subtract(numArray13, element7.x, y.x);
            Curve25519Field.MultiplyAddToExt(y.x, numArray10, zz);
            Curve25519Field.Reduce(zz, y.x);
            Curve25519FieldElement element9 = new Curve25519FieldElement(z);
            if (!isOne)
            {
                Curve25519Field.Multiply(element9.x, element3.x, element9.x);
            }
            if (!flag2)
            {
                Curve25519Field.Multiply(element9.x, element6.x, element9.x);
            }
            uint[] zSquared = (!isOne || !flag2) ? null : numArray11;
            Curve25519FieldElement element10 = this.CalculateJacobianModifiedW(element9, zSquared);
            ECFieldElement[] zs = new ECFieldElement[] { element9, element10 };
            return new Curve25519Point(curve, element7, y, zs, base.IsCompressed);
        }

        protected virtual Curve25519FieldElement CalculateJacobianModifiedW(Curve25519FieldElement Z, uint[] ZSquared)
        {
            Curve25519FieldElement a = (Curve25519FieldElement) this.Curve.A;
            if (Z.IsOne)
            {
                return a;
            }
            Curve25519FieldElement element2 = new Curve25519FieldElement();
            if (ZSquared == null)
            {
                ZSquared = element2.x;
                Curve25519Field.Square(Z.x, ZSquared);
            }
            Curve25519Field.Square(ZSquared, element2.x);
            Curve25519Field.Multiply(element2.x, a.x, element2.x);
            return element2;
        }

        protected override ECPoint Detach() => 
            new Curve25519Point(null, this.AffineXCoord, this.AffineYCoord);

        protected virtual Curve25519FieldElement GetJacobianModifiedW()
        {
            ECFieldElement[] rawZCoords = base.RawZCoords;
            Curve25519FieldElement element = (Curve25519FieldElement) rawZCoords[1];
            if (element == null)
            {
                rawZCoords[1] = element = this.CalculateJacobianModifiedW((Curve25519FieldElement) rawZCoords[0], null);
            }
            return element;
        }

        public override ECFieldElement GetZCoord(int index)
        {
            if (index == 1)
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
            return new Curve25519Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
        }

        public override ECPoint ThreeTimes()
        {
            if (!base.IsInfinity && !base.RawYCoord.IsZero)
            {
                return this.TwiceJacobianModified(false).Add(this);
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
            if (base.RawYCoord.IsZero)
            {
                return curve.Infinity;
            }
            return this.TwiceJacobianModified(true);
        }

        protected virtual Curve25519Point TwiceJacobianModified(bool calculateW)
        {
            Curve25519FieldElement rawXCoord = (Curve25519FieldElement) base.RawXCoord;
            Curve25519FieldElement rawYCoord = (Curve25519FieldElement) base.RawYCoord;
            Curve25519FieldElement element3 = (Curve25519FieldElement) base.RawZCoords[0];
            Curve25519FieldElement jacobianModifiedW = this.GetJacobianModifiedW();
            uint[] z = Nat256.Create();
            Curve25519Field.Square(rawXCoord.x, z);
            uint x = Nat256.AddBothTo(z, z, z) + Nat256.AddTo(jacobianModifiedW.x, z);
            Curve25519Field.Reduce27(x, z);
            uint[] numArray2 = Nat256.Create();
            Curve25519Field.Twice(rawYCoord.x, numArray2);
            uint[] numArray3 = Nat256.Create();
            Curve25519Field.Multiply(numArray2, rawYCoord.x, numArray3);
            uint[] numArray4 = Nat256.Create();
            Curve25519Field.Multiply(numArray3, rawXCoord.x, numArray4);
            Curve25519Field.Twice(numArray4, numArray4);
            uint[] numArray5 = Nat256.Create();
            Curve25519Field.Square(numArray3, numArray5);
            Curve25519Field.Twice(numArray5, numArray5);
            Curve25519FieldElement element5 = new Curve25519FieldElement(numArray3);
            Curve25519Field.Square(z, element5.x);
            Curve25519Field.Subtract(element5.x, numArray4, element5.x);
            Curve25519Field.Subtract(element5.x, numArray4, element5.x);
            Curve25519FieldElement y = new Curve25519FieldElement(numArray4);
            Curve25519Field.Subtract(numArray4, element5.x, y.x);
            Curve25519Field.Multiply(y.x, z, y.x);
            Curve25519Field.Subtract(y.x, numArray5, y.x);
            Curve25519FieldElement element7 = new Curve25519FieldElement(numArray2);
            if (!Nat256.IsOne(element3.x))
            {
                Curve25519Field.Multiply(element7.x, element3.x, element7.x);
            }
            Curve25519FieldElement element8 = null;
            if (calculateW)
            {
                element8 = new Curve25519FieldElement(numArray5);
                Curve25519Field.Multiply(element8.x, jacobianModifiedW.x, element8.x);
                Curve25519Field.Twice(element8.x, element8.x);
            }
            return new Curve25519Point(this.Curve, element5, y, new ECFieldElement[] { element7, element8 }, base.IsCompressed);
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
            return this.TwiceJacobianModified(false).Add(b);
        }
    }
}

