namespace Org.BouncyCastle.Asn1.X9
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using System;

    public class X9FieldID : Asn1Encodable
    {
        private readonly DerObjectIdentifier id;
        private readonly Asn1Object parameters;

        private X9FieldID(Asn1Sequence seq)
        {
            this.id = DerObjectIdentifier.GetInstance(seq[0]);
            this.parameters = seq[1].ToAsn1Object();
        }

        public X9FieldID(BigInteger primeP)
        {
            this.id = X9ObjectIdentifiers.PrimeField;
            this.parameters = new DerInteger(primeP);
        }

        public X9FieldID(int m, int k1) : this(m, k1, 0, 0)
        {
        }

        public X9FieldID(int m, int k1, int k2, int k3)
        {
            this.id = X9ObjectIdentifiers.CharacteristicTwoField;
            Asn1Encodable[] v = new Asn1Encodable[] { new DerInteger(m) };
            Asn1EncodableVector vector = new Asn1EncodableVector(v);
            if (k2 == 0)
            {
                if (k3 != 0)
                {
                    throw new ArgumentException("inconsistent k values");
                }
                Asn1Encodable[] objs = new Asn1Encodable[] { X9ObjectIdentifiers.TPBasis, new DerInteger(k1) };
                vector.Add(objs);
            }
            else
            {
                if ((k2 <= k1) || (k3 <= k2))
                {
                    throw new ArgumentException("inconsistent k values");
                }
                Asn1Encodable[] objs = new Asn1Encodable[2];
                objs[0] = X9ObjectIdentifiers.PPBasis;
                Asn1Encodable[] encodableArray4 = new Asn1Encodable[] { new DerInteger(k1), new DerInteger(k2), new DerInteger(k3) };
                objs[1] = new DerSequence(encodableArray4);
                vector.Add(objs);
            }
            this.parameters = new DerSequence(vector);
        }

        public static X9FieldID GetInstance(object obj)
        {
            if (obj is X9FieldID)
            {
                return (X9FieldID) obj;
            }
            if (obj == null)
            {
                return null;
            }
            return new X9FieldID(Asn1Sequence.GetInstance(obj));
        }

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(new Asn1Encodable[] { this.id, this.parameters });

        public DerObjectIdentifier Identifier =>
            this.id;

        public Asn1Object Parameters =>
            this.parameters;
    }
}

