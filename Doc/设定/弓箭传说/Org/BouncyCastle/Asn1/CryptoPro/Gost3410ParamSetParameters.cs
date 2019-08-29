namespace Org.BouncyCastle.Asn1.CryptoPro
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public class Gost3410ParamSetParameters : Asn1Encodable
    {
        private readonly int keySize;
        private readonly DerInteger p;
        private readonly DerInteger q;
        private readonly DerInteger a;

        private Gost3410ParamSetParameters(Asn1Sequence seq)
        {
            if (seq.Count != 4)
            {
                throw new ArgumentException("Wrong number of elements in sequence", "seq");
            }
            this.keySize = DerInteger.GetInstance(seq[0]).Value.IntValue;
            this.p = DerInteger.GetInstance(seq[1]);
            this.q = DerInteger.GetInstance(seq[2]);
            this.a = DerInteger.GetInstance(seq[3]);
        }

        public Gost3410ParamSetParameters(int keySize, BigInteger p, BigInteger q, BigInteger a)
        {
            this.keySize = keySize;
            this.p = new DerInteger(p);
            this.q = new DerInteger(q);
            this.a = new DerInteger(a);
        }

        public static Gost3410ParamSetParameters GetInstance(object obj)
        {
            if ((obj == null) || (obj is Gost3410ParamSetParameters))
            {
                return (Gost3410ParamSetParameters) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("Invalid GOST3410Parameter: " + Platform.GetTypeName(obj));
            }
            return new Gost3410ParamSetParameters((Asn1Sequence) obj);
        }

        public static Gost3410ParamSetParameters GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { new DerInteger(this.keySize), this.p, this.q, this.a });

        public int KeySize =>
            this.keySize;

        public BigInteger P =>
            this.p.PositiveValue;

        public BigInteger Q =>
            this.q.PositiveValue;

        public BigInteger A =>
            this.a.PositiveValue;
    }
}

