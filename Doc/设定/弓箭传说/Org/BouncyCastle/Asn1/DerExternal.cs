namespace Org.BouncyCastle.Asn1
{
    using System;
    using System.IO;

    public class DerExternal : Asn1Object
    {
        private DerObjectIdentifier directReference;
        private DerInteger indirectReference;
        private Asn1Object dataValueDescriptor;
        private int encoding;
        private Asn1Object externalContent;

        public DerExternal(Asn1EncodableVector vector)
        {
            int index = 0;
            Asn1Object objFromVector = GetObjFromVector(vector, index);
            switch (objFromVector)
            {
                case (DerObjectIdentifier _):
                    this.directReference = (DerObjectIdentifier) objFromVector;
                    index++;
                    objFromVector = GetObjFromVector(vector, index);
                    break;
            }
            if (objFromVector is DerInteger)
            {
                this.indirectReference = (DerInteger) objFromVector;
                index++;
                objFromVector = GetObjFromVector(vector, index);
            }
            if (!(objFromVector is Asn1TaggedObject))
            {
                this.dataValueDescriptor = objFromVector;
                index++;
                objFromVector = GetObjFromVector(vector, index);
            }
            if (vector.Count != (index + 1))
            {
                throw new ArgumentException("input vector too large", "vector");
            }
            if (!(objFromVector is Asn1TaggedObject))
            {
                throw new ArgumentException("No tagged object found in vector. Structure doesn't seem to be of type External", "vector");
            }
            Asn1TaggedObject obj3 = (Asn1TaggedObject) objFromVector;
            this.Encoding = obj3.TagNo;
            if ((this.encoding < 0) || (this.encoding > 2))
            {
                throw new InvalidOperationException("invalid encoding value");
            }
            this.externalContent = obj3.GetObject();
        }

        public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1Object dataValueDescriptor, DerTaggedObject externalData) : this(directReference, indirectReference, dataValueDescriptor, externalData.TagNo, externalData.ToAsn1Object())
        {
        }

        public DerExternal(DerObjectIdentifier directReference, DerInteger indirectReference, Asn1Object dataValueDescriptor, int encoding, Asn1Object externalData)
        {
            this.DirectReference = directReference;
            this.IndirectReference = indirectReference;
            this.DataValueDescriptor = dataValueDescriptor;
            this.Encoding = encoding;
            this.ExternalContent = externalData.ToAsn1Object();
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            if (this == asn1Object)
            {
                return true;
            }
            DerExternal external = asn1Object as DerExternal;
            if (external == null)
            {
                return false;
            }
            return (((object.Equals(this.directReference, external.directReference) && object.Equals(this.indirectReference, external.indirectReference)) && object.Equals(this.dataValueDescriptor, external.dataValueDescriptor)) && this.externalContent.Equals(external.externalContent));
        }

        protected override int Asn1GetHashCode()
        {
            int hashCode = this.externalContent.GetHashCode();
            if (this.directReference != null)
            {
                hashCode ^= this.directReference.GetHashCode();
            }
            if (this.indirectReference != null)
            {
                hashCode ^= this.indirectReference.GetHashCode();
            }
            if (this.dataValueDescriptor != null)
            {
                hashCode ^= this.dataValueDescriptor.GetHashCode();
            }
            return hashCode;
        }

        internal override void Encode(DerOutputStream derOut)
        {
            MemoryStream ms = new MemoryStream();
            WriteEncodable(ms, this.directReference);
            WriteEncodable(ms, this.indirectReference);
            WriteEncodable(ms, this.dataValueDescriptor);
            WriteEncodable(ms, new DerTaggedObject(8, this.externalContent));
            derOut.WriteEncoded(0x20, 8, ms.ToArray());
        }

        private static Asn1Object GetObjFromVector(Asn1EncodableVector v, int index)
        {
            if (v.Count <= index)
            {
                throw new ArgumentException("too few objects in input vector", "v");
            }
            return v[index].ToAsn1Object();
        }

        private static void WriteEncodable(MemoryStream ms, Asn1Encodable e)
        {
            if (e != null)
            {
                byte[] derEncoded = e.GetDerEncoded();
                ms.Write(derEncoded, 0, derEncoded.Length);
            }
        }

        public Asn1Object DataValueDescriptor
        {
            get => 
                this.dataValueDescriptor;
            set => 
                (this.dataValueDescriptor = value);
        }

        public DerObjectIdentifier DirectReference
        {
            get => 
                this.directReference;
            set => 
                (this.directReference = value);
        }

        public int Encoding
        {
            get => 
                this.encoding;
            set
            {
                if ((this.encoding < 0) || (this.encoding > 2))
                {
                    throw new InvalidOperationException("invalid encoding value: " + this.encoding);
                }
                this.encoding = value;
            }
        }

        public Asn1Object ExternalContent
        {
            get => 
                this.externalContent;
            set => 
                (this.externalContent = value);
        }

        public DerInteger IndirectReference
        {
            get => 
                this.indirectReference;
            set => 
                (this.indirectReference = value);
        }
    }
}

