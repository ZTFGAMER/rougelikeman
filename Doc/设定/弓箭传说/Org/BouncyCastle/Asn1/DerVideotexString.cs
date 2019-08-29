namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerVideotexString : DerStringBase
    {
        private readonly byte[] mString;

        public DerVideotexString(byte[] encoding)
        {
            this.mString = Arrays.Clone(encoding);
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerVideotexString str = asn1Object as DerVideotexString;
            if (str == null)
            {
                return false;
            }
            return Arrays.AreEqual(this.mString, str.mString);
        }

        protected override int Asn1GetHashCode() => 
            Arrays.GetHashCode(this.mString);

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(0x15, this.mString);
        }

        public static DerVideotexString GetInstance(object obj)
        {
            if ((obj == null) || (obj is DerVideotexString))
            {
                return (DerVideotexString) obj;
            }
            if (obj is byte[])
            {
                try
                {
                    return (DerVideotexString) Asn1Object.FromByteArray((byte[]) obj);
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("encoding error in GetInstance: " + exception.ToString(), "obj");
                }
            }
            throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
        }

        public static DerVideotexString GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerVideotexString))
            {
                return new DerVideotexString(((Asn1OctetString) obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public byte[] GetOctets() => 
            Arrays.Clone(this.mString);

        public override string GetString() => 
            Strings.FromByteArray(this.mString);
    }
}

