namespace Org.BouncyCastle.Asn1
{
    using System.IO;

    public interface Asn1OctetStringParser : IAsn1Convertible
    {
        Stream GetOctetStream();
    }
}

