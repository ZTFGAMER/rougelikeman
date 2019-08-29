namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;

    public class DHValidationParms : Asn1Encodable
    {
        private readonly DerBitString seed;
        private readonly DerInteger pgenCounter;

        private DHValidationParms(Asn1Sequence seq)
        {
            if (seq.Count != 2)
            {
                throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
            }
            this.seed = DerBitString.GetInstance(seq[0]);
            this.pgenCounter = DerInteger.GetInstance(seq[1]);
        }

        public DHValidationParms(DerBitString seed, DerInteger pgenCounter)
        {
            if (seed == null)
            {
                throw new ArgumentNullException("seed");
            }
            if (pgenCounter == null)
            {
                throw new ArgumentNullException("pgenCounter");
            }
            this.seed = seed;
            this.pgenCounter = pgenCounter;
        }

        public static DHValidationParms GetInstance(object obj)
        {
            if ((obj == null) || (obj is DHDomainParameters))
            {
                return (DHValidationParms) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("Invalid DHValidationParms: " + Platform.GetTypeName(obj), "obj");
            }
            return new DHValidationParms((Asn1Sequence) obj);
        }

        public static DHValidationParms GetInstance(Asn1TaggedObject obj, bool isExplicit) => 
            GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { this.seed, this.pgenCounter });

        public DerBitString Seed =>
            this.seed;

        public DerInteger PgenCounter =>
            this.pgenCounter;
    }
}

