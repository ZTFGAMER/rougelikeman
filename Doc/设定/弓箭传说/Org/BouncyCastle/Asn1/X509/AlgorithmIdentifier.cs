namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class AlgorithmIdentifier : Asn1Encodable
    {
        private readonly DerObjectIdentifier algorithm;
        private readonly Asn1Encodable parameters;

        internal AlgorithmIdentifier(Asn1Sequence seq)
        {
            if ((seq.Count < 1) || (seq.Count > 2))
            {
                throw new ArgumentException("Bad sequence size: " + seq.Count);
            }
            this.algorithm = DerObjectIdentifier.GetInstance(seq[0]);
            this.parameters = (seq.Count >= 2) ? seq[1] : null;
        }

        public AlgorithmIdentifier(DerObjectIdentifier algorithm)
        {
            this.algorithm = algorithm;
        }

        [Obsolete("Use version taking a DerObjectIdentifier")]
        public AlgorithmIdentifier(string algorithm)
        {
            this.algorithm = new DerObjectIdentifier(algorithm);
        }

        public AlgorithmIdentifier(DerObjectIdentifier algorithm, Asn1Encodable parameters)
        {
            this.algorithm = algorithm;
            this.parameters = parameters;
        }

        public static AlgorithmIdentifier GetInstance(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is AlgorithmIdentifier)
            {
                return (AlgorithmIdentifier) obj;
            }
            return new AlgorithmIdentifier(Asn1Sequence.GetInstance(obj));
        }

        public static AlgorithmIdentifier GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object()
        {
            Asn1Encodable[] v = new Asn1Encodable[] { this.algorithm };
            Asn1EncodableVector vector = new Asn1EncodableVector(v);
            Asn1Encodable[] objs = new Asn1Encodable[] { this.parameters };
            vector.AddOptional(objs);
            return new DerSequence(vector);
        }

        public virtual DerObjectIdentifier Algorithm =>
            this.algorithm;

        [Obsolete("Use 'Algorithm' property instead")]
        public virtual DerObjectIdentifier ObjectID =>
            this.algorithm;

        public virtual Asn1Encodable Parameters =>
            this.parameters;
    }
}

