namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Math.Field;
    using System;

    public class X9ECParameters : Asn1Encodable
    {
        private X9FieldID fieldID;
        private ECCurve curve;
        private X9ECPoint g;
        private BigInteger n;
        private BigInteger h;
        private byte[] seed;

        public X9ECParameters(Asn1Sequence seq)
        {
            if (!(seq[0] is DerInteger) || !((DerInteger) seq[0]).Value.Equals(BigInteger.One))
            {
                throw new ArgumentException("bad version in X9ECParameters");
            }
            X9Curve curve = new X9Curve(X9FieldID.GetInstance(seq[1]), Asn1Sequence.GetInstance(seq[2]));
            this.curve = curve.Curve;
            object obj2 = seq[3];
            if (obj2 is X9ECPoint)
            {
                this.g = (X9ECPoint) obj2;
            }
            else
            {
                this.g = new X9ECPoint(this.curve, (Asn1OctetString) obj2);
            }
            this.n = ((DerInteger) seq[4]).Value;
            this.seed = curve.GetSeed();
            if (seq.Count == 6)
            {
                this.h = ((DerInteger) seq[5]).Value;
            }
        }

        public X9ECParameters(ECCurve curve, ECPoint g, BigInteger n) : this(curve, g, n, null, null)
        {
        }

        public X9ECParameters(ECCurve curve, X9ECPoint g, BigInteger n, BigInteger h) : this(curve, g, n, h, null)
        {
        }

        public X9ECParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h) : this(curve, g, n, h, null)
        {
        }

        public X9ECParameters(ECCurve curve, X9ECPoint g, BigInteger n, BigInteger h, byte[] seed)
        {
            this.curve = curve;
            this.g = g;
            this.n = n;
            this.h = h;
            this.seed = seed;
            if (ECAlgorithms.IsFpCurve(curve))
            {
                this.fieldID = new X9FieldID(curve.Field.Characteristic);
            }
            else
            {
                if (!ECAlgorithms.IsF2mCurve(curve))
                {
                    throw new ArgumentException("'curve' is of an unsupported type");
                }
                IPolynomialExtensionField field = (IPolynomialExtensionField) curve.Field;
                int[] exponentsPresent = field.MinimalPolynomial.GetExponentsPresent();
                if (exponentsPresent.Length == 3)
                {
                    this.fieldID = new X9FieldID(exponentsPresent[2], exponentsPresent[1]);
                }
                else
                {
                    if (exponentsPresent.Length != 5)
                    {
                        throw new ArgumentException("Only trinomial and pentomial curves are supported");
                    }
                    this.fieldID = new X9FieldID(exponentsPresent[4], exponentsPresent[1], exponentsPresent[2], exponentsPresent[3]);
                }
            }
        }

        public X9ECParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h, byte[] seed) : this(curve, new X9ECPoint(g), n, h, seed)
        {
        }

        public static X9ECParameters GetInstance(object obj)
        {
            if (obj is X9ECParameters)
            {
                return (X9ECParameters) obj;
            }
            if (obj != null)
            {
                return new X9ECParameters(Asn1Sequence.GetInstance(obj));
            }
            return null;
        }

        public byte[] GetSeed() => 
            this.seed;

        public override Asn1Object ToAsn1Object()
        {
            Asn1Encodable[] v = new Asn1Encodable[] { new DerInteger(BigInteger.One), this.fieldID, new X9Curve(this.curve, this.seed), this.g, new DerInteger(this.n) };
            Asn1EncodableVector vector = new Asn1EncodableVector(v);
            if (this.h != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerInteger(this.h) };
                vector.Add(objs);
            }
            return new DerSequence(vector);
        }

        public ECCurve Curve =>
            this.curve;

        public ECPoint G =>
            this.g.Point;

        public BigInteger N =>
            this.n;

        public BigInteger H =>
            this.h;

        public X9Curve CurveEntry =>
            new X9Curve(this.curve, this.seed);

        public X9FieldID FieldIDEntry =>
            this.fieldID;

        public X9ECPoint BaseEntry =>
            this.g;
    }
}

