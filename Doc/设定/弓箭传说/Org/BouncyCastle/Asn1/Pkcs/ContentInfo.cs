namespace Org.BouncyCastle.Asn1.Pkcs
{
    using Org.BouncyCastle.Asn1;
    using System;

    public class ContentInfo : Asn1Encodable
    {
        private readonly DerObjectIdentifier contentType;
        private readonly Asn1Encodable content;

        private ContentInfo(Asn1Sequence seq)
        {
            this.contentType = (DerObjectIdentifier) seq[0];
            if (seq.Count > 1)
            {
                this.content = ((Asn1TaggedObject) seq[1]).GetObject();
            }
        }

        public ContentInfo(DerObjectIdentifier contentType, Asn1Encodable content)
        {
            this.contentType = contentType;
            this.content = content;
        }

        public static ContentInfo GetInstance(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            ContentInfo info = obj as ContentInfo;
            if (info != null)
            {
                return info;
            }
            return new ContentInfo(Asn1Sequence.GetInstance(obj));
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1Encodable[] v = new Asn1Encodable[] { this.contentType };
            Asn1EncodableVector vector = new Asn1EncodableVector(v);
            if (this.content != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new BerTaggedObject(0, this.content) };
                vector.Add(objs);
            }
            return new BerSequence(vector);
        }

        public DerObjectIdentifier ContentType =>
            this.contentType;

        public Asn1Encodable Content =>
            this.content;
    }
}

