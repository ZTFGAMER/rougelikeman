namespace Org.BouncyCastle.Asn1.Pkcs
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using System;
    using System.Collections;

    public class DHParameter : Asn1Encodable
    {
        internal DerInteger p;
        internal DerInteger g;
        internal DerInteger l;

        public DHParameter(Asn1Sequence seq)
        {
            IEnumerator enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            this.p = (DerInteger) enumerator.Current;
            enumerator.MoveNext();
            this.g = (DerInteger) enumerator.Current;
            if (enumerator.MoveNext())
            {
                this.l = (DerInteger) enumerator.Current;
            }
        }

        public DHParameter(BigInteger p, BigInteger g, int l)
        {
            this.p = new DerInteger(p);
            this.g = new DerInteger(g);
            if (l != 0)
            {
                this.l = new DerInteger(l);
            }
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1Encodable[] v = new Asn1Encodable[] { this.p, this.g };
            Asn1EncodableVector vector = new Asn1EncodableVector(v);
            if (this.l != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { this.l };
                vector.Add(objs);
            }
            return new DerSequence(vector);
        }

        public BigInteger P =>
            this.p.PositiveValue;

        public BigInteger G =>
            this.g.PositiveValue;

        public BigInteger L =>
            this.l?.PositiveValue;
    }
}

