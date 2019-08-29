namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DigestInfo : Asn1Encodable
    {
        private readonly byte[] digest;
        private readonly AlgorithmIdentifier algID;

        private DigestInfo(Asn1Sequence seq)
        {
            if (seq.Count != 2)
            {
                throw new ArgumentException("Wrong number of elements in sequence", "seq");
            }
            this.algID = AlgorithmIdentifier.GetInstance(seq[0]);
            this.digest = Asn1OctetString.GetInstance(seq[1]).GetOctets();
        }

        public DigestInfo(AlgorithmIdentifier algID, byte[] digest)
        {
            this.digest = digest;
            this.algID = algID;
        }

        public byte[] GetDigest() => 
            this.digest;

        public static DigestInfo GetInstance(object obj)
        {
            if (obj is DigestInfo)
            {
                return (DigestInfo) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new DigestInfo((Asn1Sequence) obj);
        }

        public static DigestInfo GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { this.algID, new DerOctetString(this.digest) });

        public AlgorithmIdentifier AlgorithmID =>
            this.algID;
    }
}

