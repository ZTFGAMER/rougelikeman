namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities.IO;
    using System;
    using System.IO;

    public class Asn1InputStream : FilterStream
    {
        private readonly int limit;
        private readonly byte[][] tmpBuffers;

        public Asn1InputStream(Stream inputStream) : this(inputStream, FindLimit(inputStream))
        {
        }

        public Asn1InputStream(byte[] input) : this(new MemoryStream(input, false), input.Length)
        {
        }

        public Asn1InputStream(Stream inputStream, int limit) : base(inputStream)
        {
            this.limit = limit;
            this.tmpBuffers = new byte[0x10][];
        }

        internal virtual Asn1EncodableVector BuildDerEncodableVector(DefiniteLengthInputStream dIn) => 
            new Asn1InputStream(dIn).BuildEncodableVector();

        internal Asn1EncodableVector BuildEncodableVector()
        {
            Asn1Object obj2;
            Asn1EncodableVector vector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            while ((obj2 = this.ReadObject()) != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { obj2 };
                vector.Add(objs);
            }
            return vector;
        }

        private Asn1Object BuildObject(int tag, int tagNo, int length)
        {
            bool isConstructed = (tag & 0x20) != 0;
            DefiniteLengthInputStream inStream = new DefiniteLengthInputStream(base.s, length);
            if ((tag & 0x40) != 0)
            {
                return new DerApplicationSpecific(isConstructed, tagNo, inStream.ToArray());
            }
            if ((tag & 0x80) != 0)
            {
                return new Asn1StreamParser(inStream).ReadTaggedObject(isConstructed, tagNo);
            }
            if (!isConstructed)
            {
                return CreatePrimitiveDerObject(tagNo, inStream, this.tmpBuffers);
            }
            if (tagNo == 0x10)
            {
                return this.CreateDerSequence(inStream);
            }
            if (tagNo == 0x11)
            {
                return this.CreateDerSet(inStream);
            }
            if (tagNo != 4)
            {
                if (tagNo != 8)
                {
                    throw new IOException("unknown tag " + tagNo + " encountered");
                }
                return new DerExternal(this.BuildDerEncodableVector(inStream));
            }
            return new BerOctetString(this.BuildDerEncodableVector(inStream));
        }

        internal virtual DerSequence CreateDerSequence(DefiniteLengthInputStream dIn) => 
            DerSequence.FromVector(this.BuildDerEncodableVector(dIn));

        internal virtual DerSet CreateDerSet(DefiniteLengthInputStream dIn) => 
            DerSet.FromVector(this.BuildDerEncodableVector(dIn), false);

        internal static Asn1Object CreatePrimitiveDerObject(int tagNo, DefiniteLengthInputStream defIn, byte[][] tmpBuffers)
        {
            if (tagNo != 1)
            {
                if (tagNo == 10)
                {
                    return DerEnumerated.FromOctetString(GetBuffer(defIn, tmpBuffers));
                }
                if (tagNo == 6)
                {
                    return DerObjectIdentifier.FromOctetString(GetBuffer(defIn, tmpBuffers));
                }
            }
            else
            {
                return DerBoolean.FromOctetString(GetBuffer(defIn, tmpBuffers));
            }
            byte[] str = defIn.ToArray();
            switch (tagNo)
            {
                case 0x12:
                    return new DerNumericString(str);

                case 0x13:
                    return new DerPrintableString(str);

                case 20:
                    return new DerT61String(str);

                case 0x15:
                    return new DerVideotexString(str);

                case 0x16:
                    return new DerIA5String(str);

                case 0x17:
                    return new DerUtcTime(str);

                case 0x18:
                    return new DerGeneralizedTime(str);

                case 0x19:
                    return new DerGraphicString(str);

                case 0x1a:
                    return new DerVisibleString(str);

                case 0x1b:
                    return new DerGeneralString(str);

                case 0x1c:
                    return new DerUniversalString(str);

                case 30:
                    return new DerBmpString(str);

                case 2:
                    return new DerInteger(str);

                case 3:
                    return DerBitString.FromAsn1Octets(str);

                case 4:
                    return new DerOctetString(str);

                case 5:
                    return DerNull.Instance;

                case 12:
                    return new DerUtf8String(str);
            }
            throw new IOException("unknown tag " + tagNo + " encountered");
        }

        internal static int FindLimit(Stream input)
        {
            if (input is LimitedInputStream)
            {
                return ((LimitedInputStream) input).GetRemaining();
            }
            if (input is MemoryStream)
            {
                MemoryStream stream = (MemoryStream) input;
                return (int) (stream.Length - stream.Position);
            }
            return 0x7fffffff;
        }

        internal static byte[] GetBuffer(DefiniteLengthInputStream defIn, byte[][] tmpBuffers)
        {
            int remaining = defIn.GetRemaining();
            if (remaining >= tmpBuffers.Length)
            {
                return defIn.ToArray();
            }
            byte[] buf = tmpBuffers[remaining];
            if (buf == null)
            {
                buf = tmpBuffers[remaining] = new byte[remaining];
            }
            defIn.ReadAllIntoByteArray(buf);
            return buf;
        }

        internal static int ReadLength(Stream s, int limit)
        {
            int num = s.ReadByte();
            if (num < 0)
            {
                throw new EndOfStreamException("EOF found when length expected");
            }
            if (num == 0x80)
            {
                return -1;
            }
            if (num > 0x7f)
            {
                int num2 = num & 0x7f;
                if (num2 > 4)
                {
                    throw new IOException("DER length more than 4 bytes: " + num2);
                }
                num = 0;
                for (int i = 0; i < num2; i++)
                {
                    int num4 = s.ReadByte();
                    if (num4 < 0)
                    {
                        throw new EndOfStreamException("EOF found reading length");
                    }
                    num = (num << 8) + num4;
                }
                if (num < 0)
                {
                    throw new IOException("Corrupted stream - negative length found");
                }
                if (num >= limit)
                {
                    throw new IOException("Corrupted stream - out of bounds length found");
                }
            }
            return num;
        }

        public Asn1Object ReadObject()
        {
            Asn1Object obj2;
            int tag = this.ReadByte();
            if (tag <= 0)
            {
                if (tag == 0)
                {
                    throw new IOException("unexpected end-of-contents marker");
                }
                return null;
            }
            int num2 = ReadTagNumber(base.s, tag);
            bool flag = (tag & 0x20) != 0;
            int length = ReadLength(base.s, this.limit);
            if (length < 0)
            {
                if (!flag)
                {
                    throw new IOException("indefinite length primitive encoding encountered");
                }
                IndefiniteLengthInputStream inStream = new IndefiniteLengthInputStream(base.s, this.limit);
                Asn1StreamParser parser = new Asn1StreamParser(inStream, this.limit);
                if ((tag & 0x40) != 0)
                {
                    return new BerApplicationSpecificParser(num2, parser).ToAsn1Object();
                }
                if ((tag & 0x80) != 0)
                {
                    return new BerTaggedObjectParser(true, num2, parser).ToAsn1Object();
                }
                if (num2 == 0x10)
                {
                    return new BerSequenceParser(parser).ToAsn1Object();
                }
                if (num2 == 0x11)
                {
                    return new BerSetParser(parser).ToAsn1Object();
                }
                if (num2 == 4)
                {
                    return new BerOctetStringParser(parser).ToAsn1Object();
                }
                if (num2 != 8)
                {
                    throw new IOException("unknown BER object encountered");
                }
                return new DerExternalParser(parser).ToAsn1Object();
            }
            try
            {
                obj2 = this.BuildObject(tag, num2, length);
            }
            catch (ArgumentException exception)
            {
                throw new Asn1Exception("corrupted stream detected", exception);
            }
            return obj2;
        }

        internal static int ReadTagNumber(Stream s, int tag)
        {
            int num = tag & 0x1f;
            if (num != 0x1f)
            {
                return num;
            }
            num = 0;
            int num2 = s.ReadByte();
            if ((num2 & 0x7f) == 0)
            {
                throw new IOException("Corrupted stream - invalid high tag number found");
            }
            while ((num2 >= 0) && ((num2 & 0x80) != 0))
            {
                num |= num2 & 0x7f;
                num = num << 7;
                num2 = s.ReadByte();
            }
            if (num2 < 0)
            {
                throw new EndOfStreamException("EOF found inside tag value.");
            }
            return (num | (num2 & 0x7f));
        }
    }
}

