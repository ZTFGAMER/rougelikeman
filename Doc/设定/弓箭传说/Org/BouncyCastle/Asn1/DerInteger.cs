namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerInteger : Asn1Object
    {
        private readonly byte[] bytes;

        public DerInteger(BigInteger value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.bytes = value.ToByteArray();
        }

        public DerInteger(int value)
        {
            this.bytes = BigInteger.ValueOf((long) value).ToByteArray();
        }

        public DerInteger(byte[] bytes)
        {
            this.bytes = bytes;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerInteger integer = asn1Object as DerInteger;
            if (integer == null)
            {
                return false;
            }
            return Arrays.AreEqual(this.bytes, integer.bytes);
        }

        protected override int Asn1GetHashCode() => 
            Arrays.GetHashCode(this.bytes);

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(2, this.bytes);
        }

        public static DerInteger GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerInteger))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerInteger) obj;
        }

        public static DerInteger GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerInteger))
            {
                return new DerInteger(Asn1OctetString.GetInstance(obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public override string ToString() => 
            this.Value.ToString();

        public BigInteger Value =>
            new BigInteger(this.bytes);

        public BigInteger PositiveValue =>
            new BigInteger(1, this.bytes);
    }
}

