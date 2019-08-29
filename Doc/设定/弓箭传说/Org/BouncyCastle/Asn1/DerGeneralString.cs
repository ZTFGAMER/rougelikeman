namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerGeneralString : DerStringBase
    {
        private readonly string str;

        public DerGeneralString(byte[] str) : this(Strings.FromAsciiByteArray(str))
        {
        }

        public DerGeneralString(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            this.str = str;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerGeneralString str = asn1Object as DerGeneralString;
            if (str == null)
            {
                return false;
            }
            return this.str.Equals(str.str);
        }

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(0x1b, this.GetOctets());
        }

        public static DerGeneralString GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerGeneralString))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerGeneralString) obj;
        }

        public static DerGeneralString GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerGeneralString))
            {
                return new DerGeneralString(((Asn1OctetString) obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public byte[] GetOctets() => 
            Strings.ToAsciiByteArray(this.str);

        public override string GetString() => 
            this.str;
    }
}

