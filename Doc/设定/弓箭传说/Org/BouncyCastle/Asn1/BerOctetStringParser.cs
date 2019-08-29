namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.IO;

    public class BerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
    {
        private readonly Asn1StreamParser _parser;

        internal BerOctetStringParser(Asn1StreamParser parser)
        {
            this._parser = parser;
        }

        public Stream GetOctetStream() => 
            new ConstructedOctetStream(this._parser);

        public Asn1Object ToAsn1Object()
        {
            Asn1Object obj2;
            try
            {
                obj2 = new BerOctetString(Streams.ReadAll(this.GetOctetStream()));
            }
            catch (IOException exception)
            {
                throw new Asn1ParsingException("IOException converting stream to byte array: " + exception.Message, exception);
            }
            return obj2;
        }
    }
}

