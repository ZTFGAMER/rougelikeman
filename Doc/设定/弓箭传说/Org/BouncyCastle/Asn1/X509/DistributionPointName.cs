namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    public class DistributionPointName : Asn1Encodable, IAsn1Choice
    {
        internal readonly Asn1Encodable name;
        internal readonly int type;
        public const int FullName = 0;
        public const int NameRelativeToCrlIssuer = 1;

        public DistributionPointName(Asn1TaggedObject obj)
        {
            this.type = obj.TagNo;
            if (this.type == 0)
            {
                this.name = GeneralNames.GetInstance(obj, false);
            }
            else
            {
                this.name = Asn1Set.GetInstance(obj, false);
            }
        }

        public DistributionPointName(GeneralNames name) : this(0, name)
        {
        }

        public DistributionPointName(int type, Asn1Encodable name)
        {
            this.type = type;
            this.name = name;
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

        public static DistributionPointName GetInstance(object obj)
        {
            if ((obj == null) || (obj is DistributionPointName))
            {
                return (DistributionPointName) obj;
            }
            if (!(obj is Asn1TaggedObject))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new DistributionPointName((Asn1TaggedObject) obj);
        }

        public static DistributionPointName GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1TaggedObject.GetInstance(obj, true));

        public override Asn1Object ToAsn1Object() => 
            new DerTaggedObject(false, this.type, this.name);

        public override string ToString()
        {
            string newLine = Platform.NewLine;
            StringBuilder buf = new StringBuilder();
            buf.Append("DistributionPointName: [");
            buf.Append(newLine);
            if (this.type == 0)
            {
                this.appendObject(buf, newLine, "fullName", this.name.ToString());
            }
            else
            {
                this.appendObject(buf, newLine, "nameRelativeToCRLIssuer", this.name.ToString());
            }
            buf.Append("]");
            buf.Append(newLine);
            return buf.ToString();
        }

        public int PointType =>
            this.type;

        public Asn1Encodable Name =>
            this.name;
    }
}

