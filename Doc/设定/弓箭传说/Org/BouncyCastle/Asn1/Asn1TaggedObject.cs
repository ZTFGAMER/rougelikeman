namespace Org.BouncyCastle.Asn1
{
    using Org.BouncyCastle.Utilities;
    using System;

    public abstract class Asn1TaggedObject : Asn1Object, Asn1TaggedObjectParser, IAsn1Convertible
    {
        internal int tagNo;
        internal bool explicitly;
        internal Asn1Encodable obj;

        protected Asn1TaggedObject(int tagNo, Asn1Encodable obj)
        {
            this.explicitly = true;
            this.explicitly = true;
            this.tagNo = tagNo;
            this.obj = obj;
        }

        protected Asn1TaggedObject(bool explicitly, int tagNo, Asn1Encodable obj)
        {
            this.explicitly = true;
            this.explicitly = explicitly || (obj is IAsn1Choice);
            this.tagNo = tagNo;
            this.obj = obj;
        }

        protected override bool Asn1Equals(Asn1Object asn1Object)
        {
            Asn1TaggedObject obj2 = asn1Object as Asn1TaggedObject;
            if (obj2 == null)
            {
                return false;
            }
            return (((this.tagNo == obj2.tagNo) && (this.explicitly == obj2.explicitly)) && object.Equals(this.GetObject(), obj2.GetObject()));
        }

        protected override int Asn1GetHashCode()
        {
            int hashCode = this.tagNo.GetHashCode();
            if (this.obj != null)
            {
                hashCode ^= this.obj.GetHashCode();
            }
            return hashCode;
        }

        public static Asn1TaggedObject GetInstance(object obj)
        {
            if ((obj != null) && !(obj is Asn1TaggedObject))
            {
                throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
            }
            return (Asn1TaggedObject) obj;
        }

        public static Asn1TaggedObject GetInstance(Asn1TaggedObject obj, bool explicitly)
        {
            if (!explicitly)
            {
                throw new ArgumentException("implicitly tagged tagged object");
            }
            return (Asn1TaggedObject) obj.GetObject();
        }

        public Asn1Object GetObject()
        {
            if (this.obj != null)
            {
                return this.obj.ToAsn1Object();
            }
            return null;
        }

        public IAsn1Convertible GetObjectParser(int tag, bool isExplicit)
        {
            if (tag != 0x11)
            {
                if (tag == 0x10)
                {
                    return Asn1Sequence.GetInstance(this, isExplicit).Parser;
                }
                if (tag == 4)
                {
                    return Asn1OctetString.GetInstance(this, isExplicit).Parser;
                }
            }
            else
            {
                return Asn1Set.GetInstance(this, isExplicit).Parser;
            }
            if (!isExplicit)
            {
                throw Platform.CreateNotImplementedException("implicit tagging for tag: " + tag);
            }
            return this.GetObject();
        }

        internal static bool IsConstructed(bool isExplicit, Asn1Object obj)
        {
            if ((isExplicit || (obj is Asn1Sequence)) || (obj is Asn1Set))
            {
                return true;
            }
            Asn1TaggedObject obj2 = obj as Asn1TaggedObject;
            if (obj2 == null)
            {
                return false;
            }
            return IsConstructed(obj2.IsExplicit(), obj2.GetObject());
        }

        public bool IsEmpty() => 
            false;

        public bool IsExplicit() => 
            this.explicitly;

        public override string ToString()
        {
            object[] objArray1 = new object[] { "[", this.tagNo, "]", this.obj };
            return string.Concat(objArray1);
        }

        public int TagNo =>
            this.tagNo;
    }
}

