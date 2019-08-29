namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    public class DistributionPoint : Asn1Encodable
    {
        internal readonly Org.BouncyCastle.Asn1.X509.DistributionPointName distributionPoint;
        internal readonly ReasonFlags reasons;
        internal readonly GeneralNames cRLIssuer;

        private DistributionPoint(Asn1Sequence seq)
        {
            for (int i = 0; i != seq.Count; i++)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[i]);
                switch (instance.TagNo)
                {
                    case 0:
                        this.distributionPoint = Org.BouncyCastle.Asn1.X509.DistributionPointName.GetInstance(instance, true);
                        break;

                    case 1:
                        this.reasons = new ReasonFlags(DerBitString.GetInstance(instance, false));
                        break;

                    case 2:
                        this.cRLIssuer = GeneralNames.GetInstance(instance, false);
                        break;
                }
            }
        }

        public DistributionPoint(Org.BouncyCastle.Asn1.X509.DistributionPointName distributionPointName, ReasonFlags reasons, GeneralNames crlIssuer)
        {
            this.distributionPoint = distributionPointName;
            this.reasons = reasons;
            this.cRLIssuer = crlIssuer;
        }

        private void appendObject(StringBuilder buf, string sep, string name, string val)
        {
            string str = "    ";
            buf.Append(str);
            buf.Append(name);
            buf.Append(":");
            buf.Append(sep);
            buf.Append(str);
            buf.Append(str);
            buf.Append(val);
            buf.Append(sep);
        }

        public static DistributionPoint GetInstance(object obj)
        {
            if ((obj == null) || (obj is DistributionPoint))
            {
                return (DistributionPoint) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("Invalid DistributionPoint: " + Platform.GetTypeName(obj));
            }
            return new DistributionPoint((Asn1Sequence) obj);
        }

        public static DistributionPoint GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            if (this.distributionPoint != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(0, this.distributionPoint) };
                v.Add(objs);
            }
            if (this.reasons != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(false, 1, this.reasons) };
                v.Add(objs);
            }
            if (this.cRLIssuer != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(false, 2, this.cRLIssuer) };
                v.Add(objs);
            }
            return new DerSequence(v);
        }

        public override string ToString()
        {
            string newLine = Platform.NewLine;
            StringBuilder buf = new StringBuilder();
            buf.Append("DistributionPoint: [");
            buf.Append(newLine);
            if (this.distributionPoint != null)
            {
                this.appendObject(buf, newLine, "distributionPoint", this.distributionPoint.ToString());
            }
            if (this.reasons != null)
            {
                this.appendObject(buf, newLine, "reasons", this.reasons.ToString());
            }
            if (this.cRLIssuer != null)
            {
                this.appendObject(buf, newLine, "cRLIssuer", this.cRLIssuer.ToString());
            }
            buf.Append("]");
            buf.Append(newLine);
            return buf.ToString();
        }

        public Org.BouncyCastle.Asn1.X509.DistributionPointName DistributionPointName =>
            this.distributionPoint;

        public ReasonFlags Reasons =>
            this.reasons;

        public GeneralNames CrlIssuer =>
            this.cRLIssuer;
    }
}

