namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class X509CertificateStructure : Asn1Encodable
    {
        private readonly TbsCertificateStructure tbsCert;
        private readonly AlgorithmIdentifier sigAlgID;
        private readonly DerBitString sig;

        private X509CertificateStructure(Asn1Sequence seq)
        {
            if (seq.Count != 3)
            {
                throw new ArgumentException("sequence wrong size for a certificate", "seq");
            }
            this.tbsCert = TbsCertificateStructure.GetInstance(seq[0]);
            this.sigAlgID = AlgorithmIdentifier.GetInstance(seq[1]);
            this.sig = DerBitString.GetInstance(seq[2]);
        }

        public X509CertificateStructure(TbsCertificateStructure tbsCert, AlgorithmIdentifier sigAlgID, DerBitString sig)
        {
            if (tbsCert == null)
            {
                throw new ArgumentNullException("tbsCert");
            }
            if (sigAlgID == null)
            {
                throw new ArgumentNullException("sigAlgID");
            }
            if (sig == null)
            {
                throw new ArgumentNullException("sig");
            }
            this.tbsCert = tbsCert;
            this.sigAlgID = sigAlgID;
            this.sig = sig;
        }

        public static X509CertificateStructure GetInstance(object obj)
        {
            if (obj is X509CertificateStructure)
            {
                return (X509CertificateStructure) obj;
            }
            if (obj == null)
            {
                return null;
            }
            return new X509CertificateStructure(Asn1Sequence.GetInstance(obj));
        }

        public static X509CertificateStructure GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public byte[] GetSignatureOctets() => 
            this.sig.GetOctets();

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { this.tbsCert, this.sigAlgID, this.sig });

        public TbsCertificateStructure TbsCertificate =>
            this.tbsCert;

        public int Version =>
            this.tbsCert.Version;

        public DerInteger SerialNumber =>
            this.tbsCert.SerialNumber;

        public X509Name Issuer =>
            this.tbsCert.Issuer;

        public Time StartDate =>
            this.tbsCert.StartDate;

        public Time EndDate =>
            this.tbsCert.EndDate;

        public X509Name Subject =>
            this.tbsCert.Subject;

        public Org.BouncyCastle.Asn1.X509.SubjectPublicKeyInfo SubjectPublicKeyInfo =>
            this.tbsCert.SubjectPublicKeyInfo;

        public AlgorithmIdentifier SignatureAlgorithm =>
            this.sigAlgID;

        public DerBitString Signature =>
            this.sig;
    }
}

