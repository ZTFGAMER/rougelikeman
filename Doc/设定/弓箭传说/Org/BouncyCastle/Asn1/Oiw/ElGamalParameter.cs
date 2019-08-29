namespace Org.BouncyCastle.Asn1.Oiw
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using System;

    public class ElGamalParameter : Asn1Encodable
    {
        internal DerInteger p;
        internal DerInteger g;

        public ElGamalParameter(Asn1Sequence seq)
        {
            if (seq.Count != 2)
            {
                throw new ArgumentException("Wrong number of elements in sequence", "seq");
            }
            this.p = DerInteger.GetInstance(seq[0]);
            this.g = DerInteger.GetInstance(seq[1]);
        }

        public ElGamalParameter(BigInteger p, BigInteger g)
        {
            this.p = new DerInteger(p);
            this.g = new DerInteger(g);
        }

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { this.p, this.g });

        public BigInteger P =>
            this.p.PositiveValue;

        public BigInteger G =>
            this.g.PositiveValue;
    }
}

