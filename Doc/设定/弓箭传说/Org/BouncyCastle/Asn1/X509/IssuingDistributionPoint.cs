namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    public class IssuingDistributionPoint : Asn1Encodable
    {
        private readonly DistributionPointName _distributionPoint;
        private readonly bool _onlyContainsUserCerts;
        private readonly bool _onlyContainsCACerts;
        private readonly ReasonFlags _onlySomeReasons;
        private readonly bool _indirectCRL;
        private readonly bool _onlyContainsAttributeCerts;
        private readonly Asn1Sequence seq;

        private IssuingDistributionPoint(Asn1Sequence seq)
        {
            this.seq = seq;
            for (int i = 0; i != seq.Count; i++)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[i]);
                switch (instance.TagNo)
                {
                    case 0:
                        this._distributionPoint = DistributionPointName.GetInstance(instance, true);
                        break;

                    case 1:
                        this._onlyContainsUserCerts = DerBoolean.GetInstance(instance, false).IsTrue;
                        break;

                    case 2:
                        this._onlyContainsCACerts = DerBoolean.GetInstance(instance, false).IsTrue;
                        break;

                    case 3:
                        this._onlySomeReasons = new ReasonFlags(DerBitString.GetInstance(instance, false));
                        break;

                    case 4:
                        this._indirectCRL = DerBoolean.GetInstance(instance, false).IsTrue;
                        break;

                    case 5:
                        this._onlyContainsAttributeCerts = DerBoolean.GetInstance(instance, false).IsTrue;
                        break;

                    default:
                        throw new ArgumentException("unknown tag in IssuingDistributionPoint");
                }
            }
        }

        public IssuingDistributionPoint(DistributionPointName distributionPoint, bool onlyContainsUserCerts, bool onlyContainsCACerts, ReasonFlags onlySomeReasons, bool indirectCRL, bool onlyContainsAttributeCerts)
        {
            this._distributionPoint = distributionPoint;
            this._indirectCRL = indirectCRL;
            this._onlyContainsAttributeCerts = onlyContainsAttributeCerts;
            this._onlyContainsCACerts = onlyContainsCACerts;
            this._onlyContainsUserCerts = onlyContainsUserCerts;
            this._onlySomeReasons = onlySomeReasons;
            Asn1EncodableVector v = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
            if (distributionPoint != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(true, 0, distributionPoint) };
                v.Add(objs);
            }
            if (onlyContainsUserCerts)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(false, 1, DerBoolean.True) };
                v.Add(objs);
            }
            if (onlyContainsCACerts)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(false, 2, DerBoolean.True) };
                v.Add(objs);
            }
            if (onlySomeReasons != null)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(false, 3, onlySomeReasons) };
                v.Add(objs);
            }
            if (indirectCRL)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(false, 4, DerBoolean.True) };
                v.Add(objs);
            }
            if (onlyContainsAttributeCerts)
            {
                Asn1Encodable[] objs = new Asn1Encodable[] { new DerTaggedObject(false, 5, DerBoolean.True) };
                v.Add(objs);
            }
            this.seq = new DerSequence(v);
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

        public static IssuingDistributionPoint GetInstance(object obj)
        {
            if ((obj == null) || (obj is IssuingDistributionPoint))
            {
                return (IssuingDistributionPoint) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new IssuingDistributionPoint((Asn1Sequence) obj);
        }

        public static IssuingDistributionPoint GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object() => 
            this.seq;

        public override string ToString()
        {
            string newLine = Platform.NewLine;
            StringBuilder buf = new StringBuilder();
            buf.Append("IssuingDistributionPoint: [");
            buf.Append(newLine);
            if (this._distributionPoint != null)
            {
                this.appendObject(buf, newLine, "distributionPoint", this._distributionPoint.ToString());
            }
            if (this._onlyContainsUserCerts)
            {
                this.appendObject(buf, newLine, "onlyContainsUserCerts", this._onlyContainsUserCerts.ToString());
            }
            if (this._onlyContainsCACerts)
            {
                this.appendObject(buf, newLine, "onlyContainsCACerts", this._onlyContainsCACerts.ToString());
            }
            if (this._onlySomeReasons != null)
            {
                this.appendObject(buf, newLine, "onlySomeReasons", this._onlySomeReasons.ToString());
            }
            if (this._onlyContainsAttributeCerts)
            {
                this.appendObject(buf, newLine, "onlyContainsAttributeCerts", this._onlyContainsAttributeCerts.ToString());
            }
            if (this._indirectCRL)
            {
                this.appendObject(buf, newLine, "indirectCRL", this._indirectCRL.ToString());
            }
            buf.Append("]");
            buf.Append(newLine);
            return buf.ToString();
        }

        public bool OnlyContainsUserCerts =>
            this._onlyContainsUserCerts;

        public bool OnlyContainsCACerts =>
            this._onlyContainsCACerts;

        public bool IsIndirectCrl =>
            this._indirectCRL;

        public bool OnlyContainsAttributeCerts =>
            this._onlyContainsAttributeCerts;

        public DistributionPointName DistributionPoint =>
            this._distributionPoint;

        public ReasonFlags OnlySomeReasons =>
            this._onlySomeReasons;
    }
}

