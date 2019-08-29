namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerVisibleString : DerStringBase
    {
        private readonly string str;

        public DerVisibleString(byte[] str) : this(Strings.FromAsciiByteArray(str))
        {
        }

        public DerVisibleString(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            this.str = str;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerVisibleString str = asn1Object as DerVisibleString;
            if (str == null)
            {
                return false;
            }
            return this.str.Equals(str.str);
        }

        protected override int Asn1GetHashCode() => 
            this.str.GetHashCode();

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(0x1a, this.GetOctets());
        }

        public static DerVisibleString GetInstance(object obj)
        {
            if ((obj == null) || (obj is DerVisibleString))
            {
                return (DerVisibleString) obj;
            }
            if (obj is Asn1OctetString)
            {
                return new DerVisibleString(((Asn1OctetString) obj).GetOctets());
            }
            if (!(obj is Asn1TaggedObject))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return GetInstance(((Asn1TaggedObject) obj).GetObject());
        }

        public static DerVisibleString GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(obj.GetObject());

        public byte[] GetOctets() => 
            Strings.ToAsciiByteArray(this.str);

        public override string GetString() => 
            this.str;
    }
}

