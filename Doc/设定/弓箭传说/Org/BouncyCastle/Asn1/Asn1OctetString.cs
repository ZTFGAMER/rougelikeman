namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.Utilities.Encoders;
    using System;
    using System.IO;

    public abstract class Asn1OctetString : Asn1Object, Asn1OctetStringParser, IAsn1Convertible
    {
        internal byte[] str;

        internal Asn1OctetString(byte[] str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            this.str = str;
        }

        internal Asn1OctetString(Asn1Encodable obj)
        {
            try
            {
                this.str = obj.GetEncoded("DER");
            }
            catch (IOException exception)
            {
                throw new ArgumentException("Error processing object : " + exception.ToString());
            }
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerOctetString str = asn1Object as DerOctetString;
            if (str == null)
            {
                return false;
            }
            return Arrays.AreEqual(this.GetOctets(), str.GetOctets());
        }

        protected override int Asn1GetHashCode() => 
            Arrays.GetHashCode(this.GetOctets());

        public static Asn1OctetString GetInstance(object obj)
        {
            if ((obj == null) || (obj is Asn1OctetString))
            {
                return (Asn1OctetString) obj;
            }
            if (!(obj is Asn1TaggedObject))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return GetInstance(((Asn1TaggedObject) obj).GetObject());
        }

        public static Asn1OctetString GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is Asn1OctetString))
            {
                return BerOctetString.FromSequence(Asn1Sequence.GetInstance(obj2));
            }
            return GetInstance(obj2);
        }

        public virtual byte[] GetOctets() => 
            this.str;

        public Stream GetOctetStream() => 
            new MemoryStream(this.str, false);

        public override string ToString() => 
            ("#" + Hex.ToHexString(this.str));

        public Asn1OctetStringParser Parser =>
            this;
    }
}

