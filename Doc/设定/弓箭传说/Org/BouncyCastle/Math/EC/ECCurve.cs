namespace Org.BouncyCastle.Math.EC
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC.Endo;
    using Org.BouncyCastle.Math.EC.Multiplier;
    using Org.BouncyCastle.Math.Field;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public abstract class ECCurve
    {
        public const int COORD_AFFINE = 0;
        public const int COORD_HOMOGENEOUS = 1;
        public const int COORD_JACOBIAN = 2;
        public const int COORD_JACOBIAN_CHUDNOVSKY = 3;
        public const int COORD_JACOBIAN_MODIFIED = 4;
        public const int COORD_LAMBDA_AFFINE = 5;
        public const int COORD_LAMBDA_PROJECTIVE = 6;
        public const int COORD_SKEWED = 7;
        protected readonly IFiniteField m_field;
        protected ECFieldElement m_a;
        protected ECFieldElement m_b;
        protected BigInteger m_order;
        protected BigInteger m_cofactor;
        protected int m_coord;
        protected ECEndomorphism m_endomorphism;
        protected ECMultiplier m_multiplier;

        protected ECCurve(IFiniteField field)
        {
            this.m_field = field;
        }

        protected virtual void CheckPoint(ECPoint point)
        {
            if ((point == null) || (this != point.Curve))
            {
                throw new ArgumentException("must be non-null and on this curve", "point");
            }
        }

        protected virtual void CheckPoints(ECPoint[] points)
        {
            this.CheckPoints(points, 0, points.Length);
        }

        protected virtual void CheckPoints(ECPoint[] points, int off, int len)
        {
            if (points == null)
            {
                throw new ArgumentNullException("points");
            }
            if (((off < 0) || (len < 0)) || (off > (points.Length - len)))
            {
                throw new ArgumentException("invalid range specified", "points");
            }
            for (int i = 0; i < len; i++)
            {
                ECPoint point = points[off + i];
                if ((point != null) && (this != point.Curve))
                {
                    throw new ArgumentException("entries must be null or on this curve", "points");
                }
            }
        }

        protected abstract ECCurve CloneCurve();
        public virtual Config Configure() => 
            new Config(this, this.m_coord, this.m_endomorphism, this.m_multiplier);

        protected virtual ECMultiplier CreateDefaultMultiplier()
        {
            GlvEndomorphism glvEndomorphism = this.m_endomorphism as GlvEndomorphism;
            if (glvEndomorphism != null)
            {
                return new GlvMultiplier(this, glvEndomorphism);
            }
            return new WNafL2RMultiplier();
        }

        public virtual ECPoint CreatePoint(BigInteger x, BigInteger y) => 
            this.CreatePoint(x, y, false);

        public virtual ECPoint CreatePoint(BigInteger x, BigInteger y, bool withCompression) => 
            this.CreateRawPoint(this.FromBigInteger(x), this.FromBigInteger(y), withCompression);

        protected internal abstract ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression);
        protected internal abstract ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression);
        public virtual ECPoint DecodePoint(byte[] encoded)
        {
            ECPoint infinity = null;
            int length = (this.FieldSize + 7) / 8;
            byte num2 = encoded[0];
            switch (num2)
            {
                case 0:
                    if (encoded.Length != 1)
                    {
                        throw new ArgumentException("Incorrect length for infinity encoding", "encoded");
                    }
                    infinity = this.Infinity;
                    break;

                case 2:
                case 3:
                {
                    if (encoded.Length != (length + 1))
                    {
                        throw new ArgumentException("Incorrect length for compressed encoding", "encoded");
                    }
                    int yTilde = num2 & 1;
                    BigInteger integer = new BigInteger(1, encoded, 1, length);
                    infinity = this.DecompressPoint(yTilde, integer);
                    if (!infinity.SatisfiesCofactor())
                    {
                        throw new ArgumentException("Invalid point");
                    }
                    break;
                }
                case 4:
                {
                    if (encoded.Length != ((2 * length) + 1))
                    {
                        throw new ArgumentException("Incorrect length for uncompressed encoding", "encoded");
                    }
                    BigInteger x = new BigInteger(1, encoded, 1, length);
                    BigInteger y = new BigInteger(1, encoded, 1 + length, length);
                    infinity = this.ValidatePoint(x, y);
                    break;
                }
                case 6:
                case 7:
                {
                    if (encoded.Length != ((2 * length) + 1))
                    {
                        throw new ArgumentException("Incorrect length for hybrid encoding", "encoded");
                    }
                    BigInteger x = new BigInteger(1, encoded, 1, length);
                    BigInteger y = new BigInteger(1, encoded, 1 + length, length);
                    if (y.TestBit(0) != (num2 == 7))
                    {
                        throw new ArgumentException("Inconsistent Y coordinate in hybrid encoding", "encoded");
                    }
                    infinity = this.ValidatePoint(x, y);
                    break;
                }
                default:
                    throw new FormatException("Invalid point encoding " + num2);
            }
            if ((num2 != 0) && infinity.IsInfinity)
            {
                throw new ArgumentException("Invalid infinity encoding", "encoded");
            }
            return infinity;
        }

        protected abstract ECPoint DecompressPoint(int yTilde, BigInteger X1);
        public virtual bool Equals(ECCurve other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            return ((this.Field.Equals(other.Field) && this.A.ToBigInteger().Equals(other.A.ToBigInteger())) && this.B.ToBigInteger().Equals(other.B.ToBigInteger()));
        }

        public override bool Equals(object obj) => 
            this.Equals(obj as ECCurve);

        public abstract ECFieldElement FromBigInteger(BigInteger x);
        public static int[] GetAllCoordinateSystems() => 
            new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };

        public virtual ECEndomorphism GetEndomorphism() => 
            this.m_endomorphism;

        public override int GetHashCode() => 
            ((this.Field.GetHashCode() ^ Integers.RotateLeft(this.A.ToBigInteger().GetHashCode(), 8)) ^ Integers.RotateLeft(this.B.ToBigInteger().GetHashCode(), 0x10));

        public virtual ECMultiplier GetMultiplier()
        {
            object obj2 = this;
            lock (obj2)
            {
                if (this.m_multiplier == null)
                {
                    this.m_multiplier = this.CreateDefaultMultiplier();
                }
                return this.m_multiplier;
            }
        }

        public virtual PreCompInfo GetPreCompInfo(ECPoint point, string name)
        {
            this.CheckPoint(point);
            object obj2 = point;
            lock (obj2)
            {
                IDictionary preCompTable = point.m_preCompTable;
                return ((preCompTable != null) ? ((PreCompInfo) preCompTable[name]) : null);
            }
        }

        public virtual ECPoint ImportPoint(ECPoint p)
        {
            if (this == p.Curve)
            {
                return p;
            }
            if (p.IsInfinity)
            {
                return this.Infinity;
            }
            p = p.Normalize();
            return this.ValidatePoint(p.XCoord.ToBigInteger(), p.YCoord.ToBigInteger(), p.IsCompressed);
        }

        public abstract bool IsValidFieldElement(BigInteger x);
        public virtual void NormalizeAll(ECPoint[] points)
        {
            this.NormalizeAll(points, 0, points.Length, null);
        }

        public virtual void NormalizeAll(ECPoint[] points, int off, int len, ECFieldElement iso)
        {
            this.CheckPoints(points, off, len);
            switch (this.CoordinateSystem)
            {
                case 0:
                case 5:
                    if (iso != null)
                    {
                        throw new ArgumentException("not valid for affine coordinates", "iso");
                    }
                    return;
            }
            ECFieldElement[] zs = new ECFieldElement[len];
            int[] numArray = new int[len];
            int index = 0;
            for (int i = 0; i < len; i++)
            {
                ECPoint point = points[off + i];
                if ((point != null) && ((iso != null) || !point.IsNormalized()))
                {
                    zs[index] = point.GetZCoord(0);
                    numArray[index++] = off + i;
                }
            }
            if (index != 0)
            {
                ECAlgorithms.MontgomeryTrick(zs, 0, index, iso);
                for (int j = 0; j < index; j++)
                {
                    int num5 = numArray[j];
                    points[num5] = points[num5].Normalize(zs[j]);
                }
            }
        }

        public virtual void SetPreCompInfo(ECPoint point, string name, PreCompInfo preCompInfo)
        {
            this.CheckPoint(point);
            object obj2 = point;
            lock (obj2)
            {
                IDictionary preCompTable = point.m_preCompTable;
                if (preCompTable == null)
                {
                    point.m_preCompTable = preCompTable = Platform.CreateHashtable(4);
                }
                preCompTable[name] = preCompInfo;
            }
        }

        public virtual bool SupportsCoordinateSystem(int coord) => 
            (coord == 0);

        public virtual ECPoint ValidatePoint(BigInteger x, BigInteger y)
        {
            ECPoint point = this.CreatePoint(x, y);
            if (!point.IsValid())
            {
                throw new ArgumentException("Invalid point coordinates");
            }
            return point;
        }

        public virtual ECPoint ValidatePoint(BigInteger x, BigInteger y, bool withCompression)
        {
            ECPoint point = this.CreatePoint(x, y, withCompression);
            if (!point.IsValid())
            {
                throw new ArgumentException("Invalid point coordinates");
            }
            return point;
        }

        public abstract int FieldSize { get; }

        public abstract ECPoint Infinity { get; }

        public virtual IFiniteField Field =>
            this.m_field;

        public virtual ECFieldElement A =>
            this.m_a;

        public virtual ECFieldElement B =>
            this.m_b;

        public virtual BigInteger Order =>
            this.m_order;

        public virtual BigInteger Cofactor =>
            this.m_cofactor;

        public virtual int CoordinateSystem =>
            this.m_coord;

        public class Config
        {
            protected ECCurve outer;
            protected int coord;
            protected ECEndomorphism endomorphism;
            protected ECMultiplier multiplier;

            internal Config(ECCurve outer, int coord, ECEndomorphism endomorphism, ECMultiplier multiplier)
            {
                this.outer = outer;
                this.coord = coord;
                this.endomorphism = endomorphism;
                this.multiplier = multiplier;
            }

            public ECCurve Create()
            {
                if (!this.outer.SupportsCoordinateSystem(this.coord))
                {
                    throw new InvalidOperationException("unsupported coordinate system");
                }
                ECCurve curve = this.outer.CloneCurve();
                if (curve == this.outer)
                {
                    throw new InvalidOperationException("implementation returned current curve");
                }
                curve.m_coord = this.coord;
                curve.m_endomorphism = this.endomorphism;
                curve.m_multiplier = this.multiplier;
                return curve;
            }

            public ECCurve.Config SetCoordinateSystem(int coord)
            {
                this.coord = coord;
                return this;
            }

            public ECCurve.Config SetEndomorphism(ECEndomorphism endomorphism)
            {
                this.endomorphism = endomorphism;
                return this;
            }

            public ECCurve.Config SetMultiplier(ECMultiplier multiplier)
            {
                this.multiplier = multiplier;
                return this;
            }
        }
    }
}

