namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public abstract class Asn1Object : Asn1Encodable
    {
        protected Asn1Object()
        {
        }

        protected abstract bool Asn1Equals(Asn1Object asn1Object);
        protected abstract int Asn1GetHashCode();
        internal bool CallAsn1Equals(Asn1Object obj) => 
            this.Asn1Equals(obj);

        internal int CallAsn1GetHashCode() => 
            this.Asn1GetHashCode();

        internal abstract void Encode(DerOutputStream derOut);
        public static Asn1Object FromByteArray(byte[] data)
        {
            Asn1Object obj3;
            try
            {
                MemoryStream inputStream = new MemoryStream(data, false);
                Asn1Object obj2 = new Asn1InputStream(inputStream, data.Length).ReadObject();
                if (inputStream.Position != inputStream.Length)
                {
                    throw new IOException("extra data found after object");
                }
                obj3 = obj2;
            }
            catch (InvalidCastException)
            {
                throw new IOException("cannot recognise object in byte array");
            }
            return obj3;
        }

        public static Asn1Object FromStream(Stream inStr)
        {
            Asn1Object obj2;
            try
            {
                obj2 = new Asn1InputStream(inStr).ReadObject();
            }
            catch (InvalidCastException)
            {
                throw new IOException("cannot recognise object in stream");
            }
            return obj2;
        }

        public sealed override Asn1Object ToAsn1Object() => 
            this;
    }
}

