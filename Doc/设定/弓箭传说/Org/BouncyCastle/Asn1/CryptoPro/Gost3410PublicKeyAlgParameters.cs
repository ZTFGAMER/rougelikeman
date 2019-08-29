namespace Org.BouncyCastle.Asn1.CryptoPro
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Gost3410PublicKeyAlgParameters : Asn1Encodable
    {
        private DerObjectIdentifier publicKeyParamSet;
        private DerObjectIdentifier digestParamSet;
        private DerObjectIdentifier encryptionParamSet;

        public Gost3410PublicKeyAlgParameters(Asn1Sequence seq)
        {
            this.publicKeyParamSet = (DerObjectIdentifier) seq[0];
            this.digestParamSet = (DerObjectIdentifier) seq[1];
            if (seq.Count > 2)
            {
                this.encryptionParamSet = (DerObjectIdentifier) seq[2];
            }
        }

        public Gost3410PublicKeyAlgParameters(DerObjectIdentifier publicKeyParamSet, DerObjectIdentifier digestParamSet) : this(publicKeyParamSet, digestParamSet, null)
        {
        }

        public Gost3410PublicKeyAlgParameters(DerObjectIdentifier publicKeyParamSet, DerObjectIdentifier digestParamSet, DerObjectIdentifier encryptionParamSet)
        {
            if (publicKeyParamSet == null)
            {
                throw new ArgumentNullException("publicKeyParamSet");
            }
            if (digestParamSet == null)
            {
                throw new ArgumentNullException("digestParamSet");
            }
            this.publicKeyParamSet = publicKeyParamSet;
            this.digestParamSet = digestParamSet;
            this.encryptionParamSet = encryptionParamSet;
        }

        public static Gost3410PublicKeyAlgParameters GetInstance(object obj)
        {
            if ((obj == null) || (obj is Gost3410PublicKeyAlgParameters))
            {
                return (Gost3410PublicKeyAlgParameters) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("Invalid GOST3410Parameter: " + Platform.GetTypeName(obj));
            }
            return new Gost3410PublicKeyAlgParameters((Asn1Sequence) obj);
        }

        public static Gost3410PublicKeyAlgParameters GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object()
        {
            Asn1Encodable[] v = new Asn1Encodable[] { this.publicKeyParamSet, this.digestParamSet };
            Asn1EncodableVector vector = new Asn1EncodableVector(v);
            if (this.encryptionParamSet != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { this.encryptionParamSet };
                vector.Add(objs);
            }
            return new DerSequence(vector);
        }

        public DerObjectIdentifier PublicKeyParamSet =>
            this.publicKeyParamSet;

        public DerObjectIdentifier DigestParamSet =>
            this.digestParamSet;

        public DerObjectIdentifier EncryptionParamSet =>
            this.encryptionParamSet;
    }
}

