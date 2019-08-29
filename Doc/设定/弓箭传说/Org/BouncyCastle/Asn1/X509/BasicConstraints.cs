namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Utilities;
    using System;

    public class BasicConstraints : Asn1Encodable
    {
        private readonly DerBoolean cA;
        private readonly DerInteger pathLenConstraint;

        private BasicConstraints(Asn1Sequence seq)
        {
            if (seq.Count > 0)
            {
                if (seq[0] is DerBoolean)
                {
                    this.cA = DerBoolean.GetInstance(seq[0]);
                }
                else
                {
                    this.pathLenConstraint = DerInteger.GetInstance(seq[0]);
                }
                if (seq.Count > 1)
                {
                    if (this.cA == null)
                    {
                        throw new ArgumentException("wrong sequence in constructor", "seq");
                    }
                    this.pathLenConstraint = DerInteger.GetInstance(seq[1]);
                }
            }
        }

        public BasicConstraints(bool cA)
        {
            if (cA)
            {
                this.cA = DerBoolean.True;
            }
        }

        public BasicConstraints(int pathLenConstraint)
        {
            this.cA = DerBoolean.True;
            this.pathLenConstraint = new DerInteger(pathLenConstraint);
        }

        public static BasicConstraints GetInstance(object obj)
        {
            if ((obj == null) || (obj is BasicConstraints))
            {
                return (BasicConstraints) obj;
            }
            if (obj is Asn1Sequence)
            {
                return new BasicConstraints((Asn1Sequence) obj);
            }
            if (!(obj is X509Extension))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return GetInstance(X509Extension.ConvertValueToObject((X509Extension) obj));
        }

        public static BasicConstraints GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public bool IsCA() => 
            ((this.cA != null) && this.cA.IsTrue);

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            if (this.cA != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { this.cA };
                v.Add(objs);
            }
            if (this.pathLenConstraint != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { this.pathLenConstraint };
                v.Add(objs);
            }
            return new DerSequence(v);
        }

        public override string ToString()
        {
            if (this.pathLenConstraint == null)
            {
                return ("BasicConstraints: isCa(" + this.IsCA() + ")");
            }
            object[] objArray1 = new object[] { "BasicConstraints: isCa(", this.IsCA(), "), pathLenConstraint = ", this.pathLenConstraint.Value };
            return string.Concat(objArray1);
        }

        public BigInteger PathLenConstraint =>
            this.pathLenConstraint?.Value;
    }
}

