namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public abstract class Asn1Encodable : IAsn1Convertible
    {
        public const string Der = "DER";
        public const string Ber = "BER";

        protected Asn1Encodable()
        {
        }

        public sealed override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            IAsn1Convertible convertible = obj as IAsn1Convertible;
            if (convertible == null)
            {
                return false;
            }
            Asn1Object obj2 = this.ToAsn1Object();
            Asn1Object obj3 = convertible.ToAsn1Object();
            return ((obj2 == obj3) || obj2.CallAsn1Equals(obj3));
        }

        public byte[] GetDerEncoded()
        {
            try
            {
                return this.GetEncoded("DER");
            }
            catch (IOException)
            {
                return null;
            }
        }

        public byte[] GetEncoded()
        {
            MemoryStream os = new MemoryStream();
            new Asn1OutputStream(os).WriteObject(this);
            return os.ToArray();
        }

        public byte[] GetEncoded(string encoding)
        {
            if (encoding.Equals("DER"))
            {
                MemoryStream os = new MemoryStream();
                new DerOutputStream(os).WriteObject(this);
                return os.ToArray();
            }
            return this.GetEncoded();
        }

        public sealed override int GetHashCode() => 
            this.ToAsn1Object().CallAsn1GetHashCode();

        public abstract Asn1Object ToAsn1Object();
    }
}

