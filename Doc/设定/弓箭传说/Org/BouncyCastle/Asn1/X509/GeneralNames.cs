namespace Org.BouncyCastle.Asn1.X509
{
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Utilities;
    using System;
    using System.Text;

    public class GeneralNames : Asn1Encodable
    {
        private readonly GeneralName[] names;

        private GeneralNames(Asn1Sequence seq)
        {
            this.names = new GeneralName[seq.Count];
            for (int i = 0; i != seq.Count; i++)
            {
                this.names[i] = GeneralName.GetInstance(seq[i]);
            }
        }

        public GeneralNames(GeneralName name)
        {
            this.names = new GeneralName[] { name };
        }

        public GeneralNames(GeneralName[] names)
        {
            this.names = (GeneralName[]) names.Clone();
        }

        public static GeneralNames GetInstance(object obj)
        {
            if ((obj == null) || (obj is GeneralNames))
            {
                return (GeneralNames) obj;
            }
            if (!(obj is Asn1Sequence))
            {
                throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
            }
            return new GeneralNames((Asn1Sequence) obj);
        }

        public static GeneralNames GetInstance(Asn1TaggedObject obj, bool explicitly) => 
            GetInstance(Asn1Sequence.GetInstance(obj, explicitly));

        public GeneralName[] GetNames() => 
            ((GeneralName[]) this.names.Clone());

        public override Asn1Object ToAsn1Object() => 
            new DerSequence(this.names);

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            string newLine = Platform.NewLine;
            builder.Append("GeneralNames:");
            builder.Append(newLine);
            foreach (GeneralName name in this.names)
            {
                builder.Append("    ");
                builder.Append(name);
                builder.Append(newLine);
            }
            return builder.ToString();
        }
    }
}

