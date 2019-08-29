namespace Org.BouncyCastle.Asn1
{
    using System;

    public interface Asn1TaggedObjectParser : IAsn1Convertible
    {
        IAsn1Convertible GetObjectParser(int tag, bool isExplicit);

        int TagNo { get; }
    }
}

