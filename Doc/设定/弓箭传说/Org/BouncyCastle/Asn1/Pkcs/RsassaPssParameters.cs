namespace Org.BouncyCastle.Asn1.Pkcs
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Utilities;
    using System;

    public class RsassaPssParameters : Asn1Encodable
    {
        private AlgorithmIdentifier hashAlgorithm;
        private AlgorithmIdentifier maskGenAlgorithm;
        private DerInteger saltLength;
        private DerInteger trailerField;
        public static readonly AlgorithmIdentifier DefaultHashAlgorithm = new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1, DerNull.Instance);
        public static readonly AlgorithmIdentifier DefaultMaskGenFunction = new AlgorithmIdentifier(PkcsObjectIdentifiers.IdMgf1, DefaultHashAlgorithm);
        public static readonly DerInteger DefaultSaltLength = new DerInteger(20);
        public static readonly DerInteger DefaultTrailerField = new DerInteger(1);

        public RsassaPssParameters()
        {
            this.hashAlgorithm = DefaultHashAlgorithm;
            this.maskGenAlgorithm = DefaultMaskGenFunction;
            this.saltLength = DefaultSaltLength;
            this.trailerField = DefaultTrailerField;
        }

        public RsassaPssParameters(Asn1Sequence seq)
        {
            this.hashAlgorithm = DefaultHashAlgorithm;
            this.maskGenAlgorithm = DefaultMaskGenFunction;
            this.saltLength = DefaultSaltLength;
            this.trailerField = DefaultTrailerField;
            for (int i = 0; i != seq.Count; i++)
            {
                Asn1TaggedObject obj2 = (Asn1TaggedObject) seq[i];
                switch (obj2.TagNo)
                {
                    case 0:
                        this.hashAlgorithm = AlgorithmIdentifier.GetInstance(obj2, true);
                        break;

                    case 1:
                        this.maskGenAlgorithm = AlgorithmIdentifier.GetInstance(obj2, true);
                        break;

                    case 2:
                        this.saltLength = DerInteger.GetInstance(obj2, true);
                        break;

                    case 3:
                        this.trailerField = DerInteger.GetInstance(obj2, true);
                        break;

                    default:
                        throw new ArgumentException("unknown tag");
                }
            }
        }

        public RsassaPssParameters(AlgorithmIdentifier hashAlgorithm, AlgorithmIdentifier maskGenAlgorithm, DerInteger saltLength, DerInteger trailerField)
        {
            this.hashAlgorithm = hashAlgorithm;
            this.maskGenAlgorithm = maskGenAlgorithm;
            this.saltLength = saltLength;
            this.trailerField = trailerField;
        }

        public static RsassaPssParameters GetInstance(object obj)
        {
            if ((obj == null) || (obj is RsassaPssParameters))
            {
                return (RsassaPssParameters) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new RsassaPssParameters((Asn1Sequence) obj);
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            if (!this.hashAlgorithm.Equals(DefaultHashAlgorithm))
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(true, 0, this.hashAlgorithm) };
                v.Add(objs);
            }
            if (!this.maskGenAlgorithm.Equals(DefaultMaskGenFunction))
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(true, 1, this.maskGenAlgorithm) };
                v.Add(objs);
            }
            if (!this.saltLength.Equals(DefaultSaltLength))
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(true, 2, this.saltLength) };
                v.Add(objs);
            }
            if (!this.trailerField.Equals(DefaultTrailerField))
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(true, 3, this.trailerField) };
                v.Add(objs);
            }
            return new DerSequence(v);
        }

        public AlgorithmIdentifier HashAlgorithm =>
            this.hashAlgorithm;

        public AlgorithmIdentifier MaskGenAlgorithm =>
            this.maskGenAlgorithm;

        public DerInteger SaltLength =>
            this.saltLength;

        public DerInteger TrailerField =>
            this.trailerField;
    }
}

