namespace Org.BouncyCastle.Asn1
{
    using System;

    public class DerTaggedObject : Asn1TaggedObject
    {
        public DerTaggedObject(int tagNo) : base(false, tagNo, DerSequence.Empty)
        {
        }

        public DerTaggedObject(int tagNo, Asn1Encodable obj) : base(tagNo, obj)
        {
        }

        public DerTaggedObject(bool explicitly, int tagNo, Asn1Encodable obj) : base(explicitly, tagNo, obj)
        {
        }

        internal override void Encode(DerOutputStream derOut)
        {
            if (!base.IsEmpty())
            {
                byte[] derEncoded = base.obj.GetDerEncoded();
                if (base.explicitly)
                {
                    derOut.WriteEncoded(160, base.tagNo, derEncoded);
                }
                else
                {
                    int flags = (derEncoded[0] & 0x20) | 0x80;
                    derOut.WriteTag(flags, base.tagNo);
                    derOut.Write(derEncoded, 1, derEncoded.Length - 1);
                }
            }
            else
            {
                derOut.WriteEncoded(160, base.tagNo, new byte[0]);
            }
        }
    }
}

