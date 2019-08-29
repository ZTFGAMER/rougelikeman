namespace Org.BouncyCastle.Asn1.Ocsp
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;

    public class ResponseBytes : Asn1Encodable
    {
        private readonly DerObjectIdentifier responseType;
        private readonly Asn1OctetString response;

        private ResponseBytes(Asn1Sequence seq)
        {
            if (seq.Count != 2)
            {
                throw new ArgumentException("Wrong number of elements in sequence", "seq");
            }
            this.responseType = DerObjectIdentifier.GetInstance(seq[0]);
            this.response = Asn1OctetString.GetInstance(seq[1]);
        }

        public ResponseBytes(DerObjectIdentifier responseType, Asn1OctetString response)
        {
            if (responseType == null)
            {
                throw new ArgumentNullException("responseType");
            }
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            this.responseType = responseType;
            this.response = response;
        }

        public static ResponseBytes GetInstance(object obj)
        {
            if ((obj == null) || (obj is ResponseBytes))
            {
                return (ResponseBytes) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new ResponseBytes((Asn1Sequence) obj);
        }

        public static ResponseBytes GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { this.responseType, this.response });

        public DerObjectIdentifier ResponseType =>
            this.responseType;

        public Asn1OctetString Response =>
            this.response;
    }
}

