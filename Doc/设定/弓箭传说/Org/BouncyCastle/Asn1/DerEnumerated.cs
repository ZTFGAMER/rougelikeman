namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DerEnumerated : Asn1Object
    {
        private readonly byte[] bytes;
        private static readonly DerEnumerated[] cache = new DerEnumerated[12];

        public DerEnumerated(BigInteger val)
        {
            this.bytes = val.ToByteArray();
        }

        public DerEnumerated(int val)
        {
            this.bytes = BigInteger.ValueOf((long) val).ToByteArray();
        }

        public DerEnumerated(byte[] bytes)
        {
            this.bytes = bytes;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            DerEnumerated enumerated = asn1Object as DerEnumerated;
            if (enumerated == null)
            {
                return false;
            }
            return Arrays.AreEqual(this.bytes, enumerated.bytes);
        }

        protected override int Asn1GetHashCode() => 
            Arrays.GetHashCode(this.bytes);

        internal override void Encode(DerOutputStream derOut)
        {
            derOut.WriteEncoded(10, this.bytes);
        }

        internal static DerEnumerated FromOctetString(byte[] enc)
        {
            if (enc.Length == 0)
            {
                throw new ArgumentException("ENUMERATED has zero length", "enc");
            }
            if (enc.Length == 1)
            {
                int index = enc[0];
                if (index < cache.Length)
                {
                    DerEnumerated enumerated2;
                    DerEnumerated enumerated = cache[index];
                    if (enumerated != null)
                    {
                        return enumerated;
                    }
                    cache[index] = enumerated2 = new DerEnumerated(Arrays.Clone(enc));
                    return enumerated2;
                }
            }
            return new DerEnumerated(Arrays.Clone(enc));
        }

        public static DerEnumerated GetInstance(object obj)
        {
            if ((obj != null) && !(obj is DerEnumerated))
            {
                throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
            }
            return (DerEnumerated) obj;
        }

        public static DerEnumerated GetInstance(Asn1TaggedObject obj, bool isExplicit)
        {
            Asn1Object obj2 = obj.GetObject();
            if (!isExplicit && !(obj2 is DerEnumerated))
            {
                return FromOctetString(((Asn1OctetString) obj2).GetOctets());
            }
            return GetInstance(obj2);
        }

        public BigInteger Value =>
            new BigInteger(this.bytes);
    }
}

