namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public class DerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
    {
        private readonly DefiniteLengthInputStream stream;

        internal DerOctetStringParser(DefiniteLengthInputStream stream)
        {
            this.stream = stream;
        }

        public Stream GetOctetStream() => 
            this.stream;

        public Asn1Object ToAsn1Object()
        {
            Asn1Object obj2;
            try
            {
                obj2 = new DerOctetString(this.stream.ToArray());
            }
            catch (IOException exception)
            {
                throw new InvalidOperationException("IOException converting stream to byte array: " + exception.Message, exception);
            }
            return obj2;
        }
    }
}

