namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerNumericString : DerStringBase
    {
        private readonly string str;

        public DerNumericString(byte[] str) : this(Strings.FromAsciiByteArray(str), false)
        {
        }

        public DerNumericString(string str) : this(str, false)
        {
        }

        public DerNumericString(string str, bool validate)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (validate && !IsNumericString(str))
            {
                throw new ArgumentException("string contains illegal characters", "str");
            }
            this.str = str;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerNumericString str = asn1Object as DerNumericString;
            if (str == null)
            {
                return false;
            }
            return this.str.Equals(str.str);
        }

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(0x12, this.GetOctets());
        }

        public static DerNumericString GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerNumericString))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerNumericString) obj;
        }

        public static DerNumericString GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerNumericString))
            {
                return new DerNumericString(Asn1OctetString.GetInstance(obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public byte[] GetOctets() => 
            Strings.ToAsciiByteArray(this.str);

        public override string GetString() => 
            this.str;

        public static bool IsNumericString(string str)
        {
            foreach (char ch in str)
            {
                if ((ch > '\x007f') || ((ch != ' ') && !char.IsDigit(ch)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

