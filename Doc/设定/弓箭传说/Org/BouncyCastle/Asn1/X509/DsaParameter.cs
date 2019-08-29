namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DsaParameter : Asn1Encodable
    {
        internal readonly DerInteger p;
        internal readonly DerInteger q;
        internal readonly DerInteger g;

        private DsaParameter(Asn1Sequence seq)
        {
            if (seq.Count != 3)
            {
                throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
            }
            this.p = DerInteger.GetInstance(seq[0]);
            this.q = DerInteger.GetInstance(seq[1]);
            this.g = DerInteger.GetInstance(seq[2]);
        }

        public DsaParameter(BigInteger p, BigInteger q, BigInteger g)
        {
            this.p = new DerInteger(p);
            this.q = new DerInteger(q);
            this.g = new DerInteger(g);
        }

        public static DsaParameter GetInstance(object obj)
        {
            if ((obj == null) || (obj is DsaParameter))
            {
                return (DsaParameter) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("Invalid DsaParameter: " + Platform.GetTypeName(obj));
            }
            return new DsaParameter((Asn1Sequence) obj);
        }

        public static DsaParameter GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { this.p, this.q, this.g });

        public BigInteger P =>
            this.p.PositiveValue;

        public BigInteger Q =>
            this.q.PositiveValue;

        public BigInteger G =>
            this.g.PositiveValue;
    }
}

