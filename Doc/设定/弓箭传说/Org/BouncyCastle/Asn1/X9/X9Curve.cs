namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities;
    using System;

    public class X9Curve : Asn1Encodable
    {
        private readonly ECCurve curve;
        private readonly byte[] seed;
        private readonly DerObjectIdentifier fieldIdentifier;

        public X9Curve(ECCurve curve) : this(curve, null)
        {
        }

        public X9Curve(X9FieldID fieldID, Asn1Sequence seq)
        {
            if (fieldID == null)
            {
                throw new ArgumentNullException("fieldID");
            }
            if (seq == null)
            {
                throw new ArgumentNullException("seq");
            }
            this.fieldIdentifier = fieldID.Identifier;
            if (this.fieldIdentifier.Equals(X9ObjectIdentifiers.PrimeField))
            {
                BigInteger p = ((DerInteger) fieldID.Parameters).Value;
                X9FieldElement element = new X9FieldElement(p, (Asn1OctetString) seq[0]);
                X9FieldElement element2 = new X9FieldElement(p, (Asn1OctetString) seq[1]);
                this.curve = new FpCurve(p, element.Value.ToBigInteger(), element2.Value.ToBigInteger());
            }
            else if (this.fieldIdentifier.Equals(X9ObjectIdentifiers.CharacteristicTwoField))
            {
                DerSequence parameters = (DerSequence) fieldID.Parameters;
                int intValue = ((DerInteger) parameters[0]).Value.IntValue;
                DerObjectIdentifier identifier = (DerObjectIdentifier) parameters[1];
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                if (identifier.Equals(X9ObjectIdentifiers.TPBasis))
                {
                    num2 = ((DerInteger) parameters[2]).Value.IntValue;
                }
                else
                {
                    DerSequence sequence2 = (DerSequence) parameters[2];
                    num2 = ((DerInteger) sequence2[0]).Value.IntValue;
                    num3 = ((DerInteger) sequence2[1]).Value.IntValue;
                    num4 = ((DerInteger) sequence2[2]).Value.IntValue;
                }
                X9FieldElement element3 = new X9FieldElement(intValue, num2, num3, num4, (Asn1OctetString) seq[0]);
                X9FieldElement element4 = new X9FieldElement(intValue, num2, num3, num4, (Asn1OctetString) seq[1]);
                this.curve = new F2mCurve(intValue, num2, num3, num4, element3.Value.ToBigInteger(), element4.Value.ToBigInteger());
            }
            if (seq.Count == 3)
            {
                this.seed = ((DerBitString) seq[2]).GetBytes();
            }
        }

        public X9Curve(ECCurve curve, byte[] seed)
        {
            if (curve == null)
            {
                throw new ArgumentNullException("curve");
            }
            this.curve = curve;
            this.seed = Arrays.Clone(seed);
            if (ECAlgorithms.IsFpCurve(curve))
            {
                this.fieldIdentifier = X9ObjectIdentifiers.PrimeField;
            }
            else
            {
                if (!ECAlgorithms.IsF2mCurve(curve))
                {
                    throw new ArgumentException("This type of ECCurve is not implemented");
                }
                this.fieldIdentifier = X9ObjectIdentifiers.CharacteristicTwoField;
            }
        }

        public byte[] GetSeed() => 
            Arrays.Clone(this.seed);

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            if (this.fieldIdentifier.Equals(X9ObjectIdentifiers.PrimeField) || this.fieldIdentifier.Equals(X9ObjectIdentifiers.CharacteristicTwoField))
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new X9FieldElement(this.curve.A).ToAsn1Object() };
                v.Add(objs);
                Asn1Encodable[] encodableArray2 = new Asn1Encodable[] { new X9FieldElement(this.curve.B).ToAsn1Object() };
                v.Add(encodableArray2);
            }
            if (this.seed != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerBitString(this.seed) };
                v.Add(objs);
            }
            return new DerSequence(v);
        }

        public ECCurve Curve =>
            this.curve;
    }
}

