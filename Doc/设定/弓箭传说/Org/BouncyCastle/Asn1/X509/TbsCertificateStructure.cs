namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class TbsCertificateStructure : Asn1Encodable
    {
        internal Asn1Sequence seq;
        internal DerInteger version;
        internal DerInteger serialNumber;
        internal AlgorithmIdentifier signature;
        internal X509Name issuer;
        internal Time startDate;
        internal Time endDate;
        internal X509Name subject;
        internal Org.BouncyCastle.Asn1.X509.SubjectPublicKeyInfo subjectPublicKeyInfo;
        internal DerBitString issuerUniqueID;
        internal DerBitString subjectUniqueID;
        internal X509Extensions extensions;

        internal TbsCertificateStructure(Asn1Sequence seq)
        {
            int num = 0;
            this.seq = seq;
            if (seq[0] is DerTaggedObject)
            {
                this.version = DerInteger.GetInstance((Asn1TaggedObject) seq[0], true);
            }
            else
            {
                num = -1;
                this.version = new DerInteger(0);
            }
            this.serialNumber = DerInteger.GetInstance(seq[num + 1]);
            this.signature = AlgorithmIdentifier.GetInstance(seq[num + 2]);
            this.issuer = X509Name.GetInstance(seq[num + 3]);
            Asn1Sequence sequence = (Asn1Sequence) seq[num + 4];
            this.startDate = Time.GetInstance(sequence[0]);
            this.endDate = Time.GetInstance(sequence[1]);
            this.subject = X509Name.GetInstance(seq[num + 5]);
            this.subjectPublicKeyInfo = Org.BouncyCastle.Asn1.X509.SubjectPublicKeyInfo.GetInstance(seq[num + 6]);
            for (int i = (seq.Count - (num + 6)) - 1; i > 0; i--)
            {
                DerTaggedObject obj2 = (DerTaggedObject) seq[(num + 6) + i];
                switch (obj2.TagNo)
                {
                    case 1:
                        this.issuerUniqueID = DerBitString.GetInstance(obj2, false);
                        break;

                    case 2:
                        this.subjectUniqueID = DerBitString.GetInstance(obj2, false);
                        break;

                    case 3:
                        this.extensions = X509Extensions.GetInstance(obj2);
                        break;
                }
            }
        }

        public static TbsCertificateStructure GetInstance(object obj)
        {
            if (obj is TbsCertificateStructure)
            {
                return (TbsCertificateStructure) obj;
            }
            if (obj != null)
            {
                return new TbsCertificateStructure(Asn1Sequence.GetInstance(obj));
            }
            return null;
        }

        public static TbsCertificateStructure GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object() => 
            this.seq;

        public int Version =>
            (this.version.Value.IntValue + 1);

        public DerInteger VersionNumber =>
            this.version;

        public DerInteger SerialNumber =>
            this.serialNumber;

        public AlgorithmIdentifier Signature =>
            this.signature;

        public X509Name Issuer =>
            this.issuer;

        public Time StartDate =>
            this.startDate;

        public Time EndDate =>
            this.endDate;

        public X509Name Subject =>
            this.subject;

        public Org.BouncyCastle.Asn1.X509.SubjectPublicKeyInfo SubjectPublicKeyInfo =>
            this.subjectPublicKeyInfo;

        public DerBitString IssuerUniqueID =>
            this.issuerUniqueID;

        public DerBitString SubjectUniqueID =>
            this.subjectUniqueID;

        public X509Extensions Extensions =>
            this.extensions;
    }
}

