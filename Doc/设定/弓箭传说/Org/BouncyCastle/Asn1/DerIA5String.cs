namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerIA5String : DerStringBase
    {
        private readonly string str;

        public DerIA5String(byte[] str) : this(Strings.FromAsciiByteArray(str), false)
        {
        }

        public DerIA5String(string str) : this(str, false)
        {
        }

        public DerIA5String(string str, bool validate)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (validate && !IsIA5String(str))
            {
                throw new ArgumentException("string contains illegal characters", "str");
            }
            this.str = str;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerIA5String str = asn1Object as DerIA5String;
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
            derOut.WriteEncoded(0x16, this.GetOctets());
        }

        public static DerIA5String GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerIA5String))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerIA5String) obj;
        }

        public static DerIA5String GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerIA5String))
            {
                return new DerIA5String(((Asn1OctetString) obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public byte[] GetOctets() => 
            Strings.ToAsciiByteArray(this.str);

        public override string GetString() => 
            this.str;

        public static bool IsIA5String(string str)
        {
            foreach (char ch in str)
            {
                if (ch > '\x007f')
                {
                    return false;
                }
            }
            return true;
        }
    }
}

