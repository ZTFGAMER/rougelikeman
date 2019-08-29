namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    public class DerUtf8String : DerStringBase
    {
        private readonly string str;

        public DerUtf8String(byte[] str) : this(Encoding.UTF8.GetString(str, 0, str.Length))
        {
        }

        public DerUtf8String(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            this.str = str;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerUtf8String str = asn1Object as DerUtf8String;
            if (str == null)
            {
                return false;
            }
            return this.str.Equals(str.str);
        }

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(12, Encoding.UTF8.GetBytes(this.str));
        }

        public static DerUtf8String GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerUtf8String))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerUtf8String) obj;
        }

        public static DerUtf8String GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerUtf8String))
            {
                return new DerUtf8String(Asn1OctetString.GetInstance(obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public override string GetString() => 
            this.str;
    }
}

