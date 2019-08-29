namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public class Asn1StreamParser
    {
        private readonly Stream _in;
        private readonly int _limit;
        private readonly byte[][] tmpBuffers;

        public Asn1StreamParser(Stream inStream) : this(inStream, Asn1InputStream.FindLimit(inStream))
        {
        }

        public Asn1StreamParser(byte[] encoding) : this(new MemoryStream(encoding, false), encoding.Length)
        {
        }

        public Asn1StreamParser(Stream inStream, int limit)
        {
            if (!inStream.CanRead)
            {
                throw new ArgumentException("Expected stream to be readable", "inStream");
            }
            this._in = inStream;
            this._limit = limit;
            this.tmpBuffers = new byte[0x10][];
        }

        internal IAsn1Convertible ReadImplicit(bool constructed, int tag)
        {
            if (this._in is IndefiniteLengthInputStream)
            {
                if (!constructed)
                {
                    throw new IOException("indefinite length primitive encoding encountered");
                }
                return this.ReadIndef(tag);
            }
            if (constructed)
            {
                if (tag == 0x11)
                {
                    return new DerSetParser(this);
                }
                if (tag == 0x10)
                {
                    return new DerSequenceParser(this);
                }
                if (tag == 4)
                {
                    return new BerOctetStringParser(this);
                }
            }
            else
            {
                if (tag == 0x11)
                {
                    throw new Asn1Exception("sequences must use constructed encoding (see X.690 8.9.1/8.10.1)");
                }
                if (tag == 0x10)
                {
                    throw new Asn1Exception("sets must use constructed encoding (see X.690 8.11.1/8.12.1)");
                }
                if (tag == 4)
                {
                    return new DerOctetStringParser((DefiniteLengthInputStream) this._in);
                }
            }
            throw new Asn1Exception("implicit tagging not implemented");
        }

        internal IAsn1Convertible ReadIndef(int tagValue)
        {
            if (tagValue == 0x10)
            {
                return new BerSequenceParser(this);
            }
            if (tagValue == 0x11)
            {
                return new BerSetParser(this);
            }
            if (tagValue == 4)
            {
                return new BerOctetStringParser(this);
            }
            if (tagValue != 8)
            {
                throw new Asn1Exception("unknown BER object encountered: 0x" + tagValue.ToString("X"));
            }
            return new DerExternalParser(this);
        }

        public virtual IAsn1Convertible ReadObject()
        {
            IAsn1Convertible convertible;
            int tag = this._in.ReadByte();
            if (tag == -1)
            {
                return null;
            }
            this.Set00Check(false);
            int num2 = Asn1InputStream.ReadTagNumber(this._in, tag);
            bool isConstructed = (tag & 0x20) != 0;
            int length = Asn1InputStream.ReadLength(this._in, this._limit);
            if (length < 0)
            {
                if (!isConstructed)
                {
                    throw new IOException("indefinite length primitive encoding encountered");
                }
                IndefiniteLengthInputStream stream = new IndefiniteLengthInputStream(this._in, this._limit);
                Asn1StreamParser parser = new Asn1StreamParser(stream, this._limit);
                if ((tag & 0x40) != 0)
                {
                    return new BerApplicationSpecificParser(num2, parser);
                }
                if ((tag & 0x80) != 0)
                {
                    return new BerTaggedObjectParser(true, num2, parser);
                }
                return parser.ReadIndef(num2);
            }
            DefiniteLengthInputStream inStream = new DefiniteLengthInputStream(this._in, length);
            if ((tag & 0x40) != 0)
            {
                return new DerApplicationSpecific(isConstructed, num2, inStream.ToArray());
            }
            if ((tag & 0x80) != 0)
            {
                return new BerTaggedObjectParser(isConstructed, num2, new Asn1StreamParser(inStream));
            }
            if (isConstructed)
            {
                switch (num2)
                {
                    case 0x10:
                        return new DerSequenceParser(new Asn1StreamParser(inStream));

                    case 0x11:
                        return new DerSetParser(new Asn1StreamParser(inStream));

                    case 4:
                        return new BerOctetStringParser(new Asn1StreamParser(inStream));

                    case 8:
                        return new DerExternalParser(new Asn1StreamParser(inStream));
                }
                throw new IOException("unknown tag " + num2 + " encountered");
            }
            if (num2 == 4)
            {
                return new DerOctetStringParser(inStream);
            }
            try
            {
                convertible = Asn1InputStream.CreatePrimitiveDerObject(num2, inStream, this.tmpBuffers);
            }
            catch (ArgumentException exception)
            {
                throw new Asn1Exception("corrupted stream detected", exception);
            }
            return convertible;
        }

        internal Asn1Object ReadTaggedObject(bool constructed, int tag)
        {
            if (!constructed)
            {
                DefiniteLengthInputStream stream = (DefiniteLengthInputStream) this._in;
                return new DerTaggedObject(false, tag, new DerOctetString(stream.ToArray()));
            }
            Asn1EncodableVector v = this.ReadVector();
            if (this._in is IndefiniteLengthInputStream)
            {
                return ((v.Count != 1) ? new BerTaggedObject(false, tag, BerSequence.FromVector(v)) : new BerTaggedObject(true, tag, v[0]));
            }
            return ((v.Count != 1) ? new DerTaggedObject(false, tag, DerSequence.FromVector(v)) : new DerTaggedObject(true, tag, v[0]));
        }

        internal Asn1EncodableVector ReadVector()
        {
            IAsn1Convertible convertible;
            Asn1EncodableVector vector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            while ((convertible = this.ReadObject()) != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { convertible.ToAsn1Object() };
                vector.Add(objs);
            }
            return vector;
        }

        private void Set00Check(bool enabled)
        {
            if (this._in is IndefiniteLengthInputStream)
            {
                ((IndefiniteLengthInputStream) this._in).SetEofOn00(enabled);
            }
        }
    }
}

