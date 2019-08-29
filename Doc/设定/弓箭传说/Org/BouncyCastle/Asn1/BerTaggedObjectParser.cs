namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public class BerTaggedObjectParser : Asn1TaggedObjectParser, IAsn1Convertible
    {
        private bool _constructed;
        private int _tagNumber;
        private Asn1StreamParser _parser;

        internal BerTaggedObjectParser(bool constructed, int tagNumber, Asn1StreamParser parser)
        {
            this._constructed = constructed;
            this._tagNumber = tagNumber;
            this._parser = parser;
        }

        [Obsolete]
        internal BerTaggedObjectParser(int baseTag, int tagNumber, Stream contentStream) : this((baseTag & 0x20) != 0, tagNumber, new Asn1StreamParser(contentStream))
        {
        }

        public IAsn1Convertible GetObjectParser(int tag, bool isExplicit)
        {
            if (!isExplicit)
            {
                return this._parser.ReadImplicit(this._constructed, tag);
            }
            if (!this._constructed)
            {
                throw new IOException("Explicit tags must be constructed (see X.690 8.14.2)");
            }
            return this._parser.ReadObject();
        }

        public Asn1Object ToAsn1Object()
        {
            Asn1Object obj2;
            try
            {
                obj2 = this._parser.ReadTaggedObject(this._constructed, this._tagNumber);
            }
            catch (IOException exception)
            {
                throw new Asn1ParsingException(exception.Message);
            }
            return obj2;
        }

        public bool IsConstructed =>
            this._constructed;

        public int TagNo =>
            this._tagNumber;
    }
}

