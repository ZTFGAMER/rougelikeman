namespace Org.BouncyCastle.Asn1.Ocsp
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;

    public class OcspResponse : Asn1Encodable
    {
        private readonly OcspResponseStatus responseStatus;
        private readonly Org.BouncyCastle.Asn1.Ocsp.ResponseBytes responseBytes;

        private OcspResponse(Asn1Sequence seq)
        {
            this.responseStatus = new OcspResponseStatus(DerEnumerated.GetInstance(seq[0]));
            if (seq.Count == 2)
            {
                this.responseBytes = Org.BouncyCastle.Asn1.Ocsp.ResponseBytes.GetInstance((Asn1TaggedObject) seq[1], true);
            }
        }

        public OcspResponse(OcspResponseStatus responseStatus, Org.BouncyCastle.Asn1.Ocsp.ResponseBytes responseBytes)
        {
            if (responseStatus == null)
            {
                throw new ArgumentNullException("responseStatus");
            }
            this.responseStatus = responseStatus;
            this.responseBytes = responseBytes;
        }

        public static OcspResponse GetInstance(object obj)
        {
            if ((obj == null) || (obj is OcspResponse))
            {
                return (OcspResponse) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new OcspResponse((Asn1Sequence) obj);
        }

        public static OcspResponse GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object()
        {
            Asn1Encodable[] v = new Asn1Encodable[] { this.responseStatus };
            Asn1EncodableVector vector = new Asn1EncodableVector(v);
            if (this.responseBytes != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(true, 0, this.responseBytes) };
                vector.Add(objs);
            }
            return new DerSequence(vector);
        }

        public OcspResponseStatus ResponseStatus =>
            this.responseStatus;

        public Org.BouncyCastle.Asn1.Ocsp.ResponseBytes ResponseBytes =>
            this.responseBytes;
    }
}

