namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using System;
    using System.Collections;
    using System.Text;

    public abstract class ECPoint
    {
        protected static ECFieldElement[] EMPTY_ZS = new ECFieldElement[0];
        protected internal readonly ECCurve m_curve;
        protected internal readonly ECFieldElement m_x;
        protected internal readonly ECFieldElement m_y;
        protected internal readonly ECFieldElement[] m_zs;
        protected internal readonly bool m_withCompression;
        protected internal IDictionary m_preCompTable;

        protected ECPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : this(curve, x, y, GetInitialZCoords(curve), withCompression)
        {
        }

        internal ECPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
        {
            this.m_curve = curve;
            this.m_x = x;
            this.m_y = y;
            this.m_zs = zs;
            this.m_withCompression = withCompression;
        }

        public abstract ECPoint Add(ECPoint b);
        protected virtual void CheckNormalized()
        {
            if (!this.IsNormalized())
            {
                throw new InvalidOperationException("point not in normal form");
            }
        }

        protected virtual ECPoint CreateScaledPoint(ECFieldElement sx, ECFieldElement sy) => 
            this.Curve.CreateRawPoint(this.RawXCoord.Multiply(sx), this.RawYCoord.Multiply(sy), this.IsCompressed);

        protected abstract ECPoint Detach();
        public virtual bool Equals(ECPoint other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            ECCurve curve = this.Curve;
            ECCurve curve2 = other.Curve;
            bool flag = null == curve;
            bool flag2 = null == curve2;
            bool isInfinity = this.IsInfinity;
            bool flag4 = other.IsInfinity;
            if (isInfinity || flag4)
            {
                return ((isInfinity && flag4) && ((flag || flag2) || curve.Equals(curve2)));
            }
            ECPoint point = this;
            ECPoint p = other;
            if (!flag || !flag2)
            {
                if (flag)
                {
                    p = p.Normalize();
                }
                else if (flag2)
                {
                    point = point.Normalize();
                }
                else
                {
                    if (!curve.Equals(curve2))
                    {
                        return false;
                    }
                    ECPoint[] points = new ECPoint[] { this, curve.ImportPoint(p) };
                    curve.NormalizeAll(points);
                    point = points[0];
                    p = points[1];
                }
            }
            return (point.XCoord.Equals(p.XCoord) && point.YCoord.Equals(p.YCoord));
        }

        public override bool Equals(object obj) => 
            this.Equals(obj as ECPoint);

        public ECPoint GetDetachedPoint() => 
            this.Normalize().Detach();

        public virtual byte[] GetEncoded() => 
            this.GetEncoded(this.m_withCompression);

        public abstract byte[] GetEncoded(bool compressed);
        public override int GetHashCode()
        {
            ECCurve curve = this.Curve;
            int num = (curve != null) ? ~curve.GetHashCode() : 0;
            if (!this.IsInfinity)
            {
                ECPoint point = this.Normalize();
                num ^= point.XCoord.GetHashCode() * 0x11;
                num ^= point.YCoord.GetHashCode() * 0x101;
            }
            return num;
        }

        protected static ECFieldElement[] GetInitialZCoords(ECCurve curve)
        {
            int num = (curve != null) ? curve.CoordinateSystem : 0;
            switch (num)
            {
                case 0:
                case 5:
                    return EMPTY_ZS;

                default:
                {
                    ECFieldElement element = curve.FromBigInteger(BigInteger.One);
                    switch (num)
                    {
                        case 1:
                        case 2:
                        case 6:
                            return new ECFieldElement[] { element };

                        case 3:
                            return new ECFieldElement[] { element, element, element };

                        case 4:
                            return new ECFieldElement[] { element, curve.A };
                    }
                    break;
                }
            }
            throw new ArgumentException("unknown coordinate system");
        }

        public virtual ECFieldElement GetZCoord(int index) => 
            (((index >= 0) && (index < this.m_zs.Length)) ? this.m_zs[index] : null);

        public virtual ECFieldElement[] GetZCoords()
        {
            int length = this.m_zs.Length;
            if (length == 0)
            {
                return this.m_zs;
            }
            ECFieldElement[] destinationArray = new ECFieldElement[length];
            Array.Copy(this.m_zs, 0, destinationArray, 0, length);
            return destinationArray;
        }

        public virtual bool IsNormalized()
        {
            int curveCoordinateSystem = this.CurveCoordinateSystem;
            return ((((curveCoordinateSystem == 0) || (curveCoordinateSystem == 5)) || this.IsInfinity) || this.RawZCoords[0].IsOne);
        }

        public bool IsValid()
        {
            if (!this.IsInfinity && (this.Curve != null))
            {
                if (!this.SatisfiesCurveEquation())
                {
                    return false;
                }
                if (!this.SatisfiesCofactor())
                {
                    return false;
                }
            }
            return true;
        }

        public abstract ECPoint Multiply(BigInteger b);
        public abstract ECPoint Negate();
        public virtual ECPoint Normalize()
        {
            if (this.IsInfinity)
            {
                return this;
            }
            switch (this.CurveCoordinateSystem)
            {
                case 0:
                case 5:
                    return this;
            }
            ECFieldElement element = this.RawZCoords[0];
            if (element.IsOne)
            {
                return this;
            }
            return this.Normalize(element.Invert());
        }

        internal virtual ECPoint Normalize(ECFieldElement zInv)
        {
            switch (this.CurveCoordinateSystem)
            {
                case 1:
                case 6:
                    return this.CreateScaledPoint(zInv, zInv);

                case 2:
                case 3:
                case 4:
                {
                    ECFieldElement sx = zInv.Square();
                    ECFieldElement sy = sx.Multiply(zInv);
                    return this.CreateScaledPoint(sx, sy);
                }
            }
            throw new InvalidOperationException("not a projective coordinate system");
        }

        protected internal bool SatisfiesCofactor()
        {
            BigInteger cofactor = this.Curve.Cofactor;
            return (((cofactor == null) || cofactor.Equals(BigInteger.One)) || !ECAlgorithms.ReferenceMultiply(this, cofactor).IsInfinity);
        }

        protected abstract bool SatisfiesCurveEquation();
        public virtual ECPoint ScaleX(ECFieldElement scale) => 
            (!this.IsInfinity ? this.Curve.CreateRawPoint(this.RawXCoord.Multiply(scale), this.RawYCoord, this.RawZCoords, this.IsCompressed) : this);

        public virtual ECPoint ScaleY(ECFieldElement scale) => 
            (!this.IsInfinity ? this.Curve.CreateRawPoint(this.RawXCoord, this.RawYCoord.Multiply(scale), this.RawZCoords, this.IsCompressed) : this);

        public abstract ECPoint Subtract(ECPoint b);
        public virtual ECPoint ThreeTimes() => 
            this.TwicePlus(this);

        public virtual ECPoint TimesPow2(int e)
        {
            if (e < 0)
            {
                throw new ArgumentException("cannot be negative", "e");
            }
            ECPoint point = this;
            while (--e >= 0)
            {
                point = point.Twice();
            }
            return point;
        }

        public override string ToString()
        {
            if (this.IsInfinity)
            {
                return "INF";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append('(');
            builder.Append(this.RawXCoord);
            builder.Append(',');
            builder.Append(this.RawYCoord);
            for (int i = 0; i < this.m_zs.Length; i++)
            {
                builder.Append(',');
                builder.Append(this.m_zs[i]);
            }
            builder.Append(')');
            return builder.ToString();
        }

        public abstract ECPoint Twice();
        public virtual ECPoint TwicePlus(ECPoint b) => 
            this.Twice().Add(b);

        public virtual ECCurve Curve =>
            this.m_curve;

        protected virtual int CurveCoordinateSystem =>
            ((this.m_curve != null) ? this.m_curve.CoordinateSystem : 0);

        [Obsolete("Use AffineXCoord, or Normalize() and XCoord, instead")]
        public virtual ECFieldElement X =>
            this.Normalize().XCoord;

        [Obsolete("Use AffineYCoord, or Normalize() and YCoord, instead")]
        public virtual ECFieldElement Y =>
            this.Normalize().YCoord;

        public virtual ECFieldElement AffineXCoord
        {
            get
            {
                this.CheckNormalized();
                return this.XCoord;
            }
        }

        public virtual ECFieldElement AffineYCoord
        {
            get
            {
                this.CheckNormalized();
                return this.YCoord;
            }
        }

        public virtual ECFieldElement XCoord =>
            this.m_x;

        public virtual ECFieldElement YCoord =>
            this.m_y;

        protected internal ECFieldElement RawXCoord =>
            this.m_x;

        protected internal ECFieldElement RawYCoord =>
            this.m_y;

        protected internal ECFieldElement[] RawZCoords =>
            this.m_zs;

        public bool IsInfinity =>
            ((this.m_x == null) && (this.m_y == null));

        public bool IsCompressed =>
            this.m_withCompression;

        protected internal abstract bool CompressionYTilde { get; }
    }
}

