namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerT61String : DerStringBase
    {
        private readonly string str;

        public DerT61String(byte[] str) : this(Strings.FromByteArray(str))
        {
        }

        public DerT61String(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            this.str = str;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerT61String str = asn1Object as DerT61String;
            if (str == null)
            {
                return false;
            }
            return this.str.Equals(str.str);
        }

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(20, this.GetOctets());
        }

        public static DerT61String GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerT61String))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerT61String) obj;
        }

        public static DerT61String GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerT61String))
            {
                return new DerT61String(Asn1OctetString.GetInstance(obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public byte[] GetOctets() => 
            Strings.ToByteArray(this.str);

        public override string GetString() => 
            this.str;
    }
}

