namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using System;
    using System.Collections;

    public class CertificateList : Asn1Encodable
    {
        private readonly TbsCertificateList tbsCertList;
        private readonly AlgorithmIdentifier sigAlgID;
        private readonly DerBitString sig;

        private CertificateList(Asn1Sequence seq)
        {
            if (seq.Count != 3)
            {
                throw new ArgumentException("sequence wrong size for CertificateList", "seq");
            }
            this.tbsCertList = TbsCertificateList.GetInstance(seq[0]);
            this.sigAlgID = AlgorithmIdentifier.GetInstance(seq[1]);
            this.sig = DerBitString.GetInstance(seq[2]);
        }

        public static CertificateList GetInstance(object obj)
        {
            if (obj is CertificateList)
            {
                return (CertificateList) obj;
            }
            if (obj != null)
            {
                return new CertificateList(Asn1Sequence.GetInstance(obj));
            }
            return null;
        }

        public static CertificateList GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public IEnumerable GetRevokedCertificateEnumeration() => 
            this.tbsCertList.GetRevokedCertificateEnumeration();

        public CrlEntry[] GetRevokedCertificates() => 
            this.tbsCertList.GetRevokedCertificates();

        public byte[] GetSignatureOctets() => 
            this.sig.GetOctets();

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { this.tbsCertList, this.sigAlgID, this.sig });

        public TbsCertificateList TbsCertList =>
            this.tbsCertList;

        public AlgorithmIdentifier SignatureAlgorithm =>
            this.sigAlgID;

        public DerBitString Signature =>
            this.sig;

        public int Version =>
            this.tbsCertList.Version;

        public X509Name Issuer =>
            this.tbsCertList.Issuer;

        public Time ThisUpdate =>
            this.tbsCertList.ThisUpdate;

        public Time NextUpdate =>
            this.tbsCertList.NextUpdate;
    }
}

