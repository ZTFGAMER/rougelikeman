namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Math.EC;
    using System;

    public class X9FieldElement : Asn1Encodable
    {
        private ECFieldElement f;

        public X9FieldElement(ECFieldElement f)
        {
            this.f = f;
        }

        public X9FieldElement(BigInteger p, Asn1OctetString s) : this(new FpFieldElement(p, new BigInteger(1, s.GetOctets())))
        {
        }

        public X9FieldElement(int m, int k1, int k2, int k3, Asn1OctetString s) : this(new F2mFieldElement(m, k1, k2, k3, new BigInteger(1, s.GetOctets())))
        {
        }

        public override Asn1Object ToAsn1Object()
        {
            int byteLength = X9IntegerConverter.GetByteLength(this.f);
            return new DerOctetString(X9IntegerConverter.IntegerToBytes(this.f.ToBigInteger(), byteLength));
        }

        public ECFieldElement Value =>
            this.f;
    }
}

