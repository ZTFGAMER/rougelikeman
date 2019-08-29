namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class SubjectPublicKeyInfo : Asn1Encodable
    {
        private readonly AlgorithmIdentifier algID;
        private readonly DerBitString keyData;

        private SubjectPublicKeyInfo(Asn1Sequence seq)
        {
            if (seq.Count != 2)
            {
                throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
            }
            this.algID = AlgorithmIdentifier.GetInstance(seq[0]);
            this.keyData = DerBitString.GetInstance(seq[1]);
        }

        public SubjectPublicKeyInfo(AlgorithmIdentifier algID, Asn1Encodable publicKey)
        {
            this.keyData = new DerBitString(publicKey);
            this.algID = algID;
        }

        public SubjectPublicKeyInfo(AlgorithmIdentifier algID, byte[] publicKey)
        {
            this.keyData = new DerBitString(publicKey);
            this.algID = algID;
        }

        public static SubjectPublicKeyInfo GetInstance(object obj)
        {
            if (obj is SubjectPublicKeyInfo)
            {
                return (SubjectPublicKeyInfo) obj;
            }
            if (obj != null)
            {
                return new SubjectPublicKeyInfo(Asn1Sequence.GetInstance(obj));
            }
            return null;
        }

        public static SubjectPublicKeyInfo GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public Asn1Object GetPublicKey() => 
            Asn1Object.FromByteArray(this.keyData.GetOctets());

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { this.algID, this.keyData });

        public AlgorithmIdentifier AlgorithmID =>
            this.algID;

        public DerBitString PublicKeyData =>
            this.keyData;
    }
}

