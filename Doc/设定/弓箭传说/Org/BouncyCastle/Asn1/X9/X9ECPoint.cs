namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math.EC;
    using Org.BouncyCastle.Utilities;
    using System;

    public class X9ECPoint : Asn1Encodable
    {
        private readonly Asn1OctetString encoding;
        private ECCurve c;
        private ECPoint p;

        public X9ECPoint(ECPoint p) : this(p, false)
        {
        }

        public X9ECPoint(ECCurve c, byte[] encoding)
        {
            this.c = c;
            this.encoding = new DerOctetString(Arrays.Clone(encoding));
        }

        public X9ECPoint(ECCurve c, Asn1OctetString s) : this(c, s.GetOctets())
        {
        }

        public X9ECPoint(ECPoint p, bool compressed)
        {
            this.p = p.Normalize();
            this.encoding = new DerOctetString(p.GetEncoded(compressed));
        }

        public byte[] GetPointEncoding() => 
            Arrays.Clone(this.encoding.GetOctets());

        public override Asn1Object ToAsn1Object() => 
            this.encoding;

        public ECPoint Point
        {
            get
            {
                if (this.p == null)
                {
                    this.p = this.c.DecodePoint(this.encoding.GetOctets()).Normalize();
                }
                return this.p;
            }
        }

        public bool IsPointCompressed
        {
            get
            {
                byte[] octets = this.encoding.GetOctets();
                return (((octets != null) && (octets.Length > 0)) && ((octets[0] == 2) || (octets[0] == 3)));
            }
        }
    }
}

