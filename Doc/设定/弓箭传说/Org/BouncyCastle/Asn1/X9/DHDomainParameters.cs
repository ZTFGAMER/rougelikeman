namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Collections;

    public class DHDomainParameters : Asn1Encodable
    {
        private readonly DerInteger p;
        private readonly DerInteger g;
        private readonly DerInteger q;
        private readonly DerInteger j;
        private readonly DHValidationParms validationParms;

        private DHDomainParameters(Asn1Sequence seq)
        {
            if ((seq.Count < 3) || (seq.Count > 5))
            {
                throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
            }
            IEnumerator e = seq.GetEnumerator();
            this.p = DerInteger.GetInstance(GetNext(e));
            this.g = DerInteger.GetInstance(GetNext(e));
            this.q = DerInteger.GetInstance(GetNext(e));
            Asn1Encodable next = GetNext(e);
            if ((next != null) && (next is DerInteger))
            {
                this.j = DerInteger.GetInstance(next);
                next = GetNext(e);
            }
            if (next != null)
            {
                this.validationParms = DHValidationParms.GetInstance(next.ToAsn1Object());
            }
        }

        public DHDomainParameters(DerInteger p, DerInteger g, DerInteger q, DerInteger j, DHValidationParms validationParms)
        {
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (q == null)
            {
                throw new ArgumentNullException("q");
            }
            this.p = p;
            this.g = g;
            this.q = q;
            this.j = j;
            this.validationParms = validationParms;
        }

        public static DHDomainParameters GetInstance(object obj)
        {
            if ((obj == null) || (obj is DHDomainParameters))
            {
                return (DHDomainParameters) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("Invalid DHDomainParameters: " + Platform.GetTypeName(obj), "obj");
            }
            return new DHDomainParameters((Asn1Sequence) obj);
        }

        public static DHDomainParameters GetInstance(Asn1TaggedObject obj, bool isExplicit) => 
            GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));

        private static Asn1Encodable GetNext(IEnumerator e) => 
            (!e.MoveNext() ? null : ((Asn1Encodable) e.Current));

        public override Asn1Object ToAsn1Object()
        {
            Asn1Encodable[] v = new Asn1Encodable[] { this.p, this.g, this.q };
            Asn1EncodableVector vector = new Asn1EncodableVector(v);
            if (this.j != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { this.j };
                vector.Add(objs);
            }
            if (this.validationParms != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { this.validationParms };
                vector.Add(objs);
            }
            return new DerSequence(vector);
        }

        public DerInteger P =>
            this.p;

        public DerInteger G =>
            this.g;

        public DerInteger Q =>
            this.q;

        public DerInteger J =>
            this.j;

        public DHValidationParms ValidationParms =>
            this.validationParms;
    }
}

