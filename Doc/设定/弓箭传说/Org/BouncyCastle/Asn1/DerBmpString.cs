namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerBmpString : DerStringBase
    {
        private readonly string str;

        public DerBmpString(byte[] str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            char[] chArray = new char[str.Length / 2];
            for (int i = 0; i != chArray.Length; i++)
            {
                chArray[i] = (char) ((str[2 * i] << 8) | (str[(2 * i) + 1] & 0xff));
            }
            this.str = new string(chArray);
        }

        public DerBmpString(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            this.str = str;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerBmpString str = asn1Object as DerBmpString;
            if (str == null)
            {
                return false;
            }
            return this.str.Equals(str.str);
        }

        internal override void Encode(DerOutputStream derOut)
        {
            char[] chArray = this.str.ToCharArray();
            byte[] bytes = new byte[chArray.Length * 2];
            for (int i = 0; i != chArray.Length; i++)
            {
                bytes[2 * i] = (byte) (chArray[i] >> 8);
                bytes[(2 * i) + 1] = (byte) chArray[i];
            }
            derOut.WriteEncoded(30, bytes);
        }

        public static DerBmpString GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerBmpString))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerBmpString) obj;
        }

        public static DerBmpString GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerBmpString))
            {
                return new DerBmpString(Asn1OctetString.GetInstance(obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public override string GetString() => 
            this.str;
    }
}

