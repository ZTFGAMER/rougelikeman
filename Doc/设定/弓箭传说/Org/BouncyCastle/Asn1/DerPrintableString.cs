namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerPrintableString : DerStringBase
    {
        private readonly string str;

        public DerPrintableString(byte[] str) : this(Strings.FromAsciiByteArray(str), false)
        {
        }

        public DerPrintableString(string str) : this(str, false)
        {
        }

        public DerPrintableString(string str, bool validate)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (validate && !IsPrintableString(str))
            {
                throw new ArgumentException("string contains illegal characters", "str");
            }
            this.str = str;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerPrintableString str = asn1Object as DerPrintableString;
            if (str == null)
            {
                return false;
            }
            return this.str.Equals(str.str);
        }

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(0x13, this.GetOctets());
        }

        public static DerPrintableString GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerPrintableString))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerPrintableString) obj;
        }

        public static DerPrintableString GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerPrintableString))
            {
                return new DerPrintableString(Asn1OctetString.GetInstance(obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public byte[] GetOctets() => 
            Strings.ToAsciiByteArray(this.str);

        public override string GetString() => 
            this.str;

        public static bool IsPrintableString(string str)
        {
            foreach (char ch in str)
            {
                if (ch > '\x007f')
                {
                    return false;
                }
                if (!char.IsLetterOrDigit(ch))
                {
                    switch (ch)
                    {
                        case '\'':
                        case '(':
                        case ')':
                        case '+':
                        case ',':
                        case '-':
                        case '.':
                        case '/':
                        {
                            continue;
                        }
                        case ':':
                        case '=':
                        case '?':
                        {
                            continue;
                        }
                    }
                    if (ch != ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

