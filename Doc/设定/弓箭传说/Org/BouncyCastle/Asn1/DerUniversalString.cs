namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    public class DerUniversalString : DerStringBase
    {
        private static readonly char[] table = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        private readonly byte[] str;

        public DerUniversalString(byte[] str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            this.str = str;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerUniversalString str = asn1Object as DerUniversalString;
            if (str == null)
            {
                return false;
            }
            return Arrays.AreEqual(this.str, str.str);
        }

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(0x1c, this.str);
        }

        public static DerUniversalString GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerUniversalString))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerUniversalString) obj;
        }

        public static DerUniversalString GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerUniversalString))
            {
                return new DerUniversalString(Asn1OctetString.GetInstance(obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public byte[] GetOctets() => 
            ((byte[]) this.str.Clone());

        public override string GetString()
        {
            StringBuilder builder = new StringBuilder("#");
            byte[] derEncoded = base.GetDerEncoded();
            for (int i = 0; i != derEncoded.Length; i++)
            {
                uint num2 = derEncoded[i];
                builder.Append(table[(int) ((IntPtr) ((num2 >> 4) & 15))]);
                builder.Append(table[derEncoded[i] & 15]);
            }
            return builder.ToString();
        }
    }
}

