namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    public class CrlDistPoint : Asn1Encodable
    {
        internal readonly Asn1Sequence seq;

        private CrlDistPoint(Asn1Sequence seq)
        {
            this.seq = seq;
        }

        public CrlDistPoint(DistributionPoint[] points)
        {
            this.seq = new DerSequence(points);
        }

        public DistributionPoint[] GetDistributionPoints()
        {
            DistributionPoint[] pointArray = new DistributionPoint[this.seq.Count];
            for (int i = 0; i != this.seq.Count; i++)
            {
                pointArray[i] = DistributionPoint.GetInstance(this.seq[i]);
            }
            return pointArray;
        }

        public static CrlDistPoint GetInstance(object obj)
        {
            if ((obj is CrlDistPoint) || (obj == null))
            {
                return (CrlDistPoint) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new CrlDistPoint((Asn1Sequence) obj);
        }

        public static CrlDistPoint GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public override Asn1Object ToAsn1Object() => 
            this.seq;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            string newLine = Platform.NewLine;
            builder.Append("CRLDistPoint:");
            builder.Append(newLine);
            DistributionPoint[] distributionPoints = this.GetDistributionPoints();
            for (int i = 0; i != distributionPoints.Length; i++)
            {
                builder.Append("    ");
                builder.Append(distributionPoints[i]);
                builder.Append(newLine);
            }
            return builder.ToString();
        }
    }
}

