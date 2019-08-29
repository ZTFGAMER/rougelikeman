namespace Org.BouncyCastle.Asn1
{
    using System;

    public class BerApplicationSpecific : DerApplicationSpecific
    {
        public BerApplicationSpecific(int tagNo, Asn1EncodableVector vec) : base(tagNo, vec)
        {
        }
    }
}

